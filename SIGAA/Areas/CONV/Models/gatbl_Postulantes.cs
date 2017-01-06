using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGAA.Areas.CONV.Models
{
    public class gatbl_Postulantes
    {
        [Key]
        public int lPostulante_id { get; set; }

        [Display(Name = "Fecha de Registro", Description = "Fecha de Registro")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        //[DisplayFormat(DataFormatString = "{0:d}")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime dtFecha_registro_dt { get; set; }

        [Display(Name = "Tipo de documento", Description = "Tipo de documento")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int lTipoDocumentoPersonal_id { get; set; }

        [Display(Name = "Numero Doc.", Description = "Numero Doc.")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(50)]
        public string sDocumento_nro { get; set; }

        [Display(Name = "Procedencia", Description = "Procedencia")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int lProcedencia_id { get; set; }

        [Display(Name = "Nacionalidad", Description = "Nacionalidad")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int lNacionalidad_id { get; set; }

        [Display(Name = "Departamento", Description = "Departamento")]
        [StringLength(2, ErrorMessage = "El campo {0} debe tener {1} caracteres.", MinimumLength = 2)]
        public string sDepartamento_fl { get; set; }

        [Display(Name = "Apellido Paterno", Description = "Apellido Paterno")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(200)]
        public string sApPaterno_desc { get; set; }

        [Display(Name = "Apellido Materno", Description = "Apellido Materno")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(200)]
        public string sApMaterno_desc { get; set; }

        [Display(Name = "Nombres", Description = "Nombres")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(200)]
        public string sNombre_desc { get; set; }

        [Display(Name = "Nro. Registro", Description = "Nro. Registro")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public string sNroRegistro { get; set; }

        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Direccion")]
        [DisplayFormat(NullDisplayText = "Direccion")]
        [DataType(DataType.MultilineText)]
        [StringLength(250, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 5)]
        public string sDireccion_desc { get; set; }

        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Telefonos")]
        [DisplayFormat(NullDisplayText = "Telefonos")]
        [StringLength(100, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 7)]
        public string sTelefonos_desc { get; set; }

        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Correo Electronico")]
        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        public string sMail_desc { get; set; }

        [Display(Name = "Responsable", Description = "Responsable")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(100)]
        public string lResponsable_id { get; set; }

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
        [StringLength(50, ErrorMessage = "El campo {0} debe tener {1} caracteres.", MinimumLength = 8)]
        public string sCreated_by { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Display(Name = "Concurrencia")]
        public int iConcurrencia_id { get; set; }

        [Display(Name = "Postulante")]
        public virtual string NombreCompleto { get { return string.Format("{0} {1}, {2}", sApPaterno_desc, sApMaterno_desc, sNombre_desc); } }


        [ForeignKey(name: "lTipoDocumentoPersonal_id")]
        public virtual TipoDocumentoPersonal TipoDocumentoPersonal { get; set; }

        [ForeignKey(name: "lProcedencia_id")]
        public virtual Nacionalidad Procedencia { get; set; }

        [ForeignKey(name: "lNacionalidad_id")]
        public virtual Nacionalidad Nacionalidad { get; set; }

        [ForeignKey(name: "sDepartamento_fl")]
        public virtual Departamento Departamento { get; set; }

        [ForeignKey(name: "lResponsable_id")]
        public virtual agenda Responsables { get; set; }

        public virtual ICollection<gatbl_PConvalidaciones> gatbl_PConvalidaciones { get; set; }
    }
}