using PluginContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace ActivePlugins
{
    public static class GenericPluginLoader<T>
    {
        public static ICollection<T> LoadPlugins(string path)
        {
            if (Directory.Exists(path))
            {
                ICollection<Assembly> assemblies = LoadAssemblies(path);

                Type pluginType = typeof(T);
                ICollection<Type> pluginTypes = new List<Type>();
                foreach (Assembly assembly in assemblies)
                {
                    if (assembly != null)
                    {
                        Type[] types = assembly.GetTypes();

                        foreach (Type type in types)
                        {
                            if (type.IsInterface || type.IsAbstract)
                            {
                                continue;
                            }
                            else
                            {
                                if (type.GetInterface(pluginType.FullName) != null)
                                {
                                    pluginTypes.Add(type);
                                }
                            }
                        }
                    }
                }

                ICollection<T> plugins = new List<T>(pluginTypes.Count);
                foreach (Type type in pluginTypes)
                {
                    T plugin = (T)Activator.CreateInstance(type);
                    plugins.Add(plugin);
                }

                return plugins;
            }

            return null;
        }

        public static ICollection<Assembly> LoadAssemblies(string path)
        {
            string[] dllFileNames = null;
            if (Directory.Exists(path))
            {
                string assemblyPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
                dllFileNames = Directory.GetFiles(assemblyPath, "*.dll");

                ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Length);
                foreach (string dllFile in dllFileNames)
                {
                    Assembly assembly = Assembly.LoadFile(dllFile);
                    assemblies.Add(assembly);
                }
                return assemblies;
            }
            return null;
        }
    }

}
