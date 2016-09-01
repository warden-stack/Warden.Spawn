using System.Collections.Generic;

namespace Warden.Spawn.Integrations.SendGrid
{
    public class SendGridSpawnIntegrationHooksConfiguration
    {
        public string Message { get; set; }
        public string Subject { get; set; }
        public IEnumerable<string> Receivers { get; set; }
    }
}