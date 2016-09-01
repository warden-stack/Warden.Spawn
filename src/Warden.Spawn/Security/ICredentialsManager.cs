namespace Warden.Spawn.Security
{
    public interface ICredentialsManager
    {
        string Get(string warden, string name, string watcher = "", string integration = "", string hook = "",
            string type = "");

        void Save(string warden, string name, string value, string watcher = "", string integration = "",
            string hook = "", string type = "");

        void Remove(string warden, string name, string watcher = "", string integration = "", string hook = "",
            string type = "");
    }
}