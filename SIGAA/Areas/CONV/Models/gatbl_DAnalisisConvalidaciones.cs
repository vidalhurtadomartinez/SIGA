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
    public class gatbl_DAnalisisConvalidaciones
    {
        [Key]
        public int lDAnalisisConvalidacion_id { get; set; }

        [Display(Name = "Analisis Convalidacion", Description = "Analisis Convalidacion")]
        [Required]
        public int lAnalisisConvalidacion_id { get; set; }

        [Display(Name = "Origen Materia", Description = "Origen Materia")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(2)]
        public string sOrigenMateria_fl { get; set; }

        [Display(Name = "Materia", Description = "Materia")]
        [Required]
        public int lMateria_id { get; set; }

        [Display(Name = "Unidad", Description = "Unidad")]
        [Required]
        public int lUnidad_id { get; set; }

        [Display(Name = "Tema", Description = "Tema")]
        [Required]
        public int lTema_id { get; set; }

        [Display(Name = "Calificacion Unidad", Description = "Calificacion Unidad")]
        [Required]
        public decimal CalificacionUnidad { get; set; }

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

        
        public virtual gatbl_AnalisisConvalidaciones gatbl_AnalisisConvalidaciones { get; set; }

        [ForeignKey(name: "lMateria_id")]
        public virtual gatbl_ProgramasAnaliticos gatbl_ProgramasAnaliticos { get; set; }

        [ForeignKey(name: "lUnidad_id")]
        public virtual gatbl_DProgramasAnaliticos gatbl_DProgramasAnaliticos { get; set; }

        [ForeignKey(name: "lTema_id")]
        public virtual gatbl_DProgramasAnaliticosTemas gatbl_DProgramasAnaliticosTemas { get; set; }

        public virtual ICollection<gatbl_UnidadesConvalidadas> gatbl_UnidadesConvalidadas { get; set; }
    }
}