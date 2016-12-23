using System.Web.Mvc;

namespace SIGAA.Areas.EGRE
{
    public class EGREAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "EGRE";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "EGRE_default",
                "EGRE/{controller}/{action}/{id}",
                new { controller = "Perfil", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] {"SIGAA.Areas.EGRE.Controllers"}//agregado
            );
        }
    }
}