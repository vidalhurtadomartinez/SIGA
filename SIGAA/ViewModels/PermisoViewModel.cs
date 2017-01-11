using SIGAA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGAA.ViewModels
{
    public class PermisoViewModel
    {
        public List<Permiso> PermisosSEGU { get; set; }
        public List<Permiso> PermisosEGRE { get; set; }
        public List<Permiso> PermisosOYM { get; set; }
        public List<Permiso> PermisosCONV { get; set; }
        public List<Permiso> PermisosCRM { get; set; }
    }
}