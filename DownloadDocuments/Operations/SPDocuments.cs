using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadDocuments.Operations
{
    /// <summary>
    /// Helpers on SharePoint document library
    /// </summary>
    public static class SPDocuments
    {
        public static void UploadDocuments(ClientContext ctx, string spDocumentLibraryTitle, string filePath)
        {
            List spList = ctx.Web.Lists.GetByTitle(spDocumentLibraryTitle);
            Folder spFolder = spList.RootFolder;
            File newFile = spFolder.Files.Add(new FileCreationInformation()
            {

            });
        }

        public static void UploadDocuments(ClientContext ctx, string spDocumentLibraryTitle, string folderName, string filePath)
        {
            List docs = ctx.Web.Lists.GetByTitle(spDocumentLibraryTitle);
            Folder spFolder = docs.RootFolder.EnsureFolder(folderName);
            using (System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open))
            {
                FileCreationInformation flciNewFile = new FileCreationInformation();
                flciNewFile.ContentStream = fs;
                flciNewFile.Url = System.IO.Path.GetFileName(filePath);
                flciNewFile.Overwrite = true;

                File uploadFile = spFolder.Files.Add(flciNewFile);

                ctx.Load(uploadFile);
                ctx.ExecuteQuery();
            }
        }
    }
}
