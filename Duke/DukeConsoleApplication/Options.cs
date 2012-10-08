using System;
using CommandLine;
using CommandLine.Text;

namespace DukeConsoleApplication
{
    internal class Options
    {
        [Option("c", "cfgfile", Required = true, HelpText = "XML configuration file.")]
        public string ConfigFile { get; set; }

        [Option("p", "progress", DefaultValue = false, HelpText = "Show progress report while running.")]
        public bool ShowProgress { get; set; }

        [Option("s", "showmatches", DefaultValue = false, HelpText = "Show matches while running.")]
        public bool ShowMatches { get; set; }

        [Option("l", "linkfile", Required = false, HelpText = "Output matches to link file.")]
        public string LinkFile { get; set; }

        [Option("i", "interactive", DefaultValue = false, HelpText = "Query user before outputting link file matches.")]
        public bool IsInteractive { get; set; }

        [Option("t", "testfile", Required = false, HelpText = "Output accuracy statistics.")]
        public string TestFile { get; set; }

        [Option("d", "testdebug", DefaultValue = false, HelpText = "Display failures.")]
        public bool IsTestDebug { get; set; }

        [Option("v", "verbose", DefaultValue = false, HelpText = "Display diagnostics.")]
        public bool ShowVerbose { get; set; }

        [Option("n", "noreindex", DefaultValue = false, HelpText = "Reuse existing Lucene index.")]
        public bool IsNoReindex { get; set; }

        [Option("b", "batchsize", DefaultValue = 1, HelpText = "Set size of Lucene indexing batches.")]
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
            help.AddPreOptionsLine("Usage: DukeConsoleApplication.exe -c <cfgfile.xml> [options] ");
            help.AddOptions(this);

            return string.Format("{0}{1}{2}", padding, Environment.NewLine, help);
        }
    }
}