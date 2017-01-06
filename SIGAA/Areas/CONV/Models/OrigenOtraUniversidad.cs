using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIGAA.Areas.CONV.Models
{
    public class OrigenOtraUniversidad
    {
        [Key]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Codigo")]
        [DisplayFormat(NullDisplayText = "Codigo")]
        [StringLength(2, ErrorMessage = "El campo {0} debe tener {1} caracteres.", MinimumLength = 2)]
        public string sOrigen_fl { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Origen")]
        [DisplayFormat(NullDisplayText = "Descripcion")]
        [StringLength(30, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 4)]
        public string sDescripcion { get; set; }

        [Display(Name = "Activo")]
        [DefaultValue(true)]
        public bool bActivo { get; set; }

        public virtual ICollection<gatbl_Universidades> gatbl_Universidades { get; set; }
    }
}