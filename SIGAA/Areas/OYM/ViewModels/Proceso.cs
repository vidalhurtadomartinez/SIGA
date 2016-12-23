using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIGAA.Areas.OYM.Models;

namespace SIGAA.Areas.OYM.ViewModels
{
    public class Proceso
    {
        public tblProceso tblProceso { get; set; }
        public tblProcesoDetalle tblProcesoDetalle { get; set; }
        public tblProcesoDetalleCategoria tblProcesoDetalleCategoria { get; set; }
        public List<tblProcesoDetalle> tblProcesoDetalles { get; set; }
        public List<tblProcesoDetalleCategoria> tblProcesoDetalleCategorias { get; set; }
        public List<tblProcesoDetalle> ProcesosEliminados { get; set; }
        public List<tblProcesoDetalleCategoria> ProcesosCategoriaEliminados { get; set; }
    }
}