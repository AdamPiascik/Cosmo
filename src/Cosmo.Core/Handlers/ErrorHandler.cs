using CommandLine;
using Cosmo.Core.Constants;
using Cosmo.Core.Enums;
using Cosmo.Core.Types.ErrorTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cosmo.Core.Handlers
{
    public class ErrorHandler
    {
        public static void InvalidTestConfig(IList<ConfigError> allErrors)
        {
            IList<ConfigError> fatalErrors = allErrors.Where(x => x.Severity == ErrorLevel.Fatal).ToList();
            IList<ConfigError> nonFatalErrors = allErrors.Where(x => x.Severity != ErrorLevel.Fatal).ToList();

            if (nonFatalErrors.Any())
            {
                Globals.LoggingHandler.LogWarning("There were non-fatal errors during configuration:\n");
                foreach (ConfigError error in nonFatalErrors)
                {
                    Globals.LoggingHandler.LogWarning($"\t{error.Severity} ({error.Type}): {error.Message}\n");
                }
            }

            if (fatalErrors.Any())
            {
                Globals.LoggingHandler.LogConsole("There were fatal errors during configuration:\n");
                foreach (ConfigError error in nonFatalErrors)
                {
                    Globals.LoggingHandler.LogConsole($"\t{error.Type} error: {error.Message}\n");
                    Globals.LoggingHandler.LogError($"\t{error.Type} error: {error.Message}\n");
                }

                Environment.Exit(1);
            }
        }

        public static void ConfigFileNotFound(string path)
        {
            Console.WriteLine($"The config file at {path} was not found. Exiting...\n");
            Environment.Exit(1);
        }

        public static void ArgumentParserError(IEnumerable<Error> errs)
        {
            Environment.Exit(160);
        }

        public static void InitialisationError(ErrorLevel level, InitialiserErrorType type, IList<SetupError> errors)
        {
            IList<SetupError> fatalErrors = errors.Where(x => x.Severity == ErrorLevel.Fatal).ToList();
            IList<SetupError> nonFatalErrors = errors.Where(x => x.Severity != ErrorLevel.Fatal).ToList();

            if (nonFatalErrors.Any())
            {
                Globals.LoggingHandler.LogWarning("There were non-fatal errors initialising the test:\n");
                foreach (SetupError error in nonFatalErrors)
                {
                    Globals.LoggingHandler.LogWarning($"\t{error.Severity} ({error.Type}): {error.Message}\n");
                }
            }

            if (fatalErrors.Any())
            {
                Globals.LoggingHandler.LogConsole("There were fatal errors initialising the test:\n");
                Globals.LoggingHandler.LogError("There were fatal errors initialising the test:\n");
                foreach (SetupError error in fatalErrors)
                {
                    Globals.LoggingHandler.LogConsole($"\t{error.Type} error: {error.Message}\n");
                    Globals.LoggingHandler.LogError($"\t{error.Type} error: {error.Message}\n");
                }

                Environment.Exit(1);
            }
        }
    }
}
