using CommandLine;
using RestfulTestTool.Core.Config;

namespace RestfulTestTool.Core.Handlers
{
    public class ConfigHandler
    {
        public static TestConfig ConfigureTestParameters(string[] args)
        {
            TestConfig testConfig = new TestConfig();
            Parser.Default.ParseArguments<AppOptions>(args)
                    .WithParsed(options => ConfigureApp(options, testConfig))
                    .WithNotParsed<AppOptions>((errors) => ErrorHandler.ArgumentParserError(errors));

            return testConfig;
        }

        public static void ConfigureTest(AppOptions options, TestConfig testConfig)
        {
            testConfig.Verbose = options.Verbose;
            ParseConfigFile(options.ConfigFilePath, testConfig);
        }

        public static void ParseConfigFile(string configFilePath, TestConfig testConfig)
        {
            try
            {
                using (StreamReader file = File.OpenText(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    testConfig = (TestConfig)serializer.Deserialize(file, typeof(TestConfig));
                }
            }
            catch (Exception ex)
            {
                testConfig.AddConfigError(ConfigErrorType.JsonParser,
                                          ErrorLevel.Fatal,
                                          ex.Message);   
            }

            if (!testConfig.IsValid())
                ErrorHandler.InvalidTestConfig(testConfig.ConfigErrors);

            return testConfig;
        }
    }
}
