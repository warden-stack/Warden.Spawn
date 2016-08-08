namespace Warden.Spawn.Integrations.SendGrid
{
    public class SendGridIntegrationSpawnConfiguration
    {
        /// <summary>
        /// Username of the SendGrid account.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password of the SendGrid account.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// API key of the SendGrid account.
        /// </summary>
        public string ApiKey { get; set; }

        public string Sender { get; set; }
    }
}