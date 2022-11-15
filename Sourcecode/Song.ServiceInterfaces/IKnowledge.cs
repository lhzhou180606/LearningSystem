using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// ֪ʶ�����
    /// </summary>
    public interface IKnowledge : WeiSha.Core.IBusinessInterface
    {
        #region ֪ʶ�����
        /// <summary>
        /// ���֪ʶ��
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        int KnowledgeAdd(Knowledge entity);
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void KnowledgeSave(Knowledge entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void KnowledgeDelete(long identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        Knowledge KnowledgeSingle(long identify);
        Knowledge KnowledgeSingle(string uid);
        /// <summary>
        /// ��ǰ֪ʶ����һ��֪ʶ
        /// </summary>
        /// <param name="couid">�γ�id����С��0ʱȡ���У�����0ʲôҲ��ȡ��</param>
        /// <param name="kns">����uid</param>
        /// <param name="id"></param>
        /// <returns></returns>
        Knowledge KnowledgePrev(long couid, long kns, int id);
        /// <summary>
        /// ��ǰ֪ʶ����һ��֪ʶ
        /// </summary>
        /// <param name="couid">�γ�id����С��0ʱȡ���У�����0ʲôҲ��ȡ��</param>
        /// <param name="kns">����uid</param>
        /// <param name="id"></param>
        /// <returns></returns>
        Knowledge KnowledgeNext(long couid, long kns, int id);
        /// <summary>
        /// ��ȡ֪ʶ��
        /// </summary>
        /// <param name="isUse"></param>
        /// <param name="kns">����uid</param>
        /// <param name="count"></param>
        /// <returns></returns>
        Knowledge[] KnowledgeCount(int orgid, bool? isUse, long kns, int count);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="couid">�γ�id����С��0ʱȡ���У�����0ʲôҲ��ȡ��</param>
        /// <param name="kns">����uid</param>
        /// <param name="searTxt"></param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Knowledge[] KnowledgeCount(int orgid, long couid, long kns, string searTxt, bool? isUse, int count);
        /// <summary>
        /// �����ж�����
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="kns"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        int KnowledgeOfCount(int orgid, long kns, bool? isUse);
        int KnowledgeOfCount(int orgid, long couid, long kns, bool? isUse);
        /// <summary>
        /// ��ҳ��ȡ
        /// </summary>
        /// <param name="isUse"></param>
        /// <param name="kns">����id</param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Knowledge[] KnowledgePager(int orgid, bool? isUse, long kns, string searTxt, int size, int index, out int countSum);
        Knowledge[] KnowledgePager(int orgid, long couid, long kns, bool? isUse, bool? isHot, bool? isRec, bool? isTop, string searTxt, int size, int index, out int countSum);
        /// <summary>
        /// ��ǰ�γ̵�֪ʶ��
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="kns">����id�����ŷָ�</param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Knowledge[] KnowledgePager(long couid, long kns, string searTxt, bool? isUse, int size, int index, out int countSum);
        #endregion

        #region ֪ʶ��������
        /// <summary>
        /// ���
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        int SortAdd(KnowledgeSort entity);
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void SortSave(KnowledgeSort entity);
        /// <summary>
        /// �޸ķ�������
        /// </summary>
        /// <param name="xml"></param>
        void SortSaveOrder(string xml);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void SortDelete(long identify);
        void SortDelete(KnowledgeSort sort);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        KnowledgeSort SortSingle(long identify);
        /// <summary>
        /// ��ȡ���з���
        /// </summary>
        /// <param name="orgid">��������</param>
        /// <param name="couid">�γ�id����С��0ʱȡ���У�����0ʲôҲ��ȡ��</param>
        /// <param name="search"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        KnowledgeSort[] GetSortAll(int orgid, long couid,string search, bool? isUse);
        /// <summary>
        /// ��ȡ���з���
        /// </summary>
        /// <param name="orgid">��������id</param>
        /// <param name="couid">�γ�id����С��0ʱȡ���У�����0ʲôҲ��ȡ��</param>
        /// <param name="pid">��id���༶���ࣩ</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        KnowledgeSort[] GetSortAll(int orgid, long couid, int pid, bool? isUse);
        /// <summary>
        /// ��ȡ��ǰ�������һ���Ӷ���
        /// </summary>
        /// <param name="couid">�γ�id����С��0ʱȡ���У�����0ʲôҲ��ȡ��</param>
        /// <param name="pid">�ϼ�</param>
        /// <returns>��ǰ�������һ���Ӷ���</returns>
        KnowledgeSort[] GetSortChilds(long pid, long couid, bool? isUse);
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="list">�����б�Kns_ID��Kns_PID��Kns_Tax</param>
        /// <returns></returns>
        bool SortUpdateTaxis(KnowledgeSort[] list);
        /// <summary>
        /// ��ǰ�����µ�������רҵid
        /// </summary>
        /// <param name="kns">��ǰ����id</param>
        /// <param name="orgid">������ID</param>
        List<long> TreeID(long kns, int orgid);
        #endregion

    }
}
