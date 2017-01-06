using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.CONV.Models
{
    public class TipoPresentacionDocumento
    {
        [Key]
        [Display(Name = "Codigo")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [DisplayFormat(NullDisplayText = "Codigo")]
        [StringLength(2, ErrorMessage = "El campo {0} debe tener {1} caracteres.", MinimumLength = 2)]
        public string sTipoPresentacion_fl { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Tipo de Presentacion")]
        [DisplayFormat(NullDisplayText = "Descripcion")]
        [StringLength(30, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 4)]
        public string sDescripcion { get; set; }

        [Display(Name = "Activo")]
        [DefaultValue(true)]
        public bool bActivo { get; set; }

        public virtual ICollection<gatbl_DPConvalidaciones> gatbl_DPConvalidaciones { get; set; }
        public virtual ICollection<gatbl_CertificadosMateria> gatbl_CertificadosMateria { get; set; }
    }
}