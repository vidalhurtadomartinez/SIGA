using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIGAA.Areas.OYM.Models;
using SIGAA.Etiquetas;

namespace SIGAA.Areas.OYM.Controllers
{
    [Autenticado]
    public class tblPlantillaMarcadoresController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblPlantillaMarcadores
        public ActionResult Index(int id)
        {
            ViewBag.PlantillaID = id;
            //var tblPlantillaMarcador = db.tblPlantillaMarcador.Include(t => t.agenda).Include(t => t.RolParticipante).Include(t => t.tblCategoria).Include(t => t.tblProceso);

            var tblPlantillaMarcador = ((ViewModels.Plantilla)Session["vPlantilla"]).tblPlantillaMarcadores;

            return PartialView("_Index", tblPlantillaMarcador.ToList());
        }

        [ChildActionOnly]
        public ActionResult List(int id)
        {
            ViewBag.PlantillaID = id;
            var tblPlantillaMarcador = db.tblPlantillaMarcador.Where(a => a.lPlantilla_id == id);

            return PartialView("_List", tblPlantillaMarcador.ToList());
        }

        public JsonResult ObtenerParticipantes(string text)
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



            return Json((from ag in db.agenda
                         select new { agd_nombres = ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + ", " + ag.agd_nombres.Trim(), agd_codigo = ag.agd_codigo.Trim(), agd_docnro = ag.agd_docnro.Trim() }).Where(g => g.agd_nombres.Contains(text)).OrderBy(g => g.agd_nombres), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerParticipantesPersonal(string text)
        {
            return Json((from p in db.vVistaPersonal
                         select new { Id = p.idcont, Nombre = p.Nombre }).Where(g => g.Nombre.Contains(text)).OrderBy(g => g.Nombre), JsonRequestBehavior.AllowGet);
        }

        // GET: tblPlantillaMarcadors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPlantillaMarcador tblPlantillaMarcador = db.tblPlantillaMarcador.Find(id);
            if (tblPlantillaMarcador == null)
            {
                return HttpNotFound();
            }
            return View(tblPlantillaMarcador);
        }

        // GET: tblPlantillaMarcadors/Create
        public ActionResult Create(int PlantillaID)
        {
            tblPlantillaMarcador tblPlantillaMarcador = new tblPlantillaMarcador();
            tblPlantillaMarcador.lPlantilla_id = PlantillaID;

            ViewBag.lDocumentoCaracteristica_id = new SelectList(db.tblDocumentoCaracteristica, "lDocumentoCaracteristica_id", "sTitulo");
            
            return PartialView("_Create", tblPlantillaMarcador);
        }

        // POST: tblPlantillaMarcadors/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblPlantillaMarcador tblPlantillaMarcador)
        {
            if (ModelState.IsValid)
            {
                var doccaracteristica = db.tblDocumentoCaracteristica.Find(tblPlantillaMarcador.lDocumentoCaracteristica_id);

                ViewModels.Plantilla plantilla = (ViewModels.Plantilla)Session["vPlantilla"];

                int DetalleId = plantilla.tblPlantillaMarcadores.Count() + 1;
                tblPlantillaMarcador.lPlantillaMarcador_id = -DetalleId;
                tblPlantillaMarcador.tblDocumentoCaracteristica = doccaracteristica;

                plantilla.tblPlantillaMarcadores.Add(tblPlantillaMarcador);

                string url = Url.Action("Index", "tblPlantillaMarcadores", new { id = tblPlantillaMarcador.lPlantilla_id });
                return Json(new { success = true, url = url });

            }
            
            ViewBag.lDocumentoCaracteristica_id = new SelectList(db.tblDocumentoCaracteristica, "lDocumentoCaracteristica_id", "sTitulo", tblPlantillaMarcador.lDocumentoCaracteristica_id);

            return PartialView("_Create", tblPlantillaMarcador);
        }

        // GET: tblPlantillaMarcadors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            tblPlantillaMarcador tblPlantillaMarcador = ((ViewModels.Plantilla)Session["vPlantilla"]).tblPlantillaMarcadores.FirstOrDefault(t => t.lPlantillaMarcador_id == id);
            if (tblPlantillaMarcador == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.lDocumentoCaracteristica_id = new SelectList(db.tblDocumentoCaracteristica, "lDocumentoCaracteristica_id", "sTitulo", tblPlantillaMarcador.lDocumentoCaracteristica_id);

            return PartialView("_Edit", tblPlantillaMarcador);
        }

        // POST: tblPlantillaMarcadors/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblPlantillaMarcador tblPlantillaMarcador)
        {
            if (ModelState.IsValid)
            {
                tblPlantillaMarcador plantillaactual = ((ViewModels.Plantilla)Session["vPlantilla"]).tblPlantillaMarcadores.FirstOrDefault(t => t.lPlantillaMarcador_id == tblPlantillaMarcador.lPlantillaMarcador_id);

                var doccaracteristica = db.tblDocumentoCaracteristica.Find(tblPlantillaMarcador.lDocumentoCaracteristica_id);

                if (plantillaactual != null)
                {
                    plantillaactual.lDocumentoCaracteristica_id = tblPlantillaMarcador.lDocumentoCaracteristica_id;
                    plantillaactual.sMarcador = tblPlantillaMarcador.sMarcador;
                    plantillaactual.lPlantilla_id = tblPlantillaMarcador.lPlantilla_id;
                    
                    plantillaactual.tblDocumentoCaracteristica = doccaracteristica;
                }

                //db.Entry(tblPlantillaMarcador).State = EntityState.Modified;
                //db.SaveChanges();

                string url = Url.Action("Index", "tblPlantillaMarcadores", new { id = tblPlantillaMarcador.lPlantilla_id });
                return Json(new { success = true, url = url });
            }

            ViewBag.lDocumentoCaracteristica_id = new SelectList(db.tblDocumentoCaracteristica, "lDocumentoCaracteristica_id", "sTitulo", tblPlantillaMarcador.lDocumentoCaracteristica_id);

            return PartialView("_Edit", tblPlantillaMarcador);
        }

        // GET: tblPlantillaMarcadors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            tblPlantillaMarcador tblPlantillaMarcador = ((ViewModels.Plantilla)Session["vPlantilla"]).tblPlantillaMarcadores.FirstOrDefault(t => t.lPlantillaMarcador_id == id);
            if (tblPlantillaMarcador == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Delete", tblPlantillaMarcador);
        }

        // POST: tblPlantillaMarcadors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var plantilla = (ViewModels.Plantilla)Session["vPlantilla"];
            int PlantillaID = 0;
            if (plantilla != null)
            {
                tblPlantillaMarcador tblPlantillaMarcador = plantilla.tblPlantillaMarcadores.FirstOrDefault(t => t.lPlantillaMarcador_id == id);

                if (tblPlantillaMarcador != null)
                {
                    PlantillaID = tblPlantillaMarcador.lPlantilla_id;

                    if (id > 0)
                    {
                        plantilla.ItemPlantillaEliminados.Add(tblPlantillaMarcador);
                    }

                    plantilla.tblPlantillaMarcadores.Remove(tblPlantillaMarcador);
                }
            }

            string url = Url.Action("Index", "tblPlantillaMarcadores", new { id = PlantillaID });
            return Json(new { success = true, url = url });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
















        //public ActionResult Index()
        //{
        //    var tblPlantillaMarcador = db.tblPlantillaMarcador.Include(t => t.tblDocumentoCaracteristica).Include(t => t.tblPlantilla);
        //    return View(tblPlantillaMarcador.ToList());
        //}

        //// GET: tblPlantillaMarcadores/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    tblPlantillaMarcador tblPlantillaMarcador = db.tblPlantillaMarcador.Find(id);
        //    if (tblPlantillaMarcador == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tblPlantillaMarcador);
        //}

        //// GET: tblPlantillaMarcadores/Create
        //public ActionResult Create()
        //{
        //    ViewBag.lDocumentoCaracteristica_id = new SelectList(db.tblDocumentoCaracteristica, "lDocumentoCaracteristica_id", "sNombre");
        //    ViewBag.lPlantilla_id = new SelectList(db.tblPlantilla, "lPlantilla_id", "Nombre");
        //    return View();
        //}

        //// POST: tblPlantillaMarcadores/Create
        //// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        //// más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "lPlantillaMarcador_id,lDocumentoCaracteristica_id,sMarcador,lPlantilla_id")] tblPlantillaMarcador tblPlantillaMarcador)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.tblPlantillaMarcador.Add(tblPlantillaMarcador);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.lDocumentoCaracteristica_id = new SelectList(db.tblDocumentoCaracteristica, "lDocumentoCaracteristica_id", "sNombre", tblPlantillaMarcador.lDocumentoCaracteristica_id);
        //    ViewBag.lPlantilla_id = new SelectList(db.tblPlantilla, "lPlantilla_id", "Nombre", tblPlantillaMarcador.lPlantilla_id);
        //    return View(tblPlantillaMarcador);
        //}

        //// GET: tblPlantillaMarcadores/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    tblPlantillaMarcador tblPlantillaMarcador = db.tblPlantillaMarcador.Find(id);
        //    if (tblPlantillaMarcador == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.lDocumentoCaracteristica_id = new SelectList(db.tblDocumentoCaracteristica, "lDocumentoCaracteristica_id", "sNombre", tblPlantillaMarcador.lDocumentoCaracteristica_id);
        //    ViewBag.lPlantilla_id = new SelectList(db.tblPlantilla, "lPlantilla_id", "Nombre", tblPlantillaMarcador.lPlantilla_id);
        //    return View(tblPlantillaMarcador);
        //}

        //// POST: tblPlantillaMarcadores/Edit/5
        //// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        //// más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "lPlantillaMarcador_id,lDocumentoCaracteristica_id,sMarcador,lPlantilla_id")] tblPlantillaMarcador tblPlantillaMarcador)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(tblPlantillaMarcador).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.lDocumentoCaracteristica_id = new SelectList(db.tblDocumentoCaracteristica, "lDocumentoCaracteristica_id", "sNombre", tblPlantillaMarcador.lDocumentoCaracteristica_id);
        //    ViewBag.lPlantilla_id = new SelectList(db.tblPlantilla, "lPlantilla_id", "Nombre", tblPlantillaMarcador.lPlantilla_id);
        //    return View(tblPlantillaMarcador);
        //}

        //// GET: tblPlantillaMarcadores/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    tblPlantillaMarcador tblPlantillaMarcador = db.tblPlantillaMarcador.Find(id);
        //    if (tblPlantillaMarcador == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tblPlantillaMarcador);
        //}

        //// POST: tblPlantillaMarcadores/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    tblPlantillaMarcador tblPlantillaMarcador = db.tblPlantillaMarcador.Find(id);
        //    db.tblPlantillaMarcador.Remove(tblPlantillaMarcador);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
