using PluginContracts;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActivePlugins
{
    static class CLI
    {
        static Serilog.Core.Logger log = new LoggerConfiguration()
                .WriteTo.File("ActivePlugins.log")
                .CreateLogger();

        public static bool HelpRequired(string param)
        {
            return param == "-h" || param == "--help" || param == "/?";
        }

        public static bool ListOfPluginsRequired(string param)
        {
            return param == "-l" || param == "--list" || param == "/l";
        }

        public static bool ExecutePluginRequired(string param)
        {
            return param == "-e" || param == "--exec" || param == "/e";
        }

        public static bool InteractiveModeRequired(string param)
        {
            return param == "-i" || param == "--interactive" || param == "/i";
        }

        public static void DisplayHelpWithLogging()
        {
            DisplayHelp();
            log.Information("Interactive mode: Display help");
        }

        public static void DisplayHelp()
        {
            Console.WriteLine("\nUSAGE:");
            Console.WriteLine("\tdotnet ActivePlugins.dll [-h | --help | /? |\n\t\t\t\t  -l | --list | /l |" +
                "\n\t\t\t\t  -e plugin_name \"string\" | --exec plugin_name \"string\" | /e plugin_name \"string\" |" +
                "\n\t\t\t\t  -i | --interactive | /i |]");

            Console.WriteLine("\nOptions:");
            Console.WriteLine("  -e plugin_name \"string\", --exec plugin_name \"string\", /e plugin_name \"string\"    Execute a selected plugin with a given input string.");
            Console.WriteLine("  -h, --help, /?                                                                   Display this help message.");
            Console.WriteLine("  -l, --list, /l                                                                   Display list all available plugins with descriptions.");
            Console.WriteLine("  -i, --interactive, /i                                                            Run app in the interactive mode.");
            Console.WriteLine("\n\nAll actions are logging in the log file \"ActivePlugins.log\". You can browse actions history via LogViewer web application.\n\n");
        }

        public static void DisplayListOfPlugins(ICollection<IPlugin> plugins)
        {
            Console.WriteLine("\n\nList of available plugins:");
            int i = 1;
            foreach (var plugin in plugins)
            {
                Console.WriteLine(i++ + ") " + plugin.Name + " - " + plugin.Descripton + ".");
            }
        }

        public static void RunInteractiveMode(ICollection<IPlugin> plugins, string arg)
        {
            log.Information("Running app with a parameter: " + arg);

            bool shouldfinish = false;
            ConsoleCommandHolder holder = new ConsoleCommandHolder();
            holder.AddRange(new ConsoleCommand[]{ new SimpleCommand("Stop application", new string[]{"stop"},
                (x)=>
                    {
                        Console.WriteLine("Do you really want to stop? (type 'yes' to stop)");
                        string acceptString = Console.ReadLine();
                        if (acceptString == "yes")
                        {
                            shouldfinish = true;
                        }

                    }),
                    new SimpleCommand("HELP", new string[] { "help" }, (x) => DisplayHelpWithLogging()),
            });

            foreach (var plugin in plugins)
            {
                holder.Add(new SimpleCommand(plugin.Name, new string[] { plugin.Name.ToLower() }, (x) => ExecutePlugin(plugin)));
            }

            while (shouldfinish == false)
            {
                string command = Console.ReadLine();
                ConsoleCommand toExecute = holder.Find(command);
                if (toExecute != null)
                    toExecute.Do(command);
                else Console.WriteLine(holder);
            }
        }

        public static void ExecutePlugin(IPlugin plugin)
        {
            Console.WriteLine("\nWrite sentence and click enter:");
            string sentence = Console.ReadLine();
            Console.WriteLine("\nResult: \n" + plugin.Execute(sentence));
            log.Information("Interactive mode: Running plugin \"" + plugin.Name + "\" on string \"" + sentence + "\"");
        }

        public static void ExecutePlugin(ICollection<IPlugin> plugins, string[] args)
        {
            foreach (var plugin in plugins)
            {
                if (plugin.Name.ToLower() == args[1].ToLower())
                {
                    Console.WriteLine(plugin.Execute(args[2]));
                }
            }
        }
    }
}
