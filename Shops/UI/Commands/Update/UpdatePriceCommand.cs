using System;
using Shops.Config;
using Shops.Managers;
using Shops.Tools.Exceptions;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Shops.UI.Commands.Update
{
    public class UpdatePriceCommand : Command<UpdatePriceCommand.Settings>
    {
        private readonly ShopManager _shopManager = ManagerConfig.ShopManager;

        public override int Execute(CommandContext context, Settings settings)
        {
            try
            {
                _shopManager.UpdateProductPrice(
                    settings.ShopId,
                    settings.ProductId,
                    settings.NewPrice);
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
            [CommandArgument(0, "<shop_id>")]
            public uint ShopId { get; init; }

            [CommandArgument(1, "<product_id>")]
            public uint ProductId { get; init; }

            [CommandArgument(2, "<new_price>")]
            public double NewPrice { get; init; }

            public override ValidationResult Validate()
            {
                return NewPrice > 0.0 ?
                    ValidationResult.Success() :
                    ValidationResult.Error("Price must be positive");
            }
        }
    }
}