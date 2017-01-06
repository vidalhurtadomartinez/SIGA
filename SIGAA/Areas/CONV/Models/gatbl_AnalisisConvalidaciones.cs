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
    public class gatbl_AnalisisConvalidaciones
    {
        [Key]
        public int lAnalisisConvalidacion_id { get; set; }

        [Display(Name = "Pre Analisis", Description = "Pre Analisis")]
        [Required]
        public int lAnalisisPreConvalidacion_id { get; set; }

        [Display(Name = "Responsable", Description = "Responsable")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(10)]
        public string lResponsable_id { get; set; }
        
        [Display(Name = "Fecha de Analisis", Description = "Fecha de Analisis")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        //[DisplayFormat(DataFormatString = "{0:d}")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime dtAnalisisConvalidacion_dt { get; set; }

        [Display(Name = "% Equivalencia Promedio", Description = "% Equivalencia Promedio")]
        [Required]
        public decimal dEquivalencia_Promedio { get; set; }

        [Display(Name = "Equivalencia %", Description = "Equivalencia %")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(400)]
        public string sEquivalencia_porc { get; set; }

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


        [ForeignKey(name: "lAnalisisPreConvalidacion_id")]
        public virtual gatbl_AnalisisPreConvalidaciones gatbl_AnalisisPreConvalidaciones { get; set; }        

        [ForeignKey(name: "lResponsable_id")]
        public virtual agenda Responsables { get; set; }

        public virtual ICollection<gatbl_DAnalisisConvalidaciones> gatbl_DAnalisisConvalidaciones { get; set; }
    }
}