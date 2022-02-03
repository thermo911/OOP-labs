using System;
using Shops.Config;
using Shops.Entities;
using Shops.Managers;
using Shops.Tools.Exceptions;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Shops.UI.Commands.Add
{
    public class AddProductCommand : Command<AddProductCommand.Settings>
    {
        private readonly ShopManager _shopManager = ManagerConfig.ShopManager;

        public override int Execute(CommandContext context, Settings settings)
        {
            try
            {
                _shopManager.AddProductsToShop(
                    settings.ShopId,
                    settings.ProductId,
                    settings.Price,
                    settings.Amount);
            }
            catch (ArgumentException)
            {
                AnsiConsole.MarkupLine("Incorrect arguments.");
            }
            catch (ShopNotFoundException)
            {
                AnsiConsole.MarkupLine($"Shop with id {settings.ShopId} not found");
            }
            catch (Exception)
            {
                AnsiConsole.MarkupLine("Failed to execute 'add product' command");
            }

            return 0;
        }

        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<shop_id>")]
            public uint ShopId { get; init; }

            [CommandArgument(0, "<product_id>")]
            public uint ProductId { get; init; }

            [CommandArgument(0, "<amount>")]
            public uint Amount { get; init; }

            [CommandArgument(0, "<price>")]
            public double Price { get; init; }

            public override ValidationResult Validate()
            {
                if (Amount == 0)
                    return ValidationResult.Error("Product amount must be positive");

                if (Price < 0.0)
                    return ValidationResult.Error("Product price must be positive");

                return ValidationResult.Success();
            }
        }
    }
}