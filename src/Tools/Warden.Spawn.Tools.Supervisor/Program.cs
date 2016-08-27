using System;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.Transport.Msmq;
using Rebus.Logging;
using Warden.Spawn.Tools.Core.Messages;
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
                activator.Register(() => new CreateWardenMessageHandler());
                Configure.With(activator)
                    .Logging(l => l.ColoredConsole(minLevel: LogLevel.Warn))
                    .Transport(t => t.UseMsmq("wardenspawn-supervisor"))
                    .Routing(r => r.TypeBased().MapAssemblyOf<CreateWardenMessage>("wardenspawn-orchestrator"))
                    .Start();

                activator.Bus.Subscribe<CreateWardenMessage>().Wait();
                Console.WriteLine("Press enter to quit");
                Console.ReadLine();
            }
        }
    }
}
