using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public enum ParTipoObsDRecepcionesTFG
    {
        [Display(Name = "De forma")]
        DE_FORMA,

        [Display(Name = "De fondo")]
        DE_FONDO
    }
}