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
            id: $api.querystring('id'),
            entity: {},         //当前实体
            organ: {},
            subjects: [],     //所有专业数据
            defaultProps: {
                children: 'children',
                label: 'Sbj_Name',
                value: 'Sbj_ID',
                expandTrigger: 'hover',
                checkStrictly: true
            },
            sbjSelects: [],      //选择中的专业项
            //图片文件
            upfile: null, //本地上传文件的对象   

            rules: {
                Cou_Tax: [{ required: true, message: '不得为空', trigger: 'blur' }]
            },
            //文本内容
            textual: {
                target: '',
                intro: ''
            },
            loading_obj: {},
            loading: false,
            loading_init: true   //初始加载
        },
        mounted: function () {
            var th = this;
            this.loading_obj = this.$loading({
                lock: true,
                text: 'Loading',
                spinner: 'el-icon-loading',
                background: 'rgba(255, 255, 255, 0.7)'
            });
            $api.get('Organization/Current').then(function (req) {
                if (req.data.success) {
                    th.organ = req.data.result;
                    th.getTreeData();
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {
            //有没有图片
            'islogo': function () {
                var et = this.entity;
                var etlogo = JSON.stringify(et) != '{}' && et != null && et['Cou_Logo'];
                return etlogo || this.upfile != null
            }
        },
        watch: {
            'sbjSelects': function (nv, ov) {
                console.log(nv);
            }
        },
        methods: {
            //获取课程专业的数据
            getTreeData: function () {
                var th = this;
                this.loading = true;
                var form = {
                    orgid: th.organ.Org_ID,
                    search: '', isuse: true
                };
                $api.get('Subject/Tree', form).then(function (req) {
                    if (req.data.success) {
                        th.subjects = req.data.result;
                        th.getEntity();
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            //获取课程实体
            getEntity: function () {
                var th = this;
                if (th.id == '' || th.id == null) return;
                $api.get('Course/ForID', { 'id': th.id }).then(function (req) {
                    th.loading_init = false;
                    th.loading = false;
                    th.loading_obj.close();
                    if (req.data.success) {
                        th.entity = req.data.result;
                        th.textual.target = th.entity.Cou_Target;
                        th.textual.intro = th.entity.Cou_Intro;
                        //将当前课程的专业，在控件中显示
                        var arr = [];
                        arr.push(th.entity.Sbj_ID);
                        var sbj = th.traversalQuery(th.entity.Sbj_ID, th.subjects);
                        if (sbj == null) {
                            throw '课程的专业“' + th.entity.Sbj_Name + '”不存在，或该专业被禁用';
                        }
                        arr = th.getParentPath(sbj, th.subjects, arr);
                        th.sbjSelects = arr;
                    } else {
                        throw '未查询到数据';
                    }
                }).catch(function (err) {
                    th.$alert(err, '错误');
                    console.error(err);
                });
            },
            //获取当前专业的上级路径
            getParentPath: function (entity, datas, arr) {
                if (entity == null) return null;
                var obj = this.traversalQuery(entity.Sbj_PID, datas);
                if (obj == null) return arr;
                arr.splice(0, 0, obj.Sbj_ID);
                arr = this.getParentPath(obj, datas, arr);
                return arr;
            },
            //从树中遍历对象
            traversalQuery: function (sbjid, datas) {
                var obj = null;
                for (let i = 0; i < datas.length; i++) {
                    const d = datas[i];
                    if (d.Sbj_ID == sbjid) {
                        obj = d;
                        break;
                    }
                    if (d.children && d.children.length > 0) {
                        obj = this.traversalQuery(sbjid, d.children);
                        if (obj != null) break;
                    }
                }
                return obj;
            },
            btnEnter: function (formName) {
                var th = this; 
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        var obj = th.clone(th.entity);
                        obj.Cou_Name = '';

                        th.loading = true;
                        //接口参数，如果有上传文件，则增加file
                        var para = {};
                        if (th.upfile == null) para = { 'course': obj };
                        else
                            para = { 'file': th.upfile, 'course': obj };
                        $api.post('Course/Modify', para).then(function (req) {
                            th.loading = false;
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '修改成功!',
                                    center: true
                                });
                                th.callparent();
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
            //为上传数据作处理
            clone: function (entity) {
                var obj = $api.clone(entity);
                //上级专业
                if (this.sbjSelects.length > 0)
                    obj.Sbj_ID = this.sbjSelects[this.sbjSelects.length - 1];
                return obj;
            },
            //调用父级方法
            callparent: function () {
                var win = window.parent;
                if(win && win.vapp){
                    win.vapp.close_fresh('vapp.handleCurrentChange()');
                }               
            }
        }
    });

}, ["/Utilities/editor/vue-html5-editor.js"]);