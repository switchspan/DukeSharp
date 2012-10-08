using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace DukeConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var linkFile = "NONE";
            var statsFile = "NONE";
            var options = new Options();
            var parser = new CommandLineParser();
            if (parser.ParseArguments(args, options))
            {
                // write the application header
                Console.WriteLine(GetApplicationHeader());

                Console.WriteLine("Execution Options:");
                Console.WriteLine(String.Format("Configuration File = {0}", options.ConfigFile));

                // consume Options type properties
                Console.WriteLine(String.Format("Show ShowProgress = {0}", options.ShowProgress));
                Console.WriteLine(String.Format("Show Matches = {0}", options.ShowMatches));
                if (!String.IsNullOrEmpty(options.LinkFile))
                {
                    linkFile = options.LinkFile;
                    Console.WriteLine(String.Format("Link File = {0}", linkFile));
                }
                Console.WriteLine(String.Format("IsInteractive = {0}",options.IsInteractive));
                if (!String.IsNullOrEmpty(options.TestFile))
                {
                    statsFile = options.TestFile;
                    Console.WriteLine(String.Format("Test File = {0}", statsFile));    
                }
                Console.WriteLine(String.Format("Verbose = {0}", options.ShowVerbose));
                Console.WriteLine(String.Format("No Reindex = {0}", options.IsNoReindex));
                Console.WriteLine(String.Format("Batch Size = {0}", options.BatchSize));

            }
            else
            {
                Console.WriteLine(options.GetUsage());
                //Console.WriteLine("Error reading commandline arguments!");
            }

        }

        private static string GetApplicationHeader()
        {
            var sb = new StringBuilder();
            var padding = "==============================";
            
            sb.AppendLine(padding);
            
            var help = new HelpText
            {
                Heading = new HeadingInfo("Duke Console Application", "1.0"),
                Copyright = new CopyrightInfo("Ken Taylor", 2012),
                AdditionalNewLineAfterOption = false,
                AddDashesToOption = false
            };

            sb.AppendLine(help);
            sb.AppendLine(padding);

            return sb.ToString();
        }
    }
}
