using System.Collections.Generic;
using Warden.Spawn.Configurations;
using Warden.Spawn.Security;

namespace Warden.Spawn.Integrations.SendGrid
{
    public class SendGridSpawnIntegrationConfiguration : ISpawnIntegrationConfiguration
    {
        /// <summary>
        /// Username of the SendGrid account.
        /// </summary>
        [Credential]
        public string Username { get; set; }

        /// <summary>
        /// Password of the SendGrid account.
        /// </summary>
        [Credential]
        public string Password { get; set; }

        /// <summary>
        /// API key of the SendGrid account.
        /// </summary>
        [Credential]
        public string ApiKey { get; set; }

        public string Sender { get; set; }

        public string DefaultMessage { get; set; }
        public string DefaultSubject { get; set; }
        public IEnumerable<string> DefaultReceivers { get; set; }
    }
}