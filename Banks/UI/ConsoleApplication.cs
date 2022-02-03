using System;
using Spectre.Console.Cli;

namespace Banks.UI
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