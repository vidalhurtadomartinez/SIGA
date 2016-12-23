using System.Web.Mvc;

namespace SIGAA.Areas.OYM
{
    public class OYMAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "OYM";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "OYM_default",
                "OYM/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SIGAA.Areas.OYM.Controllers" }
            );
        }
    }
}