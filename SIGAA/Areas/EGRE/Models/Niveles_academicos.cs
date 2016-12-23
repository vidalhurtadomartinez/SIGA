using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIGAA.Areas.EGRE.Models
{
    public class Niveles_academicos
    {
        [Key]
        public string Nva_codigo { get; set; }
        public string Nva_descripcion { get; set; }
        public bool Nva_status { get; set; }

        //propiedades de navegacion
        public virtual IEnumerable<carreras> carreras { get; set; }

    }
}