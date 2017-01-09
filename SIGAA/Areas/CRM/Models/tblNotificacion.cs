using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.CRM.Models
{
    public class tblNotificacion
    {
        [Key]
        public int lNotificacion_id { get; set; }

        [Display(Name = "Notificar cada (Minutos)", Description = "Descripcion", Prompt = "Descripcion")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int TiempoAlerta { get; set; }

        [Display(Name = "Descripcion", Description = "Descripcion", Prompt = "Descripcion")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public string Descripcion { get; set; }

        [Display(Name = "Activo", Description = "Activo", Prompt = "Activo")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public bool Activo { get; set; }


        public virtual ICollection<tblEvento> tblEvento { get; set; }
    }
}