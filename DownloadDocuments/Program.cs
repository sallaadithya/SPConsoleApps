using DownloadDocuments.Helpers;
using DownloadDocuments.Operations;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadDocuments
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (ClientContext context = SPHelper.GetClientContext(FromAppSettings.SPSiteURL, FromAppSettings.SPIsSiteOnPremise))
                {
                    //ExcelDownloadDocuments.CreateExcelAndDownloadDocuments(context);
                    string folderName = DateTime.Now.ToString("ddMMyyyy_hh_mm");
                    string filePath = @"C:\Users\IJ\Documents\Sample docs\Lorem Ipsum.docx";
                    SPDocuments.UploadDocuments(context, FromAppSettings.SPDocLibTitle, folderName, filePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception occured. Message: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Application has been executed. Press ENTER to exit");
                Console.ReadKey();
            }
        }
    }
}
