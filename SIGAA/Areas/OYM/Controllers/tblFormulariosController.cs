using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIGAA.Areas.OYM.Models;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using SIGAA.Etiquetas;
using SIGAA.Commons;

namespace SIGAA.Areas.OYM.Controllers
{
    [Autenticado]
    public class tblFormulariosController : Controller
    {
        private DataDb db = new DataDb();

        [Permiso(Permiso = RolesPermisos.OYM_formularios_puedeVerIndice)]
        public async Task<ActionResult> Index()
        {
            return View(await db.tblFormulario.Include(t => t.AprobadoPor).Include(t => t.ControlCalidad).Include(t => t.ElaboradoPor).Include(t => t.EstadoDocumento).Include(t => t.OrigenDocumento).Include(t => t.RevisadoPor).Include(t => t.tblDocumento).Include(t => t.tblTipoProceso).Include(t => t.tblDirectorio).ToListAsync());
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

        [Permiso(Permiso = RolesPermisos.OYM_formularios_puedeVerIndice)]
        public ActionResult Index_Read([DataSourceRequest] DataSourceRequest request)
        {
            var car = from c in db.tblFormulario.Include(t => t.AprobadoPor).Include(t => t.ControlCalidad).Include(t => t.ElaboradoPor).Include(t => t.EstadoDocumento).Include(t => t.OrigenDocumento).Include(t => t.RevisadoPor).Include(t => t.tblTipoProceso).Include(t => t.tblDirectorio)
                      select c;
            return Json(car.ToDataSourceResult(request, c => new
            {
                c.Id,
                c.dtFechaCreacion_dt,
                c.Titulo,
                c.NombreArchivo,
                c.lOrigenDocumento_id,
                //c.lTipoDocumento_id,
                c.lTipoProceso_id,
                c.sCorrelativo,
                c.sCodigo,
                c.lVersion,
                c.dtValidoDesde_dt,
                c.dtUltimaActualizacion_dt,
                c.UbicacionArchivo,
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
                //tblTipoDocumento = new
                //{
                //    c.lTipoDocumento_id,
                //    c.tblTipoDocumento.sTipoDocumento
                //},
                tblTipoProceso = new
                {
                    c.lTipoProceso_id,
                    c.tblTipoProceso.sDescripcion
                },
                //tblPlantilla = new
                //{
                //    c.lPlantilla_id,
                //    c.tblPlantilla.Nombre,
                //    c.tblPlantilla.Descripcion
                //},
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

        public JsonResult DocumentoList(int id)
        {
            var documentos = from s in db.tblDocumento
                             where s.lTipoProceso_id == id
                             select s;

            return Json(new SelectList(documentos.ToArray(), "Id", "sCodigo"), JsonRequestBehavior.AllowGet);
        }

        [Permiso(Permiso = RolesPermisos.OYM_formularios_puedeVerDetalle)]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblFormulario document = await db.tblFormulario.Where(p => p.Id == id).Include(t => t.AprobadoPor).Include(t => t.ControlCalidad).Include(t => t.ElaboradoPor).Include(t => t.EstadoDocumento).Include(t => t.OrigenDocumento).Include(t => t.RevisadoPor).Include(t => t.tblDocumento).Include(t => t.tblTipoProceso).SingleAsync();
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        [Permiso(Permiso = RolesPermisos.OYM_formularios_puedeCrearNuevo)]
        public ActionResult Create()
        {
            ViewModels.Formulario formulario = new ViewModels.Formulario();

            formulario.tblFormulario = new tblFormulario();
            formulario.tblFormularioRelacionados = new List<tblFormularioRelacionado>();
            formulario.FormulariosEliminados = new List<tblFormularioRelacionado>();

            Session["vFormulario"] = formulario;

            var ListDir = from d in db.tblDirectorio
                          orderby d.sCodigo
                          where d.bActivo == true
                          select d;

            ViewBag.lEstado_id = new SelectList(db.EstadoDocumento, "lEstadoDocumento_id", "sDescripcion");
            ViewBag.lOrigenDocumento_id = new SelectList(db.OrigenDocumento, "lOrigenDocumento_id", "sDescripcion");
            ViewBag.lPlantilla_id = new SelectList(db.tblPlantilla, "lPlantilla_id", "Descripcion");
            ViewBag.TipoProcesoList = db.tblTipoProcesos;
            ViewBag.lDirectorio_id = new SelectList(ListDir, "lDirectorio_id", "NombreDirectorio");
            ViewBag.lTipoDocumento_id = new SelectList(db.tblTipoDocumentos, "lTipoDocumento_id", "sTipoDocumento");
            ViewBag.lDocumento_id = new SelectList(db.tblDocumento, "Id", "sCodigo");
            ViewBag.lTipoProceso_id = new SelectList(db.tblTipoProcesos, "lTipoProceso_id", "sDescripcion");

            return View(new tblFormulario() { sCorrelativo = "00?", sCodigo = "?????", lVersion = 0, dtUltimaActualizacion_dt = DateTime.Now, dtFechaCreacion_dt = DateTime.Now, dtValidoDesde_dt = DateTime.Now });
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(tblFormulario tblFormulario, HttpPostedFileBase Documentoupload)
        {
            try
            {
                var vFormulario = (ViewModels.Formulario)Session["vFormulario"];
                //if (ModelState.IsValid)
                //{

                var documento = new tblFormulario
                {
                    Id = tblFormulario.Id,
                    Titulo = tblFormulario.Titulo,
                    dtFechaCreacion_dt = DateTime.Now,
                    lOrigenDocumento_id = tblFormulario.lOrigenDocumento_id,
                    lDocumento_id = tblFormulario.lDocumento_id,
                    lTipoProceso_id = tblFormulario.lTipoProceso_id,
                    sCorrelativo = tblFormulario.sCorrelativo,
                    sCodigo = tblFormulario.sCodigo,
                    lVersion = tblFormulario.lVersion,
                    dtValidoDesde_dt = tblFormulario.dtValidoDesde_dt,
                    dtUltimaActualizacion_dt = tblFormulario.dtUltimaActualizacion_dt,
                    //lPlantilla_id = tblFormulario.lPlantilla_id,
                    lDirectorio_id = tblFormulario.lDirectorio_id,
                    sComentarios = tblFormulario.sComentarios,
                    lEstado_id = tblFormulario.lEstado_id,
                    sISO = tblFormulario.sISO,
                    lControlCalidad_id = tblFormulario.lControlCalidad_id,
                    lElaboradoPor_id = tblFormulario.lElaboradoPor_id,
                    lRevisadoPor_id = tblFormulario.lRevisadoPor_id,
                    lAprobadoPor_id = tblFormulario.lAprobadoPor_id,
                    lUsuario_id = FrontUser.Get().iUsuario_id
                };                

                var CantidadDoc = db.tblFormulario.Where(t=>t.lDocumento_id == tblFormulario.lDocumento_id).Count() + 1;
                //string strCorrelativo = CantidadDoc.ToString().PadLeft(3, '0');

                //var tipodoc = db.tblTipoDocumentos.Find(tblFormulario.lTipoDocumento_id);
                //var tipopro = db.tblTipoProcesos.Find(tblFormulario.lTipoProceso_id);
                var doc = db.tblDocumento.Find(tblFormulario.lDocumento_id);


                documento.sCorrelativo = CantidadDoc.ToString();
                documento.sCodigo = string.Format("{0}-{1}", doc.sCodigo, CantidadDoc.ToString());

                if (Documentoupload != null && Documentoupload.ContentLength > 0)
                {
                    //string fileName = System.IO.Path.GetFileName(Documentoupload.FileName);
                    //string pathFile = string.Format("{0}/Adjunto/{1}", Server.MapPath("~"), fileName);

                    //documento.NombreArchivo = fileName;
                    //documento.UbicacionArchivo = string.Format("Adjunto/{0}", fileName);

                    string strDate = DateTime.Now.ToString("ddMMyyyyHHmmss");
                    string pathRoot = getPathRoot(documento.lDirectorio_id);
                    string fileName = string.Format("{0}_{1}.{2}", strDate, documento.sCodigo, System.IO.Path.GetExtension(Documentoupload.FileName));
                    string pathFile = string.Format("{0}/Adjunto/{1}/{2}", Server.MapPath("~"), pathRoot, fileName);

                    documento.NombreArchivo = fileName;
                    documento.UbicacionArchivo = string.Format("Adjunto/{0}/{1}", pathRoot, fileName);

                    Documentoupload.SaveAs(pathFile);
                }

                db.tblFormulario.Add(documento);
                await db.SaveChangesAsync();


                var FormID = db.tblFormulario.Select(c => c.Id).Max();
                foreach (var item in vFormulario.tblFormularioRelacionados)
                {
                    var detalle = new tblFormularioRelacionado
                    {
                        Id = item.Id,
                        NombreArchivo = item.NombreArchivo,
                        Ubicacion = item.Ubicacion,
                        Relacion = item.Relacion,
                        FormularioID = item.FormularioID
                    };

                    if (detalle.Id < 0)
                    {
                        detalle.FormularioID = FormID;
                        db.tblFormularioRelacionado.Add(detalle);
                    }
                    else
                    {
                        db.Entry(detalle).State = EntityState.Modified;
                    }

                }

                await db.SaveChangesAsync();

                foreach (var item in vFormulario.FormulariosEliminados)
                {
                    tblFormularioRelacionado tblFormularioRelacionado = db.tblFormularioRelacionado.Find(item.Id);
                    db.tblFormularioRelacionado.Remove(tblFormularioRelacionado);
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

                ViewBag.lEstado_id = new SelectList(db.EstadoDocumento, "lEstadoDocumento_id", "sDescripcion", tblFormulario.lEstado_id);
                ViewBag.lOrigenDocumento_id = new SelectList(db.OrigenDocumento, "lOrigenDocumento_id", "sDescripcion", tblFormulario.lOrigenDocumento_id);
                //ViewBag.lPlantilla_id = new SelectList(db.tblPlantilla, "lPlantilla_id", "Descripcion", tblFormulario.lPlantilla_id);
                //ViewBag.lTipoDocumento_id = new SelectList(db.tblTipoDocumentos, "lTipoDocumento_id", "sTipoDocumento", tblFormulario.lTipoDocumento_id);
                ViewBag.TipoProcesoList = db.tblTipoProcesos;
                ViewBag.lDirectorio_id = new SelectList(ListDir, "lDirectorio_id", "NombreDirectorio", tblFormulario.lDirectorio_id);
                ViewBag.lTipoProceso_id = new SelectList(db.tblTipoProcesos, "lTipoProceso_id", "sDescripcion", tblFormulario.lTipoProceso_id);

                return View(tblFormulario);
            }
        }

        [Permiso(Permiso = RolesPermisos.OYM_formularios_puedeEditar)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var document = await db.tblFormulario
                        .Include(p => p.tblFormularioRelacionados)
                        .Where(p => p.Id == id)
                        .SingleAsync();


            if (document == null)
            {
                return HttpNotFound();
            }

            ViewModels.Formulario formulario = new ViewModels.Formulario();

            formulario.tblFormulario = document;
            formulario.tblFormularioRelacionados = db.tblFormularioRelacionado.Where(t => t.FormularioID == id).ToList();
            formulario.FormulariosEliminados = new List<tblFormularioRelacionado>();

            Session["vFormulario"] = formulario;

            var ListDir = from d in db.tblDirectorio
                          orderby d.sCodigo
                          where d.bActivo == true
                          select d;

            ViewBag.lEstado_id = new SelectList(db.EstadoDocumento, "lEstadoDocumento_id", "sDescripcion", document.lEstado_id);
            ViewBag.lOrigenDocumento_id = new SelectList(db.OrigenDocumento, "lOrigenDocumento_id", "sDescripcion", document.lOrigenDocumento_id);
            //ViewBag.lPlantilla_id = new SelectList(db.tblPlantilla, "lPlantilla_id", "Descripcion", document.lPlantilla_id);
            ViewBag.lDocumento_id = new SelectList(db.tblDocumento, "Id", "sCodigo", document.lDocumento_id);
            ViewBag.lDirectorio_id = new SelectList(ListDir, "lDirectorio_id", "NombreDirectorio", document.lDirectorio_id);
            ViewBag.lTipoProceso_id = new SelectList(db.tblTipoProcesos, "lTipoProceso_id", "sDescripcion", document.lTipoProceso_id);
            return View(document);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(tblFormulario tblFormulario, HttpPostedFileBase Documentoupload)
        {
            try
            {
                var vFormulario = (ViewModels.Formulario)Session["vFormulario"];

                var documento = db.tblFormulario.FirstOrDefault(p => p.Id == tblFormulario.Id);

                documento.lOrigenDocumento_id = tblFormulario.lOrigenDocumento_id;
                documento.lDocumento_id = tblFormulario.lDocumento_id;
                documento.Titulo = tblFormulario.Titulo;
                documento.lTipoProceso_id = tblFormulario.lTipoProceso_id;
                documento.sCorrelativo = tblFormulario.sCorrelativo;
                documento.sCodigo = tblFormulario.sCodigo;
                documento.lVersion = tblFormulario.lVersion;
                documento.dtValidoDesde_dt = tblFormulario.dtValidoDesde_dt;
                documento.dtUltimaActualizacion_dt = tblFormulario.dtUltimaActualizacion_dt;
                documento.lDirectorio_id = tblFormulario.lDirectorio_id;
                //documento.lPlantilla_id = tblFormulario.lPlantilla_id;
                documento.sComentarios = tblFormulario.sComentarios;
                documento.lEstado_id = tblFormulario.lEstado_id;
                documento.sISO = tblFormulario.sISO;
                documento.lControlCalidad_id = tblFormulario.lControlCalidad_id;
                documento.lElaboradoPor_id = tblFormulario.lElaboradoPor_id;
                documento.lRevisadoPor_id = tblFormulario.lRevisadoPor_id;
                documento.lAprobadoPor_id = tblFormulario.lAprobadoPor_id;

                if (vFormulario.tblFormulario.lEstado_id != tblFormulario.lEstado_id && tblFormulario.lEstado_id == 3)
                {
                    documento.lVersion = tblFormulario.lVersion + 1;

                    //FinalizeDocument(documento);
                }

                //if (ModelState.IsValid)
                //{
                if (Documentoupload != null && Documentoupload.ContentLength > 0)
                {
                    //string fileName = System.IO.Path.GetFileName(Documentoupload.FileName);
                    //string pathFile = string.Format("{0}/Adjunto/{1}", Server.MapPath("~"), fileName);

                    //documento.NombreArchivo = fileName;
                    //documento.UbicacionArchivo = string.Format("Adjunto/{0}", fileName);

                    string strDate = DateTime.Now.ToString("ddMMyyyyHHmmss");
                    string pathRoot = getPathRoot(documento.lDirectorio_id);
                    string fileName = string.Format("{0}_{1}.{2}", strDate, documento.sCodigo, System.IO.Path.GetExtension(Documentoupload.FileName));
                    string pathFile = string.Format("{0}/Adjunto/{1}/{2}", Server.MapPath("~"), pathRoot, fileName);

                    documento.NombreArchivo = fileName;
                    documento.UbicacionArchivo = string.Format("Adjunto/{0}/{1}", pathRoot, fileName);

                    Documentoupload.SaveAs(pathFile);
                }

                db.Entry(documento).State = EntityState.Modified;
                await db.SaveChangesAsync();


                foreach (var item in vFormulario.tblFormularioRelacionados)
                {
                    var detalle = new tblFormularioRelacionado
                    {
                        Id = item.Id,
                        NombreArchivo = item.NombreArchivo,
                        Ubicacion = item.Ubicacion,
                        Relacion = item.Relacion,
                        FormularioID = item.FormularioID
                    };

                    if (detalle.Id < 0)
                    {
                        detalle.FormularioID = documento.Id;
                        db.tblFormularioRelacionado.Add(detalle);
                    }
                    else
                    {
                        db.Entry(detalle).State = EntityState.Modified;
                    }

                }

                await db.SaveChangesAsync();


                foreach (var item in vFormulario.FormulariosEliminados)
                {
                    tblFormularioRelacionado tblFormularioRelacionado = db.tblFormularioRelacionado.Find(item.Id);
                    db.tblFormularioRelacionado.Remove(tblFormularioRelacionado);
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

                ViewBag.lEstado_id = new SelectList(db.EstadoDocumento, "lEstadoDocumento_id", "sDescripcion", tblFormulario.lEstado_id);
                ViewBag.lOrigenDocumento_id = new SelectList(db.OrigenDocumento, "lOrigenDocumento_id", "sDescripcion", tblFormulario.lOrigenDocumento_id);
                //ViewBag.lPlantilla_id = new SelectList(db.tblPlantilla, "lPlantilla_id", "Descripcion", tblFormulario.lPlantilla_id);
                ViewBag.lDocumento_id = new SelectList(db.tblDocumento, "Id", "sCodigo", tblFormulario.lDocumento_id);
                ViewBag.lDirectorio_id = new SelectList(ListDir, "lDirectorio_id", "NombreDirectorio", tblFormulario.lDirectorio_id);
                ViewBag.lTipoProceso_id = new SelectList(db.tblTipoProcesos, "lTipoProceso_id", "sDescripcion", tblFormulario.lTipoProceso_id);

                return View(tblFormulario);
            }
        }

        [Permiso(Permiso = RolesPermisos.OYM_formularios_puedeEliminar)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblFormulario document = await db.tblFormulario.FindAsync(id);
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
            tblFormulario document = await db.tblFormulario.FindAsync(id);
            db.tblFormulario.Remove(document);
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
