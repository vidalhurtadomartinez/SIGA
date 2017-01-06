using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGAA.Areas.CONV.Models
{
    public class gatbl_EscalaCalificaciones
    {
        [Key]
        public int lEscalaCalificacion_id { get; set; }

        [Display(Name = "Universidad", Description = "Universidad")]
        public int lUniversidad_id { get; set; }

        [Display(Name = "Tipo de Escala", Description = "Tipo de Escala")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(2, ErrorMessage = "El campo {0} debe tener {1} caracteres.", MinimumLength = 2)]
        public string sTipoEscala_fl { get; set; }

        [Display(Name = "Escala", Description = "Escala")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(10, ErrorMessage = "El campo {0} debe tener {1} caracteres.", MinimumLength = 1)]
        public string sEscala_desc { get; set; }

        [Display(Name = "Equivalencia Destino", Description = "Equivalencia Destino")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Range(typeof(int), "0", "100", ErrorMessage = "{0} solo puede estar entre {1} y {2}")]
        public int sEquivalencia_destino { get; set; }

        [HiddenInput(DisplayValue = false)]
        [DefaultValue("1")]
        [StringLength(2)]
        public string iEstado_fl { get; set; }

        [HiddenInput(DisplayValue = false)]
        [DefaultValue("1")]
        [StringLength(2)]
        public string iEliminado_fl { get; set; }

        [HiddenInput(DisplayValue = false)]
        [DefaultValue("")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener {1} caracteres.", MinimumLength = 8)]
        public string sCreated_by { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Display(Name = "Concurrencia")]
        public int iConcurrencia_id { get; set; }

        public virtual gatbl_Universidades gatbl_Universidades { get; set; }
        public virtual TipoEscalaEvaluacion TipoEscalaEvaluacion { get; set; }
        public virtual ICollection<gatbl_ConvalidacionesExternasPA> gatbl_ConvalidacionesExternasPA { get; set; }
    }
}