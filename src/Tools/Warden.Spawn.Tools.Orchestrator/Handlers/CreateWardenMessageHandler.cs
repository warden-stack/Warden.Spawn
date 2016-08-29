using System;
using System.Threading.Tasks;
using Rebus.Bus;
using Rebus.Handlers;
using Warden.Shared.Messages;

namespace Warden.Spawn.Tools.Orchestrator.Handlers
{
    public class CreateWardenMessageHandler : IHandleMessages<CreateWardenMessage>
    {
        private readonly IBus _bus;

        public CreateWardenMessageHandler(IBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(CreateWardenMessage message)
        {
            Console.WriteLine("Received configuration and passing it further.");
            await _bus.Publish(new SpawnWardenMessage(message.ConfigurationId, message.Token));
        }
    }
}