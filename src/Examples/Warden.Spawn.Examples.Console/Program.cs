using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Warden.Spawn.Configurations;
using Warden.Spawn.Hooks;
using Warden.Spawn.Integrations.SendGrid;
using Warden.Spawn.Watchers.Web;

namespace Warden.Spawn.Examples.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var spawnFactory = new WardenSpawnFactory(new WardenSpawnFactoryConfiguration(
                new WardenSpawnJsonConfigurationReader(new List<IExtensionType>
                {
                    WebWatcherSpawn.Type
                },
                    null,
                    new List<IWatcherHookActionResolver>
                    {
                        new WatcherHookActionResolver(new SendGridIntegrationWatcherHooksResolver(),
                            typeof(SendGridIntegrationSpawnConfiguration))
                    }),
                new WardenSpawnConfigurator(new List<IConfigurationWithConfigurator>
                {
                    new ConfigurationWithConfigurator(typeof(WebWatcherSpawnConfiguration),
                        typeof(WebWatcherSpawnConfigurator))
                }),
                File.ReadAllText("configuration.json")));
            var spawn = spawnFactory.Create();
            var warden = spawn.Spawn();
            System.Console.WriteLine($"Warden: '{warden.Name}' has been created and started monitoring.");
            Task.WaitAll(warden.StartAsync());
        }
    }
}
