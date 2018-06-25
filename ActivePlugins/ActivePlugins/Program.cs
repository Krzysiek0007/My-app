using PluginContracts;
using Serilog;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ActivePlugins
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, IPlugin> _Plugins = new Dictionary<int, IPlugin>();
            ICollection<IPlugin> plugins = GenericPluginLoader<IPlugin>.LoadPlugins("Plugins");
            int i = 1;
            foreach (var plugin in plugins)
            {
                _Plugins.Add(i++, plugin);
            }

            if (args.Length == 1 && CLI.InteractiveModeRequired(args[0]))
            {
                #region INTERACTIVE_MODE
                CLI.RunInteractiveMode(plugins,args[0]);
                #endregion
            }
            else
            {
                #region NORMAL_MODE
                var log = new LoggerConfiguration()
                .WriteTo.File("ActivePlugins.log")
                .CreateLogger();
                if (args.Length == 0)
                {
                    log.Information("Running app without any parameters.");
                    Console.WriteLine("\n##################################################################################### ");
                    Console.WriteLine("This is a plugginable console application in .NET Core 2.0 with plugin discovery. ");
                    Console.WriteLine("This app performs various actions on strings (depending on available plugins).");
                    Console.WriteLine("##################################################################################### ");
                    Console.WriteLine("\nFor HELP run program with -h or --help or /? parameter.");
                    Console.WriteLine("\nList of available plugins:");

                    i = 0;
                    foreach (var plugin in plugins)
                    {
                        Console.WriteLine(++i + ") " + plugin.Descripton + ".");
                    }

                    Console.WriteLine("\nWrite a sentence and click enter:");
                    string sentence = Console.ReadLine();

                    Console.WriteLine("\nWhat do you want to do with your sentence? \n(choose a number of plugin and click enter)");
                    Console.WriteLine("Your choice: ");
                    string pluginNumberString = Console.ReadLine();
                    int pluginNumber = 0;
                    while (!int.TryParse(pluginNumberString, out pluginNumber) || pluginNumber <= 0 || pluginNumber > _Plugins.Count)
                    {
                        Console.WriteLine("Choose correct number of operation. Your choice: ");
                        pluginNumberString = Console.ReadLine();
                    };
                    Console.WriteLine("\nResult: \n" + _Plugins[pluginNumber].Execute(sentence));
                    log.Information("Running plugin \"" + _Plugins[pluginNumber].Name + "\" on string \"" + sentence + "\"");
                    Console.ReadKey();
                }
                else if (args.Length == 1 && CLI.HelpRequired(args[0]))
                {
                    log.Information("Running app with a parameter: " + args[0]);
                    CLI.DisplayHelp();
                }
                else if (args.Length == 1 && CLI.ListOfPluginsRequired(args[0]))
                {
                    log.Information("Running app with a parameter: " + args[0]);
                    CLI.DisplayListOfPlugins(plugins);
                }
                else if (args.Length == 3 && CLI.ExecutePluginRequired(args[0]))
                {
                    log.Information("Running app with parameters: " + args[0] + " | " + args[1] + " | \"" + args[2] + "\".");
                    CLI.ExecutePlugin(plugins, args);
                }
                else
                {
                    string parameters = "";
                    foreach (var arg in args)
                    {
                        parameters += arg + " | ";
                    }
                    log.Error("Running app with wrong parameters: " + parameters);
                }
                #endregion
            }
        }
    }
}