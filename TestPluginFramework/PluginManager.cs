using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using McMaster.NETCore.Plugins;
using Microsoft.Rest;
using Newtonsoft.Json.Linq;

namespace TestPluginFramework
{
    public class PluginManager
    {
        public static List<IDataCollectorPlugin> LoadPlugins(string pluginsDir)
        {
            var loaders = new List<PluginLoader>();
            var plugins = new List<IDataCollectorPlugin>();

            foreach (var depsFile in Directory.GetFiles(pluginsDir, "*.deps.json"))
            {
                var dllFile = depsFile.Replace(".deps.json", ".dll");
                var loader = PluginLoader.CreateFromAssemblyFile(
                    dllFile, 
                    sharedTypes: new[] 
                    {
                        typeof(IDataCollectorPlugin),
                        typeof(TokenCredentials),
                        typeof(JObject),
                    }
                );

                loaders.Add(loader);
            }


            foreach (var loader in loaders)
            {
                foreach (var pluginType in loader
                     .LoadDefaultAssembly()
                     .GetTypes()
                     .Where(t => typeof(IDataCollectorPlugin).IsAssignableFrom(t) && !t.IsAbstract))
                {
                    IDataCollectorPlugin plugin = (IDataCollectorPlugin)Activator.CreateInstance(pluginType);
                    plugins.Add(plugin);
                    Console.WriteLine($"Created plugin instance '{plugin.PluginName}'.");
                }
            }

            return plugins;
        }
    }
}
