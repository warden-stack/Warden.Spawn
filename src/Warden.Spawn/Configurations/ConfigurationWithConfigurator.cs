using System;

namespace Warden.Spawn.Configurations
{
    public class ConfigurationWithConfigurator : IConfigurationWithConfigurator
    {
        public Type ConfigurationType { get; }
        public Type ConfiguratorType { get; }

        public ConfigurationWithConfigurator(Type configurationType, Type configuratorType)
        {
            ConfigurationType = configurationType;
            ConfiguratorType = configuratorType;
        }
    }
}