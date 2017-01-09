using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.CRM.Models
{
    public class tblCarrera
    {
        [Key]
        public int lCarrera_id { get; set; }

        [Display(Name = "Universidad", Description = "Universidad", Prompt = "Universidad")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int lUniversidad_id { get; set; }

        [Display(Name = "Descripcion", Description = "Descripcion", Prompt = "Descripcion")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public string Descripcion { get; set; }

        [Display(Name = "Activo", Description = "Activo", Prompt = "Activo")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public bool Activo { get; set; }


        [ForeignKey(name: "lUniversidad_id")]
        public virtual tblUniversidad tblUniversidad { get; set; }


        public virtual ICollection<tblCliente> tblCliente { get; set; }
    }
}