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
    public class gatbl_DAnalisisPreConvalidaciones
    {
        [Key]
        public int lDAnalisisConvalidacion_id { get; set; }

        [Display(Name = "Analisis Pre Convalidacion", Description = "Analisis Pre Convalidacion")]
        [Required]
        public int lAnalisisPreConvalidacion_id { get; set; }

        [Display(Name = "Origen Materia", Description = "Origen Materia")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(2)]
        public string sOrigenMateria_fl { get; set; }

        [Display(Name = "Materia", Description = "Materia")]
        [Required]
        public int lMateria_id { get; set; }

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

        
        public virtual gatbl_AnalisisPreConvalidaciones gatbl_AnalisisPreConvalidaciones { get; set; }

        [ForeignKey(name: "lMateria_id")]
        public virtual gatbl_ProgramasAnaliticos gatbl_ProgramasAnaliticos { get; set; }    
    }
}