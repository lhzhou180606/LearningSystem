using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// ֧���ӿڹ���
    /// </summary>
    public interface IPayInterface : WeiSha.Core.IBusinessInterface
    {
        /// <summary>
        /// ���
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void PayAdd(PayInterface entity);
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void PaySave(PayInterface entity);
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void PayDelete(PayInterface entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void PayDelete(int identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        PayInterface PaySingle(int identify);
        /// <summary>
        /// ��ȡ���У�
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="platform">�ӿ�ƽ̨������Ϊweb���ֻ�Ϊmobi</param>
        /// <param name="isEnable">�Ƿ�����</param>
        /// <returns></returns>
        PayInterface[] PayAll(int orgid, string platform, bool? isEnable);
        /// <summary>
        /// �ӿ��Ƿ���ڣ��ӿ����Ʋ����ظ���
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="entity">ҵ��ʵ��</param>
        /// <param name="scope">��ѯ��Χ��1��ѯ���нӿڣ�2ͬһ���ͣ�Pai_InterfaceType����ͬһ�豸�ˣ�Pai_Platform����������</param>
        /// <returns></returns>
        PayInterface PayIsExist(int orgid, PayInterface entity, int scope);
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        bool UpdateTaxis(PayInterface[] items);
        /// <summary>
        /// ����ĳһ��֧���ӿڵ�����
        /// </summary>
        /// <param name="paid">֧���ӿڵ�id</param>
        /// <returns></returns>
        decimal Summary(int paid);
    }
}
