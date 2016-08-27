using System;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Logging;
using Rebus.Transport.Msmq;
using Warden.Spawn.Tools.Core.Messages;

namespace Warden.Spawn.Tools.Orchestrator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var activator = new BuiltinHandlerActivator())
            {
                Console.Title = "Warden.Spawn.Tools.Orchestrator";
                Configure.With(activator)
                    .Logging(l => l.ColoredConsole(minLevel: LogLevel.Warn))
                    .Transport(t => t.UseMsmq("wardenspawn-orchestrator"))
                    .Start();

                SendTestConfiguration(activator);
                Console.WriteLine("Press enter to quit");
                Console.ReadLine();
            }
        }

        private static void SendTestConfiguration(BuiltinHandlerActivator activator)
        {
            var timer = new System.Timers.Timer();
            timer.Elapsed += (s, e) =>
            {
                Console.WriteLine("Sending configuration");
                activator.Bus.Publish(new CreateWardenMessage("Test")).Wait();
            };
            timer.Interval = 1000;
            timer.Start();
        }
    }
}
