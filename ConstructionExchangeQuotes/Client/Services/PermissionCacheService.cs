using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ConstructionExchangeQuotes.Client.Services
{
    public class PermissionCacheService
    {
        public bool CanDoCrudActions { get; set; }
        private readonly HttpClient httpClient;

        public PermissionCacheService(HttpClient httpClient)
        {
            this.httpClient = httpClient;

        }

        public async Task InitializePermissionCheck()
        {
            CanDoCrudActions = await httpClient.GetFromJsonAsync<bool>("permission");
        }
    }
}
