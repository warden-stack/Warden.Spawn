namespace Warden.Spawn.Security
{
    public interface ICredentialsManager
    {
        string Get(string name);
        void Save(string name, string value);
        void Remove(string name);
        void SetConfiguration(object configuration);
    }
}