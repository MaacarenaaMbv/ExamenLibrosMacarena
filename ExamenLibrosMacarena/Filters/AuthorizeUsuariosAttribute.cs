using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExamenLibrosMacarena.Filters
{
    public class AuthorizeUsuariosAttribute: AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user.Identity.IsAuthenticated == false)
            {
                context.Result = this.GetRoute("Managed", "Login");            }
        }

        public RedirectToRouteResult GetRoute(string controller, string action) 
        {
            RouteValueDictionary ruta =
                new RouteValueDictionary(
                new { controller = controller, action = action });
            RedirectToRouteResult result =
            new RedirectToRouteResult(ruta);
            return result;
        }

    }
}
