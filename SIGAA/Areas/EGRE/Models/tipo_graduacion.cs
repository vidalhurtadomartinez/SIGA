using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIGAA.Areas.EGRE.Models
{
    public class tipo_graduacion
    {
        [Key]
        public string tttg_codigo { get; set; }
        public string tttg_descripcion { get; set; }
        public bool tttg_status { get; set; }
        public string nva_codigo { get; set; }

        //propiedades de navegacion
        public virtual IEnumerable<gatbl_Perfiles> Perfiles { get; set; }
    }
}
