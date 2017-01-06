using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGAA.Areas.CONV.Models
{
    public class gatbl_Facultades
    {
        [Key]
        public int lFacultad_id { get; set; }

        [Display(Name = "Universidad", Description = "Universidad")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int lUniversidad_id { get; set; }

        [Display(Name = "Facultad", Description = "Facultad")]
        [DisplayFormat(NullDisplayText = "NINGUNO")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(200, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 5)]
        public string sFacultad_nm { get; set; }

        [Display(Name = "Responsable", Description = "Responsable")]        
        [StringLength(100)]
        public string sResponsable_nm { get; set; }

        [Display(Name = "Telefonos", Description = "Telefonos")]        
        [StringLength(50)]
        public string sTelefono_desc { get; set; }

        [Display(Name = "Mail", Description = "Correo electronico")]
        [StringLength(100)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Por favor, ingrese una dirección de correo electrónico válida.")]
        public string sMail_desc { get; set; }

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

        public virtual gatbl_Universidades gatbl_Universidades { get; set; }
        public virtual ICollection<gatbl_Carreras> gatbl_Carreras { get; set; }
        public virtual ICollection<gatbl_ProgramasAnaliticos> gatbl_ProgramasAnaliticos { get; set; }
    }
}