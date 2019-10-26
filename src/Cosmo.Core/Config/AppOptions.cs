using CommandLine;

namespace Cosmo.Core.Config
{
    public class AppOptions
    {
        [Option('c',
            "ConfigFile",
            Required = false,
            HelpText = "Specify a config file to use.")]
        public string ConfigFilePath { get; set; }
    }
}