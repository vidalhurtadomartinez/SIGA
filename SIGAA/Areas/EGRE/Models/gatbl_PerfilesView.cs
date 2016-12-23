using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    [NotMapped]
    public class gatbl_PerfilesView:gatbl_Perfiles
    {
        [Display(Name = "Fecha defensa final programada")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime dtDProgramada_dt { get; set; }
        public string NombreCompleto { get; set; }
        public string Carrera { get; set; }
        public string TipoGraduacionS { get; set; }
    }
}