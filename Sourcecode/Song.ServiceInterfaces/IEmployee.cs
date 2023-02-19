using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;
using WeiSha.Data;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// ԺϵԱ���Ĺ���
    /// </summary>
    public interface IEmployee : WeiSha.Core.IBusinessInterface
    {
        /// <summary>
        /// ���
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        int Add(EmpAccount entity);
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void Save(EmpAccount entity);
        /// <summary>
        /// �޸�ĳ���ֶ�
        /// </summary>
        /// <param name="acid"></param>
        /// <param name="fiels"></param>
        /// <param name="objs"></param>
        void Update(int acid, Field[] fiels, object[] objs);
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void Delete(EmpAccount entity);
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
        EmpAccount GetSingle(int identify);
        /// <summary>
        /// �����˺ŵ�¼
        /// </summary>
        /// <param name="acc">�˺ţ������֤�����ֻ�</param>
        /// <param name="pw">����</param>
        /// <param name="orgid">����Ա���ڵĻ���id�����С�ڵ����㣬ȡ���л����Ĺ���Ա</param>
        /// <param name="posid">��λ��id</param>
        /// <returns></returns>
        EmpAccount EmpLogin(string acc, string pw, int orgid,int posid);
        /// <summary>
        /// ���ڼ�¼ÿ�ε�¼���ɵ���֤�룬���ڣ�ͬһ�˺ŵ�¼ʱ����ǰ�˺�����
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        bool RecordLoginCode(int accid, string code);
        /// <summary>
        /// �޸�����
        /// </summary>
        /// <param name="accid">�˺�</param>
        /// <param name="oldpw"></param>
        /// <param name="newpw"></param>
        /// <returns></returns>
        bool ChangePw(int accid, string oldpw, string newpw);
        /// <summary>
        /// ���ݹ�˾id��ȡ����˾�Ĺ���Ա
        /// </summary>
        /// <param name="orgid">��˾id</param>
        /// <returns></returns>
        EmpAccount GetAdminByOrgId(int orgid);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�Ա���ֻ�����
        /// </summary>
        /// <param name="phoneNumber">�ֻ���</param>
        /// <returns></returns>
        EmpAccount GetSingleByPhone(string phoneNumber);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�Ա������
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        EmpAccount GetSingleByName(string name);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�Ա���ʺ�����������
        /// </summary>
        /// <param name="acc">Ա���ʺ�����</param>
        /// <param name="pw">Ա������,MD5�����ַ���</param>
        /// <returns></returns>
        EmpAccount GetSingle(string acc, string pw);
        EmpAccount GetSingle(int orgid, string acc, string pw);
        /// <summary>
        /// ��ȡ��ǰԱ�����ڵ�Ժϵ
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        Depart Get4Depart(int identify);
        /// <summary>
        /// ��ǰԱ���Ƿ�Ϊ��������Ա
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        bool IsSuperAdmin(int identify);
        /// <summary>
        /// ��ǰԱ���Ƿ�Ϊ������Ա��
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        bool IsForRoot(int identify);
        /// <summary>
        /// ��ǰ�û��Ƿ�Ϊ��������Ա
        /// </summary>
        /// <param name="acc">��ǰ�û�����</param>
        /// <returns></returns>
        bool IsSuperAdmin(EmpAccount acc);
        /// <summary>
        /// ��ǰԱ���Ƿ�Ϊ����Ա
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        bool IsAdmin(int identify);
        /// <summary>
        /// ��ǰԱ���Ƿ���ڣ�ͨ���ʺ��жϣ�
        /// </summary>
        /// <param name="accname">�˺�����</param>
        /// <param name="accid">�˺�id</param>
        /// <returns>����Ѿ����ڣ��򷵻�true</returns>
        bool IsExists(string accname, int accid);
        /// <summary>
        /// ��֤�ܷ��¼
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="accname">Ա���ʺ�</param>
        /// <param name="pw">����</param>
        /// <returns></returns>
        bool LoginCheck(int orgid, string accname, string pw);
        /// <summary>
        /// ͨ���ֻ�������֤����ǰԱ���Ƿ�Ϊ��ְԱ��
        /// </summary>
        /// <param name="phoneNumber">�ֻ���</param>
        /// <returns></returns>
        bool IsOnJob(string phoneNumber);
        /// <summary>
        /// ��ȡ���󣻼�����Ա����
        /// </summary>
        /// <returns></returns>
        EmpAccount[] GetAll(int orgid);        

        EmpAccount[] GetAll(int orgid, int depId, bool? isUse, string searTxt);
        /// <summary>
        /// ��ȡĳ���ֳ�������Ա���ʺţ�
        /// </summary>
        /// <param name="orgid">�ֳ�id</param>
        /// <param name="isUse"></param>
        /// <param name="searTxt">Ա������</param>
        /// <returns></returns>
        EmpAccount[] GetAll4Org(int orgid, bool? isUse, string searTxt);

        /// <summary>
        /// ��ҳ��ȡ���е�Ա���ʺţ�
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="posi">��λid</param>
        /// <param name="name"></param>
        /// <param name="size">ÿҳ��ʾ������¼</param>
        /// <param name="index">��ǰ�ڼ�ҳ</param>
        /// <param name="countSum">��¼����</param>
        /// <returns></returns>
        EmpAccount[] GetPager(int orgid,int posi,string name, int size, int index, out int countSum);        

        #region ְ��ͷ�Σ�����
        /// <summary>
        /// ���
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void TitileAdd(EmpTitle entity);
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void TitleSave(EmpTitle entity);
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void TitleDelete(EmpTitle entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void TitleDelete(int identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        EmpTitle TitleSingle(int identify);
        /// <summary>
        /// ��ȡ���󣻼�����ְλ��
        /// </summary>
        /// <returns></returns>
        EmpTitle[] TitleAll(int orgid);
        EmpTitle[] TitleAll(int orgid, bool? isUse);
        /// <summary>
        /// ��ҳ��ȡְ����Ϣ
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isUse"></param>
        /// <param name="name"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        EmpTitle[] TitlePager(int orgid, bool? isUse, string name, int size, int index, out int countSum);
        /// <summary>
        /// ��ȡ��ǰְ�������Ա��
        /// </summary>
        /// <param name="titleid">ְ��Id</param>
        /// <param name="isUse">�Ƿ���ְ</param>
        /// <returns></returns>
        EmpAccount[] Title4Emplyee(int titleid, bool? isUse);
        /// <summary>
        /// ��ǰ���������Ƿ�����
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        bool TitleIsExist(string name, int id, int orgid);
        /// <summary>
        /// ����ְ�������
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        bool UpdateTitleTaxis(EmpTitle[] entities);      
        #endregion


    }
}
