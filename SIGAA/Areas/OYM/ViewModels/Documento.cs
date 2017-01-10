using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIGAA.Areas.OYM.Models;

namespace SIGAA.Areas.OYM.ViewModels
{
    public class Documento
    {
        public tblDocumento tblDocumento { get; set; }
        public tblDocumentoRelacionado tblDocumentoRelacionado { get; set; }
        public List<tblDocumentoRelacionado> tblDocumentoRelacionados { get; set; }
        public List<tblDocumentoRelacionado> DocumentosEliminados { get; set; }
    }
}