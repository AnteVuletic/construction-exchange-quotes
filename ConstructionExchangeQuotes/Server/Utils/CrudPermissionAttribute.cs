using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace ConstructionExchangeQuotes.Server.Utils
{
    public class CrudPermissionAttribute : TypeFilterAttribute
    {
        public CrudPermissionAttribute() : base(typeof(CrudPermissionActionFilter))
        {

        }

        private class CrudPermissionActionFilter : IAsyncActionFilter
        {
            private readonly SimulateAuthorizationRestClient simulateAuthorizationRestClient;

            public CrudPermissionActionFilter(SimulateAuthorizationRestClient simulateAuthorizationRestClient)
            {
                this.simulateAuthorizationRestClient = simulateAuthorizationRestClient;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                var isSuccessful = context.HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader);
                var hasPermission = await simulateAuthorizationRestClient.CheckAuthorizationClient(authHeader);
                if (!hasPermission)
                    throw new AuthenticationException("User has no perssmision for CRUD actions");
                next();
            }
        }
    }

}
