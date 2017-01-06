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
    public class gatbl_AnalisisConvalidacionesUnidad
    {
        [Key]
        public int lAnalisisConvalidacionUnidad_id { get; set; }

        [Display(Name = "Pre Analisis Detalle", Description = "Pre Analisis Detalle")]
        [Required]
        public int lDetAnalisisPreConvalidacion_id { get; set; }

        [Display(Name = "Unidad de Analisis", Description = "Unidad de Analisis")]
        [Required]
        public int lDetProgramaAnalitico_id { get; set; }

        [Display(Name = "Responsable", Description = "Responsable")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(10)]
        public string lResponsable_id { get; set; }

        [Display(Name = "Fecha de Analisis", Description = "Fecha de Analisis")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [DataType(DataType.Date)]
        public DateTime dtAnalisisConvalidacionUnidad_dt { get; set; }

        [Display(Name = "Equivalencia Unidad", Description = "Equivalencia Unidad")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Range(1, 100,
        ErrorMessage = "El valor de {0} debe estar entre {1} y {2}.")]
        public decimal dEquivalencia_Unidad { get; set; }        

        [Display(Name = "Observaciones", Description = "Observaciones")]
        [StringLength(250)]
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


        [ForeignKey(name: "lDetAnalisisPreConvalidacion_id")]
        public virtual gatbl_DetAnalisisPreConvalidaciones gatbl_DetAnalisisPreConvalidaciones { get; set; }

        [ForeignKey(name: "lDetProgramaAnalitico_id")]
        public virtual gatbl_DetProgramasAnaliticos gatbl_DetProgramasAnaliticos { get; set; }

        [ForeignKey(name: "lResponsable_id")]
        public virtual agenda Responsables { get; set; }


        public virtual ICollection<gatbl_DAnalisisConvalidacionesUnidad> gatbl_DAnalisisConvalidacionesUnidad { get; set; }
    }
}