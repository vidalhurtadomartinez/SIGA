using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.CRM.Models
{
    public class tEvento
    {
        [Key]
        public int lEvento_id { get; set; }

        [Display(Name = "Fecha Registro")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public DateTime dtFechaRegistro_dt { get; set; }

        [Display(Name = "Evento")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public string Titulo { get; set; }

        [Display(Name = "Evento")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }

        [Display(Name = "Desde")]
        public DateTime dtDesde_dt { get; set; }

        [Display(Name = "Hasta")]
        public DateTime dtHasta_dt { get; set; }

        [Display(Name = "Finaliza")]
        public DateTime dtFinaliza_dt { get; set; }

        [Display(Name = "Finaliza")]
        public bool TodoElDia { get; set; }

        [Display(Name = "Zona Desde")]
        public string ZonaDesde { get; set; }

        [Display(Name = "Zona Hasta")]
        public string ZonaHasta { get; set; }

        [Display(Name = "Repetir ID")]
        public int EventRecurrenceID { get; set; }

        [Display(Name = "Regla Repetir")]
        public string RecurrenceRule { get; set; }

        [Display(Name = "Excepcion Repetir")]
        public string RecurrenceException { get; set; }

        public virtual ICollection<tblEvento> tblEvento { get; set; }
    }
}