using System;

namespace Warden.Spawn.Configurations
{
    public interface IExtensionType
    {
        Type Type { get; }
        Type ConfigurationType { get; }
        string Name { get; }
    }
}