using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;
using System.Data;
using WeiSha.Data;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// �Ծ��Ĺ���
    /// </summary>
    public interface ITestPaper : WeiSha.Core.IBusinessInterface
    {
        #region �Ծ�����
        /// <summary>
        /// �����Ծ�
        /// </summary>
        /// <param name="entity">�Ծ�����</param>
        int PaperAdd(TestPaper entity);
        /// <summary>
        /// �޸��Ծ�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void PaperSave(TestPaper entity);
        /// <summary>
        /// �޸��Ծ���ĳЩ��
        /// </summary>
        /// <param name="id">�Ծ���id</param>
        /// <param name="fiels"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        bool PaperUpdate(int id, Field[] fiels, object[] objs);
        /// <summary>
        /// ɾ���Ծ���������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void PaperDelete(int identify);
        /// <summary>
        /// ��ȡ��һ�Ծ�ʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        TestPaper PaperSingle(int identify);
        TestPaper PaperSingle(string name);
        /// <summary>
        /// ��ȡĳ���γ̵Ľ�ο���
        /// </summary>
        /// <param name="couid">�γ�id</param>
        /// <param name="use"></param>
        /// <returns></returns>
        TestPaper FinalPaper(int couid,bool? use);
        /// <summary>
        /// ��ȡ�Ծ�
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="sbjid">ѧ��id</param>
        /// <param name="couid">�γ�id</param>
        /// <param name="diff"></param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        TestPaper[] PaperCount(int orgid, int sbjid, int couid, int diff, bool? isUse, int count);
        TestPaper[] PaperCount(string search, int orgid, int sbjid, int couid, int diff, bool? isUse, int count);
        /// <summary>
        /// �����ж��ٸ��Ծ�
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="sbjid"></param>
        /// <param name="couid"></param>
        /// <param name="diff"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        int PaperOfCount(int orgid, int sbjid, int couid, int diff, bool? isUse);
        /// <summary>
        /// ��ҳ��ȡ�Ծ�
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="sbjid">ѧ��id</param>
        /// <param name="diff">�Ѷȵȼ�</param>
        /// <param name="isUse">�Ƿ�ʹ��</param>
        /// <param name="sear">�������</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        TestPaper[] PaperPager(int orgid, int sbjid, int couid, int diff, bool? isUse, string sear, int size, int index, out int countSum);

        #endregion

        #region �Ծ���������
        /// <summary>
        /// ���γ̳���ʱ���Ծ�������ռ�������
        /// </summary>
        /// <param name="tp">�Ծ�����</param>
        /// <returns></returns>
        TestPaperItem[] GetItemForAll(TestPaper tp);
        /// <summary>
        /// ���½ڳ���ʱ��������ռ��
        /// </summary>
        /// <param name="tp">�Ծ�����</param>
        /// <returns></returns>
        TestPaperItem[] GetItemForOlPercent(TestPaper tp);
        /// <summary>
        /// ���½ڳ���ʱ�����½���������
        /// </summary>
        /// <param name="tp">�Ծ�����</param>
        /// <param name="olid">�½�id�����С��1����ȡ����</param>
        /// <returns></returns>
        TestPaperItem[] GetItemForOlCount(TestPaper tp, int olid);
        /// <summary>
        /// �����Ծ��Ĵ�������ǰ��γ̣����ǰ��½�
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        TestPaperItem[] GetItemForAny(TestPaper tp);
        #endregion

        #region ����
        /// <summary>
        /// ����������Ծ�����
        /// </summary>
        /// <param name="tp">�Ծ�����</param>
        /// <returns></returns>
        Dictionary<TestPaperItem, Questions[]> Putout(TestPaper tp);
        #endregion

        #region �Ծ����ԵĴ���
        /// <summary>
        /// ���Ӳ��Գɼ�,���ص÷�
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>���ص÷�</returns>
        float ResultsAdd(TestResults entity);
        /// <summary>
        /// �޸Ĳ��Գɼ�,���ص÷�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        /// <returns>���ص÷�</returns>
        float ResultsSave(TestResults entity);
        /// <summary>
        /// ��ǰ���Եļ�����
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        float ResultsPassrate(int identify);
        /// <summary>
        /// �ο��˴�
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        int ResultsPersontime(int identify);
        /// <summary>
        /// ������Ծ������в��Ե�ƽ����
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        float ResultsAverage(int identify);
        /// <summary>
        /// ������Ծ������в��Ե���߷�
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        TestResults ResultsHighest(int identify);
        /// <summary>
        /// ������Ծ���ĳ��ѧԱ����߷�
        /// </summary>
        /// <param name="tpid">�Ծ�id</param>
        /// <param name="stid">ѧԱid</param>
        /// <returns></returns>
        double ResultsHighest(int tpid,int stid);
        /// <summary>
        /// ������Ծ������в��Ե���ͷ�
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        TestResults ResultsLowest(int identify);
        /// <summary>
        /// ɾ�����Գɼ���������ID��
        /// </summary>
        /// <param name="identify">�ɼ�id</param>
        void ResultsDelete(int identify);
        /// <summary>
        /// ���ĳ���Ծ���ĳ��ѧԱ�����в��Գɼ�
        /// </summary>
        /// <param name="acid">ѧԱid</param>
        /// <param name="tpid">�Ծ�id</param>
        int ResultsClear(int acid,int tpid);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        TestResults ResultsSingle(int identify);
        /// <summary>
        /// ��ȡĳԱ���Ĳ��Գɼ�
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="couid"></param>
        /// <param name="search"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        TestResults[] ResultsCount(int stid, int couid, string search, int count);
        /// <summary>
        /// ��ȡĳԱ���Ĳ��Գɼ�
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="tpid"></param>    
        /// <returns></returns>
        TestResults[] ResultsCount(int stid, int tpid);
        /// <summary>
        /// ��ҳ��ȡ���Գɼ�
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="sbjid"></param>
        /// <param name="couid"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        TestResults[] ResultsPager(int stid, int sbjid, int couid, int size, int index, out int countSum);
        TestResults[] ResultsPager(int stid, int sbjid, int couid, string sear, int size, int index, out int countSum);
        /// <summary>
        /// ���Ծ���ҳ���ز��Գɼ�
        /// </summary>
        /// <param name="stid">ѧԱid</param>
        /// <param name="tpid">�Ծ�id</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        TestResults[] ResultsPager(int stid, int tpid, int size, int index, out int countSum);
        #endregion


    }
}