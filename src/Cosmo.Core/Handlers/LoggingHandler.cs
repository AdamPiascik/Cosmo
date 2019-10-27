using CommandLine;
using Cosmo.Core.Constants;
using Cosmo.Core.Enums;
using Cosmo.Core.Config;
using Cosmo.Core.Types.ErrorTypes;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cosmo.Core.Handlers
{
    public class LoggingHandler
    {
        private bool bLogConsole { get; set; } = true;
        private bool bLogErrors { get; set; } = true;
        private bool bLogWarnings { get; set; } = true;
        private bool bLogResponses { get; set; } = true;
        private bool bLogPerformance { get; set; } = true;

        private string ConsoleLogFile { get; set; }
        private string ErrorLogFile { get; set; }
        private string WarningLogFile { get; set; }
        private string ResponseLogFile { get; set; }
        private string PerformanceLogFile { get; set; }

        private StringBuilder ConsoleLogSB = new StringBuilder();
        private StringBuilder ErrorLogSB = new StringBuilder();
        private StringBuilder WarningLogSB = new StringBuilder();
        private StringBuilder ResponseLogSB = new StringBuilder();
        private StringBuilder PerformanceLogSB = new StringBuilder();

        public ConcurrentQueue<string> ConsoleLogQueue { get; set; }
        public ConcurrentQueue<string> ErrorLogQueue { get; set; }
        public ConcurrentQueue<string> WarningLogQueue { get; set; }
        public ConcurrentQueue<string> ResponseLogQueue { get; set; }
        public ConcurrentQueue<string> PerformanceLogQueue { get; set; }

        public LoggingHandler(TestConfig config)
        {
            ConsoleLogQueue = new ConcurrentQueue<string>();
            ErrorLogQueue = new ConcurrentQueue<string>();
            WarningLogQueue = new ConcurrentQueue<string>();
            ResponseLogQueue = new ConcurrentQueue<string>();
            PerformanceLogQueue = new ConcurrentQueue<string>();

            bLogErrors = config.bLogErrors;
            bLogWarnings = config.bLogWarnings;
            bLogResponses = config.bLogResponses;

            CreateFilesAndFolders(config.TestName);
        }


        public void CreateFilesAndFolders(string testName)
        {
            DirectoryInfo baseTestDir = Directory.CreateDirectory($@"{Defaults.BasePath}\{testName}");
            DirectoryInfo thisTestDir = Directory.CreateDirectory($@"{baseTestDir}\{Defaults.ThisTestDirectory}");

            ConsoleLogFile = $@"{thisTestDir}\{Defaults.ConsoleLogFile}";
            ErrorLogFile = $@"{thisTestDir}\{Defaults.ErrorsFile}";
            WarningLogFile = $@"{thisTestDir}\{Defaults.WarningsFile}";
            ResponseLogFile = $@"{thisTestDir}\{Defaults.ResponseFile}";
            PerformanceLogFile = $@"{thisTestDir}\{Defaults.PerformanceFile}";

            if (bLogConsole)
                File.WriteAllText(ConsoleLogFile, string.Empty);

            if (bLogErrors)
                File.WriteAllText(ErrorLogFile, string.Empty);

            if (bLogWarnings)
                File.WriteAllText(WarningLogFile, string.Empty);

            if (bLogResponses)
                File.WriteAllText(ResponseLogFile, string.Empty);

            if (bLogPerformance)
                File.WriteAllText(PerformanceLogFile, string.Empty);
        }

        public void LogConsole(string message)
        {
            if (bLogConsole)
            {
                ConsoleLogQueue.Enqueue(message);
                Console.Write(message);
            }
        }

        public void LogError(string message)
        {
            if (bLogErrors)
                ErrorLogQueue.Enqueue(message);
        }
        public void LogWarning(string message)
        {
            if (bLogWarnings)
                WarningLogQueue.Enqueue(message);
        }
        public void LogResponse(string message)
        {
            if (bLogResponses)
                ResponseLogQueue.Enqueue(message);
        }

        public void LogPerformance(string message)
        {
            if (bLogPerformance)
                PerformanceLogQueue.Enqueue(message);
        }

        public async void StartLogQueueWatcher() =>
            await LogQueueWatcher();

        public void WaitForLoggingCompletion()
        {
            while (ConsoleLogQueue.Any()
                   || ErrorLogQueue.Any()
                   || WarningLogQueue.Any()
                   || ResponseLogQueue.Any()
                   || PerformanceLogQueue.Any())
            {
                Thread.Sleep(10);
            }

            WriteAllLogs();
        }

        private async Task LogQueueWatcher()
        {
            await Task.Run(() =>
            {
                while (Globals.bProgramRunning)
                {
                    if (ConsoleLogQueue.TryDequeue(out string consoleMessage))
                        ConsoleLogSB.Append(consoleMessage);
                    if (ErrorLogQueue.TryDequeue(out string errorMessage))
                        ErrorLogSB.Append(errorMessage);
                    if (WarningLogQueue.TryDequeue(out string warningMessage))
                        WarningLogSB.Append(warningMessage);
                    if (ResponseLogQueue.TryDequeue(out string responseMessage))
                        ResponseLogSB.Append(responseMessage);
                    if (PerformanceLogQueue.TryDequeue(out string performanceMessage))
                        PerformanceLogSB.Append(performanceMessage);
                }
            });
        }

        private void WriteAllLogs()
        {
            File.WriteAllText(ConsoleLogFile, ConsoleLogSB.ToString());
            File.WriteAllText(ErrorLogFile, ErrorLogSB.ToString());
            File.WriteAllText(WarningLogFile, WarningLogSB.ToString());
            File.WriteAllText(ResponseLogFile, ResponseLogSB.ToString());
            File.WriteAllText(PerformanceLogFile, PerformanceLogSB.ToString());
        }
    }
}
