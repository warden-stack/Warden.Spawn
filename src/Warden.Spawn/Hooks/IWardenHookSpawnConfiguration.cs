namespace Warden.Spawn.Hooks
{
    public interface IWardenHookSpawnConfiguration
    {
        WardenHookType Type { get; }
        HookCondition Condition { get; }
        string Use { get; }
        object Configuration { get; }
    }
}