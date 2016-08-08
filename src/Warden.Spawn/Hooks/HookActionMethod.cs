using System.Collections.Generic;

namespace Warden.Spawn.Hooks
{
    public class HookActionMethod
    {
        public string Name { get; protected set; }
        public IEnumerable<object> Args { get; protected set; }

        protected HookActionMethod()
        {
        }
    }
}