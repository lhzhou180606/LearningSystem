﻿
$ready(function () {

    window.vue = new Vue({
        el: '#app',
        data: {
            loading: false,  //
            id: $api.querystring('id'),
            defaultProps: {
                children: 'children',
                label: 'label'
            },
            datas: [] //数据源          
        },
        created: function () {
            var th = this;
            th.loading = true;
            $api.get('ManageMenu/OrganPurviewSelect').then(function (req) {
                if (req.data.success) {
                    th.datas = req.data.result;
                    $api.get('ManageMenu/OrganPurviewUID', { 'lvid': th.id }).then(function (req) {
                        if (req.data.success) {
                            var arr = req.data.result;
                            for (var i = 0; i < arr.length; i++) {
                                arr[i] = 'node_' + arr[i];
                            }
                            window.setTimeout(function () {
                                var trees = window.vue.$refs.tree;
                                for (var i = 0; i < trees.length; i++) {
                                    trees[i].setCheckedKeys(arr, true);
                                }
                                window.vue.loading = false;
                            }, 100);
                        } else {
                            console.error(req.data.exception);
                            throw req.data.message;
                        }
                    }).catch(function (err) {
                        alert(err);
                        console.error(err);
                    });
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            });
        },
        methods: {
            btnEnter: function () {
                if (this.loading) return;
                this.loading = true;
                //选中的菜单项
                var arr = new Array();
                var trees = this.$refs.tree;
                for (var i = 0; i < trees.length; i++) {
                    var nodes = trees[i].getCheckedNodes(true,false);
                    for (var j = 0; j < nodes.length; j++)
                        arr.push(nodes[j].MM_UID);
                }
                $api.post('ManageMenu/OrganPurviewSelected', { 'lvid': this.id, 'mms': arr }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        vue.$message({
                            type: 'success',
                            message: '操作成功!',
                            center: true
                        });
                        window.vue.loading = false;
                        vue.operateSuccess();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
                //console.log(arr);                          
            },
            //全选或清空
            selected: function (root, index) {
                var arr = new Array();
                if (root.MM_IsBold) {
                    for (var i = 0; i < root.children.length; i++)
                        arr.push(root.children[i].id);
                }
                this.$refs.tree[index].setCheckedKeys(arr);
            },
            //操作成功
            operateSuccess: function () {
                if (window.top && window.top.$pagebox)
                    window.top.$pagebox.source.tab(window.name, 'vue.handleCurrentChange', true);
            }
        },
    });

});