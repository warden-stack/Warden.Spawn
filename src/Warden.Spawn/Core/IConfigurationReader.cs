namespace Warden.Spawn.Core
{
    public interface IConfigurationReader<out T> where T : IConfiguration
    {
        T Read(string configuration);
    }
}