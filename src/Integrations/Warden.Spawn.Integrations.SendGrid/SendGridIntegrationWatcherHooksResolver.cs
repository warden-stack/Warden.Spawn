using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Warden.Integrations.SendGrid;
using Warden.Spawn.Hooks;
using Warden.Watchers;

namespace Warden.Spawn.Integrations.SendGrid
{
    public class SendGridIntegrationWatcherHooksResolver : IWatcherHooksResolver
    {
        public static IWatcherHookResolver Resolver
            => new WatcherHookResolver(new SendGridIntegrationWatcherHooksResolver(),
                typeof(SendGridIntegrationSpawnConfiguration));

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
            var config = configuration as SendGridIntegrationSpawnConfiguration;
            if (config == null)
                return null;

            var sendGridConfiguration = SendGridIntegrationConfiguration.Create(config.ApiKey, config.Sender).Build();
            var integration = new SendGridIntegration(sendGridConfiguration);
            Expression<Action<IWardenCheckResult>> expression =
                x => Task.WaitAll(integration.SendEmailAsync(x.WatcherCheckResult.Description));

            return expression;
        }

        public Expression<Func<IWardenCheckResult, Task>> OnCompletedAsync(object configuration)
        {
            var config = configuration as SendGridIntegrationSpawnConfiguration;
            if (config == null)
                return null;

            var sendGridConfiguration = SendGridIntegrationConfiguration.Create(config.ApiKey, config.Sender).Build();
            var integration = new SendGridIntegration(sendGridConfiguration);
            Expression<Func<IWardenCheckResult, Task>> expression =
                x => integration.SendEmailAsync(x.WatcherCheckResult.Description);

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