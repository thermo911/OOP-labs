using System;
using Banks.AppConfig;
using Banks.Exceptions;
using Banks.Managers;
using Banks.UI.Info;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Banks.UI.Commands.NewItem
{
    public class NewAccountCmd : Command<NewAccountCmd.Settings>
    {
        private BankManager _manager = EfManagerConfig.Manager;

        public override int Execute(CommandContext context, Settings settings)
        {
            try
            {
                Guid accountId = Guid.Empty;

                if (settings.Debit)
                {
                    accountId = _manager.CreateDebitAccount(settings.BankId, settings.ClientId);
                }
                else if (settings.Deposit)
                {
                    Console.Write("Deposit period (months): ");
                    if (int.TryParse(Console.ReadLine(), out int months))
                    {
                        if (months <= 0)
                            throw new Exception();
                    }

                    accountId = _manager.CreateDepositAccount(
                        settings.BankId,
                        settings.ClientId,
                        settings.DepositStartBalance,
                        DateTime.Today.AddMonths(months));
                }
                else
                {
                    accountId = _manager.CreateCreditAccount(settings.BankId, settings.ClientId);
                }

                AccountInfo accountInfo = _manager.GetAccountInfo(accountId);
                Console.WriteLine("Created new bank account:");
                Console.WriteLine($"Id:       {accountInfo.Id}");
                Console.WriteLine($"Type:     {accountInfo.Type}");
                Console.WriteLine($"Interest: {accountInfo.InterestRate}%");
                Console.WriteLine($"Fee:      {accountInfo.Fee}");
            }
            catch (NotFoundException e)
            {
                Console.WriteLine(e);
                return 1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("incorrect input");
                return 1;
            }

            return 0;
        }

        public class Settings : CommandSettings
        {
            private decimal _startBalance = decimal.MinValue;

            [CommandArgument(0, "<bank_id>")]
            public Guid BankId { get; init; }

            [CommandArgument(1, "<client_id>")]
            public Guid ClientId { get; init; }

            [CommandOption("--debit")]
            public bool Debit { get; init; }

            [CommandOption("--deposit <start_balance>")]
            public decimal DepositStartBalance
            {
                get => _startBalance;
                set
                {
                    _startBalance = value;
                    Deposit = true;
                }
            }

            public bool Deposit { get; set; }

            [CommandOption("--credit")]
            public bool Credit { get; init; }

            public override ValidationResult Validate()
            {
                if (!AccountTypeChosenCorrectly())
                    return ValidationResult.Error("choose only 1 account type");

                if (!Debit && !Credit)
                {
                    if (DepositStartBalance < 0.0m)
                        return ValidationResult.Error("deposit start balance is negative");
                }

                return ValidationResult.Success();
            }

            private bool AccountTypeChosenCorrectly()
            {
                return
                    (Debit && !Deposit && !Credit) ||
                    (!Debit && Deposit && !Credit) ||
                    (!Debit && !Deposit && Credit);
            }
        }
    }
}