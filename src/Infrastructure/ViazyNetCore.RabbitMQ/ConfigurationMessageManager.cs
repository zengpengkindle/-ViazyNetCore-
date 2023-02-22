using System;
using System.Collections.Concurrent;
using System.Linq;

using Microsoft.Extensions.Configuration;

namespace System.MQueue
{
    class ConfigurationMessageManager : MessageManagerBase
    {
        public const string SECTION_ROOT = "MQueue";
        public const string SECTION_OPTIONS = SECTION_ROOT + ":Options";
        public const string SECTION_CONNECTION_STRINGS = SECTION_ROOT + ":ConnectionStrings";

        private ConcurrentDictionary<string, DefaultConnectionProxyFactory>? _factories;
        private MessageOptions _options = null!;

        protected override ConcurrentDictionary<string, DefaultConnectionProxyFactory>? Factories => this._factories;

        public override MessageOptions Options => this._options;

        public ConfigurationMessageManager(IConfiguration configuration)
        {
            if(configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            this.OnChanged(configuration);
            Microsoft.Extensions.Primitives.ChangeToken.OnChange(configuration.GetReloadToken, this.OnChanged, configuration);
        }

        private void OnChanged(IConfiguration configuration)
        {
            lock(this)
            {
                var firstTime = this._factories is null;

                this.OnChangedOptions(configuration);
                this.OnChangedConnectionStrings(configuration);

                if(firstTime)
                {
                    this.FreeChannelsAsync(this._factories?.Values.ToArray()).RunNew();
                }
                else
                {
                    this.EmitChanged();
                }
            }
        }

        private void OnChangedConnectionStrings(IConfiguration configuration)
        {
            var sectionConnectionStrings = configuration.GetSection(SECTION_CONNECTION_STRINGS);
            if(!sectionConnectionStrings.Exists()) return;

            this._factories = new ConcurrentDictionary<string, DefaultConnectionProxyFactory>(
                sectionConnectionStrings.GetChildren().ToDictionary(section => section.Key, section => new DefaultConnectionProxyFactory(this, section.Key, DefaultConnectionProxyFactory.ParserConnectionString<string>(section.Value))
                , StringComparer.OrdinalIgnoreCase));
        }

        private void OnChangedOptions(IConfiguration configuration)
        {
            var sectionOptions = configuration.GetSection(SECTION_OPTIONS);
            var options = new MessageOptions();
            sectionOptions.Bind(options);
            this._options = options;
        }

    }
}
