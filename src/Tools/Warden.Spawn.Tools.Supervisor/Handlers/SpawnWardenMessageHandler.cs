using System;
using System.Threading.Tasks;
using Rebus.Bus;
using Rebus.Handlers;
using Warden.Spawn.Tools.Core.Messages;
using Warden.Spawn.Tools.Core.Services;

namespace Warden.Spawn.Tools.Supervisor.Handlers
{
    public class SpawnWardenMessageHandler : IHandleMessages<SpawnWardenMessage>
    {
        private readonly IBus _bus;

        public SpawnWardenMessageHandler(IBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(SpawnWardenMessage message)
        {
            Console.WriteLine("Received configuration and spawning new Warden isntance.");
            //TODO: Create Warden instance
        }
    }
}