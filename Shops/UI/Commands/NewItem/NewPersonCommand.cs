using System.ComponentModel;
using Shops.Config;
using Shops.Entities;
using Shops.Managers;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Shops.UI.Commands.NewItem
{
    public class NewPersonCommand : Command<NewPersonCommand.Settings>
    {
        private PersonManager _personManager = ManagerConfig.PersonManager;

        public override int Execute(CommandContext context, Settings settings)
        {
            var person = new Person(settings.PersonName, new Money(settings.PersonBalance));
            _personManager.AddPerson(person);
            AnsiConsole.MarkupLine(
                $"Created new person '{person.Name}' with id {person.Id} and balance {person.Balance.Value}");
            return 0;
        }

        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<name>")]
            public string PersonName { get; init; }

            [CommandArgument(1, "[balance]")]
            [DefaultValue(0.0)]
            public double PersonBalance { get; init; }

            public override ValidationResult Validate()
            {
                return PersonBalance < 0.0 ?
                    ValidationResult.Error("Balance must be non-negative") :
                    ValidationResult.Success();
            }
        }
    }
}