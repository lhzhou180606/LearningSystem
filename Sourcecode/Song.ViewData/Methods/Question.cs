﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiSha.Core;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using WeiSha.Data;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 试题
    /// </summary>
    [HttpGet]
    public class Question : ViewMethod, IViewAPI
    {

        /// <summary>
        /// 题型
        /// </summary>
        /// <returns></returns>
        [Cache(Expires = int.MaxValue)]
        public string[] Types()
        {
            return WeiSha.Core.App.Get["QuesType"].Split(',');
        }
        #region 试题编辑
        /// <summary>
        /// 删除试题
        /// </summary>
        /// <param name="id">试题id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin, Teacher]
        [HttpDelete]
        public int Delete(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            string[] arr = id.Split(',');
            foreach (string s in arr)
            {
                int idval = 0;
                int.TryParse(s, out idval);
                if (idval == 0) continue;
                try
                {
                    Business.Do<IQuestions>().QuesDelete(idval);
                    i++;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return i;
        }
        /// <summary>
        /// 添加试题
        /// </summary>
        /// <param name="entity">试题</param>
        /// <returns></returns>
        [Admin, Teacher]
        [HttpPost]
        [HtmlClear(Not = "entity")]
        public int Add(Song.Entities.Questions entity)
        {
            //处理单选、多选的选项
            if (entity.Qus_Type == 1 || entity.Qus_Type == 2 || entity.Qus_Type == 5)
            {
                entity.Qus_Items = Business.Do<IQuestions>().AnswerToItems(_answerToItems(entity));
            }
            Business.Do<IQuestions>().QuesAdd(entity);
            return entity.Qus_ID;
        }
        /// <summary>
        /// 修改试题
        /// </summary>
        /// <param name="entity">修改试题</param>
        /// <returns></returns>
        [Admin, Teacher]
        [HttpPost]
        [HtmlClear(Not = "entity")]
        public bool Modify(Song.Entities.Questions entity)
        {
            Song.Entities.Questions old = Business.Do<IQuestions>().QuesSingle(entity.Qus_ID);
            if (old == null) throw new Exception("Not found entity for Questions！");

            old.Copy<Song.Entities.Questions>(entity);
            //处理单选、多选的选项
            if (entity.Qus_Type == 1 || entity.Qus_Type == 2 || entity.Qus_Type == 5)
            {
                old.Qus_Items = Business.Do<IQuestions>().AnswerToItems(_answerToItems(entity));
            }
            Business.Do<IQuestions>().QuesSave(old);
            return true;
        }
        /// <summary>
        /// 将试题的答题选项(Json)转换为数组
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private Song.Entities.QuesAnswer[] _answerToItems(Song.Entities.Questions entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Qus_Items)) return null;
            List<Song.Entities.QuesAnswer> items = new List<QuesAnswer>();
            JArray jaryy = JArray.Parse(entity.Qus_Items);
            if (jaryy != null)
            {
                for(int i = 0; i < jaryy.Count; i++)
                {
                    JToken jt = jaryy[i];
                    try
                    {
                        Song.Entities.QuesAnswer obj = ExecuteMethod.GetValueToEntity<Song.Entities.QuesAnswer>(null, jt.ToString());
                        if (string.IsNullOrWhiteSpace(obj.Ans_Context)) continue;                     
                        //生成答案项的id
                        if (obj.Ans_ID <= 0)
                        {
                            obj.Ans_ID = new Random((i + 1) + DateTime.Now.Millisecond).Next(1, 1000);
                        }
                        //填空题，每项都是正确的
                        if (entity.Qus_Type == 5) obj.Ans_IsCorrect = true;
                        items.Add(obj);
                    }
                    catch { }
                }
            }
            return items.ToArray();
        }
        /// <summary>
        /// 修改使用状态
        /// </summary>
        /// <param name="id">试题id，可以是多个，用逗号分隔</param>
        /// <param name="use"></param>
        /// <returns></returns>
        [Admin, Teacher]
        [HttpPost]
        public int ChangeUse(string id, bool use)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            string[] arr = id.Split(',');
            foreach (string s in arr)
            {
                int idval = 0;
                int.TryParse(s, out idval);
                if (idval == 0) continue;
                try
                {
                    Business.Do<IQuestions>().QuesUpdate(idval,
                    new WeiSha.Data.Field[] { Song.Entities.Questions._.Qus_IsUse },
                    new object[] { use });
                    i++;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return i;
        }
        #endregion

        #region 查询
        /// <summary>
        /// 计算有多少条试题
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="couid">课程id</param>
        /// <param name="olid">章节id</param>
        /// <param name="type">试题类型</param>
        /// <param name="use">启用中的试题，null取所有</param>
        /// <returns></returns>
        public int Count(int orgid, int sbjid, int couid, int olid, int type, bool? use)
        {
            return Business.Do<IQuestions>().QuesOfCount(orgid, sbjid, couid, olid, type, use);
        }

        /// <summary>
        /// 获取试题
        /// </summary>
        /// <param name="id">试题id</param>
        /// <returns></returns>
        [Cache]
        [HttpPut]
        public Song.Entities.Questions ForID(int id)
        {
            Song.Entities.Questions ques = Business.Do<IQuestions>().QuesSingle(id);
            if (ques == null) return null;
            return _tran(ques.Clone<Song.Entities.Questions>());
        }
        /// <summary>
        /// 分页获取试题
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid"></param>
        /// <param name="couid"></param>
        /// <param name="type">试题类型</param>
        /// <param name="use">是否启用</param>
        /// <param name="error">是否有错误</param>
        /// <param name="wrong">是否有学员反馈错误</param>      
        /// <param name="search">检索字符</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult Pager(int orgid, int sbjid, int couid, int type, bool? use, bool? error, bool? wrong, string search, int size, int index)
        {
            if (orgid <= 0)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                orgid = org.Org_ID;
            }
            //总记录数
            int count = 0;
            Song.Entities.Questions[] ques = Business.Do<IQuestions>().QuesPager
                (orgid, type, sbjid, couid, -1, use, error, wrong, -1, search, size, index, out count);
            ListResult result = new ListResult(ques);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 按课程或章节输出试题
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="olid">章节id</param>
        /// <param name="type">试题的题型分类</param>
        /// <param name="count"></param>
        /// <returns></returns>
        [Study]
        public ListResult ForCourse(int couid, int olid, int type, int count)
        {
            if (couid == 0 && olid == 0) return null;
            int total = Business.Do<IQuestions>().QuesOfCount(-1, -1, couid, olid, type, true);
            Song.Entities.Questions[] ques = Business.Do<IQuestions>().QuesCount(-1, -1, couid, olid, type, -1, true, 0 - 1, count);
            for (int i = 0; i < ques.Length; i++)
            {
                ques[i] = _tran(ques[i]);
            }
            ListResult result = new ListResult(ques);
            result.Index = 1;
            result.Size = count;
            result.Total = total;
            return result;
        }

        #endregion

        #region 处理试题内容
        public static Song.Entities.Questions _tran(Song.Entities.Questions ques)
        {
            if (ques == null) return ques;
            //ques = Extend.Questions.TranText(ques);
            if (!string.IsNullOrWhiteSpace(ques.Qus_Title))
            {
                ques.Qus_Title = ques.Qus_Title.Replace("&lt;", "<");
                ques.Qus_Title = ques.Qus_Title.Replace("&gt;", ">");
                ques.Qus_Title = ques.Qus_Title.Replace("\n", "<br/>");
                ques.Qus_Title = Html.ClearScript(ques.Qus_Title);
                //ques.Qus_Title = Html.ClearHTML(ques.Qus_Title, "p", "div", "font");
                ques.Qus_Title = Html.ClearAttr(ques.Qus_Title, "p", "div", "font", "span", "a");
                ques.Qus_Title = TransformImagePath(ques.Qus_Title);
            }

            if (!string.IsNullOrWhiteSpace(ques.Qus_Answer))
            {
                ques.Qus_Answer = ques.Qus_Answer.Replace("&lt;", "<");
                ques.Qus_Answer = ques.Qus_Answer.Replace("&gt;", ">");
                ques.Qus_Answer = ques.Qus_Answer.Replace("\n", "<br/>");
                ques.Qus_Answer = Html.ClearScript(ques.Qus_Answer);
                ques.Qus_Answer = Html.ClearAttr(ques.Qus_Answer, "p", "div", "font", "span", "a");
                ques.Qus_Answer = TransformImagePath(ques.Qus_Answer);
                ques.Qus_Answer = ques.Qus_Answer.Replace("&nbsp;", " ");
            }
            if (!string.IsNullOrWhiteSpace(ques.Qus_Explain))
            {
                ques.Qus_Explain = ques.Qus_Explain.Replace("&lt;", "<");
                ques.Qus_Explain = ques.Qus_Explain.Replace("&gt;", ">");
                ques.Qus_Explain = ques.Qus_Explain.Replace("\n", "<br/>");
                ques.Qus_Explain = Html.ClearScript(ques.Qus_Explain);
                ques.Qus_Explain = Html.ClearAttr(ques.Qus_Explain, "p", "div", "font", "span", "a");
                ques.Qus_Explain = TransformImagePath(ques.Qus_Explain);
                ques.Qus_Explain = ques.Qus_Explain.Replace("&nbsp;", " ");
            }
            return ques;
        }

        /// <summary>
        /// 处理试题中的图片
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string TransformImagePath(string text)
        {
            RegexOptions options = RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase;
            //将超链接处理为相对于模版页的路径
            string linkExpr = @"<(img)[^>]+>";
            foreach (Match match in new Regex(linkExpr, options).Matches(text))
            {
                string tagName = match.Groups[1].Value.Trim();      //标签名称
                string tagContent = match.Groups[0].Value.Trim();   //标签内容
                string expr = @"(?<=\s+)(?<key>src[^=""']*)=([""'])?(?<value>[^'"">]*)\1?";
                foreach (Match m in new Regex(expr, options).Matches(tagContent))
                {
                    string key = m.Groups["key"].Value.Trim();      //属性名称
                    string val = m.Groups["value"].Value.Trim();    //属性值      
                    val = val.Replace("&apos;", "");
                    if (val.EndsWith("/")) val = val.Substring(0, val.Length - 1);
                    val = m.Groups[2].Value + "=\"" + val + "\"";
                    val = Regex.Replace(val, @"//", "/");

                    tagContent = tagContent.Replace(m.Value, val);
                }
                text = text.Replace(match.Groups[0].Value.Trim(), tagContent);
            }
            return text;
        }
        #endregion

        #region 试题收藏
        /// <summary>
        /// 学员收藏的试题
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="couid">试题id</param>
        /// <param name="type">试题类型</param>
        /// <returns></returns>
        public Song.Entities.Questions[] CollecQues(int acid, int couid, int type)
        {
            Song.Entities.Questions[] ques = Business.Do<IStudent>().CollectCount(acid, 0, couid, type, -1);
            for (int i = 0; i < ques.Length; i++)
            {
                ques[i] = _tran(ques[i]);
            }
            return ques;
        }
        /// <summary>
        /// 添加试题收藏
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="qid">试题id</param>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        [HttpPost]
        public bool CollectAdd(int acid,int qid,int couid)
        {
            try
            {
                Student_Collect stc = Business.Do<IStudent>().CollectSingle(acid, qid);
                if (stc == null)
                {
                    stc = new Entities.Student_Collect();
                    stc.Ac_ID = acid;
                    stc.Qus_ID = qid;
                    stc.Cou_ID = couid;
                    Business.Do<IStudent>().CollectAdd(stc);
                }              
                return true;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 删除收藏
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="qid">试题id</param>
        /// <returns></returns>
        [HttpDelete]
        public bool CollectDelete(int acid,int qid)
        {
            try
            {
                Business.Do<IStudent>().CollectDelete(qid, acid);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 清空试题收藏
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        [HttpDelete]
        public bool CollectClear(int acid, int couid)
        {
            try
            {
                Business.Do<IStudent>().CollectClear(couid, acid);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 是否收藏试题
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="qid">试题id</param>
        /// <returns></returns>
        [HttpGet]
        public bool CollectExist(int acid, int qid)
        {
            try
            {
                Student_Collect sc = Business.Do<IStudent>().CollectSingle(acid, qid);
                return sc != null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 试题笔记
        /// <summary>
        /// 学员记过笔记的题
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="couid">试题id</param>
        /// <param name="type">试题类型</param>
        /// <returns></returns>
        public Song.Entities.Questions[] NotesQues(int acid, int couid, int type)
        {
            Song.Entities.Questions[] ques = Business.Do<IStudent>().NotesCount(acid, couid, type, -1);
            for (int i = 0; i < ques.Length; i++)
            {
                ques[i] = _tran(ques[i]);
            }
            return ques;
        }
        /// <summary>
        /// 编辑试题的笔记
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="qid">试题id</param>
        /// <param name="note">笔记内容</param>
        /// <returns></returns>
        [HttpPost]
        public bool NotesModify(int acid, int qid, string note)
        {
            try
            {
                //如果笔记内容为空，则删除记录
                if (string.IsNullOrWhiteSpace(note))
                {
                    Business.Do<IStudent>().NotesDelete(qid, acid);
                    return false;
                }
                else
                {
                    //如果不为空
                    Song.Entities.Student_Notes sn = Business.Do<IStudent>().NotesSingle(qid, acid);
                    if (sn != null)
                    {
                        sn.Stn_Context = note;
                        Business.Do<IStudent>().NotesSave(sn);
                    }
                    else
                    {
                        sn = new Student_Notes();
                        sn.Stn_Context = note;
                        sn.Qus_ID = qid;
                        sn.Ac_ID = acid;
                        Business.Do<IStudent>().NotesAdd(sn);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 清空试题笔记
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        [HttpDelete]
        public bool NotesClear(int acid, int couid)
        {
            try
            {
                Business.Do<IStudent>().NotesClear(couid, acid);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 试题笔记
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="qid">试题id</param>
        /// <returns></returns>
        [HttpGet]
        public Song.Entities.Student_Notes NotesSingle(int acid, int qid)
        {
            try
            {
                Song.Entities.Student_Notes note = Business.Do<IStudent>().NotesSingle(qid, acid);
                return note;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 试题报错
        /// <summary>
        /// 添加试题错误
        /// </summary>
        /// <param name="qid">试题id</param>
        /// <param name="error">错误信息</param>
        /// <returns>是否为错误试题，true为错误，false为正常</returns>
        [HttpPost]
        [Student]
        public bool WrongModify(int qid, string error)
        {
            try
            {
                Song.Entities.Questions ques = Business.Do<IQuestions>().QuesSingle(qid);
                if (ques == null) return false;
                ques.Qus_WrongInfo = error;
                ques.Qus_IsWrong = !string.IsNullOrWhiteSpace(error);
                Business.Do<IQuestions>().QuesSave(ques);
                return ques.Qus_IsWrong;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        ///// <summary>
        ///// 试题错误信息
        ///// </summary>
        ///// <param name="qid"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public string WrongInfo(int qid)
        //{

        //}
        #endregion

        #region 错题回顾
        /// <summary>
        /// 学员答错题时，记录错题，以便后续复习
        /// </summary>
        /// <param name="acid">学员Id</param>
        /// <param name="qid">试题id</param>
        /// <param name="couid">试题所在课程的id</param>
        /// <returns></returns>
        [HttpPost]
        public bool ErrorAdd(int acid,int qid,int couid)
        {
            //如果未设置学员id，则取当前登录的学员账号id
            if (acid <= 0)
            {
                Song.Entities.Accounts acc = this.User;
                if (acc != null) acid = acc.Ac_ID;
            }
            try
            {
                Song.Entities.Student_Ques stc = new Entities.Student_Ques();
                stc.Ac_ID = acid;
                stc.Qus_ID = qid;
                stc.Cou_ID = couid;
                stc.Squs_CrtTime = DateTime.Now;
                Business.Do<IStudent>().QuesAdd(stc);
                return true;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 学员答错的题，
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="couid">试题id</param>
        /// <param name="type">试题类型</param>
        /// <returns></returns>
        public Song.Entities.Questions[] ErrorQues(int acid,int couid,int type)
        {
            Song.Entities.Questions[] ques = Business.Do<IStudent>().QuesAll(acid, 0, couid, type);
            for (int i = 0; i < ques.Length; i++)
            {
                ques[i] = _tran(ques[i]);
            }
            return ques;
        }
        /// <summary>
        /// 学员答错的题数
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="couid">试题id</param>
        /// <param name="type">试题类型</param>
        /// <returns></returns>
        public int ErrorQuesCount(int acid, int couid, int type)
        {
            return Business.Do<IStudent>().QuesOfCount(acid, 0, couid, type);
        }
        /// <summary>
        /// 学员错题所属的课程，即通过错题获取课程列表
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="course">课程名称，可模糊查询</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult ErrorCourse(int acid,string course, int size, int index)
        {
            int count = 0;
            Song.Entities.Course[] courses = Business.Do<IStudent>().QuesForCourse(acid, course, size, index, out count);
            for (int i = 0; i < courses.Length; i++)
            {
                Song.Entities.Course c = Methods.Course._tran(courses[i]);
                c.Cou_Intro = c.Cou_Target = c.Cou_Content = "";
                c.Cou_Name = c.Cou_Name.Replace("\"", "&quot;");
                courses[i] = c;
            }
            ListResult result = new ListResult(courses);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 高频错题，某个课程下做错最多的试题
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="type">试题类型</param>
        /// <param name="count">取多少条</param>
        /// <returns></returns>
        public Song.Entities.Questions[] ErrorOftenQues(int couid, int type,int count)
        {
            Song.Entities.Questions[] ques = Business.Do<IStudent>().QuesOftenwrong(couid, type, count);
            for (int i = 0; i < ques.Length; i++)
            {
                ques[i] = _tran(ques[i]);
            }
            return ques;
        }
        /// <summary>
        /// 删除答错的题
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="qid">试题id</param>
        /// <returns></returns>
        public bool ErrorDelete(int acid,int qid)
        {
            try
            {
                Business.Do<IStudent>().QuesDelete(qid,acid);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 清空答错的试题，按课程清除
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        [HttpDelete]
        public int ErrorClear(int acid,int couid)
        {
            try
            {
                return Business.Do<IStudent>().QuesClear(couid, acid);               
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
        #endregion

        #region 试题练习记录
        /// <summary>
        /// 记录试题练习记录
        /// </summary>
        /// <param name="acid"></param>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost, Student]
        public bool ExerciseLogSave(int acid, int couid, int olid, JObject json)
        {
            Song.Entities.Accounts acc = this.User;
            if (acc == null) return false;
            if (acid != acc.Ac_ID) return false;
            //统计数据
            int sum, answer, correct, wrong;
            double rate;
            JToken countJo = json["count"];           
            int.TryParse(countJo["sum"].ToString(), out sum);
            int.TryParse(countJo["answer"].ToString(), out answer);
            int.TryParse(countJo["correct"].ToString(), out correct);
            int.TryParse(countJo["wrong"].ToString(), out wrong);
            double.TryParse(countJo["rate"].ToString(), out rate);
            //最后时间           
            JToken currJo = json["current"];
            if (currJo != null)
            {
                if (currJo.HasValues)
                {
                    string time = currJo["time"].ToString();
                    if (time.IndexOf(".") > -1) time = time.Substring(0, time.LastIndexOf("."));
                    long timeTricks;
                    long.TryParse(time, out timeTricks);
                    timeTricks = new DateTime(1970, 1, 1).Ticks + timeTricks * 10000;
                    DateTime last = new DateTime(timeTricks);
                }
            }
            new System.Threading.Tasks.Task(() =>
            {
                Business.Do<IQuestions>().ExerciseLogSave(acc, -1, couid, olid, json.ToString(), sum, answer, correct, wrong, rate);
            }).Start();
           
            return true;
        }
        /// <summary>
        /// 获取试题练习记录
        /// </summary>
        /// <param name="acid"></param>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <returns></returns>
        public LogForStudentExercise ExerciseLogGet(int acid, int couid, int olid)
        {
            return Business.Do<IQuestions>().ExerciseLogGet(acid, couid, olid);
        }
        /// <summary>
        /// 记录试题练通过率，记录到学员购买课程的记录上
        /// </summary>
        /// <param name="acid"></param>
        /// <param name="couid"></param>
        /// <param name="rate">试题练习的通过率</param>
        /// <returns></returns>
        [Student]
        public bool ExerciseLogRecord(int acid, int couid, double rate)
        {
            Song.Entities.Accounts acc = this.User;
            if (acc.Ac_ID != acid) return false;

            Student_Course sc = Business.Do<ICourse>().StudentCourse(acid, couid);
            if (sc == null) return false;

            if (sc.Stc_QuesScore != rate)
            {
                sc.Stc_QuesScore = rate;
                Business.Do<ICourse>().StudentScoreSave(sc, -1, rate, -1);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 删除试题练习记录
        /// </summary>
        /// <param name="acid"></param>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <returns></returns>
        [HttpDelete,HttpGet(Ignore =true)]
        public bool ExerciseLogDel(int acid, int couid, int olid)
        {
            if (acid <= 0 || couid <= 0 || olid <= 0) return false;
            Business.Do<IQuestions>().ExerciseLogDel(acid, couid, olid);
            return true;
        }
        #endregion
    }
}