﻿//试题编辑中的基本信息
Vue.component('general', {
    props: ["question", "organ"],
    data: function () {
        return {
            //专业树形下拉选择器的配置项
            defaultSubjectProps: {
                children: 'children',
                label: 'Sbj_Name',
                value: 'Sbj_ID',
                expandTrigger: 'hover',
                checkStrictly: true
            },
            subjects: [],       //专业
            sbjids: [],

            courses: [],     //课程列表
            course: {},      //当前课程

            outlines: [],     //章节
            defaultOutlinesProps: {
                children: 'children',
                label: 'Ol_Name'
            },
            outlineFilterText: ''
        }
    },
    watch: {
        'question': {
            handler: function (nv, ov) {
                //如果是新增试题，总之难度小于零，默认为3
                if (!nv.Qus_Diff || nv.Qus_Diff <= 0) nv['Qus_Diff'] = 3;
                if (nv.Qus_Diff >= 5) nv.Qus_Diff = 5;
                //默认为启用状态
                if (!nv.Qus_IsUse) nv['Qus_IsUse'] = true;
                if (!nv.Qus_Tax) nv['Qus_Tax'] = 0;
            }, immediate: true
        },
        'organ': {
            handler: function (nv, ov) {
                if (nv) this.getSubjects(nv);
            }, immediate: true
        },
        //章节查询的字符
        outlineFilterText: function (val) {
            this.$refs.tree.filter(val);
        }
    },
    computed: {
        //难度
        'diff': function () {

        }
    },
    mounted: function () {
        $dom.load.css([$dom.path() + 'Question/Components/Styles/general.css']);
    },
    methods: {
        //获取课程专业的数据
        getSubjects: function (organ) {
            if (organ == null || !organ || !organ.Org_ID) return;
            var th = this;
            var form = { orgid: organ.Org_ID, search: '', isuse: null };
            $api.get('Subject/Tree', form).then(function (req) {
                if (req.data.success) {
                    th.subjects = req.data.result;
                    //将当前课程的专业，在控件中显示
                    if (th.question.Sbj_ID && th.question.Sbj_ID > 0) {
                        var arr = [];
                        arr.push(th.question.Sbj_ID);
                        var sbj = th.traversalQuery(th.question.Sbj_ID, th.subjects);
                        if (sbj == null) {
                            throw '课程的专业“' + th.question.Sbj_Name + '”不存在，或该专业被禁用';
                        }
                        arr = th.getParentPath(sbj, th.subjects, arr);
                        th.sbjids = arr;
                    }
                    th.getCourses();
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                console.error(err);
            });
        },
        //专业更改时
        changeSbj: function (val) {
            this.question['Sbj_ID'] = val.length > 0 ? val[val.length - 1] : 0;
            this.getCourses();
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
        //获取课程
        getCourses: function () {
            var th = this;
            var orgid = th.organ.Org_ID;
            var sbjid = 0;
            if (th.sbjids.length > 0) sbjid = th.sbjids[th.sbjids.length - 1];
            th.courses = [];
            $api.cache('Course/Pager', { 'orgid': orgid, 'sbjids': sbjid, 'thid': '', 'search': '', 'order': '', 'size': -1, 'index': 1 }).then(function (req) {
                if (req.data.success) {
                    th.courses = req.data.result;
                    th.getOultines();
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                //alert(err);
                Vue.prototype.$alert(err);
                console.error(err);
            });
        },
        //当试题的课程更改时
        changeCourse: function (val) {
            var th = this;
            this.question['Cou_ID'] = val;
            this.getOultines();
            //如果没有选择专业
            var sbj = this.question['Sbj_ID'];
            console.log(sbj);
            var course = this.courses.find((item) => {
                return item.Cou_ID == val;
            });
            if (course && sbj != course.Sbj_ID) {
                this.question['Sbj_ID'] = course.Sbj_ID;
                this.sbjids = [];
                var arr = [];
                arr.push(course.Sbj_ID);
                var sbj = th.traversalQuery(course.Sbj_ID, th.subjects);
                arr = th.getParentPath(sbj, th.subjects, arr);
                this.sbjids = arr;
                this.getCourses();
            }
        },
        //所取章节数据，为树形数据
        getOultines: function () {
            var th = this;
            this.loading = true;
            var couid = th.question.Cou_ID && th.question.Cou_ID != '' ? th.question.Cou_ID : -1;
            $api.get('Outline/Tree', { 'couid': couid, 'isuse': true }).then(function (req) {
                th.loading = false;
                if (req.data.success) {
                    th.outlines = req.data.result;
                    th.calcSerial(null, '');
                } else {
                    throw req.data.message;
                }
            }).catch(function (err) {
                th.outlines = [];
            });
        },
        //过滤章节树形
        filterNode: function (value, data, node) {
            if (!value) return true;
            var txt = $api.trim(value.toLowerCase());
            if (txt == '') return true;
            return data.Ol_Name.toLowerCase().indexOf(txt) !== -1;
        },
        //计算章节序号
        calcSerial: function (outline, lvl) {
            var childarr = outline == null ? this.outlines : (outline.children ? outline.children : null);
            if (childarr == null) return;
            for (let i = 0; i < childarr.length; i++) {
                childarr[i].serial = lvl + (i + 1) + '.';
                this.calcSerial(childarr[i], childarr[i].serial);
            }
        },
        //章节节点点击事件
        outlineClick: function (data, node, el) {
            this.question.Ol_ID = data.Ol_ID;
            this.question.Ol_Name = data.Ol_Name;
        }
    },
    template: `<div class="general">
        <el-form ref="question" :model="question" @submit.native.prevent label-width="80px">    
            <el-form-item label="难度" prop="Qus_Diff">
                <el-rate  v-model="question.Qus_Diff" :max="5" show-score></el-rate>
            </el-form-item>
            <el-form-item label="排序号" prop="Qus_Tax">
                <el-input-number v-model="question.Qus_Tax"></el-input-number>
                <help>数值越小越靠前，可以为负值</help>
            </el-form-item>
            <el-form-item label="" prop="Qus_IsUse">
                <el-switch  v-model="question.Qus_IsUse"  active-text="启用"  inactive-text="禁用"></el-switch>
            </el-form-item>
            <el-form-item label="专业" prop="Sbj_ID">
                <el-cascader style="width: 50%;" clearable v-model="sbjids" placeholder="请选择课程专业"
                :options="subjects" separator="／" :props="defaultSubjectProps" filterable @change="changeSbj">
                <template slot-scope="{ node, data }">
                    <span>{{ data.Sbj_Name }}</span>
                    <span class="sbj_course" v-if="data.Sbj_CouNumber>0">
                        <icon>&#xe813</icon>{{ data.Sbj_CouNumber }}
                    </span>
                </template>
                </el-cascader>
                <help>可以检索查询</help>
            </el-form-item>
            <el-form-item label="课程" prop="Cou_ID">
                <el-select v-model="question.Cou_ID" @change="changeCourse" value-key="Cou_ID" style="width: 100%;" 
                filterable placeholder="-- 课程 --" clearable :multiple-limit="1">
                    <el-option v-for="(item,i) in courses" :key="item.Cou_ID" :label="item.Cou_Name"
                        :value="item.Cou_ID">
                        <span>{{i+1}} . </span>
                        <span>{{item.Cou_Name}}</span>
                    </el-option>
                </el-select>
            </el-form-item>
            <el-form-item label="章节" prop="Ol_ID" class="tree-area">               
                <div class="outline_bar">
                    <el-input v-model="outlineFilterText" clearable style="width:160px" placeholder="搜索"
                                suffix-icon="el-icon-search"></el-input>
                    <span v-if="question.Ol_Name!=''">{{question.Ol_Name}}</span>
                    <span v-else class="noselect">未选择章节</span>
                </div>
                <el-tree :data="outlines" node-key="Ol_ID" ref="tree" :props="defaultOutlinesProps" expand-on-click-node
                    empty-text="没有供选择的章节" :filter-node-method="filterNode" :expand-on-click-node="false" @node-click="outlineClick"
                    default-expand-all>
                    <span :class="{'outline-tree-node':true,'selected':data.Ol_ID==question.Ol_ID}" slot-scope="{ node, data }">
                        <span>{{data.serial}}</span> {{ data.Ol_Name }} 
                    </span>
                </el-tree>                
            </el-form-item>
        </el-form>
    </div> `
});