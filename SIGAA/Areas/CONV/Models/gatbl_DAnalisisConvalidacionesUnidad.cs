using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.CONV.Models
{
    public class gatbl_DAnalisisConvalidacionesUnidad
    {
        [Key]
        public int lDAnalisisConvalidacionUnidad_id { get; set; }

        [Display(Name = "Analisis Unidad", Description = "Analisis Unidad")]
        [Required]
        public int lAnalisisConvalidacionUnidad_id { get; set; }

        [Display(Name = "Temas", Description = "Temas")]
        [Required]
        public int lDetProgramaAnalitico_id { get; set; }


        [ForeignKey(name: "lAnalisisConvalidacionUnidad_id")]
        public virtual gatbl_AnalisisConvalidacionesUnidad gatbl_AnalisisConvalidacionesUnidad { get; set; }

        [ForeignKey(name: "lDetProgramaAnalitico_id")]
        public virtual gatbl_DetProgramasAnaliticos gatbl_DetProgramasAnaliticos { get; set; }
    }
}