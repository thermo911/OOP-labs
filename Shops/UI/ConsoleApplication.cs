using System;
using System.Linq;
using Spectre.Console.Cli;

namespace Shops.UI
{
    public static class ConsoleApplication
    {
        private static CommandApp _app = AppConfigurator.App;

        public static void Run()
        {
            while (true)
            {
                Console.Write(">>> ");
                string entered = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(entered))
                    continue;

                if (entered is "exit")
                {
                    Console.WriteLine("logout");
                    break;
                }

                string[] args = entered.Split();
                AppConfigurator.App.Run(args);
            }
        }
    }
}