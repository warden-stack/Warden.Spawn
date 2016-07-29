using System;

namespace Warden.Spawn.Core
{
    public interface IExtension
    {
        string Name { get; }
        ExtensionType Type { get; }
        Type ConfiguratorType { get; }
    }
}