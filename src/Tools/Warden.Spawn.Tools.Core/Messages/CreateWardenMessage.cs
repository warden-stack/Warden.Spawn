namespace Warden.Spawn.Tools.Core.Messages
{
    public class CreateWardenMessage
    {
        public string Configuration { get; }

        public CreateWardenMessage(string configuration)
        {
            Configuration = configuration;
        }
    }
}
