using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.OYM.Models
{
    public class tblTipoProceso
    {
        [Key]
        public int lTipoProceso_id { get; set; }

        [Display(Name = "Sigla", Description = "Sigla", Prompt = "Sigla")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(3, ErrorMessage = "El campo {0} debe tener {1} caracteres.", MinimumLength = 2)]
        public string sSigla { get; set; }        

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Descripcion", Description = "Descripcion", Prompt = "Descripcion")]
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