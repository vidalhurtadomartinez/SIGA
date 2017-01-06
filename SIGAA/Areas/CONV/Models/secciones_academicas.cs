using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.CONV.Models
{
    public class secciones_academicas
    {
        [Key]
        public string sca_codigo { get; set; }
        public string sca_descripcion { get; set; }
        public string sca_sigla { get; set; }
        public int sca_cantidad_materias_periodo { get; set; }
        public bool sca_status { get; set; }
        public bool secuencial { get; set; }

        public virtual ICollection<carreras> carreras { get; set; }

        //public virtual ICollection<gatbl_Postulantes> gatbl_Postulantes { get; set; }
        public virtual ICollection<gatbl_ConvalidacionesExternasPA> gatbl_ConvalidacionesExternasPA { get; set; }
    }
}