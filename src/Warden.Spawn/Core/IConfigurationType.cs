using System;

namespace Warden.Spawn.Core
{
    public interface IConfigurationType
    {
        Type Type { get; }
        string Name { get; }
    }
}