//搜索框
Vue.component('search_input', {
    props: [],
    data: function () {
        return {
            search: $api.querystring("s"),       //搜索字符
            colid: 0        //栏目id           
        }
    },
    watch: {},
    computed: {},
    mounted: function () {

    },
    methods: {
        onSearch: function () {
            var file = $dom.file().toLowerCase();
            if (file != 'search' && this.search == '') return;
            var url = "/mobi/News/search?s=" + encodeURIComponent(this.search);
            window.location.href = url;
        },
    },
    template: ` <van-search v-model.trim="search" placeholder="请输入搜索关键词" background="transparent" @search="onSearch">
        <template #action>

        </template>
        <template #right-icon>
            <div @click="onSearch">搜索</div>
        </template>
        <template #left-icon>

        </template>
    </van-search>`
});

