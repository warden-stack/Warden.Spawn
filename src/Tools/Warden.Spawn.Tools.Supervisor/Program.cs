using System;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.Transport.Msmq;
using Rebus.Logging;
using Warden.Shared.Messages;
using Warden.Spawn.Tools.Supervisor.Handlers;

namespace Warden.Spawn.Tools.Supervisor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var activator = new BuiltinHandlerActivator())
            {
                Console.Title = "Warden.Spawn.Tools.Supervisor";
                activator.Register((bus, message) => new SpawnWardenMessageHandler(bus));
                Configure.With(activator)
                    .Logging(l => l.ColoredConsole(minLevel: LogLevel.Warn))
                    .Transport(t => t.UseMsmq("warden-supervisor"))
                    .Routing(r => r.TypeBased().MapAssemblyOf<SpawnWardenMessage>("warden-orchestrator"))
                    .Start();

                activator.Bus.Subscribe<SpawnWardenMessage>().Wait();
                Console.WriteLine("Press enter to quit");
                Console.ReadLine();
            }
        }
    }
}
