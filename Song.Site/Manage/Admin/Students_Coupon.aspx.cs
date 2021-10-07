using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;

namespace Song.Site.Manage.Admin
{
    public partial class Students_Coupon : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
            //������������ʾ���
            //this.trPw1.Visible = this.trPw2.Visible = id == 0;
        }
        private void fill()
        {
            Song.Entities.Accounts ea;
            if (id == 0) return;
            ea = Business.Do<IAccounts>().AccountsSingle(id);
            //Ա������
            this.lbName.Text = ea.Ac_Name;
            //�˻����
            lbCoupon.Text = ea.Ac_Coupon.ToString();

        }

        /// <summary>
        /// ��ֵ��۷�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddMoney_Click(object sender, EventArgs e)
        {            
            if (!Extend.LoginState.Admin.IsAdmin) throw new Exception("�ǹ���Ա��Ȩ�˲���Ȩ�ޣ�");
            Song.Entities.Accounts st = Business.Do<IAccounts>().AccountsSingle(id);
            if (st == null) throw new Exception("��ǰ��Ϣ�����ڣ�");
            //��������
            int type = 2;
            int.TryParse(rblOpera.SelectedItem.Value, out type);
            //�������
            int coupon = 0;
            int.TryParse(tbCoupon.Text, out coupon);
            //��������
            Song.Entities.CouponAccount ca = new CouponAccount();
            ca.Ca_Value = coupon;
            ca.Ca_Total = st.Ac_Coupon; //��ǰ��ȯ����
            ca.Ca_Remark = tbRemark.Text.Trim();
            ca.Ac_ID = st.Ac_ID;
            ca.Ca_Source = "����Ա����";
            //��ֵ��ʽ������Ա��ֵ
            ca.Ca_From = 1;
            //������
            Song.Entities.EmpAccount emp = Extend.LoginState.Admin.CurrentUser;
            try
            {
                string mobi = !string.IsNullOrWhiteSpace(emp.Acc_MobileTel) && emp.Acc_AccName != emp.Acc_MobileTel ? emp.Acc_MobileTel : "";
                //����ǳ�ֵ
                if (type == 2)
                {                   
                    ca.Ca_Info = string.Format("����Ա{0}��{1},{2}��������ֵ{3}����ȯ", emp.Acc_Name, emp.Acc_AccName, mobi, coupon);
                    Business.Do<IAccounts>().CouponAdd(ca);
                }
                //�����ת��
                if (type == 1)
                {
                    ca.Ca_Info = string.Format("����Ա{0}��{1},{2}���۳���{3}����ȯ", emp.Acc_Name, emp.Acc_AccName, mobi, coupon);
                    Business.Do<IAccounts>().CouponPay(ca);
                }
                Extend.LoginState.Accounts.Refresh(st.Ac_ID);
                Master.AlertCloseAndRefresh("�����ɹ���");
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
        }

        protected void rblOpera_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList rbl = (RadioButtonList)sender;
            if (rbl.SelectedValue == "1")
            {
                lbOperator.Text = "-";
                if (tbCoupon.Attributes["numlimit"] == null)
                {
                    tbCoupon.Attributes.Add("numlimit", lbCoupon.Text);
                }
                else
                {
                    tbCoupon.Attributes["numlimit"] = lbCoupon.Text;
                }
            }
            if (rbl.SelectedValue == "2")
            {
                lbOperator.Text = "+";
                if (tbCoupon.Attributes["numlimit"] == null)
                {
                    tbCoupon.Attributes.Add("numlimit", "0");
                }
                else
                {
                    tbCoupon.Attributes["numlimit"] = "0";
                }
            }
        }

    }
}
