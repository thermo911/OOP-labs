using System;
using Banks.AppConfig;
using Banks.Exceptions;
using Banks.Managers;
using Spectre.Console.Cli;

namespace Banks.UI.Commands.Subscribe
{
    public class UnsubscribeClientCmd : Command<UnsubscribeClientCmd.Settings>
    {
        private BankManager _manager = EfManagerConfig.Manager;

        public override int Execute(CommandContext context, Settings settings)
        {
            try
            {
                _manager.UnsubscribeClientToBank(settings.BankId, settings.ClientId);
            }
            catch (NotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }

            return 1;
        }

        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<bank_id>")]
            public Guid BankId { get; init; }

            [CommandArgument(1, "client_id")]
            public Guid ClientId { get; init; }
        }
    }
}