﻿
//顶部导航
Vue.component('page_header', {
    props: ['title', 'icon', 'fresh', 'home'],
    data: function () {
        return {

        }
    },
    watch: {
        'title': {
            handler(nv, ov) {
                document.title = nv;
            },
            immediate: true
        }
    },
    computed: {
        //是否登录
        islogin: function () {
            return JSON.stringify(this.account) != '{}' && this.account != null;
        }
    },
    mounted: function () {
        $dom.load.css([$dom.path() + 'Components/Styles/page_header.css']);
    },
    methods: {
        //刷新页面
        btnFresh: function () {
            window.location.reload();
        },
        //返回上一页
        btnback: function () {
            window.history.go(-1);
        },
        //返回主页
        btnHome: function () {
            window.location.href="/mobi/";
        }
    },
    // 同样也可以在 vm 实例中像 "this.message" 这样使用
    template: `<div  class="page_header">
           <icon class="goback" @click="btnback">&#xe72a</icon>         
           <div class="header_title">
                <icon v-html="icon" v-if="icon"></icon>   
                <span v-html="title"></span>  
           </div>
           <icon class="fresh" @click="btnFresh" v-if="fresh">&#xe694</icon>     
           <icon class="home" @click="btnHome" v-if="home">&#xa020</icon>     
        </div>`
});
