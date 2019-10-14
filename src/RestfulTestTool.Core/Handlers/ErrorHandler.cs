using CommandLine;
using RestfulTestTool.Core.Config;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestfulTestTool.Core.Handlers
{
    public enum ErrorLevel
    {
        Warning,
        Fatal
    }

    public class ErrorHandler
    {
        public static void InvalidTestConfig(IList<ConfigError> allErrors)
        {
            IList<ConfigError> fatalErrors = allErrors.Where(x => x.Severity == ErrorLevel.Fatal).ToList();
            IList<ConfigError> nonFatalErrors = allErrors.Where(x => x.Severity != ErrorLevel.Fatal).ToList();

            if (nonFatalErrors.Any())
            {
                Console.WriteLine("There were non-fatal errors configuring the test:\n");
                foreach (ConfigError error in nonFatalErrors)
                {
                    Console.WriteLine($"\t{error.Severity} ({error.Type}): {error.Message}");
                }
            }

            if (fatalErrors.Any())
            {
                Console.WriteLine("There were fatal errors configuring the test:\n");
                foreach (ConfigError error in nonFatalErrors)
                {
                    Console.WriteLine($"\t{error.Type} error: {error.Message}");
                }

                Environment.Exit(1);
            }                
        }

        public static void ArgumentParserError(IEnumerable<Error> errs)
        {
            Environment.Exit(160);
        }
    }
}
