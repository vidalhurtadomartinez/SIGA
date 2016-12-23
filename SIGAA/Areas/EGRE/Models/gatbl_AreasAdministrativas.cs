using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public class gatbl_AreasAdministrativas:BaseModelo
    {
        public gatbl_AreasAdministrativas() {
            sNombre_nm = string.Empty;
            sUbicacion_desc = string.Empty;
            sTelefono_desc = string.Empty;
            sCoordinador_nm = string.Empty;
        }

        [Key]
        public int iAreaAdministrativa_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 1)]
        [Display(Name = "Area Administrativa")]
        public string sNombre_nm { get; set; }


        [StringLength(200, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 3)]
        [Display(Name = "Ubicación")]
        public string sUbicacion_desc { get; set; }


        [StringLength(50, ErrorMessage = "el campo {0} debe tener entre {2} y {1} caracteres", MinimumLength = 3)]
        [Display(Name = "Teléfono")]
        public string sTelefono_desc { get; set; }


        [StringLength(50, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 3)]
        [Display(Name = "Jefe / Coordinador")]
        public string sCoordinador_nm { get; set; }

        public virtual IEnumerable<gatbl_ComunicacionInt> ComunicacionesInternas { get; set; }
    }
}