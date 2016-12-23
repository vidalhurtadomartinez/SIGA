using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIGAA.Areas.EGRE.Models
{
    public class disciplinas_academicas
    {
        [Key]
        public string dsc_codigo { get; set; }
        public string dsc_descripcion { get; set; }
        public string aac_codigo { get; set; }
        public bool dsc_status { get; set; }
    }
}
