using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SIGAA.Areas.OYM.Models;
using Novacode;
using System.Diagnostics;
using System.IO;
using System.Configuration;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using SIGAA.Etiquetas;
using SIGAA.Commons;

namespace SIGAA.Areas.OYM.Controllers
{
    [Autenticado]
    public class tblDocumentosController : Controller
    {
        private DataDb db = new DataDb();

       [Permiso(Permiso = RolesPermisos.OYM_documentos_puedeVerIndice)]
        public async Task<ActionResult> Index()
        {
            return View(await db.tblDocumento.Include(t => t.AprobadoPor).Include(t => t.ControlCalidad).Include(t => t.ElaboradoPor).Include(t => t.EstadoDocumento).Include(t => t.OrigenDocumento).Include(t => t.RevisadoPor).Include(t => t.tblPlantilla).Include(t => t.tblTipoDocumento).Include(t => t.tblTipoProceso).Include(t => t.tblDirectorio).ToListAsync());
        }

        private string FilePahtRoot()
        {
            string path = System.Configuration.ConfigurationManager.ConnectionStrings["FilePath"].ConnectionString;

            return path;
        }

        public JsonResult ObtenerResponsables(string text)
        {
            //ConvalidacionExternaViewModels convalidacionExterna = Session["convalidacionExterna"] as ConvalidacionExternaViewModels;

            //if (string.IsNullOrEmpty(text) && convalidacionExterna.gatbl_ConvalidacionesExternasPA.lResponsable_id != null)
            //{
            //    return Json((from ag in db.agendas
            //                 select new { agd_nombres = ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + ", " + ag.agd_nombres.Trim(), agd_codigo = ag.agd_codigo.Trim(), agd_docnro = ag.agd_docnro.Trim() }).Where(g => g.agd_codigo.Contains(convalidacionExterna.gatbl_ConvalidacionesExternasPA.lResponsable_id)).OrderBy(g => g.agd_nombres), JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    return Json((from ag in db.agendas
            //                 select new { agd_nombres = ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + ", " + ag.agd_nombres.Trim(), agd_codigo = ag.agd_codigo.Trim(), agd_docnro = ag.agd_docnro.Trim() }).Where(g => g.agd_nombres.Contains(text)).OrderBy(g => g.agd_nombres), JsonRequestBehavior.AllowGet);
            //}



            //return Json((from ag in db.agenda
            //             join agc in db.tblCargoAgenda on ag.agd_codigo equals agc.agd_codigo
            //             join ca in db.tblCargo on agc.lCargo_id equals ca.lCargo_id
            //             select new { id = agc.lCargoAgenda_id, agd_nombres = ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + ", " + ag.agd_nombres.Trim(), agd_codigo = ag.agd_codigo.Trim(), agd_docnro = ag.agd_docnro.Trim(), cargo = ca.sDescripcion }).Where(g => g.agd_nombres.Contains(text)).OrderBy(g => g.agd_nombres), JsonRequestBehavior.AllowGet);

            return Json((from p in db.vVistaPersonal
                         select new { id = p.idcont, agd_nombres = p.Nombre.Trim(), cargo = p.DescCargoNivel2 }).Where(g => g.agd_nombres.Contains(text)).OrderBy(g => g.agd_nombres), JsonRequestBehavior.AllowGet);
        }

        [Permiso(Permiso = RolesPermisos.OYM_documentos_puedeVerIndice)]
        public ActionResult Index_Read([DataSourceRequest] DataSourceRequest request)
        {
            var car = from c in db.tblDocumento.Include(t => t.AprobadoPor).Include(t => t.ControlCalidad).Include(t => t.ElaboradoPor).Include(t => t.EstadoDocumento).Include(t => t.OrigenDocumento).Include(t => t.RevisadoPor).Include(t => t.tblPlantilla).Include(t => t.tblTipoDocumento).Include(t => t.tblTipoProceso).Include(t => t.tblDirectorio)
                      select c;
            return Json(car.ToDataSourceResult(request, c => new
            {
                c.Id,
                c.dtFechaCreacion_dt,
                c.Titulo,
                c.NombreArchivo,
                c.lOrigenDocumento_id,
                c.lTipoDocumento_id,
                c.lTipoProceso_id,
                c.sCorrelativo,
                c.sCodigo,
                c.lVersion,
                c.dtValidoDesde_dt,
                c.dtUltimaActualizacion_dt,
                c.UbicacionArchivo,
                c.lPlantilla_id,
                c.sComentarios,
                c.lEstado_id,
                c.sISO,
                c.lControlCalidad_id,
                c.lElaboradoPor_id,
                c.lRevisadoPor_id,
                c.lAprobadoPor_id,
                c.lDirectorio_id,
                OrigenDocumento = new
                {
                    c.lOrigenDocumento_id,
                    c.OrigenDocumento.sDescripcion
                },
                tblTipoDocumento = new
                {
                    c.lTipoDocumento_id,
                    c.tblTipoDocumento.sTipoDocumento
                },
                tblTipoProceso = new
                {
                    c.lTipoProceso_id,
                    c.tblTipoProceso.sDescripcion
                },
                tblPlantilla = new
                {
                    c.lPlantilla_id,
                    c.tblPlantilla.Nombre,
                    c.tblPlantilla.Descripcion
                },
                EstadoDocumento = new
                {
                    c.lEstado_id,
                    c.EstadoDocumento.sDescripcion
                },
                //gatbl_Facultades = new
                //{
                //    c.lUniversidad_id,
                //    c.lFacultad_id,
                //    c.gatbl_Facultades.sFacultad_nm,
                //    c.gatbl_Facultades.sMail_desc
                //}
            }));
        }

        private string getPathRoot(int Id)
        {
            var strResultado = string.Empty;

            var dir = from d in db.tblDirectorio
                      where d.lDirectorio_id == Id
                      select new { Id = d.lDirectorio_id, Nombre = d.sNombre, DirectorioPadre = d.lDirectorioPadre_id };

            var item = dir.FirstOrDefault();

            if (item != null)
            {
                strResultado = item.Nombre;

                while (item.DirectorioPadre != null)
                {
                    dir = from d in db.tblDirectorio
                          where d.lDirectorio_id == item.DirectorioPadre
                          select new { Id = d.lDirectorio_id, Nombre = d.sNombre, DirectorioPadre = d.lDirectorioPadre_id };

                    item = dir.FirstOrDefault();

                    strResultado = item.Nombre + "/" + strResultado;
                }
            }


            return strResultado;
        }

        private void CreateDoc(tblDocumento documento)
        {
            try
            {
                string pathFile = string.Format("{0}/{1}", Server.MapPath("~"), documento.UbicacionArchivo);


                var plantilla = db.tblPlantilla.Find(documento.lPlantilla_id);
                var documentoCompleto = db.tblDocumento.Include(t => t.AprobadoPor).Include(t => t.ControlCalidad).Include(t => t.ElaboradoPor).Include(t => t.EstadoDocumento).Include(t => t.OrigenDocumento).Include(t => t.RevisadoPor).Include(t => t.tblPlantilla).Include(t => t.tblTipoDocumento).Include(t => t.tblTipoProceso).FirstOrDefault(t => t.Id == documento.Id);

                string pathFileTemplate = string.Format("{0}/{1}", Server.MapPath("~"), plantilla.sPathFormato);

                Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word.Document doc = app.Documents.Add(pathFileTemplate);


                doc.Bookmarks["CODIGO"].Range.Text = documento.sCodigo;

                //doc.Bookmarks[plantilla.sTitulo1].Range.Text = documento.sCodigo;
                //doc.Bookmarks[plantilla.sTitulo2].Range.Text = Convert.ToString(documento.lVersion);
                //doc.Bookmarks[plantilla.sTitulo3].Range.Text = Convert.ToDateTime(documentoCompleto.dtValidoDesde_dt).Date.ToString("dd/MM/yyyy");
                //doc.Bookmarks[plantilla.sTitulo4].Range.Text = documentoCompleto.tblTipoDocumento.sTipoDocumento;
                //doc.Bookmarks[plantilla.sTitulo5].Range.Text = documentoCompleto.Titulo;
                //doc.Bookmarks[plantilla.sTitulo6].Range.Text = documentoCompleto.ElaboradoPor.DescCargoNivel2;
                //doc.Bookmarks[plantilla.sTitulo7].Range.Text = documentoCompleto.ElaboradoPor.Nombre;
                //doc.Bookmarks[plantilla.sTitulo8].Range.Text = documentoCompleto.RevisadoPor.DescCargoNivel2;
                //doc.Bookmarks[plantilla.sTitulo9].Range.Text = documentoCompleto.RevisadoPor.Nombre;
                //doc.Bookmarks[plantilla.sTitulo10].Range.Text = documentoCompleto.AprobadoPor.DescCargoNivel2;
                //doc.Bookmarks["APROBADOPOR"].Range.Text = documentoCompleto.AprobadoPor.Nombre;

                doc.SaveAs(FileName: pathFile);

                //Process.Start("WINWORD.EXE", pathFile);
            }
            catch (Exception ex)
            {

            }            
        }

        private void CreateDocumentByTemplate(tblDocumento documento)
        {
            string pathFile = string.Format("{0}/{1}", Server.MapPath("~"), documento.UbicacionArchivo);


            var plantilla = db.tblPlantilla.Find(documento.lPlantilla_id);
            var marcadores = db.tblPlantillaMarcador.Include(d => d.tblDocumentoCaracteristica).Where(m => m.lPlantilla_id == documento.lPlantilla_id);
            var documentoCompleto = db.tblDocumento.Include(t => t.AprobadoPor).Include(t => t.ControlCalidad).Include(t => t.ElaboradoPor).Include(t => t.EstadoDocumento).Include(t => t.OrigenDocumento).Include(t => t.RevisadoPor).Include(t => t.tblPlantilla).Include(t => t.tblTipoDocumento).Include(t => t.tblTipoProceso).FirstOrDefault(t => t.Id == documento.Id);

            string pathFileTemplate = string.Format("{0}/{1}", Server.MapPath("~"), plantilla.sPathFormato);

            Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document doc = app.Documents.Add(pathFileTemplate);


            foreach (var item in marcadores.ToList())
            {
                switch (item.tblDocumentoCaracteristica.sNombre)
                {
                    case "Titulo":
                        doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.Titulo;
                        break;
                    case "sCodigo":
                        doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.sCodigo;
                        break;
                    case "lVersion":
                        doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.lVersion.ToString();
                        break;
                    case "dtValidoDesde_dt":
                        doc.Bookmarks[item.sMarcador].Range.Text = Convert.ToDateTime(documentoCompleto.dtValidoDesde_dt).Date.ToString("dd/MM/yyyy");
                        break;
                    case "lOrigenDocumento_id":
                        doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.OrigenDocumento.sDescripcion;
                        break;
                    case "lTipoDocumento_id":
                        doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.tblTipoDocumento.sTipoDocumento;
                        break;
                    case "lTipoProceso_id":
                        doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.tblTipoProceso.sDescripcion;
                        break;
                    case "sCorrelativo":
                        doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.sCorrelativo;
                        break;
                    case "dtUltimaActualizacion_dt":
                        doc.Bookmarks[item.sMarcador].Range.Text = Convert.ToDateTime(documentoCompleto.dtUltimaActualizacion_dt).Date.ToString("dd/MM/yyyy");
                        break;
                    case "sComentarios":
                        doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.sComentarios;
                        break;
                    case "lEstado_id":
                        doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.EstadoDocumento.sDescripcion;
                        break;
                    case "sISO":
                        doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.sISO;
                        break;
                    case "lControlCalidad_id":
                        doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.ControlCalidad.Nombre;
                        break;
                    case "lControlCalidad_cargo":
                        doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.ControlCalidad.DescCargoNivel2;
                        break;
                    case "lElaboradoPor_id":
                        doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.ElaboradoPor.Nombre;
                        break;
                    case "lElaboradoPor_cargo":
                        doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.ElaboradoPor.DescCargoNivel2;
                        break;
                    case "lRevisadoPor_id":
                        doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.RevisadoPor.Nombre;
                        break;
                    case "lRevisadoPor_cargo":
                        doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.RevisadoPor.DescCargoNivel2;
                        break;
                    case "lAprobadoPor_id":
                        doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.AprobadoPor.Nombre;
                        break;
                    case "lAprobadoPor_cargo":
                        doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.AprobadoPor.DescCargoNivel2;
                        break;
                }
            }

            doc.SaveAs(FileName: pathFile);

            //try
            //{
            //    string pathFile = string.Format("{0}/{1}", Server.MapPath("~"), documento.UbicacionArchivo);


            //    var plantilla = db.tblPlantilla.Find(documento.lPlantilla_id);
            //    var marcadores = db.tblPlantillaMarcador.Include(d => d.tblDocumentoCaracteristica).Where(m => m.lPlantilla_id == documento.lPlantilla_id);
            //    var documentoCompleto = db.tblDocumento.Include(t => t.AprobadoPor).Include(t => t.ControlCalidad).Include(t => t.ElaboradoPor).Include(t => t.EstadoDocumento).Include(t => t.OrigenDocumento).Include(t => t.RevisadoPor).Include(t => t.tblPlantilla).Include(t => t.tblTipoDocumento).Include(t => t.tblTipoProceso).FirstOrDefault(t => t.Id == documento.Id);

            //    string pathFileTemplate = string.Format("{0}/{1}", Server.MapPath("~"), plantilla.sPathFormato);

            //    Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
            //    Microsoft.Office.Interop.Word.Document doc = app.Documents.Add(pathFileTemplate);


            //    foreach (var item in marcadores.ToList())
            //    {
            //        switch (item.tblDocumentoCaracteristica.sNombre)
            //        {
            //            case "Titulo":
            //                doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.Titulo;
            //                break;
            //            case "sCodigo":
            //                doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.sCodigo;
            //                break;
            //            case "lVersion":
            //                doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.lVersion.ToString();
            //                break;
            //            case "dtValidoDesde_dt":
            //                doc.Bookmarks[item.sMarcador].Range.Text = Convert.ToDateTime(documentoCompleto.dtValidoDesde_dt).Date.ToString("dd/MM/yyyy");
            //                break;
            //            case "lOrigenDocumento_id":
            //                doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.OrigenDocumento.sDescripcion;
            //                break;
            //            case "lTipoDocumento_id":
            //                doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.tblTipoDocumento.sTipoDocumento;
            //                break;
            //            case "lTipoProceso_id":
            //                doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.tblTipoProceso.sDescripcion;
            //                break;
            //            case "sCorrelativo":
            //                doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.sCorrelativo;
            //                break;
            //            case "dtUltimaActualizacion_dt":
            //                doc.Bookmarks[item.sMarcador].Range.Text = Convert.ToDateTime(documentoCompleto.dtUltimaActualizacion_dt).Date.ToString("dd/MM/yyyy");
            //                break;
            //            case "sComentarios":
            //                doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.sComentarios;
            //                break;
            //            case "lEstado_id":
            //                doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.EstadoDocumento.sDescripcion;
            //                break;
            //            case "sISO":
            //                doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.sISO;
            //                break;
            //            case "lControlCalidad_id":
            //                doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.ControlCalidad.Nombre;
            //                break;
            //            case "lControlCalidad_cargo":
            //                doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.ControlCalidad.DescCargoNivel2;
            //                break;
            //            case "lElaboradoPor_id":
            //                doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.ElaboradoPor.Nombre;
            //                break;
            //            case "lElaboradoPor_cargo":
            //                doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.ElaboradoPor.DescCargoNivel2;
            //                break;
            //            case "lRevisadoPor_id":
            //                doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.RevisadoPor.Nombre;
            //                break;
            //            case "lRevisadoPor_cargo":
            //                doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.RevisadoPor.DescCargoNivel2;
            //                break;
            //            case "lAprobadoPor_id":
            //                doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.AprobadoPor.Nombre;
            //                break;
            //            case "lAprobadoPor_cargo":
            //                doc.Bookmarks[item.sMarcador].Range.Text = documentoCompleto.AprobadoPor.DescCargoNivel2;
            //                break;
            //        }
            //    }

            //    doc.SaveAs(FileName: pathFile);
            //}
            //catch (Exception ex)
            //{

            //}
        }

        private void CreateDocument(tblDocumento documento)
        {
            try
            {
                string pathFile = string.Format("{0}/{1}", Server.MapPath("~"), documento.UbicacionArchivo);


                var plantilla = db.tblPlantilla.Find(documento.lPlantilla_id);
                var marcadores = db.tblPlantillaMarcador.Include(d => d.tblDocumentoCaracteristica).Where(m=>m.lPlantilla_id == documento.lPlantilla_id);
                var documentoCompleto = db.tblDocumento.Include(t => t.AprobadoPor).Include(t => t.ControlCalidad).Include(t => t.ElaboradoPor).Include(t => t.EstadoDocumento).Include(t => t.OrigenDocumento).Include(t => t.RevisadoPor).Include(t => t.tblPlantilla).Include(t => t.tblTipoDocumento).Include(t => t.tblTipoProceso).FirstOrDefault(t => t.Id == documento.Id);

                string pathFileTemplate = string.Format("{0}/{1}", Server.MapPath("~"), plantilla.sPathFormato);


                var doc = DocX.Create(pathFile);
                doc.ApplyTemplate(pathFileTemplate);

                
                foreach(var item in marcadores.ToList())
                {
                    switch (item.tblDocumentoCaracteristica.sNombre)
                    {
                        case "Titulo":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.Titulo);
                            break;
                        case "sCodigo":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.sCodigo);
                            break;
                        case "lVersion":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), Convert.ToString(documentoCompleto.lVersion));
                            break;
                        case "dtValidoDesde_dt":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), Convert.ToDateTime(documentoCompleto.dtValidoDesde_dt).Date.ToString("dd/MM/yyyy"));
                            break;
                        case "lOrigenDocumento_id":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.OrigenDocumento.sDescripcion);
                            break;
                        case "lTipoDocumento_id":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.tblTipoDocumento.sTipoDocumento);
                            break;
                        case "lTipoProceso_id":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.tblTipoProceso.sDescripcion);
                            break;
                        case "sCorrelativo":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.sCorrelativo);
                            break;
                        case "dtUltimaActualizacion_dt":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), Convert.ToDateTime(documentoCompleto.dtUltimaActualizacion_dt).Date.ToString("dd/MM/yyyy"));
                            break;
                        case "sComentarios":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.sComentarios);
                            break;
                        case "lEstado_id":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.EstadoDocumento.sDescripcion);
                            break;                            
                        case "sISO":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.sISO);
                            break;
                        case "lControlCalidad_id":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.ControlCalidad.Nombre);
                            break;
                        case "lControlCalidad_cargo":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.ControlCalidad.CodCargoNivel2);
                            break;
                        case "lElaboradoPor_id":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.ElaboradoPor.Nombre);
                            break;
                        case "lElaboradoPor_cargo":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.ElaboradoPor.DescCargoNivel2);
                            break;
                        case "lRevisadoPor_id":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.RevisadoPor.Nombre);
                            break;
                        case "lRevisadoPor_cargo":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.RevisadoPor.DescCargoNivel2);
                            break;
                        case "lAprobadoPor_id":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.AprobadoPor.Nombre);
                            break;
                        case "lAprobadoPor_cargo":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.AprobadoPor.DescCargoNivel2);
                            break;
                    }
                }
                

                doc.Save();
            }
            catch (Exception ex)
            {

            }
        }

        private void FinalizeDocument(tblDocumento documento)
        {
            try
            {
                string pathFile = string.Format("{0}/{1}", Server.MapPath("~"), documento.UbicacionArchivo);


                var plantilla = db.tblPlantilla.Find(documento.lPlantilla_id);
                var marcadores = db.tblPlantillaMarcador.Include(d => d.tblDocumentoCaracteristica).Where(m => m.lPlantilla_id == documento.lPlantilla_id);
                var documentoCompleto = db.tblDocumento.Include(t => t.AprobadoPor).Include(t => t.ControlCalidad).Include(t => t.ElaboradoPor).Include(t => t.EstadoDocumento).Include(t => t.OrigenDocumento).Include(t => t.RevisadoPor).Include(t => t.tblPlantilla).Include(t => t.tblTipoDocumento).Include(t => t.tblTipoProceso).FirstOrDefault(t => t.Id == documento.Id);

                string pathFileTemplate = string.Format("{0}/{1}", Server.MapPath("~"), plantilla.sPathFormato);


                //var doc = DocX.Create(pathFile);
                //doc.ApplyTemplate(pathFileTemplate);

                var doc = DocX.Load(pathFile);
                doc.ApplyTemplate(pathFileTemplate, true);

                foreach (var item in marcadores.ToList())
                {
                    switch (item.tblDocumentoCaracteristica.sNombre)
                    {
                        case "Titulo":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.Titulo);
                            break;
                        case "sCodigo":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.sCodigo);
                            break;
                        case "lVersion":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), Convert.ToString(documentoCompleto.lVersion));
                            break;
                        case "dtValidoDesde_dt":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), Convert.ToDateTime(documentoCompleto.dtValidoDesde_dt).Date.ToString("dd/MM/yyyy"));
                            break;
                        case "lOrigenDocumento_id":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.OrigenDocumento.sDescripcion);
                            break;
                        case "lTipoDocumento_id":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.tblTipoDocumento.sTipoDocumento);
                            break;
                        case "lTipoProceso_id":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.tblTipoProceso.sDescripcion);
                            break;
                        case "sCorrelativo":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.sCorrelativo);
                            break;
                        case "dtUltimaActualizacion_dt":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), Convert.ToDateTime(documentoCompleto.dtUltimaActualizacion_dt).Date.ToString("dd/MM/yyyy"));
                            break;
                        case "sComentarios":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.sComentarios);
                            break;
                        case "lEstado_id":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.EstadoDocumento.sDescripcion);
                            break;
                        case "sISO":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.sISO);
                            break;
                        case "lControlCalidad_id":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.ControlCalidad.Nombre);
                            break;
                        case "lControlCalidad_cargo":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.ControlCalidad.CodCargoNivel2);
                            break;
                        case "lElaboradoPor_id":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.ElaboradoPor.Nombre);
                            break;
                        case "lElaboradoPor_cargo":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.ElaboradoPor.DescCargoNivel2);
                            break;
                        case "lRevisadoPor_id":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.RevisadoPor.Nombre);
                            break;
                        case "lRevisadoPor_cargo":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.RevisadoPor.DescCargoNivel2);
                            break;
                        case "lAprobadoPor_id":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.AprobadoPor.Nombre);
                            break;
                        case "lAprobadoPor_cargo":
                            doc.ReplaceText(string.Format("[{0}]", item.sMarcador), documentoCompleto.AprobadoPor.DescCargoNivel2);
                            break;
                    }
                }


                doc.Save();
            }
            catch (Exception ex)
            {

            }
        }

        private void CrearDocumentoConPlantilla(tblDocumento documento)
        {
            // Modify to siut your machine:
            string fileName = @"D:\DocumentoPlantillaDemoMarcadores.docx";
            string FilePlantilla = @"D:\PlantillaDocumento.dotx";

            var docplantilla = DocX.Load(FilePlantilla);

            var he = docplantilla.Headers.first;
            he.ReplaceText("", "", true);

            var hd = docplantilla.Headers.odd;

            

            foreach (Bookmark bookmark in docplantilla.Bookmarks)
            {
                string nombre = bookmark.Name;
                string path = nombre + "-";
            }
            //docplantilla.Bookmarks["CODIGO"].SetText("My Codigo");

            docplantilla.SaveAs(fileName);
            
            // Open in Word:
            //Process.Start("WINWORD.EXE", fileName);            
        }        
                
        private void CreateDocument()
        {
            try
            {
                //Create an instance for word app
                Microsoft.Office.Interop.Word.Application winword = new Microsoft.Office.Interop.Word.Application();

                //Set animation status for word application
                winword.ShowAnimation = false;

                //Set status for word application is to be visible or not.
                winword.Visible = false;

                //Create a missing variable for missing value
                object missing = System.Reflection.Missing.Value;

                //Create a new document
                Microsoft.Office.Interop.Word.Document document = winword.Documents.Add(ref missing, ref missing, ref missing, ref missing);

                //Add header into the document
                foreach (Microsoft.Office.Interop.Word.Section section in document.Sections)
                {
                    //Get the header range and add the header details.
                    Microsoft.Office.Interop.Word.Range headerRange = section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    headerRange.Fields.Add(headerRange, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage);
                    headerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    headerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdBlue;
                    headerRange.Font.Size = 10;
                    headerRange.Text = "Header text goes here";
                }

                //Add the footers into the document
                foreach (Microsoft.Office.Interop.Word.Section wordSection in document.Sections)
                {
                    //Get the footer range and add the footer details.
                    Microsoft.Office.Interop.Word.Range footerRange = wordSection.Footers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    footerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdDarkRed;
                    footerRange.Font.Size = 10;
                    footerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    footerRange.Text = "Footer text goes here";
                }

                
                //Save the document
                object filename = @"d:\temp1.docx";
                document.SaveAs2(ref filename);
                document.Close(ref missing, ref missing, ref missing);
                document = null;
                winword.Quit(ref missing, ref missing, ref missing);
                winword = null;
                //MessageBox.Show("Document created successfully !");
            }
            catch (Exception ex)
            {
                
            }
        }

        [Permiso(Permiso = RolesPermisos.OYM_documentos_puedeVerDetalle)]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDocumento document = await db.tblDocumento.Where(p => p.Id == id).Include(t => t.AprobadoPor).Include(t => t.ControlCalidad).Include(t => t.ElaboradoPor).Include(t => t.EstadoDocumento).Include(t => t.OrigenDocumento).Include(t => t.RevisadoPor).Include(t => t.tblPlantilla).Include(t => t.tblTipoDocumento).Include(t => t.tblTipoProceso).SingleAsync();
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        [Permiso(Permiso = RolesPermisos.OYM_documentos_puedeCrearNuevo)]
        public ActionResult Create()
        {
            ViewModels.Documento documento = new ViewModels.Documento();

            documento.tblDocumento = new tblDocumento() { sCorrelativo = "00?", sCodigo = "?????", lVersion = 0, dtFechaCreacion_dt = DateTime.Now, dtUltimaActualizacion_dt = DateTime.Now, dtValidoDesde_dt = DateTime.Now };
            documento.tblDocumentoRelacionados = new List<tblDocumentoRelacionado>();
            documento.DocumentosEliminados = new List<tblDocumentoRelacionado>();

            Session["vDocumento"] = documento;

            var ListDir = from d in db.tblDirectorio
                          orderby d.sCodigo
                          where d.bActivo == true
                          select d;

            ViewBag.lEstado_id = new SelectList(db.EstadoDocumento, "lEstadoDocumento_id", "sDescripcion");
            ViewBag.lOrigenDocumento_id = new SelectList(db.OrigenDocumento, "lOrigenDocumento_id", "sDescripcion");
            ViewBag.lPlantilla_id = new SelectList(db.tblPlantilla, "lPlantilla_id", "Descripcion");
            ViewBag.lDirectorio_id = new SelectList(ListDir, "lDirectorio_id", "NombreDirectorio");
            ViewBag.lProceso_id = new SelectList(db.tblProceso, "lProceso_id", "sNombre");
            ViewBag.lTipoDocumento_id = new SelectList(db.tblTipoDocumentos, "lTipoDocumento_id", "sTipoDocumento");
            ViewBag.lTipoProceso_id = new SelectList(db.tblTipoProcesos, "lTipoProceso_id", "sDescripcion");

            return View(documento.tblDocumento);
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(tblDocumento tblDocumento, HttpPostedFileBase Documentoupload)
        {
            try
            {

                var vDocumento = (ViewModels.Documento)Session["vDocumento"];

                //if (ModelState.IsValid)
                //{

                var documento = new tblDocumento
                {
                    Id = tblDocumento.Id,
                    dtFechaCreacion_dt = DateTime.Now,
                    Titulo = tblDocumento.Titulo,
                    lOrigenDocumento_id = tblDocumento.lOrigenDocumento_id,
                    lTipoDocumento_id = tblDocumento.lTipoDocumento_id,
                    lTipoProceso_id = tblDocumento.lTipoProceso_id,
                    sCorrelativo = tblDocumento.sCorrelativo,
                    sCodigo = tblDocumento.sCodigo,
                    lVersion = tblDocumento.lVersion,
                    dtValidoDesde_dt = tblDocumento.dtValidoDesde_dt,
                    dtUltimaActualizacion_dt = tblDocumento.dtUltimaActualizacion_dt,
                    lPlantilla_id = tblDocumento.lPlantilla_id,
                    sComentarios = tblDocumento.sComentarios,
                    lEstado_id = tblDocumento.lEstado_id,
                    lDirectorio_id = tblDocumento.lDirectorio_id,
                    sISO = tblDocumento.sISO,
                    lControlCalidad_id = tblDocumento.lControlCalidad_id,
                    lElaboradoPor_id = tblDocumento.lElaboradoPor_id,
                    lRevisadoPor_id = tblDocumento.lRevisadoPor_id,
                    lAprobadoPor_id = tblDocumento.lAprobadoPor_id,
                    lUsuario_id = FrontUser.Get().iUsuario_id
                };

                //if (Documentoupload != null && Documentoupload.ContentLength > 0)
                //{
                //    string fileName = System.IO.Path.GetFileName(Documentoupload.FileName);
                //    string pathFile = string.Format("{0}/Adjunto/{1}", Server.MapPath("~"), fileName);

                //    documento.NombreArchivo = fileName;
                //    documento.UbicacionArchivo = string.Format("Adjunto/{0}", fileName);
                //    Documentoupload.SaveAs(pathFile);
                //}                

                var CantidadDoc = db.tblDocumento.Where(d => d.lTipoProceso_id == documento.lTipoProceso_id).Count();
                //string strCorrelativo = CantidadDoc.ToString().PadLeft(3, '0');
                string strCorrelativo = (CantidadDoc + 100).ToString();

                var tipodoc = db.tblTipoDocumentos.Find(tblDocumento.lTipoDocumento_id);
                var tipopro = db.tblTipoProcesos.Find(tblDocumento.lTipoProceso_id);


                documento.sCorrelativo = strCorrelativo;
                documento.sCodigo = string.Format("{0}-{1}-{2}", tipodoc.sSigla, tipopro.sSigla, strCorrelativo);


                string pathRoot = getPathRoot(documento.lDirectorio_id);
                string fileName = string.Format("{0}.docx", documento.sCodigo);
                string pathFile = string.Format("{0}/Areas/OYM/Adjunto/{1}/{2}", Server.MapPath("~"), pathRoot, fileName);
                //string pathFile = string.Format(@"{0}\{1}\{2}", FilePahtRoot(), pathRoot, fileName);

                documento.NombreArchivo = fileName;
                documento.UbicacionArchivo = string.Format("Areas/OYM/Adjunto/{0}/{1}", pathRoot, fileName);
                //documento.UbicacionArchivo = string.Format(@"{0}\{1}", pathRoot, fileName);


                db.tblDocumento.Add(documento);
                await db.SaveChangesAsync();

                //CreateDoc(documento);
                CreateDocument(documento);

                //CreateDocumentByTemplate(documento);


                var DocumentID = db.tblDocumento.Select(c => c.Id).Max();
                foreach (var item in vDocumento.tblDocumentoRelacionados)
                {
                    var detalle = new tblDocumentoRelacionado
                    {
                        Id = item.Id,
                        NombreArchivo = item.NombreArchivo,
                        Ubicacion = item.Ubicacion,
                        Relacion = item.Relacion,
                        DocumentoID = item.DocumentoID
                    };

                    if (detalle.Id < 0)
                    {
                        detalle.DocumentoID = DocumentID;
                        db.tblDocumentoRelacionado.Add(detalle);
                    }
                    else
                    {
                        db.Entry(detalle).State = EntityState.Modified;
                    }

                }

                await db.SaveChangesAsync();

                foreach (var item in vDocumento.DocumentosEliminados)
                {
                    tblDocumentoRelacionado tblDocumentoRelacionado = db.tblDocumentoRelacionado.Find(item.Id);
                    db.tblDocumentoRelacionado.Remove(tblDocumentoRelacionado);
                }

                db.SaveChanges();

                return RedirectToAction("Index");
                //}
            }
            catch (Exception ex)
            {
                var ListDir = from d in db.tblDirectorio
                              orderby d.sCodigo
                              where d.bActivo == true
                              select d;

                ViewBag.lEstado_id = new SelectList(db.EstadoDocumento, "lEstadoDocumento_id", "sDescripcion", tblDocumento.lEstado_id);
                ViewBag.lOrigenDocumento_id = new SelectList(db.OrigenDocumento, "lOrigenDocumento_id", "sDescripcion", tblDocumento.lOrigenDocumento_id);
                ViewBag.lPlantilla_id = new SelectList(db.tblPlantilla, "lPlantilla_id", "Descripcion", tblDocumento.lPlantilla_id);
                ViewBag.lDirectorio_id = new SelectList(ListDir, "lDirectorio_id", "NombreDirectorio", tblDocumento.lDirectorio_id);
                //ViewBag.lProceso_id = new SelectList(db.tblProceso, "lProceso_id", "sNombre", tblDocumento.lProceso_id);
                ViewBag.lTipoDocumento_id = new SelectList(db.tblTipoDocumentos, "lTipoDocumento_id", "sTipoDocumento", tblDocumento.lTipoDocumento_id);
                ViewBag.lTipoProceso_id = new SelectList(db.tblTipoProcesos, "lTipoProceso_id", "sDescripcion", tblDocumento.lTipoProceso_id);

                return View(tblDocumento);
            }
        }

        [Permiso(Permiso = RolesPermisos.OYM_documentos_puedeEditar)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }            

            var document = await db.tblDocumento
                        .Include(p => p.tblDocumentoRelacionados)
                        .Where(p => p.Id == id)
                        .SingleAsync();


            if (document == null)
            {
                return HttpNotFound();
            }

            ViewModels.Documento documento = new ViewModels.Documento();

            documento.tblDocumento = document;
            documento.tblDocumentoRelacionados = db.tblDocumentoRelacionado.Where(t=>t.DocumentoID== id).ToList();
            documento.DocumentosEliminados = new List<tblDocumentoRelacionado>();

            Session["vDocumento"] = documento;


            var ListDir = from d in db.tblDirectorio
                          orderby d.sCodigo
                          where d.bActivo == true
                          select d;

            ViewBag.lEstado_id = new SelectList(db.EstadoDocumento, "lEstadoDocumento_id", "sDescripcion", document.lEstado_id);
            ViewBag.lOrigenDocumento_id = new SelectList(db.OrigenDocumento, "lOrigenDocumento_id", "sDescripcion", document.lOrigenDocumento_id);
            ViewBag.lPlantilla_id = new SelectList(db.tblPlantilla, "lPlantilla_id", "Descripcion", document.lPlantilla_id);
            ViewBag.lTipoDocumento_id = new SelectList(db.tblTipoDocumentos, "lTipoDocumento_id", "sTipoDocumento", document.lTipoDocumento_id);
            //ViewBag.lProceso_id = new SelectList(db.tblProceso, "lProceso_id", "sNombre", document.lProceso_id);
            ViewBag.lDirectorio_id = new SelectList(ListDir, "lDirectorio_id", "NombreDirectorio", document.lDirectorio_id);
            ViewBag.lTipoProceso_id = new SelectList(db.tblTipoProcesos, "lTipoProceso_id", "sDescripcion", document.lTipoProceso_id);
            return View(document);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(tblDocumento tblDocumento, HttpPostedFileBase Documentoupload)
        {
            try
            {
                var vDocumento = (ViewModels.Documento)Session["vDocumento"];

                var documento = db.tblDocumento.FirstOrDefault(p => p.Id == tblDocumento.Id);
                
                documento.lOrigenDocumento_id = tblDocumento.lOrigenDocumento_id;
                documento.lTipoDocumento_id = tblDocumento.lTipoDocumento_id;
                documento.Titulo = tblDocumento.Titulo;
                documento.lTipoProceso_id = tblDocumento.lTipoProceso_id;
                documento.sCorrelativo = tblDocumento.sCorrelativo;
                documento.sCodigo = tblDocumento.sCodigo;
                documento.lVersion = tblDocumento.lVersion;
                documento.dtValidoDesde_dt = tblDocumento.dtValidoDesde_dt;
                documento.dtUltimaActualizacion_dt = tblDocumento.dtUltimaActualizacion_dt;
                documento.lPlantilla_id = tblDocumento.lPlantilla_id;
                documento.sComentarios = tblDocumento.sComentarios;
                documento.lDirectorio_id = tblDocumento.lDirectorio_id;
                documento.lEstado_id = tblDocumento.lEstado_id;
                documento.sISO = tblDocumento.sISO;
                documento.lControlCalidad_id = tblDocumento.lControlCalidad_id;
                documento.lElaboradoPor_id = tblDocumento.lElaboradoPor_id;
                documento.lRevisadoPor_id = tblDocumento.lRevisadoPor_id;
                documento.lAprobadoPor_id = tblDocumento.lAprobadoPor_id;

                if (vDocumento.tblDocumento.lEstado_id != tblDocumento.lEstado_id && tblDocumento.lEstado_id == 3)
                {
                    documento.lVersion = tblDocumento.lVersion + 1;

                    //FinalizeDocument(documento);
                }

                    //if (ModelState.IsValid)
                    //{
                if (Documentoupload != null && Documentoupload.ContentLength > 0)
                {
                    //string fileName = System.IO.Path.GetFileName(Documentoupload.FileName);
                    //string pathFile = string.Format("{0}/Adjunto/{1}", Server.MapPath("~"), fileName);
                    string strDate = DateTime.Now.ToString("ddMMyyyyHHmmss");
                    string fileName = string.Format("{0}_{1}", strDate, documento.NombreArchivo);
                    
                    string pathFile = string.Format("{0}/Adjunto/{1}", Server.MapPath("~"), fileName);

                    documento.NombreArchivo = fileName;
                    documento.UbicacionArchivo = string.Format("Adjunto/{0}", documento.NombreArchivo);
                    Documentoupload.SaveAs(pathFile);
                }

                db.Entry(documento).State = EntityState.Modified;
                await db.SaveChangesAsync();


                foreach (var item in vDocumento.tblDocumentoRelacionados)
                {
                    var detalle = new tblDocumentoRelacionado
                    {
                        Id = item.Id,
                        NombreArchivo = item.NombreArchivo,
                        Ubicacion = item.Ubicacion,
                        Relacion = item.Relacion,
                        DocumentoID = item.DocumentoID
                    };

                    if (detalle.Id < 0)
                    {
                        detalle.DocumentoID = documento.Id;
                        db.tblDocumentoRelacionado.Add(detalle);
                    }
                    else
                    {
                        db.Entry(detalle).State = EntityState.Modified;                        
                    }

                }

                await db.SaveChangesAsync();


                foreach (var item in vDocumento.DocumentosEliminados)
                {
                    tblDocumentoRelacionado tblDocumentoRelacionado = db.tblDocumentoRelacionado.Find(item.Id);
                    db.tblDocumentoRelacionado.Remove(tblDocumentoRelacionado);                    
                }

                db.SaveChanges();

                return RedirectToAction("Index");
                //}
            }
            catch(Exception ex)
            {
                var ListDir = from d in db.tblDirectorio
                              orderby d.sCodigo
                              where d.bActivo == true
                              select d;

                ViewBag.lEstado_id = new SelectList(db.EstadoDocumento, "lEstadoDocumento_id", "sDescripcion", tblDocumento.lEstado_id);
                ViewBag.lOrigenDocumento_id = new SelectList(db.OrigenDocumento, "lOrigenDocumento_id", "sDescripcion", tblDocumento.lOrigenDocumento_id);
                ViewBag.lPlantilla_id = new SelectList(db.tblPlantilla, "lPlantilla_id", "Descripcion", tblDocumento.lPlantilla_id);
                ViewBag.lDirectorio_id = new SelectList(ListDir, "lDirectorio_id", "sNombre", tblDocumento.lDirectorio_id);
                //ViewBag.lProceso_id = new SelectList(db.tblProceso, "lProceso_id", "sNombre", tblDocumento.lProceso_id);
                ViewBag.lTipoDocumento_id = new SelectList(db.tblTipoDocumentos, "lTipoDocumento_id", "sTipoDocumento", tblDocumento.lTipoDocumento_id);
                ViewBag.lTipoProceso_id = new SelectList(db.tblTipoProcesos, "lTipoProceso_id", "sDescripcion", tblDocumento.lTipoProceso_id);

                return View(tblDocumento);
            }                        
        }

        [Permiso(Permiso = RolesPermisos.OYM_documentos_puedeEliminar)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDocumento document = await db.tblDocumento.FindAsync(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            tblDocumento document = await db.tblDocumento.FindAsync(id);
            db.tblDocumento.Remove(document);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
