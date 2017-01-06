using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public enum ParEspecialidadTutor
    {
        [Display(Name = "Maestría")]
        MAESTRIA,

        [Display(Name = "Doctorado")]
        DOCTORADO,

        [Display(Name = "Licenciatura")]
        LICENCIATURA,

        [Display(Name = "Ingeniería")]
        INGENIERIA
    }
}