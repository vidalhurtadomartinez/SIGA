using SIGAA.Areas.CONV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.CONV.ViewModels
{
    public class ConvalidacionExternaViewModels
    {
        public gatbl_ConvalidacionesExternasPA gatbl_ConvalidacionesExternasPA { get; set; }
        public gatbl_DConvalidacionesExternasPA gatbl_DConvalidacionExternaPA { get; set; }
        public gatbl_MateriasConvalidadas gatbl_MateriaConvalidada { get; set; }
        public List<gatbl_DConvalidacionesExternasPA> gatbl_DConvalidacionesExternasPA { get; set; }
        public List<gatbl_MateriasConvalidadas> gatbl_MateriasConvalidadasOrigen { get; set; }
        public List<gatbl_ProgramasAnaliticos> gatbl_ProgramasAnaliticos { get; set; }
    }
}