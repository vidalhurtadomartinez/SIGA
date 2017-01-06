using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.CONV.Models
{
    public class tipo_documentos
    {
        [Key]
        [StringLength(3)]
        public string doc_codigo { get; set; }

        [Display(Name = "Tipo de Documento")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(200)]
        public string doc_descripcion { get; set; }
        public int doc_tipo { get; set; }
        public int doc_sexo { get; set; }
        public int doc_status { get; set; }

        [StringLength(2)]
        public string doc_sigla { get; set; }
        public string doc_dependiente { get; set; }
        public string doc_colval { get; set; }        
    }
}