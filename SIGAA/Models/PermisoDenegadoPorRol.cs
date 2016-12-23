using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using SIGAA.Commons;

namespace SIGAA.Models
{
    [Table("PermisoDenegadoPorRol")]
    public class PermisoDenegadoPorRol:BaseModel
    {
        [Key]
        [Column(Order = 0)]
        [Display(Name = "Rol :")]
        public int iRol_id { get; set; }
      
        [Key]
        [Column(Order = 1)]
        [Display(Name = "Permiso :")]
        public RolesPermisos iPermiso_id { get; set; }

        //propiedades de Navegacion
        public virtual Rol Rol { get; set;}
        public virtual Permiso Permiso { get; set; }

    }
}