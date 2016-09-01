using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Warden.Spawn.Hooks;
using Warden.Watchers;

namespace Warden.Spawn.Integrations.Console
{
    public class ConsoleSpawnIntegrationAggregatedWatcherHooksResolver : IAggregatedWatcherHooksResolver
    {
        private readonly ConsoleSpawnIntegrationConfiguration _integrationConfiguration;

        public ConsoleSpawnIntegrationAggregatedWatcherHooksResolver(ConsoleSpawnIntegrationConfiguration integrationConfiguration)
        {
            _integrationConfiguration = integrationConfiguration;
        }

        public Expression<Action<IEnumerable<IWatcherCheck>>> OnStart()
        {
            throw new NotImplementedException();
        }

        public Expression<Func<IEnumerable<IWatcherCheck>, Task>> OnStartAsync()
        {
            throw new NotImplementedException();
        }

        public Expression<Action<IEnumerable<IWardenCheckResult>>> OnSuccess()
        {
            throw new NotImplementedException();
        }

        public Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnSuccessAsync()
        {
            throw new NotImplementedException();
        }

        public Expression<Action<IEnumerable<IWardenCheckResult>>> OnFirstSuccess()
        {
            throw new NotImplementedException();
        }

        public Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnFirstSuccessAsync()
        {
            throw new NotImplementedException();
        }

        public Expression<Action<IEnumerable<IWardenCheckResult>>> OnFailure()
        {
            throw new NotImplementedException();
        }

        public Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnFailureAsync()
        {
            throw new NotImplementedException();
        }

        public Expression<Action<IEnumerable<IWardenCheckResult>>> OnCompleted(object configuration)
        {
            var config = configuration as ConsoleSpawnIntegrationHooksConfiguration;
            if (config == null)
                throw new InvalidOperationException();

            var text = string.IsNullOrWhiteSpace(config.Text) ? _integrationConfiguration.DefaultText : config.Text;

            return x => System.Console.WriteLine(text);
        }

        public Expression<Func<IEnumerable<IWardenCheckResult>, Task>> OnCompletedAsync(object configuration)
        {
            var config = configuration as ConsoleSpawnIntegrationHooksConfiguration;
            if (config == null)
                throw new InvalidOperationException();

            var text = string.IsNullOrWhiteSpace(config.Text) ? _integrationConfiguration.DefaultText : config.Text;

            return x => Task.Factory.StartNew(() => System.Console.WriteLine(text));
        }

        public Expression<Action<IEnumerable<Exception>>> OnError()
        {
            throw new NotImplementedException();
        }

        public Expression<Func<IEnumerable<Exception>, Task>> OnErrorAsync()
        {
            throw new NotImplementedException();
        }

        public Expression<Action<IEnumerable<Exception>>> OnFirstError()
        {
            throw new NotImplementedException();
        }

        public Expression<Func<IEnumerable<Exception>, Task>> OnFirstErrorAsync()
        {
            throw new NotImplementedException();
        }
    }
}