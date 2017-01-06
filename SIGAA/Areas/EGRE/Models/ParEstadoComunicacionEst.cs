using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public enum ParEstadoComunicacionEst
    {
        [Display(Name = "Autorizado para habilitacion")]
        AUT_HABILITACION,

        [Display(Name = "Autorizado para defensa final")]
        DEFENSA_FINAL
    }
}