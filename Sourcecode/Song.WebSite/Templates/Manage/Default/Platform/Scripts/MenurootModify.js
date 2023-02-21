﻿
$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            loading: false,  //
            id: $api.querystring('id'),
            entity: {}, //当前数据对象
            rules: {
                MM_Name: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ],
                MM_Marker: [
                    { required: true, message: '不得为空', trigger: 'blur' }
                ]
            }
        },
        created: function () {
            //如果是新增界面
            if (this.id == '') {
                this.entity.MM_IsShow = true;
                this.entity.MM_IsUse = true;
            } else {
                //如果是修改界面
                var th = this;
                $api.get('ManageMenu/ForID', { 'id': th.id }).then(function (req) {
                    if (req.data.success) {
                        th.entity = req.data.result;
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                });
            }          
        },
        methods: {
            btnEnter: function (formName) {
                var th = this;
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        th.id == '' ? th.add() : th.modify();
                    }
                });
            },
            add: function () {
                if (this.loading) return;
                this.loading = true;
                var th = this;
                $api.post('ManageMenu/AddFuncRoot', { 'mm': th.entity }).then(function (req) {
                    if (req.data.success) {
                        th.$message({
                            type: 'success',
                            message: '添加成功!',
                            center: true
                        });
                        th.operateSuccess();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }                  
                }).catch(function (err) {
                    alert(err);
                }).finally(function () {
                    th.loading = false;
                });
            },
            modify: function () {
                if (this.loading) return;
                this.loading = true;
                var th = this;
                $api.post('ManageMenu/ModifyFuncRoot', { 'mm': th.entity }).then(function (req) {
                    if (req.data.success) {
                        th.$message({
                            type: 'success',
                            message: '修改成功!',
                            center: true
                        });
                        th.operateSuccess();
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }                  
                }).catch(function (err) {
                    alert(err, '错误');
                }).finally(function () {
                    th.loading = false;
                });
            },
            //操作成功
            operateSuccess: function () {
                window.top.$pagebox.source.tab(window.name, 'vue.loadDatas', true);
            }
        },
    });

});
