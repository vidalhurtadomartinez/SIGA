using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public enum ParNumeroEjemplar
    {
        [Display(Name = "Primero")]
        PRIMERO,

        [Display(Name = "Segundo")]
        SEGUNDO
    }
}