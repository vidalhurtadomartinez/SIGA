﻿using System.Web.Mvc;

namespace SIGAA.Areas.RRHH
{
    public class RRHHAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "RRHH";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "RRHH_default",
                "RRHH/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SIGAA.Areas.RRHH.Controllers" }
            );
        }
    }
}