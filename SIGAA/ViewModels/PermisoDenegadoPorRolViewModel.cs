using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.ViewModels
{
    public class PermisoDenegadoPorRolViewModel
    {
        [Display(Name = "Rol :")]
        public int iRol_id { get; set; }
        public List<MetodoDeAccionViewModel> MetodosDeAccionDeProcesoSeleccionado { get; set; }
        public List<ProcesoViewModel> ProcesosSEGU { get; set; }
        public List<ProcesoViewModel> ProcesosEGRE { get; set; }
        public List<ProcesoViewModel> ProcesosOYM { get; set; }
        public List<ProcesoViewModel> ProcesosCONV { get; set; }
        public List<ProcesoViewModel> ProcesosCRM { get; set; }

    }
}