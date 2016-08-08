namespace Warden.Spawn
{
    public interface IConfigurator<in TConfiguration, out TResult> where TConfiguration : IConfiguration
    {
        TResult Configure(TConfiguration configuration);
    }
}