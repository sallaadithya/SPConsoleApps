using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadDocuments.Helpers
{
    /// <summary>
    /// The SharePoint Document Library helper
    /// </summary>
    public static class SPDocLibHelper
    {
        public static ListItemCollection GetAllDocuments(ClientContext ctx, string docLibTitle)
        {
            ListItemCollection listItems = ctx.Web.Lists.GetByTitle(docLibTitle).GetItems(CamlQuery.CreateAllItemsQuery());
            ctx.Load(listItems);
            ctx.ExecuteQuery();

            return listItems;
        }

        public static ListItemCollection GetAllDocumentFolders(ClientContext ctx, string docLibTitle)
        {
            ListItemCollection listItems = ctx.Web.Lists.GetByTitle(docLibTitle).GetItems(CamlQuery.CreateAllFoldersQuery());
            ctx.Load(listItems, ls => ls.Include(li => li));
            ctx.ExecuteQuery();

            return listItems;
        }

        public static Folder GetRootFolderFiles(ClientContext ctx, string docLibTitle)
        {
            Folder rootFolder = ctx.Web.Lists.GetByTitle(docLibTitle).RootFolder;
            ctx.Load(rootFolder, folder => folder.Files, folder => folder.ServerRelativeUrl);
            ctx.ExecuteQuery();

            return rootFolder;
        }

        public static FileCollection GetFolderFiles(ClientContext ctx, Folder spFolder)
        {
            ctx.Load(spFolder, folder => folder.Files, folder => folder.ServerRelativeUrl);
            ctx.ExecuteQuery();

            return spFolder.Files;
        }

        public static System.IO.Stream GetFileStrean(ClientContext ctx, File spFile)
        {
            ClientResult<System.IO.Stream> ClientResultfileStream = spFile.OpenBinaryStream();
            ctx.ExecuteQuery();
            System.IO.Stream fileStream = ClientResultfileStream.Value;
            return fileStream;
        }

        public static ListItem GetFileListItem(ClientContext ctx, File spFile)
        {
            ctx.Load(spFile, f => f.ListItemAllFields);
            ctx.ExecuteQuery();

            return spFile.ListItemAllFields;
        }
    }
}
