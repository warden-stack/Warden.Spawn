using System;
using System.Threading.Tasks;

namespace Warden.Spawn.Integrations.Console
{
    public class ConsoleService : IConsoleService
    {
        private readonly ConsoleSpawnIntegrationConfiguration _integrationConfiguration;

        public ConsoleService(ConsoleSpawnIntegrationConfiguration integrationConfiguration)
        {
            _integrationConfiguration = integrationConfiguration;
        }

        public async Task PrintAsync(object configuration)
        {
            var config = configuration as ConsoleSpawnIntegrationHooksConfiguration;
            if (config == null)
                throw new InvalidOperationException();

            var text = string.IsNullOrWhiteSpace(config.Text) ? _integrationConfiguration.DefaultText : config.Text;

            await Task.Factory.StartNew(() => System.Console.WriteLine(text));
        }
    }
}