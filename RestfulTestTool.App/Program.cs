using RestfulTestTool.Core.Handlers;
using System;

namespace RestfulTestTool.App
{
    class Program
    {
        static void Main(string[] args)
        {
            TestConfig testConfig = ConfigHandler.ConfigureTestParameters(args);

            InitialiseTest;

            RunTest;

            ParseResults;
        }
    }
}
