using System;

namespace Warden.Spawn.Hooks
{
    public interface IWatcherHookResolver
    {
        IWatcherHooksResolver Resolver { get; }
        Type ConfigurationType { get; }
        string Use { get; }
    }

    public class WatcherHookResolver : IWatcherHookResolver
    {
        public IWatcherHooksResolver Resolver { get; }
        public Type ConfigurationType { get; }
        public string Use { get; }

        public WatcherHookResolver(IWatcherHooksResolver resolver,
            Type configurationType, string use = "")
        {
            Resolver = resolver;
            ConfigurationType = configurationType;
            var contractName = typeof(IWatcherHooksResolver).Name.Substring(1);
            use = string.IsNullOrWhiteSpace(use)
                ? resolver.GetType().Name.Replace(contractName, string.Empty)
                : use;
            Use = use;
        }
    }
}