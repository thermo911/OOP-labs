using System;
using Banks.AppConfig;
using Banks.Entities.Banks;
using Banks.Managers;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Banks.UI.Commands.NewItem
{
    public class NewBankCmd : Command<NewBankCmd.Settings>
    {
        private BankManager _manager = EfManagerConfig.Manager;

        public override int Execute(CommandContext context, Settings settings)
        {
            try
            {
                BankConfig config = CreateBankConfig(settings.DepositRatesCount);
                Guid newBankId = _manager.CreateAndRegisterBank(config);
                Console.WriteLine($"Created new bank with id {newBankId}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Incorrect input");
                return 1;
            }

            return 0;
        }

        private static BankConfig CreateBankConfig(int entriesCount)
        {
            var builder = new BankConfigBuilder();

            Console.WriteLine("Creating new bank configuration");

            Console.Write("Debit interest (%): ");
            builder.DebitInterestRate(decimal.Parse(Console.ReadLine()));

            Console.Write("Fee: ");
            builder.FeeRate(decimal.Parse(Console.ReadLine()));

            Console.Write("Credit limit: ");
            builder.FeeRate(decimal.Parse(Console.ReadLine()));
            for (int i = 0; i < entriesCount; i++)
            {
                Console.WriteLine($"Rate level no.{i + 1}: (start balance, percentage)");
                Console.Write("Start balance: ");
                decimal balance = decimal.Parse(Console.ReadLine());
                Console.Write("Percentage: ");
                decimal percentage = decimal.Parse(Console.ReadLine());
                builder.AddDepositRateEntry(new DepositRateEntry(balance, percentage));
            }

            return builder.Build();
        }

        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<deposit_rates_count>")]
            public int DepositRatesCount { get; init; }

            public override ValidationResult Validate()
            {
                return DepositRatesCount <= 0 ?
                    ValidationResult.Error("It must be at least one deposit interest rate") :
                    ValidationResult.Success();
            }
        }
    }
}