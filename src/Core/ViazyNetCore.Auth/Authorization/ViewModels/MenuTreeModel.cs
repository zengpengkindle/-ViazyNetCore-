using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Auth.Authorization.ViewModels
{
    public class MenuTreeModel
    {
        public string Id { get; set; }

        public string Label { get; set; }

        public MenuType Type { get; set; }
        public string Icon { get; set; }

        public List<MenuTreeModel> Children { get; set; }
        public string ParentId { get; set; }
    }
}
