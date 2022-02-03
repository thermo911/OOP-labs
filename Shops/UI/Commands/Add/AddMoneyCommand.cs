using System;
using Shops.Config;
using Shops.Managers;
using Shops.Tools.Exceptions;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Shops.UI.Commands.Add
{
    public class AddMoneyCommand : Command<AddMoneyCommand.Settings>
    {
        private readonly PersonManager _personManager = ManagerConfig.PersonManager;

        public override int Execute(CommandContext context, Settings settings)
        {
            try
            {
                _personManager.AddMoneyToPerson(settings.PersonId, settings.Money);
            }
            catch (ArgumentException)
            {
                AnsiConsole.MarkupLine("Incorrect arguments");
            }
            catch (PersonNotFoundException)
            {
                AnsiConsole.MarkupLine($"Person with id {settings.PersonId} not found");
            }
            catch (Exception)
            {
                AnsiConsole.MarkupLine("Failed to execute 'add money' command");
            }

            return 0;
        }

        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<person_id>")]
            public uint PersonId { get; init; }

            [CommandArgument(1, "<money>")]
            public double Money { get; init; }

            public override ValidationResult Validate()
            {
                return Money < 0.0
                    ? ValidationResult.Error("Argument <money> must be non-negative")
                    : ValidationResult.Success();
            }
        }
    }
}