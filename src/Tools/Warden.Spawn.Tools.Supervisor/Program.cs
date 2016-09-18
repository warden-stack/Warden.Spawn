using System;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.Transport.Msmq;
using Rebus.Logging;
using Warden.Services.Spawn.Commands;
using Warden.Spawn.Tools.Supervisor.Handlers;

namespace Warden.Spawn.Tools.Supervisor
{
    public class Program
    {
        private static readonly string WardenHostPath = @"D:\Projects\Warden\Repo\Warden-Spawn\src\Tools\" +
                                                        @"Warden.Spawn.Tools.Host\bin\Debug\net461\win7-x64\Warden.Spawn.Tools.Host.exe";

        public static void Main(string[] args)
        {
            using (var activator = new BuiltinHandlerActivator())
            {
                Console.Title = "Warden.Spawn.Tools.Supervisor";
                activator.Register((bus, message) => new RunWardenProcessHandler(bus, WardenHostPath));
                Configure.With(activator)
                    .Logging(l => l.ColoredConsole(minLevel: LogLevel.Debug))
                    .Transport(t => t.UseMsmq("Warden.Spawn.Tools.Supervisor"))
                    .Routing(r => r.TypeBased().Map<RunWardenProcess>("Warden.Services.Spawn"))
                    .Start();

                activator.Bus.Subscribe<RunWardenProcess>().Wait();
                Console.WriteLine("Press enter to quit");
                Console.ReadLine();
            }
        }
    }
}
