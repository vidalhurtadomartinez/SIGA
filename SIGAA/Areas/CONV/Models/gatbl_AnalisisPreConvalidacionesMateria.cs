using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.CONV.Models
{
    public class gatbl_AnalisisPreConvalidacionesMateria
    {
        [Key]
        public int lAnalisisPreConvalidacionMateria_id { get; set; }

        [Display(Name = "Materia", Description = "Materia")]
        [Required]
        public int lProgramaAnalitico_id { get; set; }

        [Display(Name = "Origen", Description = "Origen")]
        [Required]
        public int lOrigen { get; set; }
                
        [Display(Name = "Detalle Materias", Description = "Detalle Materias")]
        [Required]
        public int lDetAnalisisPreConvalidacion_id { get; set; }


        [ForeignKey(name: "lProgramaAnalitico_id")]
        public virtual gatbl_ProgramasAnaliticos gatbl_ProgramasAnaliticos { get; set; }

        [ForeignKey(name: "lDetAnalisisPreConvalidacion_id")]
        public virtual gatbl_DetAnalisisPreConvalidaciones gatbl_DetAnalisisPreConvalidaciones { get; set; }
    }
}