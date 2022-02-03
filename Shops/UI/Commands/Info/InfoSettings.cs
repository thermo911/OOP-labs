using System.ComponentModel;
using Spectre.Console.Cli;

namespace Shops.UI.Commands.Info
{
    public class InfoSettings : CommandSettings
    {
        [Description("Specifies the concrete item to print info")]
        [CommandArgument(0, "[id]")]
        [DefaultValue(0U)]
        public uint Id { get; init; }

        [Description("Prints list of all items.")]
        [CommandOption("-a|--all")]
        public bool All { get; init; }
    }
}