using System;
using Banks.AppConfig;
using Banks.Exceptions;
using Banks.Managers;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Banks.UI.Commands.Make
{
    public class MakeWdrCmd : Command<MakeWdrCmd.Settings>
    {
        private BankManager _manager = EfManagerConfig.Manager;

        public override int Execute(CommandContext context, Settings settings)
        {
            try
            {
                Guid trsId = _manager.PerformWithdrawal(settings.AccountId, settings.MoneyAmount);
                Console.WriteLine($"transaction performed, id is {trsId}");
            }
            catch (InsufficientFundsException e)
            {
                Console.WriteLine(e.Message);
                return 1;
            }
            catch (BanksException e)
            {
                Console.WriteLine(e.Message);
                return 1;
            }

            return 0;
        }

        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<account_id>")]
            public Guid AccountId { get; init; }

            [CommandArgument(1, "<money_amount>")]
            public decimal MoneyAmount { get; init; }

            public override ValidationResult Validate()
            {
                return MoneyAmount > 0.0m
                    ? ValidationResult.Success()
                    : ValidationResult.Error("money amount must be positive");
            }
        }
    }
}