using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Storage;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using TestPluginFramework;

namespace TestPluginConsole
{
    class Program
    {

        static void Main(string[] args)
        {
            var app = new CommandLineApplication
            {
                Name = "TestConsole",
                Description = "Console app for testing plugins"
            };
            app.HelpOption("-?|-h|--help");

          
            app.OnExecute(() =>
            {
                Task.Run(async () =>
                {
                    try
                    {
                        await TestDataCollection();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                }).GetAwaiter().GetResult();

             
                return 0;
            });

            app.Execute(args);

        }

        static async Task TestDataCollection()
        {
            List<IDataCollectorPlugin> plugins = PluginManager.LoadPlugins(@".\Plugins");

            IDataCollectorPlugin dataCollectorPlugin = plugins.Single(s => s.PluginName == "Test Plugin");

            var subscriptionId = "<subscription id>";
            var creds = await AzureResourceManager.GetCreds();

            // If I uncomment this, the call to the ARM client works fine
            //var azureClient = new StorageManagementClient(creds) { SubscriptionId = subscriptionId };
            //var storageAccounts = await azureClient.StorageAccounts.ListAsync();

            if (dataCollectorPlugin != null)
            {
                List<string> resourceList = await dataCollectorPlugin.ListAsync(creds, subscriptionId);

                foreach (var resource in resourceList)
                {
                    Console.WriteLine($"{resource}");
                }
            }
            else
            {
                Console.WriteLine("Unable to load a compatible data collector plugin for this resource type. Ignoring.");
            }

        }

    }
}
