using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Reporting.Processing;
using SIGAA.Commons;
using SIGAA.Etiquetas;

namespace SIGAA.Areas.OYM.Controllers
{
    [Autenticado]
    public class ReportesController : Controller
    {
        // GET: Reportes
        public ActionResult Index()
        {
            return View();
        }

        [Permiso(Permiso = RolesPermisos.OYM_reportes_documentosPorTipoDeProceso)]
        public ActionResult DocumentoTipoProceso()
        {
            ReportFollowUp.rptDocumentoTipoProceso report = new ReportFollowUp.rptDocumentoTipoProceso();
            //report.ReportParameters["Proceso"].Value = 0;

            return View(report);
        }

        [Permiso(Permiso = RolesPermisos.OYM_reportes_formulariosPorTipoDeProcesos)]
        public ActionResult FormularioTipoProceso()
        {
            ReportFollowUp.rptFormularioTipoProceso report = new ReportFollowUp.rptFormularioTipoProceso();
            //report.ReportParameters["Proceso"].Value = 0;

            return View(report);
        }

        [Permiso(Permiso = RolesPermisos.OYM_reportes_promedioDeRespuestasPorProcesosDeDocumentos)]
        public ActionResult DocumentoTiempoTipoProceso()
        {
            ReportFollowUp.rptDocumentoPromedioRespuesta report = new ReportFollowUp.rptDocumentoPromedioRespuesta();
            //report.ReportParameters["Proceso"].Value = 0;

            return View(report);
        }

        [Permiso(Permiso = RolesPermisos.OYM_reportes_promedioDeRespuestasPorProcesosDeFormularios)]
        public ActionResult FormularioTiempoTipoProceso()
        {
            ReportFollowUp.rptFormularioPromedioRespuesta report = new ReportFollowUp.rptFormularioPromedioRespuesta();
            //report.ReportParameters["Proceso"].Value = 0;

            return View(report);
        }

        [Permiso(Permiso = RolesPermisos.OYM_reportes_DetalleDeRespuestasPorProcesosDeDocumentos)]
        public ActionResult DocumentoDetalleRespuestaTipoProceso()
        {
            ReportFollowUp.rptDocumentoDetalleRespuesta report = new ReportFollowUp.rptDocumentoDetalleRespuesta();
            //report.ReportParameters["Proceso"].Value = 0;

            return View(report);
        }

        [Permiso(Permiso = RolesPermisos.OYM_reportes_DetalleDeRespuestasPorProcesosDeFormularios)]
        public ActionResult FormularioDetalleRespuestaTipoProceso()
        {
            ReportFollowUp.rptFormularioDetalleRespuesta report = new ReportFollowUp.rptFormularioDetalleRespuesta();
            //report.ReportParameters["Proceso"].Value = 0;

            return View(report);
        }

        public ActionResult DocumentoPorTipoProceso()
        {
            ReportProcessor reportProcessor = new ReportProcessor();
            Telerik.Reporting.InstanceReportSource instanceReportSource = new Telerik.Reporting.InstanceReportSource();

            //ConvalidacionesReports.rptInformeFinal rpt = new ConvalidacionesReports.rptInformeFinal();
            //rpt.ReportParameters["ID"].Value = ID;

            ReportFollowUp.rptDocumentoProceso rpt = new ReportFollowUp.rptDocumentoProceso();
            //rpt.ReportParameters["Proceso"].Value = 0;

            instanceReportSource.ReportDocument = rpt;
            RenderingResult result = reportProcessor.RenderReport("PDF", instanceReportSource, null);

            
            this.Response.Clear();
            this.Response.ContentType = result.MimeType;
            this.Response.Cache.SetCacheability(HttpCacheability.Private);
            this.Response.Expires = -1;
            this.Response.Buffer = true;

           
            this.Response.BinaryWrite(result.DocumentBytes);
            this.Response.End();

            return View();
        }

        public ActionResult FormularioPorTipoProceso()
        {
            ReportProcessor reportProcessor = new ReportProcessor();
            Telerik.Reporting.InstanceReportSource instanceReportSource = new Telerik.Reporting.InstanceReportSource();

            //ConvalidacionesReports.rptInformeFinal rpt = new ConvalidacionesReports.rptInformeFinal();
            //rpt.ReportParameters["ID"].Value = ID;

            ReportFollowUp.rptFormularioProceso rpt = new ReportFollowUp.rptFormularioProceso();
            //rpt.ReportParameters["Proceso"].Value = 0;

            instanceReportSource.ReportDocument = rpt;
            RenderingResult result = reportProcessor.RenderReport("PDF", instanceReportSource, null);


            this.Response.Clear();
            this.Response.ContentType = result.MimeType;
            this.Response.Cache.SetCacheability(HttpCacheability.Private);
            this.Response.Expires = -1;
            this.Response.Buffer = true;


            this.Response.BinaryWrite(result.DocumentBytes);
            this.Response.End();

            return View();
        }

        public ActionResult DocumentoPromedio()
        {
            ReportProcessor reportProcessor = new ReportProcessor();
            Telerik.Reporting.InstanceReportSource instanceReportSource = new Telerik.Reporting.InstanceReportSource();

            //ConvalidacionesReports.rptInformeFinal rpt = new ConvalidacionesReports.rptInformeFinal();
            //rpt.ReportParameters["ID"].Value = ID;

            ReportFollowUp.rptDocumentoPromedio rpt = new ReportFollowUp.rptDocumentoPromedio();
            //rpt.ReportParameters["Proceso"].Value = 0;

            instanceReportSource.ReportDocument = rpt;
            RenderingResult result = reportProcessor.RenderReport("PDF", instanceReportSource, null);


            this.Response.Clear();
            this.Response.ContentType = result.MimeType;
            this.Response.Cache.SetCacheability(HttpCacheability.Private);
            this.Response.Expires = -1;
            this.Response.Buffer = true;


            this.Response.BinaryWrite(result.DocumentBytes);
            this.Response.End();

            return View();
        }

        public ActionResult DocumentoHistorico()
        {
            ReportProcessor reportProcessor = new ReportProcessor();
            Telerik.Reporting.InstanceReportSource instanceReportSource = new Telerik.Reporting.InstanceReportSource();

            //ConvalidacionesReports.rptInformeFinal rpt = new ConvalidacionesReports.rptInformeFinal();
            //rpt.ReportParameters["ID"].Value = ID;

            ReportFollowUp.rptDocumentoHistorico rpt = new ReportFollowUp.rptDocumentoHistorico();
            //rpt.ReportParameters["Proceso"].Value = 0;

            instanceReportSource.ReportDocument = rpt;
            RenderingResult result = reportProcessor.RenderReport("PDF", instanceReportSource, null);


            this.Response.Clear();
            this.Response.ContentType = result.MimeType;
            this.Response.Cache.SetCacheability(HttpCacheability.Private);
            this.Response.Expires = -1;
            this.Response.Buffer = true;


            this.Response.BinaryWrite(result.DocumentBytes);
            this.Response.End();

            return View();
        }
    }
}