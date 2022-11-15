Vue.component('tabs', {
    props: ['tabindex', 'disabled'],
    data: function () {
        return {
            //选项卡
            tabs: [
                { 'label': '资源图片', 'name': 'region', 'show': true, 'icon': 'e672', 'size': 20 },
                { 'label': '上传本地图片', 'name': 'upload', 'show': true, 'icon': 'e84c', 'size': 16 },
                { 'label': '外网图片', 'name': 'outside', 'show': true, 'icon': 'a03b', 'size': 17 }

            ],
            //当前选项卡
            activeName: 'region',

            loading: false,
            loading_init: false
        }
    },
    watch: {
    },
    computed: {

    },
    mounted: function () {

    },
    methods: {
        tabClick: function (item) {
            if (this.disabled) return;
            this.activeName = item.name;
        }
    },
    template: `<div id="image-weisha" class="tabs-weisha">
        <div class="tabs">
            <div v-for="(item,i) in tabs" :disabled="disabled" @click="tabClick(item)" :current="activeName==item.name">
                <icon v-html="'&#x'+item.icon" :style="'font-size:'+item.size+'px'"></icon>{{item.label}}
            </div>
        </div>
        <div class="content">
            <div v-for="(item,i) in tabs" v-show="activeName==item.name" :class="item.name">
                <slot :name="item.name"> </slot>
            </div>
        </div>
    </div>
`
});