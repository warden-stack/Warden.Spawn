namespace Warden.Spawn.Configurations.Logger
{
    public class WardenLoggerConfiguration
    {
        public string Type { get; set; }
        public InternalConfiguration Configuration { get; set; }

        public class InternalConfiguration
        {
            public bool UseColors { get; set; }

            //TODO: Change type of the min level of Warden Logger.
            public string MinLevel { get; set; }
        }
    }
}