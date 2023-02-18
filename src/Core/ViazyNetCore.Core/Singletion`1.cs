using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Common
{
    public class Singletion<T> : BaseSingleton
    {
        private static T _instance;

        /// <summary>
        /// The singleton instance for the specified type T. Only one instance (at the time) of this object for each type of T.
        /// </summary>
        public static T Instance
        {
            get => _instance;
            set
            {
                _instance = value;
                AllSingletons[typeof(T)] = value;
            }
        }
    }

    public class BaseSingleton
    {
        static BaseSingleton()
        {
            AllSingletons = new Dictionary<Type, object>();
        }

        /// <summary>
        /// Dictionary of type to singleton instances.
        /// </summary>
        public static IDictionary<Type, object> AllSingletons { get; }
    }
}
