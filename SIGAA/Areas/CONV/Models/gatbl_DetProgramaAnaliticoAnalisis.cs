using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.CONV.Models
{
    public class gatbl_DetProgramaAnaliticoAnalisis
    {
        [Display(Name = "Unidad ID")]
        public int lDetProgramaAnalitico_id { get; set; }

        [Display(Name = "Analisis ID")]
        public int? lAnalisisConvalidacionUnidad_id { get; set; }

        [Display(Name = "Numero Unidad")]
        public int sNumero { get; set; }

        [Display(Name = "Unidad")]
        public string sDescripcion_desc { get; set; }

        [Display(Name = "Nro. de Temas")]
        public int? CantidadMateria { get; set; }

        [Display(Name = "Equivalencia")]
        public decimal? equivalencia { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [Display(Name = "Materia")]
        public string MateriaDesc { get; set; }
    }
}