using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIGAA.Areas.OYM.Models;

namespace SIGAA.Areas.OYM.ViewModels
{
    public class Formulario
    {
        public tblFormulario tblFormulario { get; set; }
        public tblFormularioRelacionado tblFormularioRelacionado { get; set; }
        public List<tblFormularioRelacionado> tblFormularioRelacionados { get; set; }
        public List<tblFormularioRelacionado> FormulariosEliminados { get; set; }
    }
}