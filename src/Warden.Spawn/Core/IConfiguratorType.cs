using System;

namespace Warden.Spawn.Core
{
    public interface IConfiguratorType
    {
        Type Type { get; }
        string Name { get; }
    }
}