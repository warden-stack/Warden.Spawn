using System.Collections.Generic;
using Warden.Spawn.Core;
using Warden.Spawn.Watchers.Web;

namespace Warden.Spawn.Examples.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var watcherConfigurations = new List<IWatcherSpawnConfiguration>
            {
                new WebWatcherSpawnConfiguration("http://www.google.com")
            };
            var integrationConfigurations = new List<IIntegrationSpawnConfiguration>();
            var spawnConfiguration = new WardenSpawnConfiguration("Warden Spawn",
                watcherConfigurations, integrationConfigurations);
            var spawnConfigurator = new WardenSpawnConfigurator();
            var spawn = spawnConfigurator.Configure(spawnConfiguration);
            var warden = spawn.Spawn();
            System.Console.WriteLine($"Warden: '{warden.Name}' has been created.");
        }
    }
}
