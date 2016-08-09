using System;

namespace Warden.Spawn.Configurations
{
    public interface IExtension
    {
        string Name { get; }
        Type Type { get; }
        Type ConfigurationType { get; }
    }
}