using System;
using System.Collections.Generic;
using System.Text;

namespace Song.SMS
{
    public interface ISMS
    {
        /// <summary>
        /// ����ƽ̨�Ĺ�����
        /// </summary>
        SmsItem Current { get; set; }
        /// <summary>
        /// �û����˺�
        /// </summary>
        string User { get; set; }
        /// <summary>
        /// �û�������
        /// </summary>
        string Password { get; set; }
        /// <summary>
        /// ���Ͷ���
        /// </summary>
        /// <param name="mobiles"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        SmsState Send(string mobiles, string context);
        /// <summary>
        /// ��ʱ���Ͷ���
        /// </summary>
        /// <param name="mobiles"></param>
        /// <param name="context"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        SmsState Send(string mobiles, string context, DateTime time);
        /// <summary>
        /// ��ѯʣ��Ķ�������
        /// </summary>
        /// <returns></returns>
        int Query();
        /// <summary>
        /// ��ѯʣ��Ķ�������
        /// </summary>
        /// <param name="user">�˺�</param>
        /// <param name="pw">����</param>
        /// <returns></returns>
        int Query(string user, string pw);
        /// <summary>
        /// ���ջط��Ķ���
        /// </summary>
        /// <param name="from">��ʼ���յ�ʱ��</param>
        /// <param name="readflag">�Ƿ��Ѷ���0:δ�����ţ�1:���ж���</param>
        /// <returns></returns>
        string ReceiveSMS(DateTime from, string readflag);
    }
}
