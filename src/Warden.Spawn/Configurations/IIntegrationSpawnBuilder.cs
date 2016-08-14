namespace Warden.Spawn.Configurations
{
    public interface IIntegrationSpawnBuilder
    {
        ISpawnIntegration Build(object configuration);
    }
}