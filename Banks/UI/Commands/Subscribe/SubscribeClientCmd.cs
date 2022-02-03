using System;
using Banks.AppConfig;
using Banks.Exceptions;
using Banks.Managers;
using Spectre.Console.Cli;

namespace Banks.UI.Commands.Subscribe
{
    public class SubscribeClientCmd : Command<SubscribeClientCmd.Settings>
    {
        private BankManager _manager = EfManagerConfig.Manager;

        public override int Execute(CommandContext context, Settings settings)
        {
            try
            {
                _manager.SubscribeClientToBank(settings.BankId, settings.ClientId);
            }
            catch (NotFoundException e)
            {
                Console.WriteLine(e.Message);
                return 1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 1;
            }

            return 0;
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