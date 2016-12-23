using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.OYM.Models
{
    public class tblPlantillaMarcador
    {
        [Key]
        public int lPlantillaMarcador_id { get; set; }

        [Display(Name = "Atributo documento", Description = "Atributo documento")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int lDocumentoCaracteristica_id { get; set; }

        [Display(Name = "Marcador", Description = "Marcador", Prompt = "Marcador")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(50)]
        public string sMarcador { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Plantilla")]
        public int lPlantilla_id { get; set; }



        [ForeignKey(name: "lDocumentoCaracteristica_id")]
        public virtual tblDocumentoCaracteristica tblDocumentoCaracteristica { get; set; }

        [ForeignKey(name: "lPlantilla_id")]
        public virtual tblPlantilla tblPlantilla { get; set; }
    }
}