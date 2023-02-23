using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Http
{
    public class EasyHttpProxy
    {
        private readonly WebProxy _proxy;

        private const int DefaultLifeSecond = 60;

        public const string Default = "default";

        public WebProxy Proxy => _proxy;

        /// <summary>
        /// 代理生命周期，默认有效时间   <see cref="DefaultLifeSecond"/>
        /// </summary>
        public TimeSpan Lifetime { get; private set; }
        public EasyHttpProxy(WebProxy proxy)
        {
            _proxy = proxy;

            Lifetime = TimeSpan.FromSeconds(DefaultLifeSecond);
        }

        public EasyHttpProxy(string proxyIp)
        {
            if (proxyIp == Default)
            {
                Lifetime = TimeSpan.FromHours(1);
                return;
            }
            var proxyArr = proxyIp.Split(':');
            if (proxyArr.Length < 2)
                throw new ArgumentException("proxyIp格式有误，形如：ip:port:user:password");
            var proxy = new WebProxy(proxyArr[0], Convert.ToInt32(proxyArr[1]));
            if (proxyArr.Length == 4)
                proxy.Credentials = new NetworkCredential(proxyArr[2], proxyArr[3]);

            _proxy = proxy;

            Lifetime = TimeSpan.FromSeconds(DefaultLifeSecond);
        }

        public EasyHttpProxy()
        {
            Lifetime = TimeSpan.FromHours(1);
        }

        public static implicit operator EasyHttpProxy(WebProxy proxy) => new(proxy);

        public static implicit operator EasyHttpProxy(string proxyIp) => new(proxyIp);

        public static implicit operator string(EasyHttpProxy proxy) => proxy?.ToString() ?? Default;

        public EasyHttpProxy SetLifetime(TimeSpan lifetime)
        {
            Lifetime = lifetime;
            return this;
        }

        public override string ToString()
        {
            if (_proxy == null)
                return Default;
            var proxyStr = $"{_proxy.Address.Host}:{_proxy.Address.Port}";
            if (_proxy.Credentials == null)
                return proxyStr;
            var credentials = (NetworkCredential) _proxy.Credentials;
            proxyStr += $":{credentials.UserName}:{credentials.Password}";
            return proxyStr;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            if (!(obj is EasyHttpProxy))
                return false;
            var proxy = (EasyHttpProxy)obj;
            return ToString()==proxy.ToString();
        }
    }
}
