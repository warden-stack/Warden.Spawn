using System;

namespace Warden.Spawn.Configurations
{
    public class Extension : IExtension
    {
        public string Name { get; }
        public Type Type { get; }
        public Type ConfigurationType { get; }

        public Extension(string name, Type type, Type configurationType)
        {
            Name = name;
            Type = type;
            ConfigurationType = configurationType;
        }
    }
}