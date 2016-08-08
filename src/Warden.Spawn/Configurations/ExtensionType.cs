using System;

namespace Warden.Spawn.Configurations
{
    public class ExtensionType : IExtensionType
    {
        public Type Type { get; }
        public Type ConfigurationType { get; }
        public string Name { get; }

        public ExtensionType(Type type, Type configurationType, string name)
        {
            Type = type;
            ConfigurationType = configurationType;
            Name = name;
        }
    }
}