using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public class gatbl_CProfesionales : BaseModelo
    {
        public gatbl_CProfesionales()
        {
            sNombre_nm = string.Empty;
            sDiereccion_desc = string.Empty;
            sTelefono_desc = string.Empty;
            sRepresentante_nm = string.Empty;
        }

        [Key]
        public int iCProfesional_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(100, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 3)]
        [Display(Name = "Nombre")]
        public string sNombre_nm { get; set; }

        [StringLength(100, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 3)]
        [Display(Name = "Dirección")]
        public string sDiereccion_desc { get; set; }

        [StringLength(50, ErrorMessage = "el campo {0} debe tener entre {2} y {1} caracteres", MinimumLength = 3)]
        [Display(Name = "Teléfono")]
        public string sTelefono_desc { get; set; }

        [StringLength(50, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 3)]
        [Display(Name = "Representante")]
        public string sRepresentante_nm { get; set; }

        public virtual IEnumerable<gatbl_Tutores> Tutores { get; set; }
        public virtual IEnumerable<gatbl_PresidenteColegioProfesional> PresidentesColegioProfesionales { get; set; }
    }
}