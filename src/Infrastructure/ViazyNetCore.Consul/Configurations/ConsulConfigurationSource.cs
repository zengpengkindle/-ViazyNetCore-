﻿using Microsoft.Extensions.Configuration;
 

namespace ViazyNetCore.Consul.Configurations
{
    public class ConsulConfigurationSource : FileConfigurationSource
    {
        public string ConfigurationKeyPrefix { get; set; }

        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            FileProvider = FileProvider ?? builder.GetFileProvider();
            return new ConsulConfigurationProvider(this);
        }
    }
}