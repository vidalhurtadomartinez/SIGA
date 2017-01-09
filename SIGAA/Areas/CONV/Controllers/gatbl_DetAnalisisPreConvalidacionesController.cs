using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIGAA.Areas.CONV.Models;
using SIGAA.Etiquetas;
using SIGAA.Commons;

namespace SIGAA.Areas.CONV.Controllers
{
    public class gatbl_DetAnalisisPreConvalidacionesController : Controller
    {
        private ConvalidacionesContext db = new ConvalidacionesContext();

        // GET: gatbl_DetAnalisisPreConvalidaciones
        public ActionResult Index(int id)
        {
            //var gatbl_DetAnalisisPreConvalidaciones = db.gatbl_DetAnalisisPreConvalidaciones.Include(g => g.gatbl_AnalisisPreConvalidaciones).Include(g => g.gatbl_DestinoProgramasAnaliticos).Include(g => g.gatbl_OrigenProgramasAnaliticos);
            //return View(gatbl_DetAnalisisPreConvalidaciones.ToList());

            ViewBag.PreAnalisisID = id;
            var gatbl_DetAnalisisPreConvalidaciones = db.gatbl_DetAnalisisPreConvalidaciones.Include(g => g.gatbl_AnalisisPreConvalidaciones).Where(m=>m.lAnalisisPreConvalidacion_id == id);
            //var gatbl_DetAnalisisPreConvalidaciones = ((ViewModels.PreAnalisis)Session["vPreAnalisis"]).gatbl_DetAnalisisPreConvalidaciones;
            //var gatbl_DetAnalisisPreConvalidaciones = ((ViewModels.PreAnalisis)Session["vPreAnalisis"]).gatbl_DetAnalisisPreConvalidacionesEliminados;

            return PartialView("_Index", gatbl_DetAnalisisPreConvalidaciones.ToList());
        }

        public JsonResult ObtenerMateriasDestino(string text)
        {
            var panalisis = (ViewModels.PreAnalisis)Session["vPreAnalisis"];
            var preanalisis = db.gatbl_AnalisisPreConvalidaciones.Find(panalisis.gatbl_AnalisisPreConvalidaciones.lAnalisisPreConvalidacion_id);
            var convalidacion = db.gatbl_PConvalidaciones.Find(preanalisis.lPConvalidacion_id);

            return Json((from c in db.gatbl_ProgramasAnaliticos.Where(m => m.lCarrera_id == convalidacion.lCarreraDestino_id && m.lPensum_id == convalidacion.lPensum_id && m.iEliminado_fl != "2")
                         select new { lProgramaAnalitico_id = c.lProgramaAnalitico_id, sMateria_desc = c.sMateria_desc }).Where(g => g.sMateria_desc.Contains(text)).OrderBy(g => g.sMateria_desc), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerMateriasOrigen(string text)
        {
            var panalisis = (ViewModels.PreAnalisis)Session["vPreAnalisis"];
            var preanalisis = db.gatbl_AnalisisPreConvalidaciones.Find(panalisis.gatbl_AnalisisPreConvalidaciones.lAnalisisPreConvalidacion_id);
            var convalidacion = db.gatbl_PConvalidaciones.Find(preanalisis.lPConvalidacion_id);

            return Json((from c in db.gatbl_ProgramasAnaliticos
                         where c.lCarrera_id == convalidacion.lCarreraOrigen_id && c.iEliminado_fl != "2"
                         && db.gatbl_CertificadosMateria.Where(m => m.lPConvalidacion_id == convalidacion.lPConvalidacion_id && m.iEstado_fl != "2").Any(m => m.lMateria_id == c.lProgramaAnalitico_id)
                         select new { lProgramaAnalitico_id = c.lProgramaAnalitico_id, sMateria_desc = c.sMateria_desc }).Where(g => g.sMateria_desc.Contains(text)).OrderBy(g => g.sMateria_desc), JsonRequestBehavior.AllowGet);
        }

        public string MateriasSeleccionadas(string Materias)
        {
            var strResultado = string.Empty;
            foreach (var it in Materias.Split(','))
            {
                if (!strResultado.Equals(string.Empty)){
                    strResultado = strResultado + ", ";
                }

                var mat = db.gatbl_ProgramasAnaliticos.Find(Convert.ToInt32(it));
                strResultado = strResultado + mat.sMateria_desc;
            }
            return strResultado;
        }

        [ChildActionOnly]
        public ActionResult List(int id)
        {
            ViewBag.PreAnalisisID = id;
            var gatbl_DetAnalisisPreConvalidaciones = db.gatbl_DetAnalisisPreConvalidaciones.Where(a => a.lAnalisisPreConvalidacion_id == id);

            return PartialView("_List", gatbl_DetAnalisisPreConvalidaciones.ToList());
        }

        [ChildActionOnly]
        public ActionResult ListAnalisis(int id)
        {
            ViewBag.PreAnalisisID = id;
            var gatbl_DetAnalisisPreConvalidaciones = db.gatbl_DetAnalisisPreConvalidaciones.Where(a => a.lAnalisisPreConvalidacion_id == id);

            return PartialView("_ListAnalisis", gatbl_DetAnalisisPreConvalidaciones.ToList());
        }

        [ChildActionOnly]
        public ActionResult ListUnit(int id)
        {
            ViewBag.PreAnalisisID = id;
            var gatbl_DetAnalisisPreConvalidaciones = db.gatbl_DetAnalisisPreConvalidaciones.Find(id);

            string sql = string.Format("Exec sp_MateriasAConvalidarDetallado {0}", id);
            var unidadesdestino = db.Database.SqlQuery<gatbl_DetProgramaAnaliticoAnalisis>(sql).ToList<gatbl_DetProgramaAnaliticoAnalisis>();            

            return PartialView("_ListUnit", unidadesdestino.ToList());
        }

        // GET: gatbl_DetAnalisisPreConvalidaciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_DetAnalisisPreConvalidaciones gatbl_DetAnalisisPreConvalidaciones = db.gatbl_DetAnalisisPreConvalidaciones.Find(id);
            if (gatbl_DetAnalisisPreConvalidaciones == null)
            {
                return HttpNotFound();
            }
            return View(gatbl_DetAnalisisPreConvalidaciones);
        }

        // GET: gatbl_DetAnalisisPreConvalidaciones/Create
        public ActionResult Create(int PreAnalisisID)
        {
            gatbl_DetAnalisisPreConvalidaciones gatbl_DetAnalisisPreConvalidaciones = new gatbl_DetAnalisisPreConvalidaciones();
            gatbl_DetAnalisisPreConvalidaciones.lAnalisisPreConvalidacion_id = PreAnalisisID;

            var preanalisis = db.gatbl_AnalisisPreConvalidaciones.Find(PreAnalisisID);
            var convalidacion = db.gatbl_PConvalidaciones.Find(preanalisis.lPConvalidacion_id);

            ViewBag.lAnalisisPreConvalidacion_id = new SelectList(db.gatbl_AnalisisPreConvalidaciones, "lAnalisisPreConvalidacion_id", "lResponsable_id");
            ViewBag.lMateriaDestino_id = new SelectList(db.gatbl_ProgramasAnaliticos.Where(m => m.lCarrera_id == convalidacion.lCarreraDestino_id && m.lPensum_id == convalidacion.lPensum_id && m.iEliminado_fl != "2"), "lProgramaAnalitico_id", "sMateria_desc");
            ViewBag.lMateriaOrigen_id = new SelectList(db.gatbl_ProgramasAnaliticos.Where(m => m.lCarrera_id == convalidacion.lCarreraOrigen_id && m.iEliminado_fl != "2"), "lProgramaAnalitico_id", "sMateria_desc");


            return PartialView("_Create", gatbl_DetAnalisisPreConvalidaciones);
        }

        // POST: gatbl_DetAnalisisPreConvalidaciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(gatbl_DetAnalisisPreConvalidaciones gatbl_DetAnalisisPreConvalidaciones)
        {            
            if (ModelState.IsValid)
            {
                //if (gatbl_DetAnalisisPreConvalidaciones.lMateriaOrigen_id != 0 && gatbl_DetAnalisisPreConvalidaciones.lMateriaDestino_id != 0)
                //{
                //    gatbl_DetAnalisisPreConvalidaciones.iEstado_fl = "1";
                //    gatbl_DetAnalisisPreConvalidaciones.iEliminado_fl = "1";
                //    gatbl_DetAnalisisPreConvalidaciones.sCreated_by = DateTime.Now.ToString();
                //    gatbl_DetAnalisisPreConvalidaciones.iConcurrencia_id = 1;


                //    db.gatbl_DetAnalisisPreConvalidaciones.Add(gatbl_DetAnalisisPreConvalidaciones);
                //    db.SaveChanges();

                //    string url = Url.Action("Index", "gatbl_DetAnalisisPreConvalidaciones", new { id = gatbl_DetAnalisisPreConvalidaciones.lAnalisisPreConvalidacion_id });
                //    return Json(new { success = true, url = url });
                //}
                //else
                //{
                //    ViewBag.Error = "Debe seleccionar la materia origen y destino.";
                //}

                gatbl_DetAnalisisPreConvalidaciones.iEstado_fl = "1";
                gatbl_DetAnalisisPreConvalidaciones.iEliminado_fl = "1";
                gatbl_DetAnalisisPreConvalidaciones.sCreated_by = DateTime.Now.ToString();
                gatbl_DetAnalisisPreConvalidaciones.iConcurrencia_id = 1;
                gatbl_DetAnalisisPreConvalidaciones.sMateriaOrigen_id = Request["materiaOrigen"];
                gatbl_DetAnalisisPreConvalidaciones.sMateriaDestino_id = Request["materiaDestino"];
                gatbl_DetAnalisisPreConvalidaciones.sMateriaOrigen = MateriasSeleccionadas(gatbl_DetAnalisisPreConvalidaciones.sMateriaOrigen_id);
                gatbl_DetAnalisisPreConvalidaciones.sMateriaDestino = MateriasSeleccionadas(gatbl_DetAnalisisPreConvalidaciones.sMateriaDestino_id);


                db.gatbl_DetAnalisisPreConvalidaciones.Add(gatbl_DetAnalisisPreConvalidaciones);
                db.SaveChanges();

                foreach(var it in gatbl_DetAnalisisPreConvalidaciones.sMateriaOrigen_id.Split(','))
                {
                    var materia = new gatbl_AnalisisPreConvalidacionesMateria()
                    {
                        lProgramaAnalitico_id = Convert.ToInt32(it),
                        lOrigen = 1,
                        lDetAnalisisPreConvalidacion_id = gatbl_DetAnalisisPreConvalidaciones.lDetAnalisisPreConvalidacion_id
                    };

                    db.gatbl_AnalisisPreConvalidacionesMateria.Add(materia);
                }

                foreach (var it in gatbl_DetAnalisisPreConvalidaciones.sMateriaDestino_id.Split(','))
                {
                    var materia = new gatbl_AnalisisPreConvalidacionesMateria()
                    {
                        lProgramaAnalitico_id = Convert.ToInt32(it),
                        lOrigen = 2,
                        lDetAnalisisPreConvalidacion_id = gatbl_DetAnalisisPreConvalidaciones.lDetAnalisisPreConvalidacion_id
                    };

                    db.gatbl_AnalisisPreConvalidacionesMateria.Add(materia);
                }

                db.SaveChanges();

                string url = Url.Action("Index", "gatbl_DetAnalisisPreConvalidaciones", new { id = gatbl_DetAnalisisPreConvalidaciones.lAnalisisPreConvalidacion_id });
                return Json(new { success = true, url = url });
            }

            var preanalisis = db.gatbl_AnalisisPreConvalidaciones.Find(gatbl_DetAnalisisPreConvalidaciones.lAnalisisPreConvalidacion_id);
            var convalidacion = db.gatbl_PConvalidaciones.Find(preanalisis.lPConvalidacion_id);

            ViewBag.lAnalisisPreConvalidacion_id = new SelectList(db.gatbl_AnalisisPreConvalidaciones, "lAnalisisPreConvalidacion_id", "lResponsable_id", gatbl_DetAnalisisPreConvalidaciones.lAnalisisPreConvalidacion_id);
            //ViewBag.lMateriaDestino_id = new SelectList(db.gatbl_ProgramasAnaliticos.Where(m => m.lCarrera_id == convalidacion.lCarreraDestino_id && m.lPensum_id == convalidacion.lPensum_id && m.iEliminado_fl != "2"), "lProgramaAnalitico_id", "sMateria_desc", gatbl_DetAnalisisPreConvalidaciones.lMateriaDestino_id);
            //ViewBag.lMateriaOrigen_id = new SelectList(db.gatbl_ProgramasAnaliticos.Where(m => m.lCarrera_id == convalidacion.lCarreraOrigen_id && m.iEliminado_fl != "2"), "lProgramaAnalitico_id", "sMateria_desc", gatbl_DetAnalisisPreConvalidaciones.lMateriaOrigen_id);

            //return View(gatbl_DetAnalisisPreConvalidaciones);

            return PartialView("_Create", gatbl_DetAnalisisPreConvalidaciones);
        }

        // GET: gatbl_DetAnalisisPreConvalidaciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_DetAnalisisPreConvalidaciones gatbl_DetAnalisisPreConvalidaciones = db.gatbl_DetAnalisisPreConvalidaciones.Find(id);
            if (gatbl_DetAnalisisPreConvalidaciones == null)
            {
                return HttpNotFound();
            }

            var detpreanalisis = db.gatbl_DetAnalisisPreConvalidaciones.Find(id);
            var preanalisis = db.gatbl_AnalisisPreConvalidaciones.Find(detpreanalisis.lAnalisisPreConvalidacion_id);
            var convalidacion = db.gatbl_PConvalidaciones.Find(preanalisis.lPConvalidacion_id);


            ViewBag.lAnalisisPreConvalidacion_id = new SelectList(db.gatbl_AnalisisPreConvalidaciones, "lAnalisisPreConvalidacion_id", "lResponsable_id", gatbl_DetAnalisisPreConvalidaciones.lAnalisisPreConvalidacion_id);
            //ViewBag.lMateriaDestino_id = new SelectList(db.gatbl_ProgramasAnaliticos.Where(m=>m.lCarrera_id == convalidacion.lCarreraDestino_id && m.lPensum_id == convalidacion.lPensum_id && m.iEliminado_fl !="2"), "lProgramaAnalitico_id", "sMateria_desc", gatbl_DetAnalisisPreConvalidaciones.lMateriaDestino_id);
            //ViewBag.lMateriaOrigen_id = new SelectList(db.gatbl_ProgramasAnaliticos.Where(m => m.lCarrera_id == convalidacion.lCarreraOrigen_id && m.iEliminado_fl != "2"), "lProgramaAnalitico_id", "sMateria_desc", gatbl_DetAnalisisPreConvalidaciones.lMateriaOrigen_id);

            return PartialView("_Edit", gatbl_DetAnalisisPreConvalidaciones);            
        }

        // POST: gatbl_DetAnalisisPreConvalidaciones/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(gatbl_DetAnalisisPreConvalidaciones gatbl_DetAnalisisPreConvalidaciones)
        {
            if (ModelState.IsValid)
            {
                var detalle = db.gatbl_DetAnalisisPreConvalidaciones.Find(gatbl_DetAnalisisPreConvalidaciones.lDetAnalisisPreConvalidacion_id);
                //detalle.lMateriaOrigen_id = gatbl_DetAnalisisPreConvalidaciones.lMateriaOrigen_id;
                //detalle.lMateriaDestino_id = gatbl_DetAnalisisPreConvalidaciones.lMateriaDestino_id;
                //detalle.lNotaOrigen = gatbl_DetAnalisisPreConvalidaciones.lNotaOrigen;
                detalle.sObservaciones = gatbl_DetAnalisisPreConvalidaciones.sObservaciones;


                db.Entry(detalle).State = EntityState.Modified;
                db.SaveChanges();


                string url = Url.Action("Index", "gatbl_DetAnalisisPreConvalidaciones", new { id = gatbl_DetAnalisisPreConvalidaciones.lAnalisisPreConvalidacion_id });
                return Json(new { success = true, url = url });
                
            }
            var detpreanalisis = db.gatbl_DetAnalisisPreConvalidaciones.Find(gatbl_DetAnalisisPreConvalidaciones.lDetAnalisisPreConvalidacion_id);
            var preanalisis = db.gatbl_AnalisisPreConvalidaciones.Find(detpreanalisis.lAnalisisPreConvalidacion_id);
            var convalidacion = db.gatbl_PConvalidaciones.Find(preanalisis.lPConvalidacion_id);

            ViewBag.lAnalisisPreConvalidacion_id = new SelectList(db.gatbl_AnalisisPreConvalidaciones, "lAnalisisPreConvalidacion_id", "lResponsable_id", gatbl_DetAnalisisPreConvalidaciones.lAnalisisPreConvalidacion_id);
            //ViewBag.lMateriaDestino_id = new SelectList(db.gatbl_ProgramasAnaliticos.Where(m => m.lCarrera_id == convalidacion.lCarreraDestino_id && m.lPensum_id == convalidacion.lPensum_id && m.iEliminado_fl != "2"), "lProgramaAnalitico_id", "sMateria_desc", gatbl_DetAnalisisPreConvalidaciones.lMateriaDestino_id);
            //ViewBag.lMateriaOrigen_id = new SelectList(db.gatbl_ProgramasAnaliticos.Where(m => m.lCarrera_id == convalidacion.lCarreraOrigen_id && m.iEliminado_fl != "2"), "lProgramaAnalitico_id", "sMateria_desc", gatbl_DetAnalisisPreConvalidaciones.lMateriaOrigen_id);

            return View(gatbl_DetAnalisisPreConvalidaciones);
        }

        // GET: gatbl_DetAnalisisPreConvalidaciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_DetAnalisisPreConvalidaciones gatbl_DetAnalisisPreConvalidaciones = db.gatbl_DetAnalisisPreConvalidaciones.Find(id);
            if (gatbl_DetAnalisisPreConvalidaciones == null)
            {
                return HttpNotFound();
            }

            return PartialView("_Delete", gatbl_DetAnalisisPreConvalidaciones);
            //return View(gatbl_DetAnalisisPreConvalidaciones);
        }

        // POST: gatbl_DetAnalisisPreConvalidaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            gatbl_DetAnalisisPreConvalidaciones gatbl_DetAnalisisPreConvalidaciones = db.gatbl_DetAnalisisPreConvalidaciones.Find(id);
            int PreAnalisisID = gatbl_DetAnalisisPreConvalidaciones.lAnalisisPreConvalidacion_id;

            foreach(var itmateria in db.gatbl_AnalisisPreConvalidacionesMateria.Where(m=>m.lDetAnalisisPreConvalidacion_id == id))
            {
                db.gatbl_AnalisisPreConvalidacionesMateria.Remove(itmateria);
            }

            db.SaveChanges();


            db.gatbl_DetAnalisisPreConvalidaciones.Remove(gatbl_DetAnalisisPreConvalidaciones);
            db.SaveChanges();
            
            string url = Url.Action("Index", "gatbl_DetAnalisisPreConvalidaciones", new { id = PreAnalisisID });
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
    }
}
