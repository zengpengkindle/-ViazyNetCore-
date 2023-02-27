using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace ViazyNetCore.Authorization.Modules
{
    public interface IUser<TKey>
    {
        public TKey Id { get; set; }

        public string Username { get; set; }
        public string Nickname { get; set; }

        public int IdentityType { get; }
        /// <summary>
        /// 是否被管制
        /// </summary>
        bool IsModerated { get; }
    }

    public interface IUser : IUser<string>
    { }

    public class SimpleUser : IUser<string>
    {
        public string Id { get; set; }

        public string Nickname { get; set; }

        public string Username { get; set; }

        public bool IsModerated { get; set; }

        public int IdentityType { get; } = 0;
    }
}
