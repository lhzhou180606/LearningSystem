﻿$dom.load.css([$dom.pagepath() + 'Components/Styles/outlinelist.css']);
//章节列表
Vue.component('outlinelist', {
  props: ["outlines", "course", "acid", "isbuy"],
  data: function () {
    return {}
  },
  watch: {},
  computed: {},
  mounted: function () {
   
  },
  methods: {},
  template: `<div v-if="outlines && outlines.length>0" class="outlinelist">
      <outline_row v-for="(o,i) in outlines" :outline="o" :course="course" :acid="acid" :isbuy="isbuy"></outlines>
  </div>`
});

//章节显示的行
Vue.component('outline_row', {
  props: ["outline", "course", "acid", "isbuy"],
  data: function () {
    return {
      state: null,
      count: {},       //练习记录的统计数据
      loading: false
    }
  },
  watch: {
    "outline": {
      deep: false, immediate: true,
      handler: function (nv, ov) {
        //创建试题练习状态的记录的操作对象
        if (this.state == null) {
          this.state = $state.create(this.acid, this.course.Cou_ID, nv.Ol_ID, "Exercises");
          //console.log(this.state);
          this.getprogress();
          this.getques_count();
        }
      }
    },
  },
  computed: {
    //是否初始化过，例如outline是否为空
    init: function () {
      return JSON.stringify(this.outline) != '{}' && this.outline != null;
    },
  },
  mounted: function () { },
  methods: {
    //获取练习进度
    getprogress: function () {
      var th = this;
      var olid = th.outline.Ol_ID;
      //获取练习记录
      this.state.restore().then(function (d) {
        th.outline.count = th.count = d.count;
        th.state = d;
      }).catch(function (d) {
        th.outline.count = th.count = d.count;
        th.state = d;
      }).finally(function () {
        var parent = window.vapp;
        th.state['olid'] = olid;
        parent.statepush(th.state);       
      });
    },
    //获取章节的试题数量
    getques_count: function () {
      var th = this;
      var couid = this.course.Cou_ID;
      var olid = this.outline.Ol_ID;
      $api.get('Question/Count', { 'orgid': -1, 'sbjid': -1, 'couid': couid, 'olid': olid, 'type': '', 'use': true }).then(function (req) {
        if (req.data.success) {
          var result = req.data.result;
          th.outline.Ol_QuesCount = result;
        } else {
          console.error(req.data.exception);
          throw req.config.way + ' ' + req.data.message;
        }
      }).catch(function (err) {
        //alert(err);
        Vue.prototype.$alert(err);
        console.error(err);
      });
    },
    //跳转
    goExercises: function () {
      var outline = this.outline;
      var course = this.course;
      //是否可以练习
      if ((course.Cou_IsTry && outline.Ol_IsFree) || this.isbuy || course.Cou_IsFree || course.Cou_IsLimitFree) {
        var couid = $api.url.get(null, 'couid');
        var uri = $api.url.set('exercises', {
          'path': outline.Ol_XPath,
          'couid': couid,
          'olid': outline.Ol_ID,
        });
        window.location.href = uri;
      } else {
        var link = window.location.href;
        link = link.substring(link.indexOf(window.location.pathname));
        var url = $api.url.set('/mobi/course/buy', {
          'couid': this.couid,
          'olid': this.olid,
          'link': encodeURIComponent(link)
        });
        window.location.href = url;
      }
    },
  },
  //
  template: `<div class="outline_row">
  <van-cell @click="goExercises()">
    <div>
      <span v-html="outline.serial"></span>
      <van-circle :rate="count.rate" v-model="count.rate" size="25px" layer-color="#ebedf0" :stroke-width="60"
        :text="count.rate<100 ? Math.round(count.rate) : '✔'" v-if="count.rate>0"></van-circle>
      <span class="olname" v-html="outline.Ol_Name"></span>
      <van-tag type="danger" v-if="!outline.Ol_IsFinish">未完结</van-tag>
    </div>
    <div class="tag">
      <template v-if="course.Cou_IsTry && outline.Ol_IsFree">
        <van-tag type="success" v-if="outline.Ol_IsVideo">免费
        </van-tag>
      </template>
      <template v-else>
        <template v-if="isbuy || course.Cou_IsFree || course.Cou_IsLimitFree">
          <van-tag plain type="primary">{{count.answer}}/{{outline.Ol_QuesCount}}
          </van-tag>
        </template>
        <template v-else>
          <van-tag type="primary" plain>购买 </van-tag>
        </template>
      </template>
    </div>
  </van-cell>
  <template #left>
    <van-button square type="info" text="正确率">正确率：{{count.rate}}%</van-button>
  </template>
  <template #right>
    <van-button square type="primary" text="答对">答对：{{count.correct}}</van-button>
    <van-button square type="warning" text="答错">答错：{{count.wrong}}</van-button>
  </template>
  <outlinelist ref="outlines" :outlines="outline.children" :course="course" :acid="acid" :isbuy="isbuy"></outlinelist>
</div>`
});