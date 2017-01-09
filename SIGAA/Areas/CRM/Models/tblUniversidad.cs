using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.CRM.Models
{
    public class tblUniversidad
    {
        [Key]
        public int lUniversidad_id { get; set; }

        [Display(Name = "Tipo", Description = "Tipo", Prompt = "Tipo")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int lTipoUniversidad_id { get; set; }

        [Display(Name = "Descripcion", Description = "Descripcion", Prompt = "Descripcion")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public string Descripcion { get; set; }

        [Display(Name = "Ciudad", Description = "Ciudad", Prompt = "Ciudad")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int lCiudad_id { get; set; }

        [Display(Name = "Activo", Description = "Activo", Prompt = "Activo")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public bool Activo { get; set; }


        [ForeignKey(name: "lTipoUniversidad_id")]
        public virtual tblTipoUniversidad tblTipoUniversidad { get; set; }

        [ForeignKey(name: "lCiudad_id")]
        public virtual tblCiudad tblCiudad { get; set; }


        public virtual ICollection<tblCarrera> tblCarrera { get; set; }
        public virtual ICollection<tblCliente> tblCliente { get; set; }
    }
}