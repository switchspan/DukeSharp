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
            var options = new Options();
            var parser = new CommandLineParser();
            if (parser.ParseArguments(args, options))
            {
               
                Console.WriteLine("Duke Commandline Application 1.0");
                Console.WriteLine("-----");
                // consume Options type properties
                Console.WriteLine(String.Format("Show Progress: {0}", options.Progress));

                
            }
            else
            {
                Console.WriteLine(options.GetUsage());
                //Console.WriteLine("Error reading commandline arguments!");
            }

            var keyPressed = Console.ReadKey();
        }
    }
}
