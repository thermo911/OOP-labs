using System;
using Shops.Config;
using Shops.Entities;
using Shops.Managers;
using Shops.Tools.Exceptions;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Shops.UI.Commands.Info
{
    public class InfoProductCommand : Command<InfoSettings>
    {
        private readonly ProductManager _productManager = ManagerConfig.ProductManager;

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
            if (!_productManager.TryGetProduct(settings.Id, out Product product))
            {
                AnsiConsole.MarkupLine($"Product with id {settings.Id} not found");
                return;
            }

            AnsiConsole.MarkupLine($"Product '{product.Name}' (id  = {settings.Id})");
        }

        private void ExecuteForAll()
        {
            AnsiConsole.Render(GetProductsTable());
        }

        private Table GetProductsTable()
        {
            var table = new Table();
            table.AddColumn("Id");
            table.AddColumn("Name");

            foreach (Product product in _productManager.GetAllProducts())
            {
                table.AddRow(product.Id.ToString(), product.Name);
            }

            return table;
        }
    }
}