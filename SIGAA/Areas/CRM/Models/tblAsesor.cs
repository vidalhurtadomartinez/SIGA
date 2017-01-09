using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGAA.Areas.CRM.Models
{
    public class tblAsesor
    {
        [Key]
        public int lAsesor_id { get; set; }

        [Display(Name = "Fecha de Registro")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public DateTime dtFechaRegistro_dt { get; set; }

        [Display(Name = "Nombres", Description = "Nombres", Prompt = "Nombres")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public string Nombres { get; set; }

        [Display(Name = "Apellido Paterno", Description = "Apellido Paterno", Prompt = "Apellido Paterno")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public string ApellidoPaterno { get; set; }

        [Display(Name = "Apellido Materno", Description = "Apellido Materno", Prompt = "Apellido Materno")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public string ApellidoMaterno { get; set; }

        [Display(Name = "Direccion", Description = "Direccion", Prompt = "Direccion")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [DataType(DataType.MultilineText)]
        public string Direccion { get; set; }

        [Display(Name = "Telefono", Description = "Telefono", Prompt = "Telefono")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public string Telefono { get; set; }

        [Display(Name = "Celular", Description = "Celular", Prompt = "Celular")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public string Celular { get; set; }

        [Display(Name = "% Comision", Description = "% Comision", Prompt = "% Comision")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public decimal Comision { get; set; }

        [HiddenInput(DisplayValue = false)]
        [DefaultValue("1")]
        [StringLength(2)]
        public string iEstado_fl { get; set; }

        [HiddenInput(DisplayValue = false)]
        [DefaultValue("1")]
        [StringLength(2)]
        public string iEliminado_fl { get; set; }

        [HiddenInput(DisplayValue = false)]
        [DefaultValue("")]
        public string sCreated_by { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Display(Name = "Concurrencia")]
        public int iConcurrencia_id { get; set; }

        [NotMapped]
        public virtual string NombreCompleto { get { return string.Format("{0} {1}, {2}", ApellidoPaterno, ApellidoMaterno, Nombres); } }


        public virtual ICollection<tblCliente> tblCliente { get; set; }
    }
}