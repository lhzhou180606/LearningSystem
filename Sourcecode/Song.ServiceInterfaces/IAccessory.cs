using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;
using System.Data.Common;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// �����Ĺ���
    /// </summary>
    public interface IAccessory : WeiSha.Core.IBusinessInterface
    {
        /// <summary>
        /// ���
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void Add(Accessory entity);
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void Save(Accessory entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void Delete(int identify);
        /// <summary>
        /// ɾ������ϵͳΨһid
        /// </summary>
        /// <param name="uid">ϵͳΨһid</param>
        void Delete(string uid, string type);
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="entity"></param>
        void Delete(Accessory entity);
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="tran">����</param>
        void Delete(string uid, WeiSha.Data.DbTrans tran);
        //void DeleteBatch(string uid);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        Accessory GetSingle(int identify);
        /// <summary>
        /// ͨ��Uid��ȡ
        /// </summary>
        /// <param name="uid">�������������Uid�������½���Ƶ���˴�Ϊ�½ڵ�uid</param>
        /// <param name="type">�������ͣ�����CourseVideo���γ���Ƶ����Course���γ����ϣ�</param>
        /// <returns></returns>
        Accessory GetSingle(string uid, string type);
        /// <summary>
        /// ĳ�����壨�����ţ������и���
        /// </summary>
        /// <param name="uid">�����Ψһ��ʶ</param>
        /// <returns></returns>
        List<Accessory> GetAll(string uid);
        /// <summary>
        /// ĳ�����壨�����ţ������и���
        /// </summary>
        /// <param name="uid">�����Ψһ��ʶ</param>
        /// <param name="type">����</param>
        /// <returns></returns>
        List<Accessory> GetAll(string uid, string type);
        /// <summary>
        /// ���ƶ��ٸ���¼
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="uid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        int OfCount(int orgid, string uid, string type);
    }
}
