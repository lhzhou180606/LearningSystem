using System;
using System.Collections.Generic;
using System.Text;

namespace Song.SMS
{
    public class SmsItem
    {
        private string user;
        /// <summary>
        /// ���ŷ����ʺ�
        /// </summary>
        public string User
        {
            get { return user; }
            set { user = value; }
        }
        private string pw;
        /// <summary>
        /// ���ŷ��͵�����
        /// </summary>
        public string Password
        {
            get { return pw; }
            set { pw = value; }
        }
        private string type;
        /// <summary>
        /// ���Žӿڵ�ʵ����
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        private string name;
        /// <summary>
        /// �ӿ�����
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string mark;
        /// <summary>
        /// ��ʶ��Ϣ
        /// </summary>
        public string Mark
        {
            get { return mark; }
            set { mark = value; }
        }
        private string _domain;
        /// <summary>
        /// �ӿڵ������򣬰����˿�
        /// </summary>
        public string Domain
        {
            get { return _domain; }
            set { _domain = value; }
        }
        private string _regurl;
        /// <summary>
        /// ע��ĵ�ַ
        /// </summary>
        public string RegisterUrl
        {
            get { return _regurl; }
            set { _regurl = value; }
        }
        private string _payurl;
        /// <summary>
        /// ��ֵ�ĵ�ַ
        /// </summary>
        public string PayUrl
        {
            get { return _payurl; }
            set { _payurl = value; }
        }
        private bool _isUse;
        /// <summary>
        /// �Ƿ����øýӿ�
        /// </summary>
        public bool IsUse
        {
            get { return _isUse; }
            set { _isUse = value; }
        }
        private bool isCurrent = false;
        /// <summary>
        /// �Ƿ�ǰ���õĶ���ƽ̨
        /// </summary>
        public bool IsCurrent
        {
            get { return isCurrent; }
            set { isCurrent = value; }
        }
    }
}
