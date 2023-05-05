using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Authorization;

namespace ViazyNetCore.OpenIddict.Domain
{
    public class ViazyIdentityOptions
    {

        /// <summary>
        /// 用户密码加密方式
        /// </summary>
        public UserPasswordFormat UserPasswordFormat { get; set; } = UserPasswordFormat.MD5;

        public ExternalLoginProviderDictionary ExternalLoginProviders { get; }

        public ViazyIdentityOptions()
        {
            ExternalLoginProviders = new ExternalLoginProviderDictionary();
        }
    }
}
