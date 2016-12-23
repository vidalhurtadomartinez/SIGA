using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public class gatbl_Profesiones : BaseModelo
    {
        public gatbl_Profesiones()
        {
            sNombre_nm = string.Empty;
        }

        [Key]
        public int iProfesion_id { get; set; }

        [Required(ErrorMessage = "el campo {0} es requerido.")]
        [StringLength(50, ErrorMessage = "el campo {0} puede tener entre {2} y {1} caracteres.", MinimumLength = 3)]
        [Display(Name = "Profesión")]
        public string sNombre_nm { get; set; }

        public virtual IEnumerable<gatbl_Tutores> Tutores { get; set; }
    }
}