using Warden.Integrations;

namespace Warden.Spawn
{
    public class EmptyIntegration : IIntegration
    {
        protected EmptyIntegration()
        {
        }

        public static EmptyIntegration Instance => new EmptyIntegration();
    }
}