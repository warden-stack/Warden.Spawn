using System;
using System.Diagnostics;

namespace Warden.Spawn.Examples.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var process = Process.Start("start.sh");
            process.WaitForExit();
            Console.WriteLine(process.Id);
        }
    }
}
