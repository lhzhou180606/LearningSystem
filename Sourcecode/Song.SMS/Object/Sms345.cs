using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using WeiSha.Core;

namespace Song.SMS.Object
{
    /// <summary>
    /// Sms345���ŷ�����
    /// </summary>
    public class Sms345 : ISMS
    {
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
        public SmsState Send(string mobiles, string context)
        {
            return Send(mobiles, context, DateTime.Now);
        }

        public SmsState Send(string mobiles, string context, DateTime time)
        {
            //�����ʺ�������
            string account = Current.User;
            string pw = Current.Password;
            
            pw = new WeiSha.Core.Param.Method.ConvertToAnyValue(pw).MD5;
            
            //��ַ
            string url = Current.Domain;
            //����
            string postString = "uid={0}&pwd={1}&mobile={2}&content={3}&encode=utf8";
            context = new WeiSha.Core.Param.Method.ConvertToAnyValue(context).UrlEncode;
            string timestr = time.ToString("yyyy-MM-dd HH:mm");
            postString = string.Format(postString, account, pw, mobiles, context);
            byte[] postData = Encoding.UTF8.GetBytes(postString); 
            string result = "";
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                url += "tx/?" + postString;
                using (System.IO.Stream stream = client.OpenRead(url))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(stream, System.Text.Encoding.GetEncoding("gb2312")))
                    {
                        result = reader.ReadToEnd();
                        reader.Close();
                    }
                    stream.Close();
                }
            }
             //����״̬
            SmsState state = new SmsState();
            state.Success = result == "100";
            state.Result = result;
            //���ͽ����״̬
            string[] resultItem = new string[]{"100|���ͳɹ�","101|��֤ʧ��","102|���Ų���","103|����ʧ��","104|�Ƿ��ַ�",
               "105|���ݹ���","106|�������","107|Ƶ�ʹ���","108|�������ݿ�",
               "109|�˺Ŷ���","110|��ֹƵ����������","111|ϵͳ�ݶ�����",
               "112|���벻��ȷ","120|ϵͳ����"};
            foreach (string str in resultItem)
            {
                string s = str.Substring(0, str.IndexOf("|"));
                string e = str.Substring(str.IndexOf("|") + 1);
                if (result == s)
                {
                    state.Result = s;
                    state.Description = e;
                }
            }
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
            pw = new WeiSha.Core.Param.Method.ConvertToAnyValue(pw).MD5;
            //��ַ
            string url = Current.Domain;
            //����
            string postString = "uid={0}&pwd={1}";
            url = string.Format(url + "mm/?" + postString, user, pw);
            //��ȡ��ʣ������
            string result = "";
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                using (System.IO.Stream stream = client.OpenRead(url))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(stream, System.Text.Encoding.GetEncoding("gb2312")))
                    {
                        result = reader.ReadToEnd();
                        reader.Close();
                    }
                    stream.Close();
                }
            }
            if (result == "") throw new Exception("��ȡʧ�ܣ�");
            int num = -1;
            if (result.IndexOf("|") > -1)
            {
                string state = result.Substring(0, result.IndexOf("|"));
                //�����ȡ����
                if (state == "100")
                {
                    string nums = result.Substring(result.LastIndexOf("|") + 1);
                    int.TryParse(nums, out num);                    
                }
            }
            return num;
        }

        public string ReceiveSMS(DateTime from, string readflag)
        {
            //throw new Exception("The method or operation is not implemented.");
            return string.Empty;
        }


    }
}
