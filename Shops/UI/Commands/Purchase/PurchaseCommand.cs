using System;
using System.ComponentModel;
using Shops.Config;
using Shops.Managers;
using Shops.Tools.Exceptions;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Shops.UI.Commands.Purchase
{
    public class PurchaseCommand : Command<PurchaseCommand.Settings>
    {
        private readonly ShopManager _shopManager = ManagerConfig.ShopManager;

        public override int Execute(CommandContext context, Settings settings)
        {
            uint shopId = settings.ShopId;
            if (settings.Cheapest)
            {
                bool cheapestFound = _shopManager.TryGetCheapestShopId(
                    settings.ProductId,
                    settings.ProductAmount,
                    out shopId);

                if (!cheapestFound)
                {
                    AnsiConsole.MarkupLine(
                        "Failed to find the cheapest shop with enough amount of products");
                    return 1;
                }
            }

            try
            {
                _shopManager.PerformPurchase(
                    settings.PersonId,
                    shopId,
                    settings.ProductId,
                    settings.ProductAmount);
                AnsiConsole.MarkupLine($"Purchase successfully performed at shop with id {shopId}");
            }
            catch (PersonNotFoundException)
            {
                AnsiConsole.MarkupLine($"Person with id {settings.ProductId} not found");
            }
            catch (ShopNotFoundException)
            {
                AnsiConsole.MarkupLine($"Shop with id {settings.ShopId} not found");
            }
            catch (ProductNotFoundException)
            {
                AnsiConsole.MarkupLine($"Product with id {settings.ProductId} not found");
            }
            catch (Exception)
            {
                AnsiConsole.MarkupLine("Failed to execute 'purchase' command");
            }

            return 0;
        }

        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<person_id>")]
            public uint PersonId { get; init; }

            [CommandArgument(1, "<product_id>")]
            public uint ProductId { get; init; }

            [CommandArgument(2, "<product_amount>")]
            public uint ProductAmount { get; init; }

            [Description("Specifies concrete shop for purchase.")]
            [CommandArgument(3, "[shop_id]")]
            [DefaultValue(0u)]
            public uint ShopId { get; init; }

            [CommandOption("--cheapest")]
            public bool Cheapest { get; init; }

            public override ValidationResult Validate()
            {
                return ProductAmount == 0
                    ? ValidationResult.Error("Product amount must be positive")
                    : ValidationResult.Success();
            }
        }
    }
}