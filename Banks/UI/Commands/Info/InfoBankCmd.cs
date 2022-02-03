using System;
using System.Collections.Generic;
using Banks.AppConfig;
using Banks.Exceptions;
using Banks.Managers;
using Banks.UI.Info;
using Spectre.Console;
using Spectre.Console.Cli;
using Table = Spectre.Console.Table;

namespace Banks.UI.Commands.Info
{
    public class InfoBankCmd : Command<InfoBankCmd.Settings>
    {
        private static BankManager _manager = EfManagerConfig.Manager;

        public override int Execute(CommandContext context, Settings settings)
        {
            try
            {
                BankInfo bankInfo = _manager.GetBankInfo(settings.BankId);
                Console.WriteLine($"Bank, id = {settings.BankId}");
                if (bankInfo.AccountInfos.Count > 0)
                    AnsiConsole.Render(GetAccountInfosTable(bankInfo.AccountInfos));
            }
            catch (NotFoundException)
            {
                Console.WriteLine($"error: bank with id {settings.BankId} not found");
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
            [CommandArgument(0, "<bank_id>")]
            public Guid BankId { get; init; }
        }
    }
}