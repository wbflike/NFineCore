using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NFineCore.Support
{
    public class WxOperatorProvider
    {
        public static WxOperatorProvider Provider
        {
            get { return new WxOperatorProvider(); }
        }
        private string WeixinAppKey = "nfinecore_weixin_app";

        public WxOperatorModel GetCurrent()
        {
            string cookieString = string.Empty;
            StaticHttpContext.Current.Request.Cookies.TryGetValue(WeixinAppKey, out cookieString);
            if (!string.IsNullOrEmpty(cookieString))
            {
                WxOperatorModel wxOperatorModel = new WxOperatorModel();
                wxOperatorModel = DESEncrypt.Decrypt(cookieString).ToObject<WxOperatorModel>();
                return wxOperatorModel;
            }
            else
            {
                return null;
            }
        }
        public void AddCurrent(WxOperatorModel wxOperatorModel)
        {
            StaticHttpContext.Current.Response.Cookies.Append(WeixinAppKey, DESEncrypt.Encrypt(wxOperatorModel.ToJson()));
        }
        public void RemoveCurrent()
        {
            StaticHttpContext.Current.Response.Cookies.Delete(WeixinAppKey);
        }
    }
}
