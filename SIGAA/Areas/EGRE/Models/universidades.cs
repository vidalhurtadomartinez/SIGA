using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public class universidades
    {
        [Key]
        [Display(Name = "Universidad")]
        public string unv_codigo { get; set; }

        [Display(Name = "Universidad")]
        public string unv_descripcion { get; set; }

        [Display(Name = "Sigla Universidad")]
        public string unv_sigla { get; set; }
        public string cdd_codigo { get; set; }

        //propiedades de navegacion
        public virtual IEnumerable<gatbl_RectorUniversidadPublica> RectoresUiversidadPublicas  { get; set; }
    }
}