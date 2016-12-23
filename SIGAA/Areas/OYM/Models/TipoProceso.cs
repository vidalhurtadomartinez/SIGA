using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.OYM.Models
{
    public class TipoProceso
    {
        [Key]
        public int lTipoProceso_id { get; set; }

        [Display(Name = "Descripcion", Description = "Descripcion", Prompt = "Descripcion")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(30)]
        public string sDescripcion { get; set; }

        public virtual ICollection<tblProceso> tblProceso { get; set; }
    }
}