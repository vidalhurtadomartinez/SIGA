using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public enum ParTipoComunicacionInt
    {
        [Display(Name = "Designación tribunal interno para revisión")]
        DESIG_TRIB_INT_REVISION,

        [Display(Name = "Designación tribunal interno para defensa final")]
        DESIG_TRIB_INT_DEFENSA_FINAL,

        [Display(Name = "Defensa final")]
        DEFENSA_FINAL
    }
}