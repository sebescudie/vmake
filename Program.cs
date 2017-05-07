using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using CommandLine;
using CommandLine.Text;

namespace vmake
{
    class Program
    {
        public class Options
        {
            [Option("init", HelpText = "Inits a new vvvv folder", Required = true)]
            public bool doInit { get; set; }

            [Option("o", HelpText = "Opens vvvv right away")]
            public bool doOpen { get; set; }

            [ParserState]
            public IParserState LastParserState { get; set; }

            [HelpOption]
            public string GetUsage()
            {
                return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
            }
        }

        static void Main(string[] args)
        {
            var options = new Options();

            Greetings();

            if(CommandLine.Parser.Default.ParseArguments(args, options))
            {
                if(options.doInit)
                {
                    BuildStructure();
                }

                if(options.doOpen)
                {
                    Process.Start("root.v4p");
                }
            }
            
            Environment.Exit(0);
        }

        #region METHODS
        
        /// <summary>
        /// Gets the folder where the app's currently running.
        /// </summary>
        /// <returns></returns>
        private static string GetCurrentPath()
        {
            try
            {
                return Environment.CurrentDirectory;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        /// <summary>
        /// Says hello
        /// </summary>
        private static void Greetings()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("vmake - vvvv project structure builder");
            Console.WriteLine("version 1.0 - may 2017");
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Creates a directory inside a specified parent folder
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        private static void CreateDir(string parent, string name)
        {
            string dir = Path.Combine(parent, name);
            try
            {
                if(Directory.Exists(dir))
                {
                    Console.WriteLine("That directory already exists");
                    return;
                }

                DirectoryInfo di = Directory.CreateDirectory(dir);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successfully created " + dir);
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Creates an empty .v4p patch
        /// </summary>
        /// <param name="parentfolder"></param>
        private static void CreateEmptyV4P(string parentfolder)
        {
            if(Directory.Exists(parentfolder))
            {
                try
                {
                    string filename = Path.Combine(parentfolder, "root.v4p");
                    File.Create(filename).Dispose();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

            }
            else
            {
                Console.WriteLine("Parent folder does not seem to exist!");
            }
        }

        /// <summary>
        /// Builds the complete project structure
        /// </summary>
        private static void BuildStructure()
        {
            string appPath = GetCurrentPath();

            // Create subs, modules and assets folder
            CreateDir(appPath, "subs");
            CreateDir(appPath, "modules");
            CreateDir(appPath, "assets");

            // Creates subfolders inside assets
            string assetsFolder = Path.Combine(appPath, "assets");
            CreateDir(assetsFolder, "tex");
            CreateDir(assetsFolder, "mesh");
            CreateDir(assetsFolder, "sounds");

            // Create empty .v4p file
            CreateEmptyV4P(appPath);
        }

        #endregion METHODS
    }
}
