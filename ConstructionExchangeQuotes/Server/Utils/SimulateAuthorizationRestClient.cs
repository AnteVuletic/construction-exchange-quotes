using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace ConstructionExchangeQuotes.Server.Utils
{
    public class SimulateAuthorizationRestClient
    {
        public async Task<bool> CheckAuthorizationClient(StringValues stringValues)
        {
            return await Task.FromResult(false);
        }
    }
}
