using Warden.Utils;

namespace Warden.Spawn.Configurations.Logger
{
    public class WardenLoggerConfiguration
    {
        public string Type { get; set; }
        public InternalConfiguration Configuration { get; set; }

        public class InternalConfiguration
        {
            public bool UseColors { get; set; }
            public WardenLoggerLevel MinLevel { get; set; }
        }
    }
}