using CommandLine;
using Newtonsoft.Json;
using Cosmo.Core.Config;
using Cosmo.Core.Constants;
using Cosmo.Core.Enums;
using Cosmo.Core.Types.ErrorTypes;
using System;
using System.IO;

namespace Cosmo.Core.Handlers
{
    public class ConfigHandler
    {
        public static TestConfig ConfigureTestParameters(string[] args)
        {
            TestConfig testConfig = new TestConfig();
            Parser.Default.ParseArguments<AppOptions>(args)
                    .WithParsed(options => ConfigureTest(options, ref testConfig))
                    .WithNotParsed<AppOptions>((errors) => ErrorHandler.ArgumentParserError(errors));

            return testConfig;
        }

        public static void ConfigureTest(AppOptions options, ref TestConfig testConfig)
        {
            if (options.ConfigFilePath != null)
            {
                if (File.Exists(options.ConfigFilePath))
                    ParseConfigFile(options.ConfigFilePath, ref testConfig);
                else
                    ErrorHandler.ConfigFileNotFound(options.ConfigFilePath);
            }
            else
            {
                if (File.Exists(Defaults.ConfigFile))
                    ParseConfigFile(Defaults.ConfigFile, ref testConfig);
                else
                    ErrorHandler.ConfigFileNotFound(Defaults.ConfigFile);
            }
        }

        public static void ParseConfigFile(string configFilePath, ref TestConfig testConfig)
        {
            try
            {
                using (StreamReader file = File.OpenText(configFilePath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    testConfig = (TestConfig)serializer.Deserialize(file, typeof(TestConfig));
                }
            }
            catch (Exception ex)
            {
                testConfig.Errors.Add(new ConfigError
                {
                    Severity = ErrorLevel.Warning,
                    Type = ConfigErrorType.InvalidCombination,
                    Message = ex.Message
                });
            }

            if (testConfig.HasErrors())
                ErrorHandler.InvalidTestConfig(testConfig.Errors);
        }
    }
}
