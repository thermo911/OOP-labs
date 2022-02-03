using System;
using Shops.Config;
using Shops.Entities;
using Shops.Managers;
using Shops.Tools.Exceptions;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Shops.UI.Commands.Info
{
    public class InfoPersonCommand : Command<InfoSettings>
    {
        private readonly PersonManager _personManager = ManagerConfig.PersonManager;

        public override int Execute(CommandContext context, InfoSettings settings)
        {
            if (settings.All)
            {
                ExecuteForAll();
            }
            else
            {
                ExecuteForOne(settings);
            }

            return 0;
        }

        private void ExecuteForOne(InfoSettings settings)
        {
            if (!_personManager.TryGetPerson(settings.Id, out Person person))
            {
                AnsiConsole.MarkupLine($"Person with id {settings.Id} not found");
                return;
            }

            AnsiConsole.MarkupLine(
                $"Person '{person.Name}' (id = {person.Id}, balance = {person.Balance.Value})");
        }

        private void ExecuteForAll()
        {
            AnsiConsole.Render(GetPersonsTable());
        }

        private Table GetPersonsTable()
        {
            var table = new Table();
            table.AddColumn("Id");
            table.AddColumn("Name");
            table.AddColumn("Balance");

            foreach (Person person in _personManager.GetAllPersons())
            {
                table.AddRow(person.Id.ToString(), person.Name, person.Balance.Value.ToString());
            }

            return table;
        }
    }
}