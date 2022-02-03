using System.ComponentModel;
using Shops.Config;
using Shops.Entities;
using Shops.Managers;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Shops.UI.Commands.NewItem
{
    public class NewProductCommand : Command<NewProductCommand.Settings>
    {
        private readonly ProductManager _productManager = ManagerConfig.ProductManager;

        public override int Execute(CommandContext context, Settings settings)
        {
            var product = new Product(settings.ProductName);
            _productManager.AddProduct(product);
            AnsiConsole.MarkupLine($"Registered new product '{product.Name}' with id {product.Id}");
            return 0;
        }

        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<product_name>")]
            public string ProductName { get; init; }
        }
    }
}