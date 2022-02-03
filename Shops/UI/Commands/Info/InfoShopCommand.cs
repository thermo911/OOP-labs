using System;
using Shops.Config;
using Shops.Entities;
using Shops.Managers;
using Shops.Tools.Exceptions;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Shops.UI.Commands.Info
{
    public class InfoShopCommand : Command<InfoSettings>
    {
        private readonly ShopManager _shopManager = ManagerConfig.ShopManager;

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
            if (!_shopManager.TryGetShop(settings.Id, out Shop shop))
            {
                AnsiConsole.MarkupLine($"Shop with id {settings.Id} not found");
                return;
            }

            AnsiConsole.MarkupLine($"Shop '{shop.Name}' (id = {shop.Id})");
            AnsiConsole.MarkupLine($"Products:");
            AnsiConsole.Render(GetShopProductsTable(shop));
        }

        private void ExecuteForAll()
        {
            AnsiConsole.Render(GetShopsTable());
        }

        private Table GetShopsTable()
        {
            var table = new Table();
            table.AddColumn("Id");
            table.AddColumn("Name");
            table.AddColumn("Address");

            foreach (Shop shop in _shopManager.GetAllShops())
            {
                table.AddRow(shop.Id.ToString(), shop.Name, shop.Address.Value);
            }

            return table;
        }

        private Table GetShopProductsTable(Shop shop)
        {
            var table = new Table();
            table.AddColumn("Id");
            table.AddColumn("Name");
            table.AddColumn("Price");
            table.AddColumn("Amount");

            foreach (ProductInfo productInfo in shop.GetAllProductInfos())
            {
                table.AddRow(
                    productInfo.Product.Id.ToString(),
                    productInfo.Product.Name,
                    productInfo.Price.Value.ToString(),
                    productInfo.Amount.ToString());
            }

            return table;
        }
    }
}