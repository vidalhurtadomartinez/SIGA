using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.CONV.Models
{
    public class rptInformeConvalidacion
    {
        [Display(Name = "ID", Description = "ID")]
        [Required]
        public int Id { get; set; }

        [Display(Name = "Fecha de Solicitud", Description = "Fecha de Solicitud")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [DataType(DataType.Date)]
        public DateTime dtFechaSolicitud_dt { get; set; }

        [Display(Name = "Fecha de Convalidacion", Description = "Fecha de Convalidacion")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [DataType(DataType.Date)]
        public DateTime dtFechaConvalidacion_dt { get; set; }
    }
}