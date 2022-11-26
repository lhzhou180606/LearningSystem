﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiSha.Core;
using En = Song.Entities;
using Song.ServiceInterfaces;
using System.Reflection;

namespace Song.ViewData.Attri
{
    /// <summary>
    /// 学员能够学习当前课程，才能使用的方法
    /// </summary>
    public class StudyAttribute : PurviewAttribute
    {
        /// <summary>
        /// 验证是否能通过
        /// </summary>
        /// <param name="method">执行的方法</param>
        /// <param name="letter">请求</param>
        /// <returns></returns>
        public static bool Verify(MemberInfo method, Letter letter)
        {
            StudyAttribute study = StudyAttribute.GetAttr<StudyAttribute>(method);
            if (study == null) return true;
            //验证登录
            string msg = string.Format("接口 '{0}/{1}' 需要", method.DeclaringType.Name, method.Name);
            //如果未登录，则直接返回false
            En.Accounts acc = LoginAccount.Status.User();
            if (acc == null) throw new Exception(msg + "学员账户登录后操作");
          
            //获取章节或课程id
            long couid = 0, olid = 0;
            couid = letter["couid"].Int64 ?? 0;
            olid = letter["olid"].Int64 ?? 0;
            if (couid == 0 && olid == 0) return true;
            En.Outline outline = null; En.Course cour = null;
            if (olid > 0) outline = Business.Do<IOutline>().OutlineSingle(olid);
            if(outline!=null) couid = outline.Cou_ID;
            if (couid > 0) cour = Business.Do<ICourse>().CourseSingle(couid);

            //判断课程
            if (cour == null) throw new Exception("课程不存在！");
            if (!cour.Cou_IsUse) throw new Exception("课程状态为“禁用”！");
            //判断课程是否可以学习
            bool allowStudy = Business.Do<ICourse>().AllowStudy(cour, acc);
            if (couid > 0 && olid <= 0)
            {
                if (allowStudy) return true;
                throw new Exception("当前课程不可以学习");
            }
            //判断章节是否可以学习          
            if (outline != null)
            {
                bool canStudy = allowStudy && outline.Ol_IsUse && outline.Ol_IsFinish;
                if (canStudy) return true;
                throw new Exception("当前章节不可以学习");
            }           
            throw new Exception("当前内容不可以学习");          
        }      
    }
}
