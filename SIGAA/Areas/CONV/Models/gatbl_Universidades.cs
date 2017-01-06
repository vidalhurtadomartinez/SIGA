using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGAA.Areas.CONV.Models
{
    public class gatbl_Universidades
    {        
        [Key]
        public int lUniversidad_id { get; set; }

        [Display(Name = "Origen", Description = "Codigo")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(2, ErrorMessage = "El campo {0} debe tener {1} caracteres.", MinimumLength = 2)]
        public string sOrigen_fl { get; set; }

        [Display(Name = "Departamento")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(2, ErrorMessage = "El campo {0} debe tener {1} caracteres.", MinimumLength = 2)]
        public string sDepartamento_fl { get; set; }
               
        [DisplayName("Universidad")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(100, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 3)]
        public string sNombre_desc { get; set; }

        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Direccion")]
        [DisplayFormat(NullDisplayText = "Direccion")]
        [StringLength(250, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 5)]
        public string sDireccion_desc { get; set; }

        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Telefonos")]
        [DisplayFormat(NullDisplayText = "Telefonos")]
        [StringLength(100, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 7)]
        public string sTelefonos_desc { get; set; }

        [Display(Name = "Interno")]
        [DefaultValue(true)]
        public bool bInterno { get; set; }

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


        [Display(Name = "Origen")]
        public virtual OrigenOtraUniversidad OrigenOtraUniversidad { get; set; }

        [Display(Name = "Departamento")]
        public virtual Departamento Departamento { get; set; }

        public virtual ICollection<gatbl_Facultades> gatbl_Facultades { get; set; }
        public virtual ICollection<gatbl_Carreras> gatbl_Carreras { get; set; }
        public virtual ICollection<Pensum> Pensums { get; set; }
        public virtual ICollection<gatbl_EscalaCalificaciones> gatbl_EscalaCalificaciones { get; set; }
        public virtual ICollection<gatbl_ProgramasAnaliticos> gatbl_ProgramasAnaliticos { get; set; }
        //public virtual ICollection<gatbl_AnalisisPreConvalidaciones> gatbl_AnalisisPreConvalidaciones { get; set; }
        //public virtual ICollection<gatbl_AnalisisConvalidaciones> gatbl_AnalisisConvalidaciones { get; set; }
    }
}