using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules
{
    public interface IApplicationWithExternalServiceProvider: IApplication
    {
        /// <summary>
        /// Sets the service provider, but not initializes the modules.
        /// </summary>
        void SetServiceProvider([NotNull] IServiceProvider serviceProvider);

        /// <summary>
        /// Sets the service provider and initializes all the modules.
        /// If <see cref="SetServiceProvider"/> was called before, the same
        /// <see cref="serviceProvider"/> instance should be passed to this method.
        /// </summary>
        Task InitializeAsync([NotNull] IServiceProvider serviceProvider);

        /// <summary>
        /// Sets the service provider and initializes all the modules.
        /// If <see cref="SetServiceProvider"/> was called before, the same
        /// <see cref="serviceProvider"/> instance should be passed to this method.
        /// </summary>
        void Initialize([NotNull] IServiceProvider serviceProvider);
    }
}
