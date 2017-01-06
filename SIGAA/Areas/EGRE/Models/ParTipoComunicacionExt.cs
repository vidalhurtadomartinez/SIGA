using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public enum ParTipoComunicacionExt
    {
        [Display(Name = "Tribunal para examen de grado")]
        TRIBUNAL_EXAMEN_DE_GRADO,

        [Display(Name = "Tribunal para TFG")]
        TRIBUNAL_TFG,

        [Display(Name = "Tribunal para excelencia")]
        TRIBUNAL_EXCELENCIA
    }
}