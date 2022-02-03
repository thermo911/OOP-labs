using System;
using System.ComponentModel;
using Shops.Config;
using Shops.Entities;
using Shops.Managers;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Shops.UI.Commands.NewItem
{
    public class NewShopCommand : Command<NewShopCommand.Settings>
    {
        private readonly ShopManager _shopManager = ManagerConfig.ShopManager;

        public override int Execute(CommandContext context, Settings settings)
        {
            var shopAddress = new ShopAddress(settings.ShopAddress);
            var shop = new Shop(settings.Name, shopAddress);
            _shopManager.AddShop(shop);

            AnsiConsole.MarkupLine($"Created new shop '{shop.Name}' with id {shop.Id}");
            return 0;
        }

        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<shop_name>")]
            public string Name { get; init; }

            [CommandArgument(1, "[shop_address]")]
            [DefaultValue("not specified")]
            public string ShopAddress { get; init; }
        }
    }
}