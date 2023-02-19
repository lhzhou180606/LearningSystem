using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// Ա����Ĺ���
    /// </summary>
    public interface IEmpGroup : WeiSha.Core.IBusinessInterface
    {
        /// <summary>
        /// ���
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void Add(EmpGroup entity);
        /// <summary>
        /// ����Ա������Ĺ���
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="grpId"></param>
        void AddRelation(int empId, int grpId);
        /// <summary>
        /// ����Ա��Id,ɾ������
        /// </summary>
        /// <param name="empId"></param>
        void DelRelation4Emplyee(int empId);
        /// <summary>
        /// ������Id,ɾ������
        /// </summary>
        /// <param name="grpId"></param>
        void DelRelation4Group(int grpId);
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void Save(EmpGroup entity);
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void Delete(EmpGroup entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void Delete(int identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        EmpGroup GetSingle(int identify);
        /// <summary>
        /// ��ȡ���󣻼������û��飻
        /// </summary>
        /// <returns></returns>
        EmpGroup[] GetAll(int orgid);
        EmpGroup[] GetAll(int orgid, bool? isUse);
        /// <summary>
        /// ��ȡĳԱ�������������飻
        /// </summary>
        /// <param name="EmpAccountId">Ա��id</param>
        /// <returns></returns>
        EmpGroup[] GetAll4Emp(int EmpAccountId);
        /// <summary>
        /// ��ȡĳ���������Ա��
        /// </summary>
        /// <param name="grpId">��id</param>
        /// <returns></returns>
        EmpAccount[] GetAll4Group(int grpId);
        /// <summary>
        /// ��ȡĳ�����������ְԱ��
        /// </summary>
        /// <param name="grpId"></param>
        /// <param name="use"></param>
        /// <returns></returns>
        EmpAccount[] GetAll4Group(int grpId,bool use);
        /// <summary>
        /// ��ǰ���������Ƿ�����
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        /// <returns></returns>
        bool IsExist(EmpGroup entity);
        bool IsExist(string name, int id, int orgid);
        /// ��������
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        bool UpdateTaxis(EmpGroup[] entities);
        /// <summary>
        /// ��ҳ��ȡ
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isUse"></param>
        /// <param name="name"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        EmpGroup[] Pager(int orgid, bool? isUse, string name, int size, int index, out int countSum);
    }
}
