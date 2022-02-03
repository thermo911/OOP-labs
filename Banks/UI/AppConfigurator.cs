using Banks.UI.Commands.Edit;
using Banks.UI.Commands.Info;
using Banks.UI.Commands.Make;
using Banks.UI.Commands.NewItem;
using Banks.UI.Commands.Subscribe;
using Spectre.Console.Cli;

namespace Banks.UI
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
                    newItem.AddCommand<NewBankCmd>("bank")
                        .WithDescription("Creates new bank");
                    newItem.AddCommand<NewClientCmd>("client")
                        .WithDescription("Creates new client");
                    newItem.AddCommand<NewAccountCmd>("account")
                        .WithDescription("Creates new bank account");
                });

                config.AddBranch("info", info =>
                {
                    info.AddCommand<InfoBankCmd>("bank")
                        .WithDescription("Prints info about specified bank");
                    info.AddCommand<InfoClientCmd>("client")
                        .WithDescription("Prints info about specified client");
                    info.AddCommand<InfoAccountCmd>("account")
                        .WithDescription("Prints info about specified account");
                });

                config.AddBranch("edit", edit =>
                {
                    edit.AddCommand<EditBankCmd>("bank")
                        .WithDescription("Edits configuration of specified bank");
                    edit.AddCommand<EditClientCmd>("client")
                        .WithDescription("Edits specified client data");
                });

                config.AddBranch("make", make =>
                {
                    make.AddCommand<MakeTopCmd>("top")
                        .WithDescription("Performs topping up of account");
                    make.AddCommand<MakeWdrCmd>("wdr")
                        .WithDescription("Performs withdrawal from account");
                    make.AddCommand<MakeTrsCmd>("trs")
                        .WithDescription("Performs transfer from source to receiver");
                    make.AddCommand<SubscribeClientCmd>("sub")
                        .WithDescription("Subscribes client for notifications from bank");
                    make.AddCommand<UnsubscribeClientCmd>("unsub")
                        .WithDescription("Unsubscribes client for notifications from bank");
                });
            });

            return app;
        }
    }
}