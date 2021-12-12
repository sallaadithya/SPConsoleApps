using DownloadDocuments.Helpers;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadDocuments.Operations
{
    public static class SPLists
    {
        public static void ListItemBreakInheritance(ClientContext ctx, string listTitle, int listItemId)
        {
            ListItem listItem = ctx.Web.Lists.GetByTitle(listTitle).GetItemById(listItemId);
            ctx.Load(listItem, li => li.HasUniqueRoleAssignments);
            RoleDefinitionCollection roleDefinitions = ctx.Site.RootWeb.RoleDefinitions;
            ctx.Load(roleDefinitions); ctx.ExecuteQuery();

            if (!listItem.HasUniqueRoleAssignments)
            {
                listItem.BreakRoleInheritance(false, true);
                listItem.Update();
                ctx.ExecuteQuery();
            }

            RoleDefinition roleDefinition = ctx.Site.RootWeb.RoleDefinitions.GetByName("Read"); //.GetByName(item.RoleDef.ToString());
            RoleDefinitionBindingCollection roleBindings = new RoleDefinitionBindingCollection(ctx) { roleDefinition };
            Principal principal = ctx.Web.SiteGroups.GetById(8);
            listItem.RoleAssignments.Add(principal, roleBindings);
            listItem.Update();
            ctx.ExecuteQuery();
        }
    }
}
