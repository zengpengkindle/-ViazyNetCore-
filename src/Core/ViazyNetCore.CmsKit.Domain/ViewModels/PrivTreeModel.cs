using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Auth.Authorization.ViewModels
{
    public class PrivTreeModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string? Path { get; set; }

        public MenuType Type { get; set; }
        public string Icon { get; set; }

        public List<string> Privs { get; set; }

        public List<PrivTreeModel> Children { get; set; }
    }
}
