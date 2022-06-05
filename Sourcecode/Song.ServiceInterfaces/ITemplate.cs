using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;
using System.Data;
using WeiSha.Core;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// ģ�����
    /// </summary>
    public interface ITemplate : WeiSha.Core.IBusinessInterface
    {
        /// <summary>
        /// ����Webģ��
        /// </summary>
        /// <returns></returns>
        WeiSha.Core.Templates.TemplateBank[] WebTemplates();
        /// <summary>
        /// �����ֻ�ģ��
        /// </summary>
        /// <returns></returns>
        WeiSha.Core.Templates.TemplateBank[] MobiTemplates();
        /// <summary>
        /// �����ĵ�ǰwebģ��
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        WeiSha.Core.Templates.TemplateBank WebCurrTemplate(int orgid);
        WeiSha.Core.Templates.TemplateBank WebCurrTemplate();
        /// <summary>
        /// �����ĵ�ǰ�ֻ�ģ��
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        WeiSha.Core.Templates.TemplateBank MobiCurrTemplate(int orgid);
        WeiSha.Core.Templates.TemplateBank MobiCurrTemplate();
        /// <summary>
        /// ���õ�ǰwebģ��
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        WeiSha.Core.Templates.TemplateBank SetWebCurr(int orgid, string tag);
        /// <summary>
        /// ���õ�ǰ�ֻ�ģ��
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        WeiSha.Core.Templates.TemplateBank SetMobiCurr(int orgid, string tag);
        /// <summary>
        /// ͨ��ģ���ʶ��ȡwebģ��
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        WeiSha.Core.Templates.TemplateBank GetWebTemplate(string tag);
        /// <summary>
        /// ͨ��ģ���ʶ��ȡ�ֻ�ģ��
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        WeiSha.Core.Templates.TemplateBank GetMobiTemplate(string tag);
        /// <summary>
        /// ����ģ����Ϣ
        /// </summary>
        /// <param name="tmp"></param>
        /// <returns></returns>
        WeiSha.Core.Templates.TemplateBank Save(WeiSha.Core.Templates.TemplateBank tmp);
        /// <summary>
        /// ����ƽ̨��Ϣ�����Ŀ��ģ�����
        /// </summary>
        void SetPlateOrganInfo();
    }
}
