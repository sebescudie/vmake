using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

using CommandLine;
using CommandLine.Text;

using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace vmake
{
    class Program
    {
        public class Options
        {
            [Option('i', "init", HelpText = "Inits a new vvvv folder", Required = true)]
            public bool doInit { get; set; }

            [Option('o', "open", HelpText = "Instantly opens root patch after creation")]
            public bool doOpen { get; set; }

            [Option('g', "git", HelpText = "Inits a git repo in the current folder")]
            public bool doRepo { get; set; }

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

                if(options.doRepo)
                {
                    BuildGitRepo(GetCurrentPath());
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

        /// <summary>
        /// Creates a git repo
        /// </summary>
        private static void BuildGitRepo(string _path)
        {
            if(!Repository.IsValid(_path))
            {
                // init
                Repository.Init(_path);
                // create readme
                File.WriteAllText(Path.Combine(_path, "README.md"), "This is the readme file");
                // create gitignore
                File.WriteAllText(Path.Combine(_path, ".gitignore"), "This is the gitignore file");
            }
            else
            {
                Console.WriteLine("This is folder is already a git repo!");
            }
        }

        #endregion METHODS
    }
}
