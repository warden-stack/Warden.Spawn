namespace Warden.Spawn.Hooks
{
    public interface IWardenHookSpawnConfiguration
    {
        WardenHookType Type { get; }
        Condition Condition { get; }
        string Use { get; }
        object Configuration { get; }
    }
}