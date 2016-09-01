namespace Warden.Spawn.Security
{
    public interface ICredentialsConfigurator
    {
        void SetConfiguration(string warden, object configuration, string watcher = "", string integration = "",
            string hook = "", string type = "");
    }
}