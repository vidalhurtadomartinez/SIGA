using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.CONV.ViewModels
{
    public class Postulante
    {
        [Display(Name = "Apellido Paterno", Description = "Apellido Paterno")]
        public string sApPaterno_desc { get; set; }

        [Display(Name = "Apellido Materno", Description = "Apellido Materno")]
        public string sApMaterno_desc { get; set; }

        [Display(Name = "Nombres", Description = "Nombres")]
        public string sNombre_desc { get; set; }

        [Display(Name = "Nro. Registro", Description = "Nro. Registro")]
        public string sNroRegistro { get; set; }

        [Display(Name = "Direccion", Description = "Direccion")]
        public string sDireccion_desc { get; set; }

        [Display(Name = "Telefono", Description = "Telefono")]
        public string sTelefonos_desc { get; set; }

        [Display(Name = "E-mail", Description = "E-mail")]
        public string sMail_desc { get; set; }

        [Display(Name = "Nombre Completo", Description = "Nombre Completo")]
        public string NombreCompleto { get; set; }
    }
}