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
            var configurationReader = WardenSpawnJsonConfigurationReader
                .Create()
                .WithWatcher(() => WebWatcherSpawn.Extension)
                .WithWatcherHooksResolver(() => SendGridIntegrationWatcherHooksResolver.Resolver)
                .Build();

            var configurator = WardenSpawnConfigurator
                .Create()
                .With(() => WebWatcherSpawn.Type)
                .Build();

            var factory = WardenSpawnFactory
                .Create()
                .WithConfigurationReader(() => configurationReader)
                .WithConfiguration(File.ReadAllText("configuration.json"))
                .WithConfigurator(() => configurator)
                .Build();

            var spawn = factory.Resolve();
            var warden = spawn.Spawn();
            System.Console.WriteLine($"Warden: '{warden.Name}' has been created and started monitoring.");
            Task.WaitAll(warden.StartAsync());
        }
    }
}
