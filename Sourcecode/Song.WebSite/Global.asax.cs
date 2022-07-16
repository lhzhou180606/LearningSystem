using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Song.ServiceInterfaces;
using Song.Entities;
using System.Data;
using WeiSha.Data;

namespace Song.WebSite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            WeiSha.Data.Gateway.Default.RegisterLogger(new logger());

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //�����Զ���ģ������
            System.Web.Mvc.ViewEngines.Engines.Clear();
            System.Web.Mvc.ViewEngines.Engines.Add(new WeiSha.Core.TemplateEngine());
            //�����е�ģ�����ã���ʼ��
            if (!WeiSha.Core.PlateOrganInfo.IsInitialization)
            {               
                WeiSha.Core.Business.Do<ITemplate>().SetPlateOrganInfo();
            }
            //�˺���Ϣ�������ڴ滺�棬���������ѯ
            //Song.ServiceImpls.AccountLogin.Buffer.Init();
        }
    }
    /// <summary>
    /// SQL��ѯ���
    /// </summary>
    public class logger : WeiSha.Data.Logger.IExecuteLog
    {
        public void Begin(IDbCommand command)
        {
            System.Web.HttpContext _context = System.Web.HttpContext.Current;
            if (_context == null) return;
            string path = _context.Request.Url.AbsolutePath.Replace("/", "_");

            string sql = command.CommandText;
            for (int i = 0; i < command.Parameters.Count; i++)
            {
                System.Data.SqlClient.SqlParameter para = (System.Data.SqlClient.SqlParameter)command.Parameters[i];
                string vl = para.Value.ToString();
                string tp = para.DbType.ToString();
                if (tp.IndexOf("Int") > -1)
                    sql = sql.Replace("@p" + i.ToString(), vl);
                if (tp == "String")
                    sql = sql.Replace("@p" + i.ToString(), "'" + vl + "'");
                if (tp == "Boolean")
                    sql = sql.Replace("@p" + i.ToString(), vl == "True" ? "1" : "0");
                if (tp == "DateTime")
                    sql = sql.Replace("@p" + i.ToString(), "'" + ((DateTime)para.Value).ToString("yyyy/MM/dd HH:mm:ss") + "'");
            }
            //string t = command.Connection
            //WeiSha.Common.Log.Info(path, sql);
            WeiSha.Core.Log.Info(path, sql);
            //WeiSha.Common.Log.Info(path, command.Connection.ConnectionString);
            //throw new NotImplementedException();
        }

        public void End(IDbCommand command, ReturnValue retValue, long elapsedTime)
        {
            //throw new NotImplementedException();
        }
    }
}
