using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGAA.Areas.CONV.Models
{
    public class gatbl_DConvalidacionesExternasPA
    {
        [Key]
        public int lDConvalidacionExternaPA_id { get; set; }

        [Display(Name = "Convalidacion Externa", Description = "Convalidacion Externa")]
        [Required]
        public int lConvalidacionExternaPA_id { get; set; }

        [Display(Name = "Universidad", Description = "Universidad")]
        [Required]
        public int lUniversidad_id { get; set; }

        [Display(Name = "Carrera", Description = "Carrera")]
        [Required]
        public int lCarrera_id { get; set; }

        [Display(Name = "Escala", Description = "Escala")]
        [Required]
        public int lEscalaCalificacion_id { get; set; }

        [Display(Name = "Observaciones", Description = "Observaciones")]
        [StringLength(200)]
        [DataType(DataType.MultilineText)]
        public string sObs_desc { get; set; }

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
        public string sCreated_by { get; set; }

        [HiddenInput(DisplayValue = false)]
        [DefaultValue(1)]
        public int iConcurrencia_id { get; set; }


        [ForeignKey(name: "lConvalidacionExternaPA_id")]
        public virtual gatbl_ConvalidacionesExternasPA gatbl_ConvalidacionesExternasPA { get; set; }

        [ForeignKey(name: "lUniversidad_id")]
        public virtual gatbl_Universidades gatbl_Universidades { get; set; }

        [ForeignKey(name: "lCarrera_id")]
        public virtual gatbl_Carreras gatbl_Carreras { get; set; }

        [ForeignKey(name: "lEscalaCalificacion_id")]
        public virtual gatbl_EscalaCalificaciones gatbl_EscalaCalificaciones { get; set; }

        public virtual ICollection<gatbl_MateriasConvalidadas> gatbl_MateriasConvalidadas { get; set; }
    }
}