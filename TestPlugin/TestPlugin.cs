using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Rest;
using Microsoft.Azure.Management.Storage;
using TestPluginFramework;

namespace TestPlugin
{
    public class TestPlugin : IDataCollectorPlugin
    {
        public string PluginName => "Test Plugin";

        public async Task<List<string>> ListAsync(TokenCredentials creds, string subscriptionId)
        {

            List<string> accounts = new List<string>();

            try
            {
                // When calling StorageManagementClient from inside the plugin, this mismatch is thrown:
                //
                // {System.ArrayTypeMismatchException: Attempted to access an element as a type incompatible with the array.
                //      at System.Collections.Generic.List`1.AddWithResize(T item)
                //      at Microsoft.Azure.Management.Storage.StorageManagementClient.Initialize()
                //      at Microsoft.Azure.Management.Storage.StorageManagementClient..ctor(ServiceClientCredentials credentials, DelegatingHandler[] handlers)
                //      at TestPlugin.TestPlugin.ListAsync(TokenCredentials creds, String subscriptionId)}
                var azureClient = new StorageManagementClient(creds) { SubscriptionId = subscriptionId };

                var storageAccounts = await azureClient.StorageAccounts.ListAsync();

                foreach (var account in storageAccounts)
                {
                    accounts.Add(account.Name);
                }
            }
            catch( Exception e )
            {
                Console.WriteLine(e);
            }

            
            return accounts;
        }
    }
}
