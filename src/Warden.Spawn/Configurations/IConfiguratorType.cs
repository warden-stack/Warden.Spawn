using System;

namespace Warden.Spawn.Configurations
{
    public interface IConfiguratorType
    {
        Type Type { get; }
        string Name { get; }
    }
}