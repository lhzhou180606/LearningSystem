﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Mvc;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;
using pili_sdk;
using System.IO;
using System.Web;
using System.Drawing.Imaging;

namespace Song.ViewData.Methods
{
    
    /// <summary>
    /// 教师账号的相关操作
    /// </summary> 
    [HttpGet]
    public class Teacher : ViewMethod, IViewAPI
    {
        //资源的虚拟路径和物理路径
        public static string PathKey = "Teacher";
        public static string VirPath = WeiSha.Core.Upload.Get[PathKey].Virtual;
        public static string PhyPath = WeiSha.Core.Upload.Get[PathKey].Physics;
        #region 教师职称
        /// <summary>
        /// 所有教师职称信息
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="name">按职称名称检索</param>
        /// <param name="use"></param>
        /// <returns></returns>
        public Song.Entities.TeacherSort[] Titles(int orgid,string name, bool? use)
        {
            return Business.Do<ITeacher>().SortAll(orgid, name, use);
        }
        /// <summary>
        /// 根据id获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Cache(Expires = 60, AdminDisable = true)]
        public Song.Entities.TeacherSort TitleForID(int id)
        {
            if (id <= 0) return null;
            return Business.Do<ITeacher>().SortSingle(id);            
        }
        /// <summary>
        /// 添加教师职称
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public bool TitleAdd(Song.Entities.TeacherSort entity)
        {
            try
            {
                Business.Do<ITeacher>().SortAdd(entity);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 修改教师职称
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public bool TitleModify(Song.Entities.TeacherSort entity)
        {
            Song.Entities.TeacherSort old = Business.Do<ITeacher>().SortSingle(entity.Ths_ID);
            if (old == null) throw new Exception("Not found entity for TeacherSort！");

            old.Copy<Song.Entities.TeacherSort>(entity);
            Business.Do<ITeacher>().SortSave(old);
            return true;
        }
        /// <summary>
        /// 设置默认的职称
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="id">职称id</param>
        /// <returns></returns>
        public bool TitleSetDefault(int orgid,int id)
        {
            Business.Do<ITeacher>().SortSetDefault(orgid, id);
            return true;
        }
        /// <summary>
        /// 删除教师职称
        /// </summary>
        /// <param name="id">账户id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete]
        public int TitleDelete(string id)
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
                    Business.Do<ITeacher>().SortDelete(idval);
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
        /// 更改教师职称的排序
        /// </summary>
        /// <param name="items">对象数组</param>
        /// <returns></returns>
        [HttpPost]
        [Admin]
        public bool TitleUpdateTaxis(Song.Entities.TeacherSort[] items)
        {
            try
            {
                Business.Do<ITeacher>().SortUpdateTaxis(items);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 教师
        /// <summary>
        /// 当前登录的教师
        /// </summary>
        /// <returns></returns> 
        [HttpPost]
        public Song.Entities.Teacher Current()
        {
            return LoginAccount.Status.Teacher(this.Letter);
        }
        /// <summary>
        /// 根据id获取教师信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Cache(Expires = 60, AdminDisable = true)]
        public Song.Entities.Teacher ForID(int id)
        {
            if (id <= 0) return null;
            Song.Entities.Teacher teacher = Business.Do<ITeacher>().TeacherSingle(id);
            if (teacher != null)
            {
                if (System.IO.File.Exists(Upload.Get["Teacher"].Physics + teacher.Th_Photo))
                    teacher.Th_Photo = Upload.Get["Teacher"].Virtual + teacher.Th_Photo;
                else
                    teacher.Th_Photo = "";
            }
            return teacher;
        }
        /// <summary>
        /// 通过教师信息创建账号（用于教师账号存在，但对应的学员账号不存在的问题)
        /// </summary>
        /// <param name="entity">教师信息</param>
        /// <returns></returns>
        [Admin,SuperAdmin]
        [HttpPost]
        public bool CreatAccount(Song.Entities.Teacher entity)
        {
            return Business.Do<ITeacher>().TeacherCreateAccount(entity);
        }
        /// <summary>
        /// 添加教师
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost, Admin]
        [Upload(Extension = "jpg,png,gif", MaxSize = 1024, CannotEmpty = false)]
        public bool Add(Song.Entities.Teacher entity)
        {
            try
            {
                //图片
                string filename = _uploadLogo();
                if (!string.IsNullOrWhiteSpace(filename)) entity.Th_Photo = filename;

                //判断账号是否存在
                bool isHav = Business.Do<ITeacher>().IsTeacherExist(entity.Org_ID, entity);
                if (isHav) throw new Exception(string.Format("当前账号 {0} 已经存在", entity.Th_AccName));
                //学员账号是否存在
                Song.Entities.Accounts acc = Business.Do<IAccounts>().IsAccountsExist(entity.Th_AccName);

                //获取教师关联的基础账号（学员账号）
                Song.Entities.Teacher th = entity;
                if (acc != null)
                {                    
                    th.Ac_ID = acc.Ac_ID;
                }
                else
                {
                    //如果基础账号不存在；
                    acc = new Song.Entities.Accounts();
                    acc.Org_ID = th.Org_ID;
                    acc.Ac_AccName = th.Th_AccName;  
                    acc.Ac_MobiTel1 = th.Th_PhoneMobi;
                    acc.Ac_Name = th.Th_Name;
                    //th.Th_AccName = th.Th_PhoneMobi;
                    acc.Ac_IsPass = th.Th_IsPass = true;
                    th.Th_IsShow = true;
                    acc.Ac_IsUse = th.Th_IsUse = true;
                              
                    acc.Ac_Sex = th.Th_Sex;        //性别
                    acc.Ac_Birthday = th.Th_Birthday;
                    acc.Ac_Qq = th.Th_Qq;
                    acc.Ac_Email = th.Th_Email;
                    acc.Ac_IDCardNumber = th.Th_IDCardNumber;  //身份证    
                    acc.Ac_IsTeacher = true;        //该账号有教师身份
                                                    //保存
                    th.Ac_ID = Business.Do<IAccounts>().AccountsAdd(acc);
                }
                Business.Do<ITeacher>().TeacherAdd(th);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 修改教师信息
        /// </summary>
        /// <param name="entity">教师实体</param>
        /// <returns></returns>
        [HttpPost, Admin,Teacher]
        [Upload(Extension = "jpg,png,gif", MaxSize = 1024, CannotEmpty = false)]
        public bool Modify(Song.Entities.Teacher entity)
        {
            Song.Entities.Teacher old = Business.Do<ITeacher>().TeacherSingle(entity.Th_ID);
            if (old == null) throw new Exception("Not found entity for Teacher！");
            //图片
            string filename = _uploadLogo();
            //如果有上传的图片，且之前也有图片,则删除原图片
            if ((!string.IsNullOrWhiteSpace(filename) || string.IsNullOrWhiteSpace(entity.Th_Photo)) && !string.IsNullOrWhiteSpace(old.Th_Photo))
                WeiSha.Core.Upload.Get[PathKey].DeleteFile(old.Th_Photo);

            //账号关联id不变
            old.Copy<Song.Entities.Teacher>(entity, "Ac_ID,Ac_UID,Th_AccName,Org_ID");
            if (!string.IsNullOrWhiteSpace(filename)) old.Th_Photo = filename;
            Business.Do<ITeacher>().TeacherSave(old);
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string _uploadLogo()
        {
            string filename = string.Empty;
            foreach (string key in this.Files)
            {
                HttpPostedFileBase file = this.Files[key];
                filename = WeiSha.Core.Request.UniqueID() + Path.GetExtension(file.FileName);
                file.SaveAs(PhyPath + filename);
                break;
            }
            //转jpg
            if (!string.IsNullOrWhiteSpace(filename))
            {
                if (!".jpg".Equals(Path.GetExtension(filename), StringComparison.CurrentCultureIgnoreCase))
                {
                    string old = filename;
                    using (System.Drawing.Image image = WeiSha.Core.Images.FileTo.ToImage(PhyPath + filename))
                    {
                        filename = Path.ChangeExtension(filename, "jpg");
                        image.Save(PhyPath + Path.ChangeExtension(filename, "jpg"), ImageFormat.Jpeg);
                    }
                    System.IO.File.Delete(PhyPath + old);
                }
            }
            return filename;
        }
        /// <summary>
        /// 删除教师
        /// </summary>
        /// <param name="id">账户id，可以是多个，用逗号分隔</param>
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
                    Business.Do<ITeacher>().TeacherDelete(idval);
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
        /// 更改状态
        /// </summary>
        /// <param name="id">教师id</param>
        /// <param name="use">使用状态</param>
        /// <param name="pass">审核状态</param>
        /// <returns>学员id</returns>
        [HttpPost]
        [Admin, SuperAdmin]
        public int ModifyState(int id, bool use, bool pass)
        {
            try
            {
                return Business.Do<ITeacher>().TeacherUpdate(id,
                    new WeiSha.Data.Field[] {
                        Song.Entities.Teacher._.Th_IsUse,
                        Song.Entities.Teacher._.Th_IsPass },
                    new object[] { use, pass });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 修改教师的照片
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Upload(Extension = "jpg,png,gif", MaxSize = 512, CannotEmpty = true)]
        [Admin, Teacher]
        public Entities.Teacher ModifyPhoto(Entities.Teacher entity)
        {
            string filename = string.Empty;
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

                Song.Entities.Teacher old = Business.Do<ITeacher>().TeacherSingle(entity.Th_ID);
                if (old == null)
                {
                    entity.Th_Photo = System.IO.File.Exists(PhyPath + entity.Th_Photo) ? VirPath + entity.Th_Photo : "";
                    return entity;
                }
                if (!string.IsNullOrWhiteSpace(old.Th_Photo))
                {
                    string filehy = PhyPath + old.Th_Photo;
                    try
                    {
                        //删除原图
                        if (System.IO.File.Exists(filehy))
                            System.IO.File.Delete(filehy);
                        //删除缩略图，如果有
                        string smallfile = WeiSha.Core.Images.Name.ToSmall(filehy);
                        if (System.IO.File.Exists(smallfile))
                            System.IO.File.Delete(smallfile);
                    }
                    catch { }
                }
                old.Th_Photo = filename;
                //Business.Do<IAccounts>().AccountsSave(old);
                Business.Do<ITeacher>().TeacherUpdate(old.Th_ID, new WeiSha.Data.Field[] {
                    Song.Entities.Teacher._.Th_Photo
                }, new object[] { filename });

                //
                old.Th_Photo = System.IO.File.Exists(PhyPath + old.Th_Photo) ? VirPath + old.Th_Photo : "";
                return old;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 分页获取教师,用于后端，包括被禁用、未审核的教师
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="titid">职称id</param>
        /// <param name="search">按名称检索</param>
        /// <param name="phone">按电话查询，包括固话与手机号</param>
        /// <param name="acc">按账号查询</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult Pager(int orgid, int titid, string search,string phone,string acc, int size, int index)
        {
            int count = 0;
            Song.Entities.Teacher[] eas = Business.Do<ITeacher>().TeacherPager(orgid, titid, null, null, search, phone, acc, size, index, out count);
            for (int i = 0; i < eas.Length; i++)
                eas[i] = _tran(eas[i]);
            ListResult result = new ListResult(eas);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        #endregion

        #region 关联课程
        /// <summary>
        /// 设置课程关联
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="teachid">教师id</param>
        /// <returns></returns>
        [HttpPost][Admin,Teacher]
        public bool SetCourse(int couid,int teachid)
        {
            Song.Entities.Teacher teacher = Business.Do<ITeacher>().TeacherSingle(teachid);
            if (teacher == null) throw new Exception("教师不存在");
            try
            {
                Business.Do<ICourse>().CourseUpdate(couid,
                    new WeiSha.Data.Field[] {
                        Song.Entities.Course._.Th_ID,
                        Song.Entities.Course._.Th_Name },
                    new object[] { teacher.Th_ID, teacher.Th_Name });
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 私有方法，处理对象的关联信息
        /// <summary>
        /// 处理教师信息，图片转为全路径，并生成clone对象
        /// </summary>
        /// <param name="cour">课程对象的clone</param>
        /// <returns></returns>
        private Song.Entities.Teacher _tran(Song.Entities.Teacher cour)
        {
            if (cour == null) return cour;
            Song.Entities.Teacher curr = cour.Clone<Song.Entities.Teacher>();
            curr.Th_Photo = System.IO.File.Exists(PhyPath + curr.Th_Photo) ? VirPath + curr.Th_Photo : "";
            if (curr.Th_Birthday > DateTime.Now.AddYears(-100))
            {
                curr.Th_Age = ((TimeSpan)(DateTime.Now - curr.Th_Birthday)).Days / 365;
            }
            return curr;
        }
        #endregion
        #region 其它
        /// <summary>
        /// 教师登录
        /// </summary>
        /// <param name="acc">账号</param>
        /// <param name="pw">密码，明文字符</param>
        /// <returns></returns>
        public Song.Entities.Teacher Login(string acc,string pw)
        {
            Song.Entities.Accounts account = Business.Do<IAccounts>().AccountsLogin(acc, pw, true);
            if (account == null) return null;
            if (!account.Ac_IsTeacher) return null;
            //
            Song.Entities.Teacher teacher = Business.Do<ITeacher>().TeacherSingle(account.Ac_ID);
            if (teacher != null)
            {
                teacher = teacher.Clone<Song.Entities.Teacher>();
                if (teacher != null) teacher.Th_Pw = string.Empty;
                teacher.Th_Photo = WeiSha.Core.Upload.Get["Accounts"].Virtual + account.Ac_Photo;
            }
            return teacher;
        }
        /// <summary>
        /// 教师的课程
        /// </summary>
        /// <param name="thid">教师id</param>
        /// <returns></returns>
        public Song.Entities.Course[] Courses(int thid)
        {
            List<Song.Entities.Course> cours = Business.Do<ICourse>().CourseAll(-1, -1, thid, true);
            if (cours == null || cours.Count < 1) return null;
            for (int i = 0; i < cours.Count; i++)
            {
                cours[i].Cou_Logo = WeiSha.Core.Upload.Get["Course"].Virtual + cours[i].Cou_Logo;
                cours[i].Cou_LogoSmall = WeiSha.Core.Upload.Get["Course"].Virtual + cours[i].Cou_LogoSmall;
            }
            return cours.ToArray<Song.Entities.Course>();
        }
        /// <summary>
        /// 教师的直播课
        /// </summary>
        /// <param name="thid">教师id</param>
        /// <param name="count">取多少条记录</param>
        /// <returns></returns>
        public Song.Entities.Course[] LiveCourses(int thid, int count)
        {
            List<Song.Entities.Course> cours = Business.Do<ICourse>().CourseCount(-1, -1, thid, true, null, true, count);
            if (cours == null || cours.Count < 1) return null;
            for (int i = 0; i < cours.Count; i++)
            {
                cours[i].Cou_Logo = WeiSha.Core.Upload.Get["Course"].Virtual + cours[i].Cou_Logo;
                cours[i].Cou_LogoSmall = WeiSha.Core.Upload.Get["Course"].Virtual + cours[i].Cou_LogoSmall;
            }
            return cours.ToArray<Song.Entities.Course>();
        }
        /// <summary>
        /// 教师的直播章节
        /// </summary>
        /// <param name="thid">教师id</param>
        /// <param name="couid">课程id</param>
        /// <returns>返回内容包括：带直播的章节的名称、ID、UID、课程名称、直播相关信息；
        /// LivePublish：直播推流地址，示例 rtmp://pili-publish.zhibo.weisha100.cn/ceshi5/132_267_548f3efe842b564c74babb4c70fc
        /// LiveHLS：播放地址，示例 http://pili-live-hls.zhibo.weisha100.cn/ceshi5/132_267_548f3efe842b564c74babb4c70fc
        /// LiveCover：直播封面，每五秒更新，示例http://pili-snapshot.zhibo.weisha100.cn/ceshi5/132_267_548f3efe842b564c74babb4c70fc
        /// LiveTime：直播开始时间，这个时间仅供于通知学员何时开始，从技术来说直播可以随时开始
        /// LiveSpan：直播课的时长，仅供告知学员课时长度，从技术上来说直播不受时长限制
        /// </returns>
        /// <remarks></remarks>
        public Outline_LiveInfo[] Lives(int thid, int couid)
        {
            Song.Entities.Course cour = Business.Do<ICourse>().CourseSingle(couid);
            if (cour == null) return null;
            if (cour.Th_ID != thid) return null;
            //
            Song.Entities.Outline[] outls = Business.Do<IOutline>().OutlineCount(couid, true, null, true, 0);
            List<Outline_LiveInfo> list = new List<Outline_LiveInfo>();
            //直播截图的域名
            string snapshot = Business.Do<ILive>().GetSnapshot;
            string proto = Business.Do<ILive>().GetProtocol;    //协议，http还是https
            foreach (Song.Entities.Outline o in outls)
            {
                pili_sdk.pili.Stream stream = Pili.API<IStream>().GetForTitle(o.Ol_LiveID);
                if (stream == null) continue;
                //推流地址
                string publist = string.Format("rtmp://{0}/{1}/{2}", stream.PublishRtmpHost, stream.HubName, stream.Title);
                //播放地址               
                string play = string.Format("{0}://{1}/{2}/{3}.m3u8", proto, stream.LiveHlsHost, stream.HubName, stream.Title);
                //封面地址
                string cover = string.Format("http://{0}/{1}/{2}.jpg", snapshot, stream.HubName, stream.Title);
                list.Add(new Outline_LiveInfo()
                {
                    Name = o.Ol_Name,
                    ID = o.Ol_ID,
                    UID = o.Ol_UID,
                    Course = cour.Cou_Name,
                    LiveID = stream.StreamID,
                    LiveTitle = stream.Title,
                    LiveTime = o.Ol_LiveTime,
                    LiveSpan = o.Ol_LiveSpan,
                    LivePublish = publist,
                    LiveHLS = play,
                    LiveCover = cover
                });

            }
            return list.ToArray<Outline_LiveInfo>();
        }
        #endregion
    }
    //章节直播信息
    public class Outline_LiveInfo
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public string UID { get; set; }
        public string Course { get; set; }      //课程名称
        public string LiveID { get; set; }      //直播ID，由z1.空间名.id三部分组成
        public string LiveTitle { get; set; }       //直播ID,这才是直播id
        public DateTime LiveTime { get; set; }      //直播开始时间
        public int LiveSpan { get; set; }           //直播时长，单位分钟
        public string LivePublish { get; set; }     //直播推送地址
        public string LiveHLS { get; set; }         //直播播放地址
        public string LiveCover { get; set; }       //直播封面地址
    }
}