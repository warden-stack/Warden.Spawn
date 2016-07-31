using System;

namespace Warden.Spawn.Core
{
    public class ConfigurationType : IConfigurationType
    {
        public Type Type { get; }
        public string Name { get; }

        public ConfigurationType(Type type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}