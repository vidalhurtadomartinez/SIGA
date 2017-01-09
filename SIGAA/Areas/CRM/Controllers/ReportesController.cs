using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Reporting.Processing;

namespace SIGAA.Areas.CRM.Controllers
{
    public class ReportesController : Controller
    {
        // GET: Reportes
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PosiblesClientes()
        {
            ReportFollowUp.rptCliente report = new ReportFollowUp.rptCliente();
            //report.ReportParameters["Proceso"].Value = 0;

            return View(report);
        }

        public ActionResult Eventos()
        {
            ReportFollowUp.rptEvento report = new ReportFollowUp.rptEvento();
            //report.ReportParameters["Proceso"].Value = 0;

            return View(report);
        }

        //public ActionResult FormularioTipoProceso()
        //{
        //    ReportFollowUp.rptFormularioTipoProceso report = new ReportFollowUp.rptFormularioTipoProceso();
        //    //report.ReportParameters["Proceso"].Value = 0;

        //    return View(report);
        //}

        //public ActionResult DocumentoTiempoTipoProceso()
        //{
        //    ReportFollowUp.rptDocumentoPromedioRespuesta report = new ReportFollowUp.rptDocumentoPromedioRespuesta();
        //    //report.ReportParameters["Proceso"].Value = 0;

        //    return View(report);
        //}

        //public ActionResult FormularioTiempoTipoProceso()
        //{
        //    ReportFollowUp.rptFormularioPromedioRespuesta report = new ReportFollowUp.rptFormularioPromedioRespuesta();
        //    //report.ReportParameters["Proceso"].Value = 0;

        //    return View(report);
        //}

        //public ActionResult DocumentoDetalleRespuestaTipoProceso()
        //{
        //    ReportFollowUp.rptDocumentoDetalleRespuesta report = new ReportFollowUp.rptDocumentoDetalleRespuesta();
        //    //report.ReportParameters["Proceso"].Value = 0;

        //    return View(report);
        //}

        //public ActionResult FormularioDetalleRespuestaTipoProceso()
        //{
        //    ReportFollowUp.rptFormularioDetalleRespuesta report = new ReportFollowUp.rptFormularioDetalleRespuesta();
        //    //report.ReportParameters["Proceso"].Value = 0;

        //    return View(report);
        //}

        public ActionResult EvaluacionPorID(int id)
        {
            ReportProcessor reportProcessor = new ReportProcessor();
            Telerik.Reporting.InstanceReportSource instanceReportSource = new Telerik.Reporting.InstanceReportSource();

            //ConvalidacionesReports.rptInformeFinal rpt = new ConvalidacionesReports.rptInformeFinal();
            //rpt.ReportParameters["ID"].Value = ID;

            ReportFollowUp.rptEvaluacionPorID rpt = new ReportFollowUp.rptEvaluacionPorID();
            rpt.ReportParameters["Id"].Value = id;

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

        public ActionResult EvaluacionPlantilla(int id)
        {
            ReportProcessor reportProcessor = new ReportProcessor();
            Telerik.Reporting.InstanceReportSource instanceReportSource = new Telerik.Reporting.InstanceReportSource();

            //ConvalidacionesReports.rptInformeFinal rpt = new ConvalidacionesReports.rptInformeFinal();
            //rpt.ReportParameters["ID"].Value = ID;

            ReportFollowUp.rptEvaluacionPlantilla rpt = new ReportFollowUp.rptEvaluacionPlantilla();
            rpt.ReportParameters["Id"].Value = id;

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

        //public ActionResult DocumentoPorTipoProceso()
        //{
        //    ReportProcessor reportProcessor = new ReportProcessor();
        //    Telerik.Reporting.InstanceReportSource instanceReportSource = new Telerik.Reporting.InstanceReportSource();

        //    //ConvalidacionesReports.rptInformeFinal rpt = new ConvalidacionesReports.rptInformeFinal();
        //    //rpt.ReportParameters["ID"].Value = ID;

        //    ReportFollowUp.rptDocumentoProceso rpt = new ReportFollowUp.rptDocumentoProceso();
        //    //rpt.ReportParameters["Proceso"].Value = 0;

        //    instanceReportSource.ReportDocument = rpt;
        //    RenderingResult result = reportProcessor.RenderReport("PDF", instanceReportSource, null);


        //    this.Response.Clear();
        //    this.Response.ContentType = result.MimeType;
        //    this.Response.Cache.SetCacheability(HttpCacheability.Private);
        //    this.Response.Expires = -1;
        //    this.Response.Buffer = true;


        //    this.Response.BinaryWrite(result.DocumentBytes);
        //    this.Response.End();

        //    return View();
        //}

        //public ActionResult FormularioPorTipoProceso()
        //{
        //    ReportProcessor reportProcessor = new ReportProcessor();
        //    Telerik.Reporting.InstanceReportSource instanceReportSource = new Telerik.Reporting.InstanceReportSource();

        //    //ConvalidacionesReports.rptInformeFinal rpt = new ConvalidacionesReports.rptInformeFinal();
        //    //rpt.ReportParameters["ID"].Value = ID;

        //    ReportFollowUp.rptFormularioProceso rpt = new ReportFollowUp.rptFormularioProceso();
        //    //rpt.ReportParameters["Proceso"].Value = 0;

        //    instanceReportSource.ReportDocument = rpt;
        //    RenderingResult result = reportProcessor.RenderReport("PDF", instanceReportSource, null);


        //    this.Response.Clear();
        //    this.Response.ContentType = result.MimeType;
        //    this.Response.Cache.SetCacheability(HttpCacheability.Private);
        //    this.Response.Expires = -1;
        //    this.Response.Buffer = true;


        //    this.Response.BinaryWrite(result.DocumentBytes);
        //    this.Response.End();

        //    return View();
        //}

        //public ActionResult DocumentoPromedio()
        //{
        //    ReportProcessor reportProcessor = new ReportProcessor();
        //    Telerik.Reporting.InstanceReportSource instanceReportSource = new Telerik.Reporting.InstanceReportSource();

        //    //ConvalidacionesReports.rptInformeFinal rpt = new ConvalidacionesReports.rptInformeFinal();
        //    //rpt.ReportParameters["ID"].Value = ID;

        //    ReportFollowUp.rptDocumentoPromedio rpt = new ReportFollowUp.rptDocumentoPromedio();
        //    //rpt.ReportParameters["Proceso"].Value = 0;

        //    instanceReportSource.ReportDocument = rpt;
        //    RenderingResult result = reportProcessor.RenderReport("PDF", instanceReportSource, null);


        //    this.Response.Clear();
        //    this.Response.ContentType = result.MimeType;
        //    this.Response.Cache.SetCacheability(HttpCacheability.Private);
        //    this.Response.Expires = -1;
        //    this.Response.Buffer = true;


        //    this.Response.BinaryWrite(result.DocumentBytes);
        //    this.Response.End();

        //    return View();
        //}

        //public ActionResult DocumentoHistorico()
        //{
        //    ReportProcessor reportProcessor = new ReportProcessor();
        //    Telerik.Reporting.InstanceReportSource instanceReportSource = new Telerik.Reporting.InstanceReportSource();

        //    //ConvalidacionesReports.rptInformeFinal rpt = new ConvalidacionesReports.rptInformeFinal();
        //    //rpt.ReportParameters["ID"].Value = ID;

        //    ReportFollowUp.rptDocumentoHistorico rpt = new ReportFollowUp.rptDocumentoHistorico();
        //    //rpt.ReportParameters["Proceso"].Value = 0;

        //    instanceReportSource.ReportDocument = rpt;
        //    RenderingResult result = reportProcessor.RenderReport("PDF", instanceReportSource, null);


        //    this.Response.Clear();
        //    this.Response.ContentType = result.MimeType;
        //    this.Response.Cache.SetCacheability(HttpCacheability.Private);
        //    this.Response.Expires = -1;
        //    this.Response.Buffer = true;


        //    this.Response.BinaryWrite(result.DocumentBytes);
        //    this.Response.End();

        //    return View();
        //}
    }
}