using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public enum ParEstadoEntregaTFG
    {
        [Display(Name = "1ra recepción")]
        PRIM_RECEPCION,

        [Display(Name = "2da recepción (corregido)")]
        SEG_RECEP_CORREGIDO,

        [Display(Name = "3ra recepción (corregido)")]
        TER_RECEP_CORREGIDO,

        [Display(Name = "Recepción final")]
        RECEPCION_FINAL
    }
}