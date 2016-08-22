using System;
using System.Linq;
using Warden.Spawn.Security;

namespace Warden.Spawn.Extensions.Security
{
    public class CredentialsConfigurator : ICredentialsConfigurator
    {
        private readonly ICredentialsManager _credentialsManager;

        public CredentialsConfigurator(ICredentialsManager credentialsManager)
        {
            _credentialsManager = credentialsManager;
        }

        public void SetConfiguration(string warden, object configuration, string watcher = "", string integration = "", string hook = "")
        {
            var configurationType = configuration.GetType();
            var properties = configurationType.GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(CredentialAttribute)));
            if (!properties.Any())
                return;

            foreach (var property in properties)
            {
                var name = $"{configurationType.Name}.{property.Name}";
                var value = _credentialsManager.Get(warden, name, watcher, integration, hook);
                if (string.IsNullOrWhiteSpace(value))
                    continue;

                property.SetValue(configuration, value);
            }
        }
    }
}