using System;
using System.Collections.Generic;
using System.Text;

namespace Song.SMS
{
    /// <summary>
    /// ���ŵķ���״̬
    /// </summary>
    public class SmsState
    {
        private bool _success;
        /// <summary>
        /// �Ƿ�ɹ�
        /// </summary>
        public bool Success
        {
            get { return _success; }
            set { _success = value; }
        }
        private string _result = "";
        /// <summary>
        /// ���Ͷ��ŵķ��ؽ��
        /// </summary>
        public string Result
        {
            get { return _result; }
            set { _result = value; }
        }
        private int _code = -1;
        /// <summary>
        /// ���ͺ�ķ��ش��룬һ��0Ϊ���ͳɹ�
        /// </summary>
        public int Code
        {
            get { return _code; }
            set { _code = value; }
        }
        private string _description = "";
        /// <summary>
        /// ���Ͷ��ŵ���ϸ������Ϣ
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        private string _failList = "";
        /// <summary>
        /// �������ʧ�ܣ�ʧ���б�
        /// </summary>
        public string FailList
        {
            get { return _failList; }
            set { _failList = value; }
        }
    }
}
