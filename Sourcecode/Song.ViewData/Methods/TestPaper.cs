﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using Newtonsoft.Json.Linq;
//using System.Web.Mvc;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;


namespace Song.ViewData.Methods
{

    /// <summary>
    /// 试卷管理
    /// </summary>
    [HttpGet]
    public class TestPaper : ViewMethod, IViewAPI
    {
        //资源的虚拟路径和物理路径
        public static string PathKey = "TestPaper";
        public static string VirPath = WeiSha.Core.Upload.Get[PathKey].Virtual;
        public static string PhyPath = WeiSha.Core.Upload.Get[PathKey].Physics;
        #region 增删改查
        /// <summary>
        /// 获取试卷信息
        /// </summary>
        /// <param name="id">试卷id</param>
        /// <returns></returns>
        public Song.Entities.TestPaper ForID(int id)
        {
            Song.Entities.TestPaper tp = Business.Do<ITestPaper>().PaperSingle(id);
            tp.Tp_Logo = System.IO.File.Exists(PhyPath + tp.Tp_Logo) ? VirPath + tp.Tp_Logo : "";
            return tp;
        }
        ///<summary>
        /// 创建试卷
        /// </summary>
        /// <param name="entity">试卷对象</param>
        /// <returns></returns>
        [Admin]
        [HttpPost, HttpGet(Ignore = true)]
        [Upload(Extension = "jpg,png,gif", MaxSize = 1024, CannotEmpty = false)]
        [HtmlClear(Not = "entity")]
        public Song.Entities.TestPaper Add(Song.Entities.TestPaper entity)
        {
            try
            {
                string filename = string.Empty, smallfile = string.Empty;
                try
                {
                    //只保存第一张图片
                    foreach (string key in this.Files)
                    {
                        HttpPostedFileBase file = this.Files[key];
                        filename = WeiSha.Core.Request.UniqueID() + Path.GetExtension(file.FileName);
                        file.SaveAs(PhyPath + filename);                      
                        break;
                    }                 
                    entity.Tp_Logo = filename;              

                    Business.Do<ITestPaper>().PaperAdd(entity);
                    return entity;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 修改试卷信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Admin,Teacher]
        [HttpPost, HttpGet(Ignore = true)]
        [Upload(Extension = "jpg,png,gif", MaxSize = 1024, CannotEmpty = false)]
        [HtmlClear(Not = "entity")]
        public Song.Entities.TestPaper Modify(Song.Entities.TestPaper entity)
        {
            try
            {
                string filename = string.Empty, smallfile = string.Empty;
                try
                {
                    Song.Entities.TestPaper old = Business.Do<ITestPaper>().PaperSingle(entity.Tp_Id);
                    if (old == null) throw new Exception("Not found entity for TestPaper！");
                    //如果有上传文件
                    if (this.Files.Count > 0)
                    {
                        //只保存第一张图片
                        foreach (string key in this.Files)
                        {
                            HttpPostedFileBase file = this.Files[key];
                            filename = WeiSha.Core.Request.UniqueID() + Path.GetExtension(file.FileName);
                            file.SaveAs(PhyPath + filename);                         
                            break;
                        }
                        entity.Tp_Logo = filename;
                        if (!string.IsNullOrWhiteSpace(old.Tp_Logo))
                            WeiSha.Core.Upload.Get[PathKey].DeleteFile(old.Tp_Logo);
                    }
                    //如果没有上传图片，且新对象没有图片，则删除旧图
                    else if (string.IsNullOrWhiteSpace(entity.Tp_Logo))
                    {
                        WeiSha.Core.Upload.Get[PathKey].DeleteFile(old.Tp_Logo);
                    }

                    old.Copy<Song.Entities.TestPaper>(entity);
                    Business.Do<ITestPaper>().PaperSave(old);
                    return old;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 删除试卷
        /// </summary>
        /// <param name="id">试卷id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
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
                    Business.Do<ITestPaper>().PaperDelete(idval);
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
        /// 修改试卷的状态
        /// </summary>
        /// <param name="id">试卷的id，可以是多个，用逗号分隔</param>
        /// <param name="use">是否启用</param>
        /// <param name="rec">是否推荐</param>
        /// <returns></returns>
        [HttpPost]
        [Admin]
        public int ModifyState(string id, bool use, bool? rec)
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
                    if (rec != null)
                    {
                        Business.Do<ITestPaper>().PaperUpdate(idval,
                        new WeiSha.Data.Field[] {
                        Song.Entities.TestPaper._.Tp_IsUse,
                        Song.Entities.TestPaper._.Tp_IsRec },
                        new object[] { use, rec });
                    }
                    else
                    {
                        Business.Do<ITestPaper>().PaperUpdate(idval, new WeiSha.Data.Field[] { Song.Entities.TestPaper._.Tp_IsUse },
                            new object[] { use });
                    }
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
        /// 前端的分页获取试卷
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="search">按名称检索</param>
        /// <param name="diff">难度等级</param>
        /// <param name="size">每页几条</param>
        /// <param name="index">第几页</param>
        /// <returns></returns>
        public ListResult ShowPager(int couid, string search, int diff, int size, int index)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            int orgid = org.Org_ID;
            int count = 0;
            Song.Entities.TestPaper[] tps = null;
            tps = Business.Do<ITestPaper>().PaperPager(orgid, -1, couid, diff, true, search, size, index, out count);
            foreach(Song.Entities.TestPaper tp in tps)
            {
                tp.Tp_Logo = System.IO.File.Exists(PhyPath + tp.Tp_Logo) ? VirPath + tp.Tp_Logo : "";
            }
            ListResult result = new ListResult(tps);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 获取某个课程的结课考试
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="use"></param>
        /// <returns></returns>
        public Song.Entities.TestPaper FinalPaper(int couid, bool? use)
        {
            Song.Entities.TestPaper tp = Business.Do<ITestPaper>().FinalPaper(couid, use);
            if (tp != null)
                tp.Tp_Logo = System.IO.File.Exists(PhyPath + tp.Tp_Logo) ? VirPath + tp.Tp_Logo : "";
            return tp;
        }
        /// <summary>
        /// 分页获取所有试卷
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="sbjid"></param>
        /// <param name="couid">课程id</param>
        /// <param name="search">按名称检索</param>
        /// <param name="diff">难度等级</param>
        /// <param name="size">每页几条</param>
        /// <param name="index">第几页</param>
        /// <returns></returns>
        public ListResult Pager(int orgid, int sbjid, int couid, string search, bool? isuse, int diff, int size, int index)
        {
            int count = 0;
            Song.Entities.TestPaper[] tps = null;
            tps = Business.Do<ITestPaper>().PaperPager(orgid, sbjid, couid, diff, isuse, search, size, index, out count);
            foreach (Song.Entities.TestPaper tp in tps)
            {
                tp.Tp_Logo = System.IO.File.Exists(PhyPath + tp.Tp_Logo) ? VirPath + tp.Tp_Logo : "";
            }
            ListResult result = new ListResult(tps);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        #endregion

        #region 出卷相关
        /// <summary>
        /// 获取试题的大项
        /// </summary>
        /// <returns></returns>
        public Song.Entities.TestPaperItem[] Types(int tpid)
        {
            Song.Entities.TestPaper tp = Business.Do<ITestPaper>().PaperSingle(tpid);
            return Business.Do<ITestPaper>().GetItemForAny(tp);
        }
        /// <summary>
        /// 生成随机试卷，试题随机抽取
        /// </summary>
        /// <param name="tpid">试卷id</param>
        /// <returns></returns>
        public JArray GenerateRandom(int tpid)
        {
            //取果是第一次打开，则随机生成试题，此为获取试卷
            Song.Entities.TestPaper paper = Business.Do<ITestPaper>().PaperSingle(tpid);
            if (paper == null) return null;
            //生成试卷
            Dictionary<TestPaperItem, Questions[]> dics = Business.Do<ITestPaper>().Putout(paper);
            JArray jarr = new JArray();
            foreach (var di in dics)
            {
                //按题型输出
                Song.Entities.TestPaperItem pi = (Song.Entities.TestPaperItem)di.Key;   //试题类型                
                Song.Entities.Questions[] ques = (Song.Entities.Questions[])di.Value;   //当前类型的试题
                int type = (int)pi.TPI_Type;    //试题类型
                int count = ques.Length;  //试题数目
                float num = (float)pi.TPI_Number;   //占用多少分
                if (count < 1) continue;
                JObject jo = new JObject();
                jo.Add("type",type);
                jo.Add("count", count);
                jo.Add("number", num);                
                jo.Add("ques", JArray.FromObject(ques));
                jarr.Add(jo);
            }
            return jarr;
        }
        #endregion


        #region 成绩
        /// <summary>
        /// 递交答题信息与成绩
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        [HttpPut, HttpPost]
        [HtmlClear(Not = "result")]
        [Student]
        public JObject InResult(string result)
        {
            JObject jo = new JObject();
            //如果为空，则返回-1，表示错误
            if (result == "")
            {
                jo.Add("score", 0);
                jo.Add("trid", 0);
                return jo;
            }
            XmlDocument resXml = new XmlDocument();
            resXml.LoadXml(result, false);
            XmlNode xn = resXml.SelectSingleNode("results");
            //学员Id,学员名称
            int stid = 0, stsid = 0;
            int.TryParse(getAttr(xn, "stid"), out stid);
            int.TryParse(getAttr(xn, "stsid"), out stsid);
            string stname = getAttr(xn, "stname");
            string stsname = getAttr(xn, "stsname");
            //***验证是否是当前学员
            Song.Entities.Accounts acc = this.User;
            if (acc.Ac_ID != stid) throw new Exception("当前登录学员信息与成绩提交的信息不匹配");

            //课程id,课程名称
            int couid = 0;
            int.TryParse(getAttr(xn, "couid"), out couid);
            string couname = getAttr(xn, "couname");
            //***课程是否购买或过期
            Student_Course purchase = Business.Do<ICourse>().StudentCourse(stid, couid);
            if (purchase == null || (!purchase.Stc_IsFree && purchase.Stc_EndTime < DateTime.Now && purchase.Stc_StartTime > DateTime.Now))
                throw new Exception("未购买课程或已经过期");

            //机构信息
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();

            //试卷id，试卷名称
            int tpid = 0;
            int.TryParse(getAttr(xn, "tpid"), out tpid);
            string tpname = getAttr(xn, "tpname");
            //***如果结课考试，则验证结课条件是否满足
            Song.Entities.TestPaper paper = Business.Do<ITestPaper>().PaperSingle(tpid);
            if (paper == null) throw new Exception("试卷不存在");
            if (paper.Tp_IsFinal)
            {
                string txt = string.Format("学员 {0} 未满足结课考试条件：", acc.Ac_Name);
                WeiSha.Core.CustomConfig config = CustomConfig.Load(org.Org_Config);
                //视频学习进度是否达成
                double condition_video = config["finaltest_condition_video"].Value.Double ?? 100;
                if (condition_video > purchase.Stc_StudyScore)
                {
                    throw new Exception(string.Format(txt + "视频学习应达到{0}%，实际学习进度{1}%", condition_video, purchase.Stc_StudyScore));
                }
                //试题练习通过率是否达成
                double condition_ques = config["finaltest_condition_ques"].Value.Double ?? 100;
                if (condition_ques > purchase.Stc_QuesScore)
                {
                    throw new Exception(string.Format(txt + "试题通过率应达到{0}%，实际通过率为{1}%", condition_ques, purchase.Stc_QuesScore));
                }
                //最多可以考几次
                int finaltest_count = config["finaltest_count"].Value.Int32 ?? 1;
                Song.Entities.TestResults[] trs = Business.Do<ITestPaper>().ResultsCount(stid, tpid);
                if (finaltest_count <= trs.Length)
                {
                    throw new Exception(string.Format("最多允许考试{0}次， 已经考了{1}次，", finaltest_count, trs.Length));
                }

            }

            //专业id,专业名称
            int sbjid = 0;
            int.TryParse(getAttr(xn, "sbjid"), out sbjid);
            string sbjname = getAttr(xn, "sbjname");

            //UID与考试主题
            string uid = getAttr(xn, "uid");
            //string theme = xn.Attributes["theme"].Value.ToString();
            //提交方式，1为自动提交，2为交卷
            int patter = Convert.ToInt32(xn.Attributes["patter"].Value);
            float score = (float)Convert.ToDouble(getAttr(xn, "score"));    //考试成绩
            bool isClac = getAttr(xn, "isclac") == "true" ? true : false;   //是否在客户端计算过成绩
            //
            Song.Entities.TestResults exr = new TestResults();
            exr.Tp_Id = tpid;
            exr.Tp_Name = tpname;
            exr.Cou_ID = couid;
            exr.Sbj_ID = sbjid;
            exr.Sbj_Name = sbjname;
            exr.Ac_ID = stid;
            exr.Ac_Name = stname;
            exr.Sts_ID = stid;
            exr.Sts_Name = stsname;
            exr.Tr_IP = WeiSha.Core.Browser.IP;
            exr.Tr_Mac = "";
            exr.Tr_UID = uid;
            exr.Tr_Results = result;
            exr.Tr_Score = score;

            exr.Org_ID = org.Org_ID;
            exr.Org_Name = org.Org_Name;
            //得分
            score = Business.Do<ITestPaper>().ResultsSave(exr);
            if (paper.Tp_IsFinal)
            {
                Thread t1 = new Thread(() =>
                {
                    try
                    {
                        double highest = Business.Do<ITestPaper>().ResultsHighest(paper.Tp_Id, stid);
                        purchase.Stc_ExamScore = highest;
                        Business.Do<ICourse>().StudentScoreSave(purchase, -1, -1, highest);
                    }
                    catch (Exception ex)
                    {
                        WeiSha.Core.Log.Error(this.GetType().FullName, ex);
                    }
                });
                t1.Start();
            }
            //返回得分与成绩id
            jo.Add("score", score);
            jo.Add("trid", exr.Tr_ID);
            jo.Add("tpid", exr.Tp_Id);
            return jo;
        }
        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="xn"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        private string getAttr(XmlNode xn, string attr)
        {
            if (xn.Attributes[attr] == null) return string.Empty;
            return xn.Attributes[attr].Value.Trim();
        }
        /// <summary>
        /// 测试成绩
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="tpid">试卷id</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult ResultsPager(int stid, int tpid, int size, int index)
        {
            int count = 0;
            Song.Entities.TestResults[] trs = Business.Do<ITestPaper>().ResultsPager(stid, tpid, size, index, out count);
            ListResult result = new ListResult(trs);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 所有测试成绩
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="tpid">试卷id</param>    
        /// <returns></returns>
        public Song.Entities.TestResults[] ResultsAll(int stid, int tpid)
        {
            if (stid <= 0) throw new Exception("学员id为空，无法获取成绩");
            if (tpid <= 0) throw new Exception("试卷id为空，无法获取成绩");
            Song.Entities.TestResults[] trs = Business.Do<ITestPaper>().ResultsCount(stid, tpid);            
            return trs;
        }
        /// <summary>
        /// 获取测试成绩
        /// </summary>
        /// <param name="id">测试成绩记录的id</param>
        /// <returns></returns>
        public Song.Entities.TestResults ResultForID(int id)
        {
            return Business.Do<ITestPaper>().ResultsSingle(id);
        }
        /// <summary>
        /// 删除测试成绩
        /// </summary>
        /// <param name="trid">测试成绩的id</param>
        /// <returns></returns>
        [HttpDelete,Admin,Teacher,Student]
        public bool ResultDelete(int trid)
        {
            try
            {
                Business.Do<ITestPaper>().ResultsDelete(trid);
                return true;
              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 清空学员的某个测试的所有历史成绩
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="tpid">试卷id</param>
        /// <returns></returns>
        [HttpDelete, Admin, Teacher, Student]
        public int ResultClear(int acid, int tpid)
        {
            Song.Entities.Accounts account = this.User;
            if (account != null && account.Ac_ID == acid)
            {
                return Business.Do<ITestPaper>().ResultsClear(acid, tpid);
            }
            return -1;
        }
        #endregion
    }
}