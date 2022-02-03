using System;
using Banks.AppConfig;
using Banks.Exceptions;
using Banks.Managers;
using Banks.UI.Info;
using Spectre.Console.Cli;

namespace Banks.UI.Commands.Info
{
    public class InfoAccountCmd : Command<InfoAccountCmd.Settings>
    {
        private BankManager _manager = EfManagerConfig.Manager;

        public override int Execute(CommandContext context, Settings settings)
        {
            try
            {
                AccountInfo accountInfo = _manager.GetAccountInfo(settings.AccountId);
                Console.WriteLine("Bank account");
                Console.WriteLine($"Id: {accountInfo.Id}");
                Console.WriteLine($"Type: {accountInfo.Type}");
                Console.WriteLine($"Balance: {accountInfo.Balance}");
                Console.WriteLine($"Interest: {accountInfo.InterestRate}%");
                Console.WriteLine($"Fee: {accountInfo.Fee}");
            }
            catch (NotFoundException e)
            {
                Console.WriteLine(e);
                return 1;
            }

            return 0;
        }

        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<account_id>")]
            public Guid AccountId { get; init; }
        }
    }
}