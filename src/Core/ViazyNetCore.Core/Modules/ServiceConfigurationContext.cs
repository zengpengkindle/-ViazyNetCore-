using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ViazyNetCore;

public class ServiceConfigurationContext
{
    public IServiceCollection Services { get; }

    public IDictionary<string, object> Items { get; }

    /// <summary>
    /// 获取或设置任意命名对象，这些对象可以在服务注册阶段存储并在模块之间共享。
    /// Returns null if given key is not found in the <see cref="Items"/> dictionary.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public object this[string key]
    {
        get => Items.GetOrDefault(key);
        set => Items[key] = value;
    }

    public ServiceConfigurationContext([NotNull] IServiceCollection services)
    {
        Services = Check.NotNull(services, nameof(services));
        Items = new Dictionary<string, object>();
    }
}
