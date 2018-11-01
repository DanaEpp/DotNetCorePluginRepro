using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestPluginFramework
{
    public interface IDataCollectorPlugin
    {
        string PluginName { get; }
        Task<List<string>> ListAsync(TokenCredentials creds, string subscriptionId);
    }
}
