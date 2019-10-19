using CommandLine;
using RestfulTestTool.Core.Constants;
using RestfulTestTool.Core.Enums;
using RestfulTestTool.Core.Types.ErrorTypes;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RestfulTestTool.Core.Handlers
{
    public class LoggingHandler
    {
        private string ConsoleLogFile { get; set; }
        private string ErrorLogFile { get; set; }
        private string WarningLogFile { get; set; }
        private string ResponseLogFile { get; set; }
        private string PerformanceLogFile { get; set; }
        public ConcurrentQueue<string> ConsoleLogQueue { get; set; }
        public ConcurrentQueue<string> ErrorLogQueue { get; set; }
        public ConcurrentQueue<string> WarningLogQueue { get; set; }
        public ConcurrentQueue<string> ResponseLogQueue { get; set; }
        public ConcurrentQueue<string> PerformanceLogQueue { get; set; }

        public LoggingHandler(string testName)
        {
            ConsoleLogQueue = new ConcurrentQueue<string>();
            ErrorLogQueue = new ConcurrentQueue<string>();
            WarningLogQueue = new ConcurrentQueue<string>();
            ResponseLogQueue = new ConcurrentQueue<string>();
            PerformanceLogQueue = new ConcurrentQueue<string>();

            CreateFilesAndFolders(testName);
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

            File.WriteAllText(ConsoleLogFile, string.Empty);
            File.WriteAllText(ErrorLogFile, string.Empty);
            File.WriteAllText(WarningLogFile, string.Empty);
            File.WriteAllText(ResponseLogFile, string.Empty);
            File.WriteAllText(PerformanceLogFile, string.Empty);
        }

        public void LogConsole(string message)
        {
            ConsoleLogQueue.Enqueue(message);
            Console.WriteLine(message);
        }

        public void LogError(string message) =>
            ErrorLogQueue.Enqueue(message);

        public void LogWarning(string message) =>
            WarningLogQueue.Enqueue(message);

        public void LogResponse(string message) =>
            ResponseLogQueue.Enqueue(message);

        public void LogPerformance(string message) =>
            PerformanceLogQueue.Enqueue(message);

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
                Thread.Sleep(50);
            }
        }

        private async Task LogQueueWatcher()
        {
            await Task.Run(() =>
            {
                while (Globals.bProgramRunning)
                {
                    if (ConsoleLogQueue.TryDequeue(out string consoleMessage))
                        File.AppendAllText(ConsoleLogFile, consoleMessage);
                    if (ErrorLogQueue.TryDequeue(out string errorMessage))
                        File.AppendAllText(ErrorLogFile, errorMessage);
                    if (WarningLogQueue.TryDequeue(out string warningMessage))
                        File.AppendAllText(WarningLogFile, warningMessage);
                    if (ResponseLogQueue.TryDequeue(out string responseMessage))
                        File.AppendAllText(ResponseLogFile, responseMessage);
                    if (PerformanceLogQueue.TryDequeue(out string performanceMessage))
                        File.AppendAllText(PerformanceLogFile, performanceMessage);
                }
            });
        }
    }
}
