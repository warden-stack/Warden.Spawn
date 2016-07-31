using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Warden.Spawn.Core;
using Warden.Spawn.Watchers.Web;

namespace Warden.Spawn.Examples.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var watcherConfigurations = new List<IConfigurationType>
            {
                WebWatcherSpawnConfiguration.Type
            };
            var configurationReader = new WardenSpawnJsonConfigurationReader(watcherConfigurations);
            var configurationInput = File.ReadAllText("configuration.json");
            var configuration = configurationReader.Read(configurationInput);

            var watcherConfigurators = new List<IConfiguratorType>
            {
                WebWatcherSpawnConfigurator.Type
            };
            var configurator = new WardenSpawnConfigurator(watcherConfigurators);
            var spawn = configurator.Configure(configuration);
            var warden = spawn.Spawn();

            System.Console.WriteLine($"Warden: '{warden.Name}' has been created and started monitoring.");
            Task.WaitAll(warden.StartAsync());
        }
    }
}
