using SIGAA.Areas.OYM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.OYM.ViewModels
{
    public class DocumentoSeguimiento
    {
        public tblDocumentoProceso tblDocumentoProceso { get; set; }
        public tblDocumento tblDocumento { get; set; }
        public tblDocumentoRelacionado tblDocumentoRelacionado { get; set; }        
    }
}