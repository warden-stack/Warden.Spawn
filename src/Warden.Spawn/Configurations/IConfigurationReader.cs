namespace Warden.Spawn.Configurations
{
    public interface IConfigurationReader<out T> where T : IConfiguration
    {
        T Read(string configuration);
    }
}