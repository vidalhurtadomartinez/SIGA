using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Models
{
    [Table("HistorialCambioRolDeUsuario")]
    public class HistorialCambioRolDeUsuario: BaseModel
    {
        [Key]
        public int idCambioRol { get; set; }

        [Required(ErrorMessage = "Debe ingresar valores al campo {0} porque es Obligatorio.")]
        [Display(Name = "Usuario :")]
        public int iUsuario_id { get; set; }

        [Display(Name = "Fecha de cambio :")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime dtFechaCambio { get; set; }

        [Required(ErrorMessage = "Debe ingresar valores al campo {0} porque es Obligatorio.")]
        [Display(Name = "Rol Anterior :")]
        public int idRolAnterior { get; set; }

        [Required(ErrorMessage = "Debe ingresar valores al campo {0} porque es Obligatorio.")]
        [Display(Name = "Rol Actual :")]
        public int idRolActual { get; set; }

        [StringLength(500, ErrorMessage = "El campo {0} debe tener una longitud maxima de {1} caracteres.")]
        [Display(Name = "Observación :")]
        [DataType(DataType.MultilineText)]
        public string sObservacion { get; set; }

        //propiedades de navegacion
        [ForeignKey("idRolAnterior")]
        public virtual Rol RolAnterior { get; set; }

        [ForeignKey("idRolActual")]
        public virtual Rol RolActual { get; set; }

        public virtual Usuario Usuario { get; set; }

    }
}