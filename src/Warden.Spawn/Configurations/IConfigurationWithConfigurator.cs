using System;

namespace Warden.Spawn.Configurations
{
    public interface IConfigurationWithConfigurator
    {
        Type ConfigurationType { get; }
        Type ConfiguratorType { get; }
    }
}