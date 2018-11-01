using McMaster.NETCore.Plugins;
using Microsoft.Azure;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestPluginFramework
{
    public class PluginManager
    {
        public static List<IDataCollectorPlugin> LoadPlugins(string pluginsDir)
        {
            var loaders = new List<PluginLoader>();
            var plugins = new List<IDataCollectorPlugin>();

            foreach (var file in Directory.GetFiles(pluginsDir, "*.dll"))
            {
                var dllFile = Directory.GetCurrentDirectory() + file;
                var loader = PluginLoader.CreateFromAssemblyFile(
                    dllFile, 
                    sharedTypes: new[] 
                    {
                        typeof(IDataCollectorPlugin),
                        typeof(TokenCredentials)
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
