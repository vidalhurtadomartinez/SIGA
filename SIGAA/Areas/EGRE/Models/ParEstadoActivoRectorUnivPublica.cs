using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public enum ParEstadoActivoRectorUnivPublica
    {
        [Display(Name = "Activo")]
        ACTIVO,

        [Display(Name = "Inactivo")]
        INACTIVO
    }
}