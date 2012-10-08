using System;
using CommandLine;
using CommandLine.Text;

namespace DukeConsoleApplication
{
    internal class Options
    {
        [Option("c", "cfgfile", Required = false, HelpText = "xml configuration file")]
        public string ConfigFile { get; set; }

        [Option("p", "progress", DefaultValue = false, HelpText = "show progress report while running")]
        public bool Progress { get; set; }

        [Option("s", "showmatches", DefaultValue = false, HelpText = "show matches while running")]
        public bool ShowMatches { get; set; }

        [Option("l", "linkfile", Required = false, HelpText = "output matches to link file")]
        public string LinkFile { get; set; }

        [Option("i", "interactive", DefaultValue = false, HelpText = "query user before outputting link file matches")]
        public bool Interactive { get; set; }

        [Option("t", "testfile", Required = false, HelpText = "output accuracy stats")]
        public string TestFile { get; set; }

        [Option("d", "testdebug", DefaultValue = false, HelpText = "display failures")]
        public bool TestDebug { get; set; }

        [Option("v", "verbose", DefaultValue = false, HelpText = "display diagnostics")]
        public bool Verbose { get; set; }

        [Option("n", "noreindex", DefaultValue = false, HelpText = "reuse existing Lucene index")]
        public bool NoReIndex { get; set; }

        [Option("b", "batchsize", DefaultValue = 1, HelpText = "set size of Lucene indexing batches")]
        public int BatchSize { get; set; }

        [HelpOption(HelpText = "Display this help screen.")]
        public string GetUsage()
        {
            string padding = "==============================";

            var help = new HelpText
                           {
                               Heading = new HeadingInfo("Duke Console Application", "1.0"),
                               Copyright = new CopyrightInfo("Require LLC", 2012),
                               AdditionalNewLineAfterOption = false,
                               AddDashesToOption = true
                           };
            help.AddPreOptionsLine(padding);
            help.AddPreOptionsLine("Usage: DukeConsoleApplication.exe -p");
            help.AddOptions(this);

            return string.Format("{0}{1}{2}", padding, Environment.NewLine, help);
        }
    }
}