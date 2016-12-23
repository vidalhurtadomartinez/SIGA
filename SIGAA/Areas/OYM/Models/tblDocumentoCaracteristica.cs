using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.OYM.Models
{
    public class tblDocumentoCaracteristica
    {
        [Key]
        public int lDocumentoCaracteristica_id { get; set; }

        [Display(Name = "Nombre", Description = "Nombre", Prompt = "Nombre")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(50)]
        public string sNombre { get; set; }

        [Display(Name = "Titulo", Description = "Titulo", Prompt = "Titulo")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(50)]
        public string sTitulo { get; set; }

        public virtual ICollection<tblPlantillaMarcador> tblPlantillaMarcador { get; set; }
    }
}