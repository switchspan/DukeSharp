using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using CommandLine;
using CommandLine.Text;
using NLog;

namespace DukeConsoleApplication
{
    internal class Program
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private static void Main(string[] args)
        {
            _logger.Info("* Application Start *");
            var options = new Options();
            var parser = new CommandLineParser();
            if (parser.ParseArguments(args, options))
            {
                // write the application header
                Console.WriteLine(GetApplicationHeader());

                // display all of the options gathered from the commandline args...
                DisplayExecutionOptions(options);

                if (!HasValidConfigFile(options.ConfigFile))
                {
                    string errMessage =
                        String.Format(
                            "The configuration file '{0}' is NOT valid.\r\n Please check the file path.",
                            options.ConfigFile);
                    DisplayErrorMessageAndExit(errMessage, ExitCode.InvalidConfigFile);
                }

                if (!HasValidXml(options.ConfigFile))
                {
                    string errMessage =
                        String.Format(
                            "The XML in the configuration file '{0}' is NOT valid.\r\n Please check the file contents.",
                            options.ConfigFile);
                    DisplayErrorMessageAndExit(errMessage, ExitCode.InvalidConfigFileXml);
                }

                
            }
            else
            {
                _logger.Debug("Application called without proper arguments.");
                Console.WriteLine(options.GetUsage());
                //Console.WriteLine("Error reading commandline arguments!");
            }

            _logger.Info("Application successfully exited");
            Environment.Exit((int) ExitCode.Success);
        }

        private static string GetApplicationHeader()
        {
            var sb = new StringBuilder();
            const string padding = "==============================";

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

        private static bool HasValidConfigFile(string pathToConfigFile)
        {
            _logger.Debug("Checking {0} to see if it is a valid config file.", pathToConfigFile);
            string directoryPath = Path.GetDirectoryName(pathToConfigFile);
            string fileName = Path.GetFileName(pathToConfigFile);

            if (directoryPath != null && !Directory.Exists(directoryPath))
            {
                directoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }

            if (fileName != null && (directoryPath != null && !File.Exists(Path.Combine(directoryPath, fileName))))
                return false;

            string extension = Path.GetExtension(fileName);

            if (extension != null && extension.ToLower() == ".xml")
            {
                return true;
            }

            return false;
        }

        private static bool HasValidXml(string pathToConfigFile)
        {
            _logger.Debug("Checking {0}  for valid XML", pathToConfigFile);
            try
            {
                if (String.IsNullOrEmpty(pathToConfigFile))
                    return false;

                var xmlDoc = new XmlDocument();
                xmlDoc.Load(pathToConfigFile);

                return true;
            }
            catch (XmlException)
            {
                return false;
            }
        }

        private static void DisplayExecutionOptions(Options options)
        {
            _logger.Debug(options.ToString);
            Console.WriteLine("Execution Options:");
            Console.WriteLine(String.Format("Configuration File = {0}", options.ConfigFile));
            // consume Options type properties
            Console.WriteLine(String.Format("Show ShowProgress = {0}", options.ShowProgress));
            Console.WriteLine(String.Format("Show Matches = {0}", options.ShowMatches));
            if (!String.IsNullOrEmpty(options.LinkFile))
            {
                string linkFile = options.LinkFile;
                Console.WriteLine(String.Format("Link File = {0}", linkFile));
            }
            Console.WriteLine(String.Format("IsInteractive = {0}", options.IsInteractive));
            if (!String.IsNullOrEmpty(options.TestFile))
            {
                string statsFile = options.TestFile;
                Console.WriteLine(String.Format("Test File = {0}", statsFile));
            }
            Console.WriteLine(String.Format("Verbose = {0}", options.ShowVerbose));
            Console.WriteLine(String.Format("No Reindex = {0}", options.IsNoReindex));
            Console.WriteLine(String.Format("Batch Size = {0}", options.BatchSize));
        }

        private static void DisplayErrorMessageAndExit(string errorMessage, ExitCode errorCode)
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(String.Format("* ERROR: {0}", errorMessage));
            _logger.Error(errorMessage);
            Environment.Exit((int) errorCode);
        }

        #region Nested type: ExitCode

        private enum ExitCode
        {
            Success = 0,
            InvalidConfigFile = 1,
            InvalidConfigFileXml = 2
        }

        #endregion
    }
}