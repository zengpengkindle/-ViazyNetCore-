using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Gateway.Cache
{
    public abstract class CacheEndpoint
    {
        /// <summary>
        /// 主机
        /// </summary>
        public string Host
        {
            get;
            set;
        }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port
        {
            get;
            set;
        }

        public abstract override string ToString();

        #region Equality members

        public override bool Equals(object obj)
        {
            var model = obj as CacheEndpoint;
            if (model == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return model.ToString() == ToString();
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public static bool operator ==(CacheEndpoint model1, CacheEndpoint model2)
        {
            return Equals(model1, model2);
        }

        public static bool operator !=(CacheEndpoint model1, CacheEndpoint model2)
        {
            return !Equals(model1, model2);
        }
        #endregion
    }
}
