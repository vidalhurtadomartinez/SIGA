using SIGAA.Commons;
using System.Web.Mvc;
using System.Web.Routing;

namespace SIGAA.Etiquetas
{
    public class PermisoAttribute : ActionFilterAttribute
    {
        public RolesPermisos Permiso { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (!FrontUser.TienePermiso(this.Permiso))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Login",
                    action = "Autenticarse",
                    area = new { area = "" }
                }));
            }
        }
    }
}