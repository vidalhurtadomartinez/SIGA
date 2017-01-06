using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public enum ParEstadoEntregaTribunales
    {
        [Display(Name = "1ra entrega")]
        PRIM_ENTREGA,

        [Display(Name = "2da entrega (corregido)")]
        SEG_ENTR_CORREGIDO,

        [Display(Name = "3ra entrega (corregido)")]
        TER_ENTR_CORREGIDO,

        [Display(Name = "Entrega final")]
        ENTREGA_FINAL
    }
}