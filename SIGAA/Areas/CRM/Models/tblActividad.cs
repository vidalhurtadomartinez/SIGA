using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.CRM.Models
{
    public class tblActividad
    {
        [Key]
        public int lActividad_id { get; set; }

        [Display(Name = "Rubro", Description = "Rubro", Prompt = "Rubro")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int lRubroActividad_id { get; set; }

        [Display(Name = "Descripcion", Description = "Descripcion", Prompt = "Descripcion")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public string Descripcion { get; set; }

        [Display(Name = "Activo", Description = "Activo", Prompt = "Activo")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public bool Activo { get; set; }

        [ForeignKey(name: "lRubroActividad_id")]
        public virtual tblRubroActividad tblRubroActividad { get; set; }


        public virtual ICollection<tblCliente> tblCliente { get; set; }
    }
}