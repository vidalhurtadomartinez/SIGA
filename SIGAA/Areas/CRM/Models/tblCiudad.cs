using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.CRM.Models
{
    public class tblCiudad
    {
        [Key]
        public int lCiudad_id { get; set; }

        [Display(Name = "Prefijo", Description = "Prefijo", Prompt = "Prefijo")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(2)]
        public string Prefijo { get; set; }

        [Display(Name = "Descripcion", Description = "Descripcion", Prompt = "Descripcion")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public string Descripcion { get; set; }

        [Display(Name = "Activo", Description = "Activo", Prompt = "Activo")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public bool Activo { get; set; }


        public virtual ICollection<tblColegio> tblColegio { get; set; }
        public virtual ICollection<tblUniversidad> tblUniversidad { get; set; }
        public virtual ICollection<tblCliente> tblCliente { get; set; }
    }
}