﻿$ready(function () {
    Vue.use(VueHtml5Editor, {
        showModuleName: true,
        image: {
            sizeLimit: 512 * 1024,
            compress: true,
            width: 500,
            height: 400,
            quality: 80
        }
    });
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            id: $api.querystring('id'),     //课程Id
            uid: $api.querystring('uid'),       //课程的uid         
            course: {},  //当前课程
            datas: [],
            defaultProps: {
                children: 'children',
                label: 'Ol_Name'
            },
            expanded: [],        //树形默认展开的节点
            expanded_storage: 'outline_for_admin_tree' + $api.querystring('id'),  //用于记录展开节点的storage名称
            filterText: '',      //查询过虑树形的字符
            total: 0,     //章节总数

            modify_show: false,        //编辑内容的面板是否显示
            modify_obj: {},          //要编辑的对象

            accessory_show: false,       //附件的列表显示
            live_show: false,            //直播的编辑显示

            loading: false,
            loadingid: -1,
            loading_sumbit: false,   //提交时的预载           
            loading_init: true
        },
        mounted: function () {

        },
        created: function () {
            var th = this;
            $api.get('Course/ForID', { 'id': th.id }).then(function (req) {
                if (req.data.success) {
                    th.course = req.data.result;
                    document.title += " - " + th.course.Cou_Name;
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            });
            th.getTreeData();
        },
        computed: {
        },
        watch: {
            filterText: function (val) {
                this.$refs.tree.filter(val);
            }
        },
        methods: {
            //所取章节数据，为树形数据
            getTreeData: function () {
                var th = this;
                this.loading = true;
                $api.get('Outline/Tree', { 'couid': th.id, 'isuse': null }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.datas = req.data.result;
                        //获取默认展开的节点
                        var arr = $api.storage(th.expanded_storage);
                        if ($api.getType(arr) == 'Array') {
                            th.expanded = arr;
                        }
                        th.calcSerial(null, '');
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                });
            },
            //拖动节点改变顺序
            handleDragEnd(draggingNode, dropNode, dropType, ev) {
                var th = this;
                th.loading_sumbit = true;
                var arr = th.tree2array(this.datas);
                $api.post('Outline/ModifyTaxis', { 'list': arr }).then(function (req) {
                    th.loading_sumbit = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        th.$message({
                            type: 'success',
                            message: '更改排序成功!',
                            center: true
                        });
                        th.getTreeData();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });

            },
            //节点展开事件
            nodeexpand: function (data, node, tree) {
                this.expanded.push(data.Ol_ID);
                $api.storage(this.expanded_storage, this.expanded);
            },
            //节点折叠事件
            nodecollapse: function (data, node, tree) {
                var index = this.expanded.indexOf(data.Ol_ID);
                if (index > -1) {
                    this.expanded.splice(index, 1);
                    $api.storage(this.expanded_storage, this.expanded);
                }
            },
            //过滤树形
            filterNode: function (value, data) {
                if (!value) return true;
                var txt = $api.trim(value.toLowerCase());
                if (txt == '') return true;
                return data.Ol_Name.toLowerCase().indexOf(txt) !== -1;
            },
            //修改状态
            changeState: function (data, field) {
                data[field] = !data[field];
                var th = this;
                this.loadingid = data.Ol_ID;
                $api.post('Outline/Modify', { 'entity': data }).then(function (req) {
                    th.loadingid = -1;
                    if (req.data.success) {
                        th.$message({
                            type: 'success',
                            message: '修改状态成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    vapp.$alert(err, '错误');
                    th.loadingid = -1;
                });
            },
            //将树形数据转到数据列表，用于递交到服务端更改专业的排序
            tree2array: function (datas) {
                var list = [];
                list = toarray(datas, 0, 1, list);
                return list;
                function toarray(arr, pid, level, list) {
                    for (let i = 0; i < arr.length; i++) {
                        const d = arr[i];
                        var obj = {
                            'Ol_ID': d.Ol_ID,
                            'Ol_PID': pid,
                            'Ol_Tax': i + 1,
                            'Ol_Level': level
                        }
                        list.push(obj);
                        if (d.children && d.children.length > 0) {
                            list = toarray(d.children, d.Ol_ID, ++level, list);
                        }
                    }
                    return list;
                }
            },
            remove: function (node, data) {
                console.log(node);
                console.log(data);
                if (!!data.children && data.children.length > 0) {
                    var msg = '当前章节“' + data.Ol_Name + '”下共有 <b>' + data.children.length + '</b> 个子章节，请先删除子章节。'
                    this.$alert(msg, '不可删除', {
                        confirmButtonText: '确定',
                        dangerouslyUseHTMLString: true,
                        callback: () => { }
                    });
                    return;
                }
                var th = this;
                th.loading_sumbit = true;
                $api.delete('Outline/Delete', { 'id': data.Ol_ID }).then(function (req) {
                    th.loading_sumbit = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        vapp.$message({
                            type: 'success',
                            message: '删除成功!',
                            center: true
                        });
                        th.getTreeData();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //新增章节的按钮事件
            addBtn: function () {
                this.modify_obj = {
                    Ol_IsUse: true
                };
                this.modify_show = true;
            },
            //计算序号
            calcSerial: function (outline, lvl) {
                var childarr = outline == null ? this.datas : (outline.children ? outline.children : null);
                if (childarr == null) return;
                for (let i = 0; i < childarr.length; i++) {
                    childarr[i].serial = lvl + (i + 1) + '.';
                    this.total++;
                    this.calcSerial(childarr[i], childarr[i].serial);
                }
            },
            //编辑当前章节
            btnModify: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        var obj = th.modify_obj;
                        obj['Cou_ID'] = th.id;
                        var apipath = 'Outline/' + (th.modify_obj.Ol_ID ? 'Modify' : 'Add');
                        $api.post(apipath, { 'entity': obj }).then(function (req) {
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                                th.getTreeData();
                                th.modify_show = false;
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            th.$alert(err, '错误');
                        });
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            //视频编辑的按钮事件
            btnVideoModify: function (outline) {
                var pagebox = window.top.$pagebox;
                if (pagebox == null) {
                    this.$message.error({
                        message: '没有$pagebox对象，无法打开编辑窗！',
                        center: true
                    });
                    return;
                }
                // 要打开的页面
                var root = String(window.document.location.href);
                var file = root.substring(0, root.lastIndexOf("/") + 1) + 'video';
                //标题
                var title = '视频管理 - ' + (outline ? outline.Ol_Name : '');
                window.top.$pagebox.create({
                    'url': $api.url.set(file,
                        {
                            'couid': this.course.Cou_ID,
                            'olid': outline.Ol_ID,
                            'uid': outline.Ol_UID
                        }),
                    'title': title, 'ico': 'e83a',
                    'width': '80%', 'height': '80%',
                    'min': false, 'full': false, 'showmask': true
                }).open();
            }
        }
    });
}, ["/Utilities/editor/vue-html5-editor.js",
    'Components/accessory.js',           //章节附件
    'Components/outline_live.js'                //章节直播的设置
]);