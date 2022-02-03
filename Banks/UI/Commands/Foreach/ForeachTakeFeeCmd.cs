using System;
using Banks.AppConfig;
using Banks.Managers;
using Spectre.Console.Cli;

namespace Banks.UI.Commands.Foreach
{
    public class ForeachTakeFeeCmd : Command
    {
        private BankManager _manager = EfManagerConfig.Manager;

        public override int Execute(CommandContext context)
        {
            try
            {
                _manager.WithdrawFees();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 1;
            }

            return 0;
        }
    }
}