using System;

namespace Warden.Spawn.Configurations
{
    public class ConfigurableTypes : IConfigurableTypes
    {
        public Type Configuration { get; }
        public Type Configurator { get; }

        public ConfigurableTypes(Type configuration, Type configurator)
        {
            Configuration = configuration;
            Configurator = configurator;
        }
    }
}