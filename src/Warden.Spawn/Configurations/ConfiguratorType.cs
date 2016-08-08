using System;

namespace Warden.Spawn.Configurations
{
    public class ConfiguratorType : IConfiguratorType
    {
        public Type Type { get; }
        public string Name { get; }

        public ConfiguratorType(Type type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}