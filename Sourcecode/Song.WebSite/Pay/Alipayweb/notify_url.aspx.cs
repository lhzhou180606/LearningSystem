﻿using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Collections.Generic;
using Com.Alipayweb;
using WeiSha.Core;
using Song.ServiceInterfaces;

/// <summary>
/// 功能：服务器异步通知页面
/// 版本：3.3
/// 日期：2012-07-10
/// 说明：
/// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
/// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考。
/// 
/// ///////////////////页面功能说明///////////////////
/// 创建该页面文件时，请留心该页面文件中无任何HTML代码及空格。
/// 该页面不能在本机电脑测试，请到服务器上做测试。请确保外部可以访问该页面。
/// 该页面调试工具请使用写文本函数logResult。
/// 如果没有收到该页面返回的 success 信息，支付宝会在24小时内按一定的时间策略重发通知
/// </summary>
public partial class Alipayweb_notify_url : System.Web.UI.Page
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        SortedDictionary<string, string> sPara = GetRequestPost();

        if (sPara.Count > 0)//判断是否有带返回参数
        {
            //支付宝返回的商户订单号，通过它查询交易记录
            string out_trade_no = Request.Form["out_trade_no"];

            Song.Entities.MoneyAccount maccount = Business.Do<IAccounts>().MoneySingle(out_trade_no);
            Notify aliNotify = new Notify(maccount.Pai_ID);
            bool verifyResult = aliNotify.Verify(sPara, Request.Form["notify_id"], Request.Form["sign"]);

            if (verifyResult)//验证成功
            {
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //请在这里加上商户的业务逻辑程序代码


                //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表

                //支付宝交易号
                string trade_no = Request.Form["trade_no"];
                //交易状态
                string trade_status = Request.Form["trade_status"];
                if (Request.Form["trade_status"] == "TRADE_FINISHED")
                {
                    //判断该笔订单是否在商户网站中已经做过处理
                    //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
		    //请务必判断请求时的total_fee、seller_id与通知时获取的total_fee、seller_id为一致的
                    //如果有做过处理，不执行商户的业务程序
                    
                    //注意：
		            //退款日期超过可退款期限后（如三个月可退款），支付宝系统发送该交易状态通知
                }
                else if(Request.Form["trade_status"] == "TRADE_SUCCESS")
                {
                    //判断该笔订单是否在商户网站中已经做过处理
                    //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
		    //请务必判断请求时的total_fee、seller_id与通知时获取的total_fee、seller_id为一致的
                    //如果有做过处理，不执行商户的业务程序

                    //注意：
                    //付款完成后，支付宝系统发送该交易状态通知
                }
                else
                {
                }

                //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——

                //付款方与收款方
                maccount.Ma_Buyer = Request.Form["buyer_email"];
                maccount.Ma_Seller = Request.Form["seller_email"];
                Business.Do<IAccounts>().MoneyConfirm(maccount);

                Response.Write("success");  //请不要修改或删除

                /////////////////////////////////////////////////////////////////////////////////////////////////////////////
            }
            else//验证失败
            {
                Response.Write("fail");
            }
        }
        else
        {
            Response.Write("无通知参数");
        }
    }

    /// <summary>
    /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
    /// </summary>
    /// <returns>request回来的信息组成的数组</returns>
    public SortedDictionary<string, string> GetRequestPost()
    {
        int i = 0;
        SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
        NameValueCollection coll;
        //Load Form variables into NameValueCollection variable.
        coll = Request.Form;

        // Get names of all forms into a string array.
        String[] requestItem = coll.AllKeys;

        for (i = 0; i < requestItem.Length; i++)
        {
            sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
        }

        return sArray;
    }
}
