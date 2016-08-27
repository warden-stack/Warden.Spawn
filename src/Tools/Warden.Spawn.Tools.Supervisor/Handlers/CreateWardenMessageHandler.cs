using System;
using System.Threading.Tasks;
using Rebus.Handlers;
using Warden.Spawn.Tools.Core.Messages;

namespace Warden.Spawn.Tools.Supervisor.Handlers
{
    public class CreateWardenMessageHandler : IHandleMessages<CreateWardenMessage>
    {
        public async Task Handle(CreateWardenMessage message)
        {
            Console.WriteLine($"The configuration is:\n{message.Configuration}");
        }
    }
}