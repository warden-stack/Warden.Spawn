using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Warden.Spawn.Hooks;
using Warden.Watchers;

namespace Warden.Spawn.Integrations.Console
{
    public class ConsoleSpawnIntegrationWatcherHooksResolver : IWatcherHooksResolver
    {
        private readonly ConsoleSpawnIntegrationConfiguration _integrationConfiguration;

        public ConsoleSpawnIntegrationWatcherHooksResolver(ConsoleSpawnIntegrationConfiguration integrationConfiguration)
        {
            _integrationConfiguration = integrationConfiguration;
        }

        public Expression<Action<IWatcherCheck>> OnStart()
        {
            throw new NotImplementedException();
        }

        public Expression<Func<IWatcherCheck, Task>> OnStartAsync()
        {
            throw new NotImplementedException();
        }

        public Expression<Action<IWardenCheckResult>> OnSuccess()
        {
            throw new NotImplementedException();
        }

        public Expression<Func<IWardenCheckResult, Task>> OnSuccessAsync()
        {
            throw new NotImplementedException();
        }

        public Expression<Action<IWardenCheckResult>> OnFirstSuccess()
        {
            throw new NotImplementedException();
        }

        public Expression<Func<IWardenCheckResult, Task>> OnFirstSuccessAsync()
        {
            throw new NotImplementedException();
        }

        public Expression<Action<IWardenCheckResult>> OnFailure()
        {
            throw new NotImplementedException();
        }

        public Expression<Func<IWardenCheckResult, Task>> OnFailureAsync()
        {
            throw new NotImplementedException();
        }

        public Expression<Action<IWardenCheckResult>> OnCompleted(object configuration)
        {
            var config = configuration as ConsoleSpawnIntegrationWatcherHooksConfiguration;
            if (config == null)
                throw new InvalidOperationException();

            var text = string.IsNullOrWhiteSpace(config.Text) ? _integrationConfiguration.DefaultText : config.Text;
            Expression<Action<IWardenCheckResult>> expression =
                x => System.Console.WriteLine(text);

            return expression;
        }

        public Expression<Func<IWardenCheckResult, Task>> OnCompletedAsync(object configuration)
        {
            var config = configuration as ConsoleSpawnIntegrationWatcherHooksConfiguration;
            if (config == null)
                throw new InvalidOperationException();

            var text = string.IsNullOrWhiteSpace(config.Text) ? _integrationConfiguration.DefaultText : config.Text;
            Expression<Func<IWardenCheckResult, Task>> expression =
                x => Task.Factory.StartNew(() => System.Console.WriteLine(text));

            return expression;
        }

        public Expression<Action<Exception>> OnError()
        {
            throw new NotImplementedException();
        }

        public Expression<Func<Exception, Task>> OnErrorAsync()
        {
            throw new NotImplementedException();
        }

        public Expression<Action<Exception>> OnFirstError()
        {
            throw new NotImplementedException();
        }

        public Expression<Func<Exception, Task>> OnFirstErrorAsync()
        {
            throw new NotImplementedException();
        }
    }
}