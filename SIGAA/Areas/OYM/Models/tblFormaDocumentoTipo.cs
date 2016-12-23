using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.OYM.Models
{
    public class tblFormaDocumentoTipo
    {
        [Key]
        public int lFormaDocumentoTipo_id { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Descripcion", Description = "Descripcion", Prompt = "Descripcion")]
        [StringLength(30, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 2)]
        public string sDescripcion { get; set; }

        [Display(Name = "Activo")]
        [DefaultValue(true)]
        public bool bActivo { get; set; }

        public virtual ICollection<tblFormaDocumento> tblFormaDocumentos { get; set; }
    }
}