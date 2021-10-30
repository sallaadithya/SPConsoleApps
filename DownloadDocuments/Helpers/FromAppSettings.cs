using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadDocuments.Helpers
{
    public static class FromAppSettings
    {
        private static Dictionary<string, string> spExcelFieldNames = new Dictionary<string, string>();

        private static string excelFilePath = string.Empty;

        public static string SPSiteURL => ConfigurationManager.AppSettings["SPSiteURL"];

        public static bool SPIsSiteOnPremise => Convert.ToBoolean(ConfigurationManager.AppSettings["SPIsSiteOnPremise"]);

        public static string SPUserName => ConfigurationManager.AppSettings["SPUserName"];

        public static string SPPassword => ConfigurationManager.AppSettings["SPPassword"];

        public static string SPDocLibTitle => ConfigurationManager.AppSettings["SPDocumentLibraryTitle"];

        public static string SaveFolderPath => ConfigurationManager.AppSettings["SaveFolderPath"];

        public static Dictionary<string, string> SPExcelFieldNames
        {
            get
            {
                string excelFields = ConfigurationManager.AppSettings["SPExcelFieldInternalNames"];
                if (!string.IsNullOrWhiteSpace(excelFields) && spExcelFieldNames.Count == 0)
                {
                    spExcelFieldNames = excelFields.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToDictionary(f => f.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0], f => f.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[1]);
                }

                return spExcelFieldNames;
            }
        }

        public static string ExcelFilePath
        {
            get
            {

                if (string.IsNullOrWhiteSpace(excelFilePath))
                {
                    excelFilePath = $"{FromAppSettings.SaveFolderPath}\\{FromAppSettings.SPDocLibTitle}_{DateTime.Now.ToString("ddMMMyyyyhhMMss")}.csv";
                }

                return excelFilePath;
            }
        }
    }
}


