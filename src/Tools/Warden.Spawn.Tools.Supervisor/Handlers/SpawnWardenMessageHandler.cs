using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Rebus.Bus;
using Rebus.Handlers;
using Warden.Shared.Messages;

namespace Warden.Spawn.Tools.Supervisor.Handlers
{
    public class SpawnWardenMessageHandler : IHandleMessages<SpawnWardenMessage>
    {
        private readonly IBus _bus;
        private readonly string _wardenHostPath;

        public SpawnWardenMessageHandler(IBus bus, string wardenHostPath)
        {
            _bus = bus;
            _wardenHostPath = wardenHostPath;
        }

        public async Task Handle(SpawnWardenMessage message)
        {
            Console.WriteLine("Received configuration and spawning new Warden instance.");
            var process = new Process
            {
                StartInfo =
                {
                    FileName = _wardenHostPath,
                    Arguments = $"{message.ConfigurationId} {message.Token}",
                    WindowStyle = ProcessWindowStyle.Normal
                }
            };
            process.Start();
            Console.WriteLine($"Warden instance has been created. PID: {process.Id}.");
        }
    }
}