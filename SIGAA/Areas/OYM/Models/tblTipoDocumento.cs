using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.OYM.Models
{
    public class tblTipoDocumento
    {
        [Key]
        public int lTipoDocumento_id { get; set; }

        [Display(Name = "Sigla", Description = "Sigla", Prompt = "Sigla")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(2, ErrorMessage = "El campo {0} debe tener {1} caracteres.", MinimumLength = 2)]
        public string sSigla { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Tipo de documento", Description = "Tipo de documento", Prompt = "Tipo de documento")]
        [StringLength(100)]
        public string sTipoDocumento { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Concepto", Description = "Concepto", Prompt = "Concepto")]
        [StringLength(400)]
        [DataType(DataType.MultilineText)]
        public string sDescripcion { get; set; }

        [Display(Name = "Activo")]
        [DefaultValue(true)]
        public bool bActivo { get; set; }

        public virtual ICollection<tblDocumento> tblDocumentos { get; set; }
        public virtual ICollection<tblDocumentoPublicado> tblDocumentoPublicado { get; set; }
    }
}