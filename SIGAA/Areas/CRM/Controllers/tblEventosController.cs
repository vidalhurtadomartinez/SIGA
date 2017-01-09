using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIGAA.Areas.CRM.Models;

namespace SIGAA.Areas.CRM.Controllers
{
    public class tblEventosController : Controller
    {
        private DataDb db = new DataDb();

        // GET: tblEventos
        public ActionResult Index()
        {
            var tblEvento = db.tblEvento.Include(t => t.tblCliente).Include(t => t.tblNotificacion).Include(t => t.tblTipoEvento).Include(t => t.tblTipoRespuesta);
            return View(tblEvento);
        }


        [ChildActionOnly]
        public ActionResult List(int id)
        {
            var tblEvento = db.tblEvento.Include(t => t.tblCliente).Include(t => t.tblNotificacion).Include(t => t.tblTipoEvento).Include(t => t.tblTipoRespuesta).Where(t=>t.lCliente_id == id);
            
            return PartialView("_List", tblEvento.ToList());
        }

        public ActionResult Events()
        {
            var tblEvento = db.tblEvento.Include(t => t.tblCliente).Include(t => t.tblNotificacion).Include(t => t.tblTipoEvento);
            return View(tblEvento.ToList());            
        }

        // GET: tblEventos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblEvento tblEvento = db.tblEvento.Find(id);
            if (tblEvento == null)
            {
                return HttpNotFound();
            }
            return View(tblEvento);
        }

        public ActionResult Init(int id)
        {
            ViewBag.lCliente_id = new SelectList(db.tblCliente, "lCliente_id", "Nombres");
            ViewBag.lTipoRespuesta_id = new SelectList(db.tblTipoRespuesta, "lTipoRespuesta_id", "Descripcion");
            ViewBag.lNotificacion_id = new SelectList(db.tblNotificacion, "lNotificacion_id", "Descripcion");
            ViewBag.lTipoEvento_id = new SelectList(db.tblTipoEvento, "lTipoEvento_id", "Descripcion");

            return View(new tblEvento() { dtFechaRegistro_dt = DateTime.Now, dtFechaProgramada_dt = null, lCliente_id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Init(tblEvento tblEvento)
        {
            if (ModelState.IsValid)
            {
                db.tblEvento.Add(tblEvento);
                db.SaveChanges();

                if(tblEvento.dtFechaProgramada_dt != null)
                {
                    var cliente = db.tblCliente.Find(tblEvento.lCliente_id);

                    var evento = new tEvento()
                    {
                        dtFechaRegistro_dt = DateTime.Now,
                        Titulo = string.Format("{0}; Telefono: {1}, {2}", cliente.NombreCompleto, cliente.Telefono, cliente.Celular),
                        Descripcion = string.Format("Devolver llamada"),
                        dtDesde_dt = Convert.ToDateTime(tblEvento.dtFechaProgramada_dt),
                        dtHasta_dt = Convert.ToDateTime(tblEvento.dtFechaProgramada_dt).AddHours(1),
                        dtFinaliza_dt = Convert.ToDateTime(tblEvento.dtFechaProgramada_dt).AddHours(1),
                        TodoElDia = false,
                        ZonaDesde = "Etc/UTC",
                        ZonaHasta = "Etc/UTC",
                        EventRecurrenceID = tblEvento.lEvento_id
                    };

                    db.tEvento.Add(evento);
                    db.SaveChanges();
                }

                

                return RedirectToAction("Index", "Scheduler");
            }

            ViewBag.lCliente_id = new SelectList(db.tblCliente, "lCliente_id", "Nombres", tblEvento.lCliente_id);
            ViewBag.lTipoRespuesta_id = new SelectList(db.tblTipoRespuesta, "lTipoRespuesta_id", "Descripcion", tblEvento.lTipoRespuesta_id);
            ViewBag.lNotificacion_id = new SelectList(db.tblNotificacion, "lNotificacion_id", "Descripcion", tblEvento.lNotificacion_id);
            ViewBag.lTipoEvento_id = new SelectList(db.tblTipoEvento, "lTipoEvento_id", "Descripcion", tblEvento.lTipoEvento_id);
            return View(tblEvento);
        }

        public ActionResult RegistrarEvento(int id)
        {
            ViewBag.lCliente_id = new SelectList(db.tblCliente, "lCliente_id", "Nombres");
            ViewBag.lTipoRespuesta_id = new SelectList(db.tblTipoRespuesta, "lTipoRespuesta_id", "Descripcion");
            ViewBag.lNotificacion_id = new SelectList(db.tblNotificacion, "lNotificacion_id", "Descripcion");
            ViewBag.lTipoEvento_id = new SelectList(db.tblTipoEvento, "lTipoEvento_id", "Descripcion");

            var evento = db.tEvento.Find(id);
            var tevento = db.tblEvento.Find(evento.EventRecurrenceID);

            var cliente = db.tblCliente.FirstOrDefault(c=>c.lCliente_id == tevento.lCliente_id);

            return View(new tblEvento() { dtFechaRegistro_dt = DateTime.Now, dtFechaProgramada_dt = null, lCliente_id = cliente.lCliente_id, tblCliente = cliente, lRecordatorio_id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarEvento(tblEvento tblEvento)
        {
            if (ModelState.IsValid)
            {
                db.tblEvento.Add(tblEvento);
                db.SaveChanges();

                if (tblEvento.dtFechaProgramada_dt != null)
                {
                    var cliente = db.tblCliente.Find(tblEvento.lCliente_id);

                    var evento = new tEvento()
                    {
                        dtFechaRegistro_dt = DateTime.Now,
                        Titulo = string.Format("{0}; Telefono: {1}, {2}", cliente.NombreCompleto, cliente.Telefono, cliente.Celular),
                        Descripcion = string.Format("Devolver llamada"),
                        dtDesde_dt = Convert.ToDateTime(tblEvento.dtFechaProgramada_dt),
                        dtHasta_dt = Convert.ToDateTime(tblEvento.dtFechaProgramada_dt).AddHours(1),
                        dtFinaliza_dt = Convert.ToDateTime(tblEvento.dtFechaProgramada_dt).AddHours(1),
                        TodoElDia = false,
                        ZonaDesde = "Etc/UTC",
                        ZonaHasta = "Etc/UTC",
                        EventRecurrenceID = tblEvento.lEvento_id
                    };

                    db.tEvento.Add(evento);
                    db.SaveChanges();
                }

                return RedirectToAction("Index", "Scheduler");
            }

            ViewBag.lCliente_id = new SelectList(db.tblCliente, "lCliente_id", "Nombres", tblEvento.lCliente_id);
            ViewBag.lTipoRespuesta_id = new SelectList(db.tblTipoRespuesta, "lTipoRespuesta_id", "Descripcion", tblEvento.lTipoRespuesta_id);
            ViewBag.lNotificacion_id = new SelectList(db.tblNotificacion, "lNotificacion_id", "Descripcion", tblEvento.lNotificacion_id);
            ViewBag.lTipoEvento_id = new SelectList(db.tblTipoEvento, "lTipoEvento_id", "Descripcion", tblEvento.lTipoEvento_id);
            return View(tblEvento);
        }

        public ActionResult GuardarEvento()
        {
            ViewBag.lCliente_id = new SelectList(db.tblCliente, "lCliente_id", "Nombres");
            ViewBag.lTipoRespuesta_id = new SelectList(db.tblTipoRespuesta, "lTipoRespuesta_id", "Descripcion");
            ViewBag.lNotificacion_id = new SelectList(db.tblNotificacion, "lNotificacion_id", "Descripcion");
            ViewBag.lTipoEvento_id = new SelectList(db.tblTipoEvento, "lTipoEvento_id", "Descripcion");

            var cliente = db.tblCliente.FirstOrDefault();

            return View(new tblEvento() { dtFechaRegistro_dt = DateTime.Now, dtFechaProgramada_dt = null, lCliente_id = cliente.lCliente_id, tblCliente = cliente });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarEvento(tblEvento tblEvento)
        {
            if (ModelState.IsValid)
            {
                db.tblEvento.Add(tblEvento);
                db.SaveChanges();

                if (tblEvento.dtFechaProgramada_dt != null)
                {
                    var cliente = db.tblCliente.Find(tblEvento.lCliente_id);

                    var evento = new tEvento()
                    {
                        dtFechaRegistro_dt = DateTime.Now,
                        Titulo = string.Format("{0}; Telefono: {1}, {2}", cliente.NombreCompleto, cliente.Telefono, cliente.Celular),
                        Descripcion = string.Format("Devolver llamada"),
                        dtDesde_dt = Convert.ToDateTime(tblEvento.dtFechaProgramada_dt),
                        dtHasta_dt = Convert.ToDateTime(tblEvento.dtFechaProgramada_dt).AddHours(1),
                        dtFinaliza_dt = Convert.ToDateTime(tblEvento.dtFechaProgramada_dt).AddHours(1),
                        TodoElDia = false,
                        ZonaDesde = "Etc/UTC",
                        ZonaHasta = "Etc/UTC",
                        EventRecurrenceID = tblEvento.lEvento_id
                    };

                    db.tEvento.Add(evento);
                    db.SaveChanges();
                }

                return RedirectToAction("Index", "Scheduler");
            }

            ViewBag.lCliente_id = new SelectList(db.tblCliente, "lCliente_id", "Nombres", tblEvento.lCliente_id);
            ViewBag.lTipoRespuesta_id = new SelectList(db.tblTipoRespuesta, "lTipoRespuesta_id", "Descripcion", tblEvento.lTipoRespuesta_id);
            ViewBag.lNotificacion_id = new SelectList(db.tblNotificacion, "lNotificacion_id", "Descripcion", tblEvento.lNotificacion_id);
            ViewBag.lTipoEvento_id = new SelectList(db.tblTipoEvento, "lTipoEvento_id", "Descripcion", tblEvento.lTipoEvento_id);
            return View(tblEvento);
        }

        // GET: tblEventos/Create
        public ActionResult Create()
        {
            ViewBag.lCliente_id = new SelectList(db.tblCliente, "lCliente_id", "Nombres");
            ViewBag.lTipoRespuesta_id = new SelectList(db.tblTipoRespuesta, "lTipoRespuesta_id", "Descripcion");
            ViewBag.lNotificacion_id = new SelectList(db.tblNotificacion, "lNotificacion_id", "Descripcion");
            ViewBag.lTipoEvento_id = new SelectList(db.tblTipoEvento, "lTipoEvento_id", "Descripcion");

            return View(new tblEvento() { dtFechaRegistro_dt = DateTime.Now, dtFechaProgramada_dt = null});
        }

        // POST: tblEventos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblEvento tblEvento)
        {
            if (ModelState.IsValid)
            {
                db.tblEvento.Add(tblEvento);
                db.SaveChanges();

                if (tblEvento.dtFechaProgramada_dt != null)
                {
                    var cliente = db.tblCliente.Find(tblEvento.lCliente_id);

                    var evento = new tEvento()
                    {
                        dtFechaRegistro_dt = DateTime.Now,
                        Titulo = string.Format("{0}; Telefono: {1}, {2}", cliente.NombreCompleto, cliente.Telefono, cliente.Celular),
                        Descripcion = string.Format("Devolver llamada"),
                        dtDesde_dt = Convert.ToDateTime(tblEvento.dtFechaProgramada_dt),
                        dtHasta_dt = Convert.ToDateTime(tblEvento.dtFechaProgramada_dt).AddHours(1),
                        dtFinaliza_dt = Convert.ToDateTime(tblEvento.dtFechaProgramada_dt).AddHours(1),
                        TodoElDia = false,
                        ZonaDesde = "Etc/UTC",
                        ZonaHasta = "Etc/UTC",
                        EventRecurrenceID = tblEvento.lEvento_id
                    };

                    db.tEvento.Add(evento);
                    db.SaveChanges();
                }

                return RedirectToAction("Index", "Scheduler");
            }

            ViewBag.lCliente_id = new SelectList(db.tblCliente, "lCliente_id", "Nombres", tblEvento.lCliente_id);
            ViewBag.lTipoRespuesta_id = new SelectList(db.tblTipoRespuesta, "lTipoRespuesta_id", "Descripcion", tblEvento.lTipoRespuesta_id);
            ViewBag.lNotificacion_id = new SelectList(db.tblNotificacion, "lNotificacion_id", "Descripcion", tblEvento.lNotificacion_id);
            ViewBag.lTipoEvento_id = new SelectList(db.tblTipoEvento, "lTipoEvento_id", "Descripcion", tblEvento.lTipoEvento_id);
            return View(tblEvento);
        }

        // GET: tblEventos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblEvento tblEvento = db.tblEvento.Find(id);
            if (tblEvento == null)
            {
                return HttpNotFound();
            }
            ViewBag.lCliente_id = new SelectList(db.tblCliente, "lCliente_id", "Nombres", tblEvento.lCliente_id);
            ViewBag.lTipoRespuesta_id = new SelectList(db.tblTipoRespuesta, "lTipoRespuesta_id", "Descripcion", tblEvento.lTipoRespuesta_id);
            ViewBag.lNotificacion_id = new SelectList(db.tblNotificacion, "lNotificacion_id", "Descripcion", tblEvento.lNotificacion_id);
            ViewBag.lTipoEvento_id = new SelectList(db.tblTipoEvento, "lTipoEvento_id", "Descripcion", tblEvento.lTipoEvento_id);
            return View(tblEvento);
        }

        // POST: tblEventos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lEvento_id,dtFechaRegistro_dt,Descripcion,dtFechaProgramada_dt,lTipoEvento_id,lNotificacion_id,lCliente_id")] tblEvento tblEvento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblEvento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.lCliente_id = new SelectList(db.tblCliente, "lCliente_id", "Nombres", tblEvento.lCliente_id);
            ViewBag.lTipoRespuesta_id = new SelectList(db.tblTipoRespuesta, "lTipoRespuesta_id", "Descripcion", tblEvento.lTipoRespuesta_id);
            ViewBag.lNotificacion_id = new SelectList(db.tblNotificacion, "lNotificacion_id", "Descripcion", tblEvento.lNotificacion_id);
            ViewBag.lTipoEvento_id = new SelectList(db.tblTipoEvento, "lTipoEvento_id", "Descripcion", tblEvento.lTipoEvento_id);
            return View(tblEvento);
        }

        // GET: tblEventos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblEvento tblEvento = db.tblEvento.Find(id);
            if (tblEvento == null)
            {
                return HttpNotFound();
            }
            return View(tblEvento);
        }

        // POST: tblEventos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblEvento tblEvento = db.tblEvento.Find(id);
            db.tblEvento.Remove(tblEvento);
            db.SaveChanges();
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
