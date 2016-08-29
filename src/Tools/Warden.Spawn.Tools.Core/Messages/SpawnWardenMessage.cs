namespace Warden.Spawn.Tools.Core.Messages
{
    public class SpawnWardenMessage
    {
        public string ConfigurationId { get; }
        public string Token { get; }

        public SpawnWardenMessage(string configurationId, string token)
        {
            ConfigurationId = configurationId;
            Token = token;
        }
    }
}
