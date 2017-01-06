using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public enum ParEstadoObservacionTutor
    {
        [Display(Name = "Recomendable")]
        RECOMENDABLE,

        [Display(Name = "Tener Cuidado")]
        TENER_CUIDADO,

        [Display(Name = "No recomendable")]
        NO_RECOMENDABLE
    }
}