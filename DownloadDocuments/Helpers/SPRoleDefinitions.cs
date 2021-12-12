using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadDocuments.Helpers
{
    public class SPRoleDefinitions
    {
        private Dictionary<string, int> RoleDefinitions { get; set; }

        public SPRoleDefinitions()
        {
            RoleDefinitions = new Dictionary<string, int>
            {
                { "Full Control", 1073741829 },
                { "Design", 1073741828 },
                { "Edit", 1073741830 },
                { "Contribute", 1073741827 },
                { "Read", 1073741826 },
                { "View Only", 1073741924 },
                { "Limited Access", 1073741825 }
            };
        }

        public int GetRoleIdByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || !RoleDefinitions.ContainsKey(name)) { return 0; }
            return RoleDefinitions[name];
        }
    }
}
