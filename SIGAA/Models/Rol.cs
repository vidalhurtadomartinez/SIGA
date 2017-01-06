using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace SIGAA.Models
{
    [Table("Rol")]
    public class Rol: BaseModel
    {
        public Rol()
        {
            this.Nombre = "";
            this.Observacion = "";
        }

        [Key]
        public int iRol_id { get; set; }

        [Required(ErrorMessage = "Debe ingresar valores al campo {0} porque es Obligatorio.")]
        [StringLength(250, ErrorMessage = "La longitud máxima no debe sobre pasar los {0} caracteres.")]
        [Display(Name = "Rol :")]
        public string Nombre { get; set; }

        [Display(Name = "Observación :")]
        [StringLength(500,ErrorMessage ="La longitud máxima no debe sobre pasar los {0} caracteres.")]
        [DataType(DataType.MultilineText)]
        public string Observacion { get; set; }

        [Display(Name = "Directorio :")]
        public int? Directorio { get; set; }

        //propiedades de navegacion
        public virtual ICollection<Usuario> Usuarios { get; set; }
        public virtual ICollection<PermisoDenegadoPorRol> PermisoDenegadoRoles { get; set; }

        [InverseProperty("idRolAnterior")]
        public virtual IEnumerable<HistorialCambioRolDeUsuario>  HistorialCambioRolDeUsuarios_anterior { get; set; }

        [InverseProperty("idRolActual")]
        public virtual IEnumerable<HistorialCambioRolDeUsuario> HistorialCambioRolDeUsuarios_actual { get; set; }
    }
}
