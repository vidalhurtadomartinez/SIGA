using SIGAA.Areas.CONV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.CONV.ViewModels
{
    public class SolicitudConvalidacion
    {
        public gatbl_PConvalidaciones gatbl_PConvalidaciones { get; set; }
        public gatbl_DPConvalidaciones gatbl_DPConvalidacion { get; set; }
        public gatbl_CertificadosMateria gatbl_CertificadosMateria { get; set; }
        public List<gatbl_DPConvalidaciones> gatbl_DPConvalidaciones { get; set; }
        public List<gatbl_CertificadosMateria> gatbl_CertificadosMaterias { get; set; }
    }
}