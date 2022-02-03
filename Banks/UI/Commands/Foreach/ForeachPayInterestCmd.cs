using System;
using Banks.AppConfig;
using Banks.Managers;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Banks.UI.Commands.Foreach
{
    public class ForeachPayInterestCmd : Command<ForeachPayInterestCmd.Settings>
    {
        private BankManager _manager = EfManagerConfig.Manager;

        public override int Execute(CommandContext context, Settings settings)
        {
            try
            {
                DateTime from = DateTime.Today.AddMonths(-settings.MonthCount);
                DateTime to = DateTime.Today;
                _manager.PayInterest(from, to);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 1;
            }

            return 0;
        }

        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<month_count>")]
            public int MonthCount { get; init; }

            public override ValidationResult Validate()
            {
                return MonthCount <= 0
                    ? ValidationResult.Error("months count must be positive")
                    : ValidationResult.Success();
            }
        }
    }
}