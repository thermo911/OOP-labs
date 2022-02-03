using System;
using System.Collections.Generic;
using Banks.AppConfig;
using Banks.Exceptions;
using Banks.Managers;
using Banks.UI.Info;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Banks.UI.Commands.Info
{
    public class InfoClientCmd : Command<InfoClientCmd.Settings>
    {
        private static BankManager _manager = EfManagerConfig.Manager;

        public override int Execute(CommandContext context, Settings settings)
        {
            try
            {
                ClientInfo clientInfo = _manager.GetClientInfo(settings.ClientId);
                Console.WriteLine("Client");
                Console.WriteLine($"Name:     {clientInfo.Name}");
                Console.WriteLine($"Surname:  {clientInfo.Surname}");
                Console.WriteLine($"Passport: {clientInfo.Passport}");
                Console.WriteLine($"Address:  {clientInfo.Address}");
                if (clientInfo.AccountInfos.Count > 0)
                    AnsiConsole.Render(GetAccountInfosTable(clientInfo.AccountInfos));
            }
            catch (NotFoundException)
            {
                Console.WriteLine($"error: client with id {settings.ClientId} not found");
                return 1;
            }

            return 0;
        }

        private static Table GetAccountInfosTable(IEnumerable<AccountInfo> accounts)
        {
            var table = new Table();
            table.Title = new TableTitle("Accounts");
            table.AddColumn("Id");
            table.AddColumn("Type");
            table.AddColumn("balance");
            table.AddColumn("Interest, %");
            table.AddColumn("Fee");

            foreach (AccountInfo acc in accounts)
            {
                table.AddRow(
                    acc.Id.ToString(),
                    acc.Type.ToString(),
                    acc.Balance.ToString(),
                    acc.InterestRate.ToString(),
                    acc.Fee.ToString());
            }

            return table;
        }

        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<client_id>")]
            public Guid ClientId { get; init; }
        }
    }
}