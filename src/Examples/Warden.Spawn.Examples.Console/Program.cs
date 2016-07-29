using System;
using System.Collections.Generic;
using System.IO;
using Warden.Spawn.Core;
using Warden.Spawn.Watchers.Web;

namespace Warden.Spawn.Examples.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var watcherConfigurationTypes = new Dictionary<string, Type>
            {
                ["Web"] = typeof(WebWatcherSpawnConfiguration)
            };
            var integrationConfigurationTypes = new Dictionary<string, Type>();
            var spawnConfigurationReader = new WardenSpawnJsonConfigurationReader(watcherConfigurationTypes, integrationConfigurationTypes);
            var configurationFile = File.ReadAllText("configuration.json");
            var spawnConfiguration = spawnConfigurationReader.Read(configurationFile);
            var spawnConfigurator = new WardenSpawnConfigurator();
            var spawn = spawnConfigurator.Configure(spawnConfiguration);
            var warden = spawn.Spawn();
            System.Console.WriteLine($"Warden: '{warden.Name}' has been created.");
        }
    }
}
