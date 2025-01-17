$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            organ: {},
            config: {},      //当前机构配置项        

            tabs: [
                { name: '登录/注册', tag: 'register', icon: '&#xe687', size: 19 },
                { name: '课程学习', tag: 'study', icon: '&#xe813', size: 16 },
                { name: '终端', tag: 'device', icon: '&#xa021', size: 14 }
            ],
            activeName: 'study',      //选项卡

            //图片文件
            upfile: null, //本地上传文件的对象   
            error: '',       //错误信息
            rules: {
            },

            loading: false,
            loading_init: true
        },
        mounted: function () {
            $api.bat(
                $api.get('Organization/Current')
            ).then(axios.spread(function (organ) {
                vapp.loading_init = false;
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.message);
                    }
                }
                //获取结果             
                vapp.organ = organ.data.result;
                //机构配置信息
                vapp.config = $api.organ(vapp.organ).config;
            })).catch(function (err) {
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {
         
        },
        watch: {
        },
        methods: {
            btnEnter: function (formName) {
                var th = this;
                if (!th.verification()) return;
                this.$refs[formName].validate((valid, obj) => {
                    if (valid) {
                        th.error = '';
                        th.loading = true;
                        var apipath = 'Organization/ConfigUpdate';
                        //接口参数，如果有上传文件，则增加file
                        var para = {
                            "orgid": th.organ.Org_ID,
                            'config': th.config
                        };
                        if (th.upfile != null) para['file'] = th.upfile;
                        $api.post(apipath, para).then(function (req) {
                            th.loading = false;
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$message({
                                    type: 'success',
                                    message: '操作成功!',
                                    center: true
                                });
                            } else {
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            //window.top.ELEMENT.MessageBox(err, '错误');
                            th.$alert(err, '错误');
                        });
                    } else {
                        console.log('error submit!!');
                        for (var t in obj) {
                            th.error = obj[t][0].message;
                            break;
                        }
                        window.setTimeout(function () {
                            th.error = '';
                        }, 5000);
                        return false;
                    }
                });
            },
            //校验录入
            verification: function () {
                //结课成绩的权重
                var finaltest_weight_video = !!this.config.finaltest_weight_video ? this.config.finaltest_weight_video : 0;
                var finaltest_weight_ques = !!this.config.finaltest_weight_ques ? this.config.finaltest_weight_ques : 0;
                var finaltest_weight_exam = !!this.config.finaltest_weight_exam ? this.config.finaltest_weight_exam : 0;
                var finaltest_score = finaltest_weight_video + finaltest_weight_ques + finaltest_weight_exam;
                if (finaltest_score != 100) {
                    this.$alert('结课成绩的各项权重必须合计等于100%', '录入错误', {
                        confirmButtonText: '确定',
                        type:'error',
                        callback: action => {

                        }
                    });
                    return false;
                }
                return true;
            }
        }
    });

});
