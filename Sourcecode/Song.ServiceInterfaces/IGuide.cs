using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// �γ�ָ�ϣ������Ľпγ�֪ͨ
    /// </summary>
    public interface IGuide : WeiSha.Core.IBusinessInterface
    {
        #region ָ��
        /// <summary>
        /// ����ָ��
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void GuideAdd(Guide entity);
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void GuideSave(Guide entity);
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void GuideDelete(Guide entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void GuideDelete(int identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        Guide GuideSingle(int identify);       
        /// <summary>
        /// ��ǰ�γ̹������һ���γ̹���
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Guide GuidePrev(Guide entity);
        /// <summary>
        /// ��ǰ�γ̹������һ���γ̹���
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Guide GuideNext(Guide entity);
        /// <summary>
        /// ȡ������
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="gcuid">����uid</param>
        /// <param name="count"></param>
        /// <returns></returns>
        Guide[] GuideCount(int orgid, int couid, string gcuid, int count);
        /// <summary>
        /// ��ҳ��ȡ
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="couid">�γ�id</param>
        /// <param name="gcuid">����</param>
        /// <param name="searTxt"></param>
        /// <param name="isShow">�Ƿ���ʾ</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Guide[] GuidePager(int orgid, int couid, string gcuid, string searTxt, bool? isShow, int size, int index, out int countSum); 
        #endregion

        #region ָ�Ϸ���
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        int ColumnsAdd(GuideColumns entity);
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void ColumnsSave(GuideColumns entity);
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void ColumnsDelete(GuideColumns entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void ColumnsDelete(int identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        GuideColumns ColumnsSingle(int identify);
        GuideColumns ColumnsSingle(string uid);
        /// <summary>
        /// ��ȡ���󣻼����з��ࣻ
        /// </summary>
        /// <returns></returns>
        GuideColumns[] GetColumnsAll(int couid, string search, bool? isUse);
        /// <summary>
        /// ��ȡ��ǰ�����µ��ӷ���
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="pid"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        GuideColumns[] GetColumnsChild(int couid, string pid, bool? isUse);
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="list">�����б���Gc_ID��Gc_PID��Gc_Tax</param>
        /// <returns></returns>
        bool ColumnsUpdateTaxis(GuideColumns[] list);

        #endregion
    }
}