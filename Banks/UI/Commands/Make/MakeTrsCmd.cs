using System;
using Banks.AppConfig;
using Banks.Exceptions;
using Banks.Managers;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Banks.UI.Commands.Make
{
    public class MakeTrsCmd : Command<MakeTrsCmd.Settings>
    {
        private BankManager _manager = EfManagerConfig.Manager;

        public override int Execute(CommandContext context, Settings settings)
        {
            try
            {
                Guid trsId = _manager.PerformTransfer(
                    settings.SourceId,
                    settings.ReceiverId,
                    settings.MoneyAmount);

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
            [CommandArgument(0, "<source_id>")]
            public Guid SourceId { get; init; }

            [CommandArgument(1, "<receiver_id>")]
            public Guid ReceiverId { get; init; }

            [CommandArgument(2, "<money_amount>")]
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