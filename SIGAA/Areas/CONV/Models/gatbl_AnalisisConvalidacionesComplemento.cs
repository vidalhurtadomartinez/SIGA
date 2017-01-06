using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.CONV.Models
{
    public class gatbl_AnalisisConvalidacionesComplemento
    {
        [Display(Name = "Detalle Pre Analisis", Description = "Detalle Pre Analisis")]
        public int lDetAnalisisPreConvalidacion_id { get; set; }

        [Display(Name = "Suma Equivalencia", Description = "Suma Equivalencia")]
        public decimal SumaEquivalencia { get; set; }

        [Display(Name = "Unidades Analizadas", Description = "Unidades Analizadas")]
        public int CantidadUnidad { get; set; }

        [Display(Name = "Promedio Equivalencia", Description = "Promedio Equivalencia")]
        public decimal PromedioEquivalencia { get; set; }
    }
}