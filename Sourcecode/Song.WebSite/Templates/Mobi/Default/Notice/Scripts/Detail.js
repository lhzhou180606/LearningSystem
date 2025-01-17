$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项          
            loading: true,
            sear_str: '',

            id: $api.dot(),     //通知公告的id
            data: {}

        },
        mounted: function () {
            var th = this;
            th.loading = true;
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Platform/PlatInfo'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (account, platinfo, organ) {
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
                th.account = account.data.result;
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
            })).catch(function (err) {
                console.error(err);
            });
            //通知公告
            $api.cache('Notice/ForID', { 'id': this.id }).then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    th.data = req.data.result;
                    $api.cache('Notice/ViewNum:60', { 'id': th.id, 'num': 1 }).then(function (req) {
                        if (req.data.success) {
                            th.data.No_ViewNum = req.data.result;
                        } else {
                            console.error(req.data.exception);
                            throw req.config.way + ' ' + req.data.message;
                        }
                    }).catch(err => console.error(err));
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                th.loading = false;
                console.error(err);
            });
        },
        created: function () {
        },
        computed: {
            //是否为空，即通知公告不存在
            isempty: function () {
                return !(JSON.stringify(this.data) != '{}' && this.data != null);
            }
        },
        watch: {
        },
        methods: {
            onSearch: function () {
                var url = '/mobi/Notice/index?search=' + encodeURIComponent(this.sear_str);
                window.location.href = url;
            }
        }
    });

});
