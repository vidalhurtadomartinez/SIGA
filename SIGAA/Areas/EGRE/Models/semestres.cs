using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIGAA.Areas.EGRE.Models
{
    public class semestres
    {
        [Key]
        public string sem_codigo { get; set; }
        public DateTime sem_fchinicial { get; set; }
        public DateTime sem_fchfinal { get; set; }
    }
}
