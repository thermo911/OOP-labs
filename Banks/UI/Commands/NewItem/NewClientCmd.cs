using System;
using Banks.AppConfig;
using Banks.Entities.Clients;
using Banks.Managers;
using Spectre.Console.Cli;

namespace Banks.UI.Commands.NewItem
{
    public class NewClientCmd : Command<NewClientCmd.Settings>
    {
        private BankManager _manager = EfManagerConfig.Manager;

        public override int Execute(CommandContext context, Settings settings)
        {
            try
            {
                Client client = CreateClient();
                _manager.SaveClient(client);
                Console.WriteLine($"Created new client with id {client.Id}");
            }
            catch (Exception)
            {
                Console.WriteLine("Error: some invalid input while creating new client");
                return 1;
            }

            return 0;
        }

        private Client CreateClient()
        {
            Console.WriteLine("Creating new client (* - necessary fields)");
            var builder = new ClientBuilder();

            Console.Write("Name*   : ");
            builder.Name(Console.ReadLine());

            Console.Write("Surname*: ");
            builder.Surname(Console.ReadLine());

            Console.Write("Passport: ");
            builder.Passport(Console.ReadLine());

            Console.Write("Address : ");
            builder.Address(Console.ReadLine());

            return builder.GetResult();
        }

        public class Settings : CommandSettings
        {
        }
    }
}