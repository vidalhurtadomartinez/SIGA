using System.Web.Mvc;

namespace SIGAA.Areas.CONV
{
    public class CONVAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CONV";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CONV_default",
                "CONV/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SIGAA.Areas.CONV.Controllers" }//agregado
            );
        }
    }
}