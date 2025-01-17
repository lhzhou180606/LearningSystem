﻿$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            couid: $api.querystring('id'),
            organ: {},
            config: {},      //当前机构配置项    
            types: [],        //试题类型，来自web.config中配置项

            outlines: [],     //所有章节数据
            defaultProps: {
                children: 'children',
                label: 'Ol_Name',
                value: 'Ol_ID',
                expandTrigger: 'hover',
                checkStrictly: true
            },
            olSelects: [],      //选择中的章节项

            form: {
                'orgid': -1, 'sbjid': -1, 'couid': '', 'olid': '',
                'type': '', 'use': '', 'error': '', 'wrong': '', 'search': '', 'size': 20, 'index': 1
            },
            querybox: false,     //更多查询的面板是否显示
            datas: [],
            total: 1, //总记录数
            totalpages: 1, //总页数
            selects: [], //数据表中选中的行

            loading: true,
            loadingid: 0,
            loading_init: true
        },
        updated: function () {
            this.$mathjax();
        },
        mounted: function () {
            var th = this;
            th.form.couid = this.couid;
            th.loading_init = true;
            $api.bat(
                $api.get('Organization/Current'),
                $api.cache('Question/Types:99999')
            ).then(axios.spread(function (organ, types) {
                th.loading_init = false;
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.exception);
                        throw arguments[i].config.way + ' ' + data.message;
                    }
                }
                //获取结果
                th.organ = organ.data.result;
                th.form.orgid = th.organ.Org_ID;
                th.config = $api.organ(th.organ).config;
                th.types = types.data.result;
                //获取章节
                th.getOutlineTree();
                th.handleCurrentChange(1);
            })).catch(function (err) {
                th.loading_init = false;
                Vue.prototype.$alert(err);
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {
        },
        watch: {
            'olSelects': function (nv, ov) {
                console.log(nv);
                if (nv.length > 0) this.form.olid = nv[nv.length - 1];
                else
                    this.form.olid = '';
                //关闭级联菜单的浮动层
                this.$refs["outlines"].dropDownVisible = false;
            }
        },
        methods: {
            //获取课程章节的数据
            getOutlineTree: function () {
                var th = this;
                th.loading = true;
                $api.put('Outline/Tree', { 'couid': th.couid, 'isuse': true }).then(function (req) {
                    if (req.data.success) {
                        th.outlines = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    console.error(err);
                }).finally(function () {
                    th.loading = false;
                });;
            },
            //加载数据页
            handleCurrentChange: function (index) {
                if (index != null) this.form.index = index;
                var th = this;
                //每页多少条，通过界面高度自动计算
                var area = document.documentElement.clientHeight - 100;
                th.form.size = Math.floor(area / 42);
                th.loading = true;
                var loading = this.$fulloading();
                $api.put("Question/Pager", th.form).then(function (d) {
                    th.loading = false;
                    if (d.data.success) {
                        var result = d.data.result;
                        for (var i = 0; i < result.length; i++) {
                            result[i].Qus_Title = result[i].Qus_Title.replace(/(<([^>]+)>)/ig, "");
                        }
                        th.datas = result;
                        th.$nextTick(function () {
                            loading.close();
                        });
                        th.totalpages = Number(d.data.totalpages);
                        th.total = d.data.total;
                    } else {
                        throw d.data.message;
                    }
                }).catch(function (err) {
                    th.$alert(err, '错误');
                    th.loading = false;
                    console.error(err);
                });
            },
            //删除
            deleteData: function (datas) {
                var th = this;
                th.loading = true;
                var loading = this.$fulloading();
                $api.delete('Question/Delete', { 'id': datas }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        vapp.$notify({
                            type: 'success',
                            message: '成功删除' + result + '条数据',
                            center: true
                        });
                        th.handleCurrentChange();
                        th.$nextTick(function () {
                            loading.close();
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.loading = false;
                    th.$alert(err, '错误');
                    console.error(err);
                });
            },
            //导出
            output: function (btn) {
                var title = btn.tips;
                var url = "../Question/Export";
                url = $api.url.set(url, { 'couid': this.couid });
                this.$refs.btngroup.pagebox(url, title, null, 800, 600, { 'ico': 'e73e' });
            },
            //导入
            input: function (btn) {
                var title = btn.tips;
                var url = "../Question/Import";
                url = $api.url.set(url, { 'couid': this.couid });
                this.$refs.btngroup.pagebox(url, title, null, 900, 650, { 'ico': 'e69f' });
            },
            //更改使用状态
            changeState: function (row) {
                var th = this;
                this.loadingid = row.Qus_ID;
                $api.post('Question/ChangeUse', { 'id': row.Qus_ID, 'use': row.Qus_IsUse }).then(function (req) {
                    this.loadingid = -1;
                    if (req.data.success) {
                        vapp.$notify({
                            type: 'success',
                            message: '修改状态成功!',
                            center: true
                        });
                    } else {
                        throw req.data.message;
                    }
                    th.loadingid = 0;
                }).catch(function (err) {
                    vapp.$alert(err, '错误');
                    th.loadingid = 0;
                });
            },
            //批量修改状态
            batchState: function (use) {
                use = Boolean(use);
                var th = this;
                this.$confirm('批量更改当前页面的试题为“' + (use ? '启用' : '禁用') + '”, 是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    var ids = '';
                    for (var i = 0; i < th.datas.length; i++) {
                        ids += th.datas[i].Qus_ID;
                        if (i < th.datas.length - 1) ids += ',';
                    }
                    var loading = this.$fulloading();
                    $api.post('Question/ChangeUse', { 'id': ids, 'use': use }).then(function (req) {
                        if (req.data.success) {
                            th.$notify({
                                type: 'success',
                                message: '修改状态成功!',
                                center: true
                            });
                            th.handleCurrentChange();
                            th.$nextTick(function () {
                                loading.close();
                            });
                        } else {
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        th.$alert(err, '错误');
                    });
                }).catch(() => {

                });
            }
        }
    });
    //显示章节名称
    Vue.component('outline_name', {
        props: ["olid"],
        data: function () {
            return {
                oultine: {},
                loading: true
            }
        },
        watch: {
            'olid': {
                handler: function (nv) {
                    var th = this;
                    th.loading = true;
                    $api.cache('Outline/ForID', { 'id': nv }).then(function (req) {
                        th.loading = false;
                        if (req.data.success) {
                            th.oultine = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(function (err) {
                        th.loading = false;
                        console.error(err);
                    });
                }, immediate: true
            }
        },
        computed: {},
        mounted: function () { },
        methods: {

        },
        template: `<span>
        <loading v-if="loading"></loading>
        <template v-else>{{oultine.Ol_Name}}</template>
        </span>`
    });
    //章节下的试题数
    Vue.component('outline_ques_count', {
        props: ["outline"],
        data: function () {
            return {
                count: 0,
                loading: true
            }
        },
        watch: {
            'outline': {
                handler: function (nv) {
                    if (nv == null) return;
                    var th = this;
                    th.loading = true;
                    $api.get('Question/Count', { 'orgid': '', 'sbjid': '', 'couid': '', 'olid': nv.Ol_ID, 'type': '', 'use': '' })
                        .then(function (req) {
                            th.loading = false;
                            if (req.data.success) {
                                th.count = req.data.result;
                            } else {
                                console.error(req.data.exception);
                                throw req.config.way + ' ' + req.data.message;
                            }
                        }).catch(function (err) {
                            th.loading = false;
                            console.error(err);
                        });
                }, immediate: true
            }
        },
        computed: {},
        mounted: function () { },
        methods: {

        },
        template: `<span class="ques_count">
        <loading v-if="loading"></loading>
        <template v-else-if="count>0">({{count}})</template>
        </span>`
    });
}, ['../Question/Components/ques_type.js']);
