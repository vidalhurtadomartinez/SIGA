using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.CRM.Models
{
    public class tblColegio
    {
        [Key]
        public int lColegio_id { get; set; }

        [Display(Name = "Tipo", Description = "Tipo", Prompt = "Tipo")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int lTipoColegio_id { get; set; }

        [Display(Name = "Descripcion", Description = "Descripcion", Prompt = "Descripcion")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public string Descripcion { get; set; }

        [Display(Name = "Ciudad", Description = "Ciudad", Prompt = "Ciudad")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int lCiudad_id { get; set; }

        [Display(Name = "Provincia", Description = "Provincia", Prompt = "Provincia")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int lProvincia_id { get; set; }

        [Display(Name = "Activo", Description = "Activo", Prompt = "Activo")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public bool Activo { get; set; }


        [ForeignKey(name: "lTipoColegio_id")]
        public virtual tblTipoColegio tblTipoColegio { get; set; }

        [ForeignKey(name: "lCiudad_id")]
        public virtual tblCiudad tblCiudad { get; set; }

        [ForeignKey(name: "lProvincia_id")]
        public virtual tblProvincia tblProvincia { get; set; }


        public virtual ICollection<tblCliente> tblCliente { get; set; }
    }
}