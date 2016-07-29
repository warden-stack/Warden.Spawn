using System;

namespace Warden.Spawn.Core
{
    public class Extension : IExtension
    {
        public string Name { get; }
        public ExtensionType Type { get; }
        public Type ConfiguratorType { get; }

        public Extension(string name, ExtensionType type, Type configuratorType)
        {
            Name = name;
            Type = type;
            ConfiguratorType = configuratorType;
        }
    }
}