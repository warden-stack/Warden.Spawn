using System;

namespace Warden.Spawn.Hooks
{
    public interface IWatcherHookActionResolver
    {
        string Action { get; }
        IWatcherHooksResolver Resolver { get; }
        Type ConfigurationType { get; }
    }

    public class WatcherHookActionResolver : IWatcherHookActionResolver
    {
        public string Action { get; }
        public IWatcherHooksResolver Resolver { get; }
        public Type ConfigurationType { get; }

        public WatcherHookActionResolver(IWatcherHooksResolver resolver,
            Type configurationType, string action = "")
        {
            Resolver = resolver;
            ConfigurationType = configurationType;
            var contractName = typeof(IWatcherHooksResolver).Name.Substring(1);
            action = string.IsNullOrWhiteSpace(action)
                ? resolver.GetType().Name.Replace(contractName, string.Empty)
                : action;
            Action = action;
        }
    }
}