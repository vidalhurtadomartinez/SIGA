using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public enum ParResultadoActaDefensaFinal
    {
        [Display(Name = "Preliminar")]
        PRELIMINAR = 0,

        [Display(Name = "Aprobado con máxima distinción")]
        APROBADO_MD = 1,

        [Display(Name = "Aprobado con distinción")]
        APROBADO_D = 2,

        [Display(Name = "Aprobado satisfactorio")]
        APROBADO_S = 3,

        [Display(Name = "Aprobado")]
        APROBADO = 4,

        [Display(Name = "Reprobado")]
        REPROBADO = 5
    }
}