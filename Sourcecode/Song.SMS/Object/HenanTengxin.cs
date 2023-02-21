using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;

namespace Song.SMS.Object
{
    /// <summary>
    /// �������ŵĶ��ſ����ӿ�
    /// </summary>
    public class HenanTengxin: ISMS
    {
        private static readonly sms1086.WsAPIs ObjWsAPIs = new sms1086.WsAPIs();
        private SmsItem _current;
        public SmsItem Current
        {
            get { return _current; }
            set { _current = value; }
        }
        /// <summary>
        /// �û����˺�
        /// </summary>
        public string User
        {
            get { return _current.User; }
            set { _current.User = value; }
        }
        /// <summary>
        /// �û�������
        /// </summary>
        public string Password
        {
            get { return _current.Password; }
            set { _current.Password = value; }
        }
        #region ISMS ��Ա

        public SmsState Send(string mobiles, string context)
        {
            return Send(mobiles, context, DateTime.Now);
        }

        public SmsState Send(string mobiles, string content, DateTime time)
        {
            //url���봦��������
            string username = System.Web.HttpUtility.UrlEncode(Current.User, Encoding.UTF8);
            content = System.Web.HttpUtility.UrlEncode(content, Encoding.UTF8);
            //����
            string url = string.Format("{0}Api/sendutf8.aspx?username={1}&password={2}&mobiles={3}&content={4}", 
                Current.Domain, username, Current.Password, mobiles, content);
            string result = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //������
            request.ContentType = "text/HTML";
            request.MaximumAutomaticRedirections = 4;
            request.MaximumResponseHeadersLength = 4;
            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //�����
            Stream receiveStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            result = readStream.ReadToEnd();
            response.Close();
            readStream.Dispose();
            //
            string stat = _getPara(result, "result");  //״ֵ̬
            string desc = _getPara(result, "description");  //����
            desc = System.Web.HttpUtility.UrlDecode(desc);
            //����״̬
            SmsState state = new SmsState();
            state.Code = int.Parse(stat);
            state.Success = state.Code == 0;
            switch (state.Code)
            {
                case 1:
                    state.Result = "�ύ��������Ϊ��";
                    break;
                case 2:
                    state.Result = "�û������������";
                    break;
                case 3:
                    state.Result = "�˺�δ����";
                    break;
                case 4:
                    state.Result = "�Ʒ��˺���Ч";
                    break;
                case 5:
                    state.Result = "��ʱʱ����Ч";
                    break;
                case 6:
                    state.Result = "ҵ��δ��ͨ";
                    break;
                case 7:
                    state.Result = "Ȩ�޲���";
                    break;
                case 8:
                    state.Result = "����";
                    break;
                case 9:
                    state.Result = "�����к�����Ч����";
                    break;
                default:
                    state.Result = desc;
                    break;
            }
            state.Description = desc;
            state.FailList = _getPara(result, "faillist");  //����ʧ�ܵĵ绰�����б�
            return state;
        }

        public int Query()
        {
            //�����ʺ�������
            string account = Current.User;
            string pw = Current.Password;
            return Query(account, pw);

        }
        /// <summary>
        /// ��ѯʣ��Ķ�������
        /// </summary>
        /// <param name="user">�˺�</param>
        /// <param name="pw">����</param>
        /// <returns></returns>
        public int Query(string user, string pw)
        {
            //��ַ
            string url = Current.Domain;
            //����
            string postString = "username={0}&password={1}";
            url = string.Format(url + "Api/Query.aspx?" + postString, user, pw);
            //��ȡ��ʣ������
            string result = "";
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                using (System.IO.Stream stream = client.OpenRead(url))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(stream, System.Text.Encoding.GetEncoding("gb2312")))
                    {
                        //result=����ֵ&balance=����&description=��������
                        result = reader.ReadToEnd();
                        reader.Close();
                    }
                    stream.Close();
                }
            }
           
            string state = _getPara(result, "result");  //״ֵ̬
            string number = _getPara(result, "balance");    //ʣ������
            string desc = _getPara(result, "description");  //����
            if (!string.IsNullOrWhiteSpace(desc))
            {
                desc = System.Web.HttpUtility.UrlDecode(desc, System.Text.Encoding.GetEncoding("GB2312"));
            }
            if (state == "0")
            {
                return Convert.ToInt32(number);
            }
            else
            {
                throw new Exception(desc);            
            }
        }
        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="url"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string _getPara(string url, string key)
        {
            string value = string.Empty;
            if (url.IndexOf("?") > -1) url = url.Substring(url.LastIndexOf("?") + 1);
            string[] paras = url.Split('&');
            foreach (string para in paras)
            {
                if (string.IsNullOrWhiteSpace(para)) continue;
                string[] arr = para.Split('=');
                if (arr.Length < 2) continue;
                if (String.Equals(arr[0], key, StringComparison.CurrentCultureIgnoreCase))
                {
                    value = arr[1];
                    break;
                }
            }
            return value;
        }
        public string ReceiveSMS(DateTime from, string readflag)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
