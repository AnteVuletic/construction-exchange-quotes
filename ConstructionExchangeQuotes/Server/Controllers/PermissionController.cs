using ConstructionExchangeQuotes.Server.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ConstructionExchangeQuotes.Server.Controllers
{
    [ApiController]
    [Route("permission")]
    public class PermissionController : ControllerBase
    {
        private readonly SimulateAuthorizationRestClient simulateAuthorizationRestClient;

        public PermissionController(SimulateAuthorizationRestClient simulateAuthorizationRestClient)
        {
            this.simulateAuthorizationRestClient = simulateAuthorizationRestClient;
        }

        [HttpGet]
        public async Task<IActionResult> CanDoCrudActions()
        {
            var isSuccessful = HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader);
            try
            {
                var canDoCrudActions = await simulateAuthorizationRestClient.CheckAuthorizationClient(authHeader);
                return Ok(canDoCrudActions);
            } catch
            {
                return Ok(false);
            }
        }
    }
}
