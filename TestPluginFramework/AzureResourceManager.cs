using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestPluginFramework
{
    public class AzureResourceManager
    {
        private static readonly string azureActiveDirectoryInstance = "https://login.microsoftonline.com/";
        private static readonly string tenant = "<tenant name>.onmicrosoft.com";
        private static readonly string clientId = "<client id>";
        private static readonly string appKey = "<app key>";
        private static readonly string resource = "https://management.core.windows.net/";

        private static readonly string authority = azureActiveDirectoryInstance + tenant;

        private static async Task<string> GetToken()
        {
            var httpClient = new HttpClient();
            var authContext = new AuthenticationContext(authority);
            var clientCredential = new ClientCredential(clientId, appKey);
            var result = await authContext.AcquireTokenAsync(resource, clientCredential);

            return result.AccessToken;
        }

        public static async Task<TokenCredentials> GetCreds()
        {
            var token = await GetToken();
            return new TokenCredentials(token);
        }
    }
}
