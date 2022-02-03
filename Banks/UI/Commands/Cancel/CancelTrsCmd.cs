using System;
using Banks.AppConfig;
using Banks.Exceptions;
using Banks.Managers;
using Spectre.Console.Cli;

namespace Banks.UI.Commands.Cancel
{
    public class CancelTrsCmd : Command<CancelTrsCmd.Settings>
    {
        private BankManager _manager = EfManagerConfig.Manager;

        public override int Execute(CommandContext context, Settings settings)
        {
            try
            {
                _manager.CancelTransfer(settings.TrsId);
                Console.WriteLine("Transaction cancelled!");
            }
            catch (BanksException e)
            {
                Console.WriteLine(e);
                return 1;
            }

            return 0;
        }

        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<transaction_id>")]
            public Guid TrsId { get; init; }
        }
    }
}