using System;
using Banks.AppConfig;
using Banks.Entities.Clients;
using Banks.Exceptions;
using Banks.Managers;
using Banks.UI.Info;
using Spectre.Console.Cli;

namespace Banks.UI.Commands.Edit
{
    public class EditClientCmd : Command<EditClientCmd.Settings>
    {
        private BankManager _manager = EfManagerConfig.Manager;

        public override int Execute(CommandContext context, Settings settings)
        {
            try
            {
                Client client = _manager.GetClient(settings.ClientId);
                client = EditClient(client);
                _manager.SaveClient(client);
                Console.WriteLine("Client data successfully updated!");
            }
            catch (NotFoundException e)
            {
                Console.WriteLine(e);
                return 1;
            }

            return 0;
        }

        private Client EditClient(Client client)
        {
            Console.WriteLine("Creating new client (* - necessary fields)");
            var builder = new ClientBuilder(client);

            Console.Write($"Name [{client.Name}]: ");
            string newName = Console.ReadLine();
            builder.Name(string.IsNullOrWhiteSpace(newName) ? client.Name : newName);

            Console.Write($"Surname [{client.Surname}]: ");
            string newSurname = Console.ReadLine();
            builder.Surname(string.IsNullOrWhiteSpace(newSurname) ? client.Surname : newSurname);

            Console.Write($"Passport [{client.Passport}]: ");
            string newPassport = Console.ReadLine();
            builder.Passport(string.IsNullOrWhiteSpace(newPassport) ? client.Passport : newPassport);

            Console.Write($"Address [{client.Address}]: ");
            string newAddress = Console.ReadLine();
            builder.Address(string.IsNullOrWhiteSpace(newAddress) ? client.Address : newAddress);

            return builder.GetResult();
        }

        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<client_id>")]
            public Guid ClientId { get; init; }
        }
    }
}