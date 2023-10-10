using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Identity.Domain
{
    public class UserUpdateDto
    {
        public string NickName { get; set; }

        public string? ExtraData { get; set; }
    }
}
