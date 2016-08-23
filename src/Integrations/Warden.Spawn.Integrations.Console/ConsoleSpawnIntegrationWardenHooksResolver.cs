using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Warden.Spawn.Hooks;

namespace Warden.Spawn.Integrations.Console
{
    public class ConsoleSpawnIntegrationWardenHooksResolver : IWardenHooksResolver
    {
        private readonly ConsoleSpawnIntegrationConfiguration _integrationConfiguration;

        public ConsoleSpawnIntegrationWardenHooksResolver(ConsoleSpawnIntegrationConfiguration integrationConfiguration)
        {
            _integrationConfiguration = integrationConfiguration;
        }

        public Expression<Action<IWardenIteration>> OnIterationCompleted(object configuration)
        {
            var config = configuration as ConsoleSpawnIntegrationHooksConfiguration;
            if (config == null)
                throw new InvalidOperationException();

            var text = string.IsNullOrWhiteSpace(config.Text) ? _integrationConfiguration.DefaultText : config.Text;

            return x => System.Console.WriteLine(text);
        }

        public Expression<Func<IWardenIteration, Task>> OnIterationCompletedAsync(object configuration)
        {
            var config = configuration as ConsoleSpawnIntegrationHooksConfiguration;
            if (config == null)
                throw new InvalidOperationException();

            var text = string.IsNullOrWhiteSpace(config.Text) ? _integrationConfiguration.DefaultText : config.Text;

            return  x => Task.Factory.StartNew(() => System.Console.WriteLine(text));
        }
    }
}