using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public enum ParEstadoEntregaAlEst
    {
        [Display(Name = "1ra entrega con observaciones")]
        PRIM_ENTR_CON_OBS,

        [Display(Name = "2da entrega con observaciones")]
        SEG_ENTR_CON_OBS,

        [Display(Name = "3ra entrega con observaciones")]
        TER_ENTR_CON_OBS,

        [Display(Name = "Entrega final")]
        ENTREGA_FINAL
    }
}