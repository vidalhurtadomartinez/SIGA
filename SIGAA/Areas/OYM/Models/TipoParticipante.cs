using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.OYM.Models
{
    public class TipoParticipante
    {
        [Key]
        public int lTipoParticipante_id { get; set; }

        [Display(Name = "Descripcion", Description = "Descripcion", Prompt = "Sigla")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(30)]
        public string sDescripcion { get; set; }

        public virtual ICollection<tblDocumentoAprobacion> tblDocumentoAprobacion { get; set; }
    }
}