using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// Ժϵְλ�Ĺ���
    /// </summary>
    public interface IPosition : WeiSha.Core.IBusinessInterface
    {
        /// <summary>
        /// ���
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void Add(Position entity);
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void Save(Position entity);
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void Delete(Position entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void Delete(int identify);
        /// <summary>
        /// ɾ������ְλ����
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="name">ְλ����</param>
        void Delete(int orgid, string name);
        /// <summary>
        /// ɾ����Ա��֮��Ĺ���
        /// </summary>
        /// <param name="identify"></param>
        void DeleteRelation4Emp(int identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        Position GetSingle(int identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�ְλ����
        /// </summary>
        /// <param name="name">ְλ����</param>
        /// <returns></returns>
        Position GetSingle(int orgid,string name);
        /// <summary>
        /// ��ȡ��������Ա��ɫ
        /// </summary>
        /// <returns></returns>
        Position GetSuper();
        /// <summary>
        /// ��ȡ���󣻼�����ְλ��
        /// </summary>
        /// <returns></returns>
        Position[] GetAll(int orgid);
        Position[] GetAll(int orgid,bool? isUse);
        /// <summary>
        /// ��ȡ��ǰ��ɫ������Ա��
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        EmpAccount[] GetAllEmplyee(int posid);
        /// <summary>
        /// ��ȡ��ǰ��ɫ��������ְԱ��
        /// </summary>
        /// <param name="posid"></param>
        /// <param name="use">�Ƿ���ְ</param>
        /// <returns></returns>
        EmpAccount[] GetAllEmplyee(int posid,bool use);
        /// <summary>
        /// ��λ�Ƿ��Ѿ�����
        /// </summary>
        /// <param name="name">��λ����</param>
        /// <param name="id">����id</param>
        /// <param name="orgid">���ڻ���id</param>
        /// <returns></returns>
        bool IsExist(string name,int id,int orgid);
        /// <summary>
        /// ���ĸ�λ����
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        bool UpdateTaxis(Position[] entities);
        /// <summary>
        /// ��ȡ�����Ĺ����λ
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        Position GetAdmin(int orgid);
    }
}
