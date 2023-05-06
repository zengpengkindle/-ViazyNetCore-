using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.DependencyInjection
{
    public interface IObjectAccessor<out T>
    {
        T Value { get; }
    }

    public class ObjectAccessor<T> : IObjectAccessor<T>
    {
        public T Value { get; set; }

        public ObjectAccessor()
        {
        }

        public ObjectAccessor(T obj)
        {
            Value = obj;
        }
    }

}
