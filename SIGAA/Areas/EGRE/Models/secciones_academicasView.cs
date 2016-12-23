using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public class secciones_academicasView
    {
        [Key]
        public string sca_codigo { get; set; }
        public string sca_descripcion { get; set; }
        public string sca_sigla { get; set; }
        public int sca_cantidad_materias_periodo { get; set; }
        public bool sca_status { get; set; }
        public int secuencial { get; set; }
    }
}