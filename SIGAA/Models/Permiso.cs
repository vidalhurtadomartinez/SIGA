using SIGAA.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace SIGAA.Models
{
    [Table("Permiso")]
    public class Permiso:BaseModel
    {
        public Permiso()
        {
            this.Modulo = "";
            this.Nemonico = "";
            this.Descripcion = "";
        }
        [Key]

        [Display(Name = "Permiso :")]
        public RolesPermisos iPermiso_id { get; set;}

        [Required(ErrorMessage = "Debe ingresar valores al campo {0} porque es Obligatorio.")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener como máximo {1} caracteres.")]
        [Display(Name = "Módulo :")]
        public string Modulo { get; set; }

        [Required(ErrorMessage = "Debe ingresar valores al campo {0} porque es Obligatorio.")]
        [StringLength(6, ErrorMessage = "El campo {0} debe tener como máximo {1} caracteres.")]
        [Display(Name = "Nemónico :")]
        public string Nemonico { get; set; }

        [Required(ErrorMessage = "Debe ingresar valores al campo {0} porque es Obligatorio.")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener como máximo {1} caracteres.")]
        [Display(Name = "Proceso :")]
        public string Proceso { get; set; }

        [StringLength(250,ErrorMessage = "El campo {0} debe tener como máximo {1} caracteres.")]
        [Display(Name = "Descripción de acción :")]
        public string Descripcion { get; set; }
        
        //pripiedades de navegacion
        public virtual IEnumerable<PermisoDenegadoPorRol> PermisoDenegadoRoles { get; set; }
    }
}
