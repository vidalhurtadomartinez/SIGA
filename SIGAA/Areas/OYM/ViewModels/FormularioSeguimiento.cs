using SIGAA.Areas.OYM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.OYM.ViewModels
{
    public class FormularioSeguimiento
    {
        public tblFormularioProceso tblFormularioProceso { get; set; }
        public tblFormulario tblFormulario { get; set; }
        public tblFormularioRelacionado tblFormularioRelacionado { get; set; }
    }
}