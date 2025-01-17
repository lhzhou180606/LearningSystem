﻿using System.Web;
using System.Text;
using System.IO;
using System.Net;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WeiSha.Core;
using Song.ServiceInterfaces;
using Song.Entities;
namespace Com.Alipaywap
{
    /// <summary>
    /// 类名：Config
    /// 功能：基础配置类
    /// 详细：设置帐户有关信息及返回路径
    /// 版本：3.3
    /// 日期：2012-07-05
    /// 说明：
    /// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
    /// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考。
    /// 
    /// 如何获取安全校验码和合作身份者ID
    /// 1.用您的签约支付宝账号登录支付宝网站(www.alipay.com)
    /// 2.点击“商家服务”(https://b.alipay.com/order/myOrder.htm)
    /// 3.点击“查询合作者身份(PID)”、“查询安全校验码(Key)”
    /// </summary>
    public class Config
    {
        #region 字段
        private string partner = "";
        private string seller_id = "";
        private string private_key = "";
        private string public_key = "";
        private string input_charset = "";
        private string sign_type = "";
        #endregion

        /// <summary>
        /// 配置项
        /// </summary>
        /// <param name="paiid">系统中的接口ID</param>
        public Config(Song.Entities.PayInterface payInterface)
        {
            _init(payInterface);
        }
        public Config(int paiid)
        {
            Song.Entities.PayInterface payInterface = Business.Do<IPayInterface>().PaySingle(paiid);
            _init(payInterface);
        }

        private void _init(Song.Entities.PayInterface payInterface)
        {
            //↓↓↓↓↓↓↓↓↓↓请在这里配置您的基本信息↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

            //合作身份者ID，以2088开头由16位纯数字组成的字符串
            partner = payInterface.Pai_ParterID;

            //收款支付宝账号，以2088开头由16位纯数字组成的字符串
            seller_id = partner;

            //商户的私钥
            WeiSha.Core.CustomConfig config = CustomConfig.Load(payInterface.Pai_Config);
            private_key = config["Privatekey"].Value.String;
            private_key = Regex.Replace(private_key, @"\r|\n|\s", "", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);

            //支付宝的公钥，无需修改该值
            public_key = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCnxj/9qwVfgoUh/y2W89L6BkRAFljhNhgPdyPuBV64bfQNN1PjbCzkIM6qRdKBoLPXmKKMiFYnkd6rAoprih3/PrQEB/VsW8OoM8fxn67UDYuyBTqA23MML9q1+ilIZwBC2AQ2UBVOrFXfFl75p6/B5KsiNG9zpgmLCUYuLkxpLQIDAQAB";

            //↑↑↑↑↑↑↑↑↑↑请在这里配置您的基本信息↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑

            //字符编码格式 目前支持 gbk 或 utf-8
            input_charset = "utf-8";
            //签名方式，选择项：RSA、DSA、MD5
            sign_type = "RSA";
        }
        #region 属性
        /// <summary>
        /// 获取或设置合作者身份ID
        /// </summary>
        public string Partner
        {
            get { return partner; }
            set { partner = value; }
        }

        /// <summary>
        /// 获取或设置合作者身份ID
        /// </summary>
        public string Seller_id
        {
            get { return seller_id; }
            set { seller_id = value; }
        }

        /// <summary>
        /// 获取或设置商户的私钥
        /// </summary>
        public string Private_key
        {
            get { return private_key; }
            set { private_key = value; }
        }

        /// <summary>
        /// 获取或设置支付宝的公钥
        /// </summary>
        public string Public_key
        {
            get { return public_key; }
            set { public_key = value; }
        }

        /// <summary>
        /// 获取字符编码格式
        /// </summary>
        public string Input_charset
        {
            get { return input_charset; }
        }

        /// <summary>
        /// 获取签名方式
        /// </summary>
        public string Sign_type
        {
            get { return sign_type; }
        }
        #endregion
    }
}