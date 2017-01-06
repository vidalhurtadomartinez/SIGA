using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.CONV.Models
{
    public class TipoEscalaEvaluacion
    {
        [Key]
        [Display(Name = "Codigo", Description = "Codigo", Prompt = "Codigo")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(2, ErrorMessage = "El campo {0} debe tener {1} caracteres.", MinimumLength = 2)]
        public string sTipoEscala_fl { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Tipo de Escala", Description = "Tipo de Escala", Prompt = "Tipo de Escala")]
        [StringLength(30, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 4)]
        public string sDescripcion { get; set; }

        [Display(Name = "Activo")]
        [DefaultValue(true)]
        public bool bActivo { get; set; }

        public virtual ICollection<gatbl_EscalaCalificaciones> gatbl_EscalaCalificaciones { get; set; }
    }
}