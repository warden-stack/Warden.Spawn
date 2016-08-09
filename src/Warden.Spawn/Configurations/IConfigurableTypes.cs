using System;

namespace Warden.Spawn.Configurations
{
    public interface IConfigurableTypes
    {
        Type Configuration { get; }
        Type Configurator { get; }
    }
}