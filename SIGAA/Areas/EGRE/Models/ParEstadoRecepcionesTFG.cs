using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public enum ParEstadoRecepcionesTFG
    {
        [Display(Name = "1ra corrección")]
        PRIM_CORRECCION,

        [Display(Name = "2da corrección")]
        SEG_CORRECCION,

        [Display(Name = "3ra corrección")]
        TER_CORRECCION,

        [Display(Name = "Entrega final")]
        ENTREGA_FINAL
    }
}