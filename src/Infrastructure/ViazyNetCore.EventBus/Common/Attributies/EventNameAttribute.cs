using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore
{
    public interface IEventNameProvider
    {
        string GetName(Type eventType);
    }

    public class EventNameAttribute : Attribute, IEventNameProvider
    {
        public virtual string Name { get; }

        public EventNameAttribute([NotNull] string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        }

        public static string? GetNameOrDefault<TEvent>()
        {
            return GetNameOrDefault(typeof(TEvent));
        }

        public static string? GetNameOrDefault([NotNull] Type eventType)
        {
            Check.NotNull(eventType, nameof(eventType));

            return eventType
                       .GetCustomAttributes(true)
                       .OfType<IEventNameProvider>()
                       .FirstOrDefault()
                       ?.GetName(eventType)
                   ?? eventType.FullName;
        }

        public string GetName(Type eventType)
        {
            return Name;
        }
    }
}
