using System;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Logging;
using Rebus.Routing.TypeBased;
using Rebus.Transport.Msmq;
using Warden.Shared.Messages;
using Warden.Spawn.Tools.Orchestrator.Handlers;

namespace Warden.Spawn.Tools.Orchestrator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var activator = new BuiltinHandlerActivator())
            {
                Console.Title = "Warden.Spawn.Tools.Orchestrator";
                activator.Register((bus, message) => new CreateWardenMessageHandler(bus));
                Configure.With(activator)
                    .Logging(l => l.ColoredConsole(minLevel: LogLevel.Warn))
                    .Transport(t => t.UseMsmq("warden-orchestrator"))
                    .Routing(r => r.TypeBased().MapAssemblyOf<CreateWardenMessage>("warden-api"))
                    .Start();

                activator.Bus.Subscribe<CreateWardenMessage>().Wait();
                Console.WriteLine("Press enter to quit");
                Console.ReadLine();
            }
        }
    }
}
