using DownloadDocuments.Helpers;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;

namespace DownloadDocuments.Operations
{
    public static class ExcelDownloadDocuments
    {
        public static void CreateExcelAndDownloadDocuments(ClientContext context)
        {
            // CSV file
            Helper.CreateFile(FromAppSettings.ExcelFilePath);
            string excelColumns = GetExcelColumns();
            System.IO.File.WriteAllText(FromAppSettings.ExcelFilePath, excelColumns);

            Folder rootFolder = SPDocLibHelper.GetRootFolderFiles(context, FromAppSettings.SPDocLibTitle);
            SaveDocumentLibraryFiles(context, rootFolder);

            ListItemCollection allDocumentFolders = SPDocLibHelper.GetAllDocumentFolders(context, FromAppSettings.SPDocLibTitle);
            foreach (ListItem folderListItem in allDocumentFolders)
            {
                Folder spFolder = folderListItem.Folder;
                SaveDocumentLibraryFiles(context, spFolder);
            }
        }

        private static string GetExcelColumns()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();
            columns.Add("SharePoint file path", "SharePoint file path");
            columns.Add("System file path", "System file path");
            foreach (KeyValuePair<string, string> valuePair in FromAppSettings.SPExcelFieldNames)
            {
                columns.Add(valuePair.Key, valuePair.Value);
            }

            string excelColumns = string.Join(",", columns.Values) + Environment.NewLine;
            return excelColumns;
        }

        private static bool SaveDocumentLibraryFiles(ClientContext context, Folder spFolder)
        {
            try
            {
                List<string> excelRows = new List<string>();

                FileCollection spFiles = SPDocLibHelper.GetFolderFiles(context, spFolder);
                foreach (File spFile in spFiles)
                {
                    System.IO.Stream fileStream = SPDocLibHelper.GetFileStrean(context, spFile);
                    string fileFullPath = GetFilePath(spFolder, spFile);
                    string filePath = Helper.SaveFile(fileFullPath, fileStream);

                    ListItem listItem = SPDocLibHelper.GetFileListItem(context, spFile);

                    List<string> excelRow = new List<string>();
                    //excelRow.Add(Convert.ToString(listItem.FieldValues["FileDirRef"]));
                    excelRow.Add(spFile.ServerRelativeUrl);
                    excelRow.Add(filePath);
                    foreach (KeyValuePair<string, string> excelFieldName in FromAppSettings.SPExcelFieldNames)
                    {
                        excelRow.Add(Convert.ToString(listItem.FieldValues[excelFieldName.Key]));
                    }

                    excelRows.Add(string.Join(",", excelRow) + Environment.NewLine);
                }

                System.IO.File.AppendAllText(FromAppSettings.ExcelFilePath, string.Join("", excelRows));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Application has been executed. Press ENTER to exit");
                throw ex;
            }

            return true;
        }

        private static string GetFilePath(Folder spFolder, File spFile)
        {
            string urlPath = string.Empty;
            string[] folderUrl = spFolder.ServerRelativeUrl.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < folderUrl.Length; i++)
            {
                urlPath += i == 0 ? "\\" : $"{folderUrl[i]}\\";
            }

            string fileFullPath = $"{FromAppSettings.SaveFolderPath}{urlPath}{spFile.Name}";
            return fileFullPath;
        }

    }
}
