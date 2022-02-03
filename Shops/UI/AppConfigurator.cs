using Shops.UI.Commands.Add;
using Shops.UI.Commands.Info;
using Shops.UI.Commands.NewItem;
using Shops.UI.Commands.Purchase;
using Shops.UI.Commands.Update;
using Spectre.Console.Cli;

namespace Shops.UI
{
    public static class AppConfigurator
    {
        private static CommandApp _app;

        public static CommandApp App
        {
            get
            {
                return _app ??= Configure();
            }
        }

        private static CommandApp Configure()
        {
            var app = new CommandApp();
            app.Configure(config =>
            {
                config.AddBranch("new", newItem =>
                {
                    newItem.AddCommand<NewShopCommand>("shop")
                        .WithDescription("Creates new shop");
                    newItem.AddCommand<NewPersonCommand>("person")
                        .WithDescription("Creates new person");
                    newItem.AddCommand<NewProductCommand>("product")
                        .WithDescription("Registers new product");
                });

                config.AddBranch("info", info =>
                {
                    info.AddCommand<InfoShopCommand>("shop")
                        .WithDescription("Prints info about shop(s)");
                    info.AddCommand<InfoProductCommand>("product")
                        .WithDescription("Prints info about products(s)");
                    info.AddCommand<InfoPersonCommand>("person")
                        .WithDescription("Prints info about person(s)");
                });

                config.AddBranch("add", add =>
                {
                    add.AddCommand<AddMoneyCommand>("money")
                        .WithDescription("Adds money to person's balance");
                    add.AddCommand<AddProductCommand>("product")
                        .WithDescription("Adds products to shop");
                });

                config.AddBranch("update", update =>
                {
                    update.AddCommand<UpdatePriceCommand>("price");
                });

                config.AddCommand<PurchaseCommand>("purchase");
            });
            return app;
        }
    }
}