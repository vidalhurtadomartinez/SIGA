using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.OYM.Models
{
    public class tblFormaDocumento
    {
        [Key]
        public int lFormaDocumento_id { get; set; }

        [Display(Name = "Tipo", Description = "Tipo")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int lFormaDocumentoTipo_id { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Concepto", Description = "Concepto", Prompt = "Concepto")]
        [StringLength(400)]
        [DataType(DataType.MultilineText)]
        public string sDescripcion { get; set; }

        [Display(Name = "Activo")]
        [DefaultValue(true)]
        public bool bActivo { get; set; }


        [ForeignKey(name: "lFormaDocumentoTipo_id")]
        public virtual tblFormaDocumentoTipo tblFormaDocumentoTipos { get; set; }
    }
}