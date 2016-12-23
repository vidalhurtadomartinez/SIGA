using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIGAA.Areas.EGRE.Models
{    
    public class secciones_academicas
    {
        [Key]
        public string sca_codigo { get; set; }
        public string sca_descripcion { get; set; }
        public string sca_sigla { get; set; }
        public int sca_cantidad_materias_periodo { get; set; }
        public bool sca_status { get; set; }
        public int secuencial { get; set; }

        //propiedades de navegabilidad
        public virtual IEnumerable<carreras> Carreras { get; set; }

    }
}
