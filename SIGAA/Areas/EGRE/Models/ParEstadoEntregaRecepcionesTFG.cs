using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public enum ParEstadoEntregaRecepcionesTFG
    {
        [Display(Name = "Aprobado")]
        APROBADO,

        [Display(Name = "Rechazado")]
        RECHAZADO,

        [Display(Name = "Con observaciones")]
        CON_OBSERVACIONES
    }
}