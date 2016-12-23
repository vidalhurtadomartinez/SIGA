using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public class Tipo_estado
    {
        [Key]
        public string tstd_codigo { get; set; }
        public string tstd_descripcion { get; set; }
        public bool tstd_activo { get; set; }
        public bool tstd_status { get; set; }
        public string tstd_tipo { get; set; }

        public virtual IEnumerable<alumnos_agenda> Alumnos { get; set; }
    }
}