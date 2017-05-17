using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using WorkChop.Common.ResponseViewModel;
using WorkChop.Common.ViewModel;

namespace WorkChop.App_Helper
{
    public static class AuthToken
    {
        public static string CONTENT_TYPE = @"application/x-www-form-urlencoded";
        public static string POST_METHOD = "POST";
        public static string GET_METHOD = "GET";
        public static string PUT_METHOD = "PUT";

        public static AccessToken PhysmodoToken;
        public static string physmodoAccessToken;

        public static string PhysmodoURL = @ConfigurationManager.AppSettings["AppDomain"];

        public static bool connectHTTP()
        {
            bool rtnFlag = false;

            var tokenUrl = PhysmodoURL + "token";
            var userName = "webdev@physmodo.com";
            var userPassword = "PhysmodoDev123#";
            // var userPassword = "Neetu1234#";  
            var request = string.Format("grant_type=password&username={0}&password={1}", HttpUtility.UrlEncode(userName), HttpUtility.UrlEncode(userPassword));
            PhysmodoToken = HttpPost(tokenUrl, request);
            if (PhysmodoToken != null)
            {
                physmodoAccessToken = PhysmodoToken.access_token;
                //HttpContext.Current.Session["accessToken"] = physmodoAccessToken;
                rtnFlag = true;
                Console.WriteLine("Sucessful log into physmodo web site with " + userName);
            }
            else
            {
                physmodoAccessToken = null;
            }
            return rtnFlag;
        }

        public static AccessToken GetLoingInfo(LoginViewModel model)
        {
            var myrul = HttpContext.Current.Request.Url.AbsoluteUri;
            var tokenUrl = myrul.Replace(HttpContext.Current.Request.Url.AbsolutePath, "/") + "token";
            var request = string.Format("grant_type=password&username={0}&password={1}", HttpUtility.UrlEncode(model.UserName), HttpUtility.UrlEncode(model.Password));
            PhysmodoToken = HttpPost(tokenUrl, request);
            return PhysmodoToken;
        }
        public static AccessToken HttpPost(string tokenUrl, string requestDetails)
        {
            AccessToken token = null;

            try
            {
                WebRequest webRequest = WebRequest.Create(tokenUrl);
                webRequest.ContentType = CONTENT_TYPE;
                webRequest.Method = POST_METHOD;
                byte[] bytes = Encoding.ASCII.GetBytes(requestDetails);
                webRequest.ContentLength = bytes.Length;
                using (Stream outputStream = webRequest.GetRequestStream())
                {
                    outputStream.Write(bytes, 0, bytes.Length);
                }
                using (WebResponse webResponse = webRequest.GetResponse())
                {
                    StreamReader newstreamreader = new StreamReader(webResponse.GetResponseStream());
                    string newresponsefromserver = newstreamreader.ReadToEnd();
                    token = Newtonsoft.Json.JsonConvert.DeserializeObject<AccessToken>(newresponsefromserver);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                token = null;
            }

            return token;
        }
    }
}