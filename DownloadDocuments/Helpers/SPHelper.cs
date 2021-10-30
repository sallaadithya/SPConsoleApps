using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DownloadDocuments.Helpers
{
    public static class SPHelper
    {
        public static ClientContext GetClientContext(string siteURL, bool isOnPremise)
        {
            ClientContext clientCtx = isOnPremise
                ? GetOnPremiseClientContext(siteURL)
                : GetOnlineClientContext(siteURL);
            return clientCtx;
        }

        public static ClientContext GetOnPremiseClientContext(string siteURL)
        {
            try
            {
                ClientContext clientCtx = new ClientContext(siteURL);
                clientCtx.Credentials = new NetworkCredential(FromAppSettings.SPUserName, FromAppSettings.SPPassword);
                clientCtx.ExecutingWebRequest += clientContext_ExecutingWebRequest;

                //Uri uri = new Uri(siteURL);
                //if (uri.Scheme == "https")
                //{
                //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                //    ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(customXertificateValidation);
                //}

                clientCtx.Load(clientCtx.Web);
                clientCtx.ExecuteQuery();
                Console.WriteLine($"OnPrem site authentication done!! Web title: {clientCtx.Web.Title}");
                return clientCtx;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        public static ClientContext GetOnlineClientContext(string siteURL)
        {
            try
            {
                ClientContext clientCtx = new ClientContext(siteURL);
                clientCtx.Credentials = new SharePointOnlineCredentials(FromAppSettings.SPUserName, Helper.ConvertToSecureString(FromAppSettings.SPPassword));

                clientCtx.Load(clientCtx.Web);
                clientCtx.ExecuteQuery();
                Console.WriteLine($"Online site authentication done!! Web title: {clientCtx.Web.Title}");
                return clientCtx;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        private static void clientContext_ExecutingWebRequest(object sender, WebRequestEventArgs e)
        {
            try
            {
                e.WebRequestExecutor.WebRequest.Headers.Add("X-FORMS_BASED_AUTH_ACCEPTED", "f");
            }
            catch
            {
                throw;
            }
        }
    }
}
