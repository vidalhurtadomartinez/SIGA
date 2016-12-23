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
using SIGAA.Etiquetas;
using SIGAA.Commons;

namespace SIGAA.Areas.OYM.Controllers
{
    [Autenticado]
    public class tblProcesosController : Controller
    {
        private DataDb db = new DataDb();

        [Permiso(Permiso = RolesPermisos.OYM_procesos_puedeVerIndice)]
        public async Task<ActionResult> Index()
        {
            return View(await db.tblProceso.Include(p => p.TipoProceso).ToListAsync());
        }

        [Permiso(Permiso = RolesPermisos.OYM_procesos_puedeVerDetalle)]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblProceso tblProceso = await db.tblProceso.Include(p => p.TipoProceso).Where(p=>p.lProceso_id == id).SingleAsync();
            if (tblProceso == null)
            {
                return HttpNotFound();
            }
            return View(tblProceso);
        }

        [Permiso(Permiso = RolesPermisos.OYM_procesos_puedeCrearNuevo)]
        public ActionResult Create(int? Tipo)
        {
            ViewModels.Proceso proceso = new ViewModels.Proceso();

            proceso.tblProceso = new tblProceso();
            proceso.tblProcesoDetalles = new List<tblProcesoDetalle>();
            proceso.tblProcesoDetalleCategorias = new List<tblProcesoDetalleCategoria>();
            proceso.ProcesosEliminados = new List<tblProcesoDetalle>();
            proceso.ProcesosCategoriaEliminados = new List<tblProcesoDetalleCategoria>();

            Session["vProceso"] = proceso;

            return View(new tblProceso { bActivo=true, lTipoProceso_id = Tipo});
        }

        // POST: tblProcesos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(tblProceso tblProceso)
        {
            if (ModelState.IsValid)
            {
                var vProceso = (ViewModels.Proceso)Session["vProceso"];
                var tipo = Request.QueryString["Tipo"] != null ? Convert.ToInt32(Request.QueryString["Tipo"]) : 0;

                tblProceso.dtFechaRegistro_dt = DateTime.Now;
                tblProceso.lTipoProceso_id = tipo;
                tblProceso.lUsuario_id = FrontUser.Get().iUsuario_id;

                db.tblProceso.Add(tblProceso);

                await db.SaveChangesAsync();


                var ProcessID = db.tblProceso.Select(c => c.lProceso_id).Max();
                foreach (var item in vProceso.tblProcesoDetalles)
                {
                    var detalle = new tblProcesoDetalle
                    {
                        lProcesoDetalle_id = item.lProcesoDetalle_id,
                        lParticipante_id = item.lParticipante_id,                        
                        lCategoria_id = item.lCategoria_id,
                        lRolParticipante_id = item.lRolParticipante_id,
                        lRevision1 = item.lRevision1,
                        lRevision2 = item.lRevision2,
                        lRevision3 = item.lRevision3,
                        lProceso_id = item.lProceso_id
                    };

                    if (detalle.lProcesoDetalle_id < 0)
                    {
                        detalle.lProceso_id = ProcessID;
                        db.tblProcesoDetalle.Add(detalle);
                    }
                    else
                    {
                        db.Entry(detalle).State = EntityState.Modified;
                    }

                }

                await db.SaveChangesAsync();

                foreach (var item in vProceso.tblProcesoDetalleCategorias)
                {
                    var detallecategoria = new tblProcesoDetalleCategoria
                    {
                        lProcesoDetalleCategoria_id = item.lProcesoDetalleCategoria_id,                        
                        lCategoria_id = item.lCategoria_id,
                        lRevision1 = item.lRevision1,
                        lRevision2 = item.lRevision2,
                        lRevision3 = item.lRevision3,
                        lProceso_id = item.lProceso_id
                    };

                    if (detallecategoria.lProcesoDetalleCategoria_id < 0)
                    {
                        detallecategoria.lProceso_id = ProcessID;
                        db.tblProcesoDetalleCategoria.Add(detallecategoria);
                    }
                    else
                    {
                        db.Entry(detallecategoria).State = EntityState.Modified;
                    }

                }

                await db.SaveChangesAsync();


                return RedirectToAction("Index");
            }


            return View(tblProceso);
        }

        [Permiso(Permiso = RolesPermisos.OYM_procesos_puedeEditar)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var process = await db.tblProceso                        
                        .Where(p => p.lProceso_id == id)
                        .SingleAsync();

            if (process == null)
            {
                return HttpNotFound();
            }

            ViewModels.Proceso proceso = new ViewModels.Proceso();

            proceso.tblProceso = process;
            proceso.tblProcesoDetalles = db.tblProcesoDetalle.Include(t => t.vVistaPersonal).Include(t => t.RolParticipante).Include(t => t.tblCategoria).Include(t => t.tblProceso).Where(t => t.lProceso_id == id).ToList();
            proceso.tblProcesoDetalleCategorias = db.tblProcesoDetalleCategoria.Include(t => t.tblCategoria).Include(t => t.tblProceso).Where(t => t.lProceso_id == id).ToList();
            proceso.ProcesosEliminados = new List<tblProcesoDetalle>();
            proceso.ProcesosCategoriaEliminados = new List<tblProcesoDetalleCategoria>();

            Session["vProceso"] = proceso;

            return View(process);
        }

        // POST: tblProcesos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(tblProceso tblProceso)
        {
            if (ModelState.IsValid)
            {                               
                var vProceso = (ViewModels.Proceso)Session["vProceso"];
                var proceso = db.tblProceso.FirstOrDefault(p=>p.lProceso_id == tblProceso.lProceso_id);

                proceso.sNombre = tblProceso.sNombre;                
                proceso.sDescripcion = tblProceso.sDescripcion;
                proceso.bActivo = tblProceso.bActivo;

                db.Entry(proceso).State = EntityState.Modified;
                await db.SaveChangesAsync();


                foreach (var item in vProceso.tblProcesoDetalles)
                {
                    var detalle = new tblProcesoDetalle
                    {
                        lProcesoDetalle_id = item.lProcesoDetalle_id,
                        lParticipante_id = item.lParticipante_id,
                        //agd_codigo = item.agd_codigo,
                        lCategoria_id = item.lCategoria_id,
                        lRolParticipante_id = item.lRolParticipante_id,
                        lRevision1 = item.lRevision1,
                        lRevision2 = item.lRevision2,
                        lRevision3 = item.lRevision3,
                        lProceso_id = item.lProceso_id
                    };

                    if (detalle.lProcesoDetalle_id < 0)
                    {
                        detalle.lProceso_id = proceso.lProceso_id;
                        db.tblProcesoDetalle.Add(detalle);
                    }
                    else
                    {
                        db.Entry(detalle).State = EntityState.Modified;
                    }

                }

                await db.SaveChangesAsync();

                foreach (var item in vProceso.tblProcesoDetalleCategorias)
                {
                    var detallecategoria = new tblProcesoDetalleCategoria
                    {
                        lProcesoDetalleCategoria_id = item.lProcesoDetalleCategoria_id,                        
                        lCategoria_id = item.lCategoria_id,
                        lRevision1 = item.lRevision1,
                        lRevision2 = item.lRevision2,
                        lRevision3 = item.lRevision3,
                        lProceso_id = item.lProceso_id
                    };

                    if (detallecategoria.lProcesoDetalleCategoria_id < 0)
                    {
                        detallecategoria.lProceso_id = proceso.lProceso_id;
                        db.tblProcesoDetalleCategoria.Add(detallecategoria);
                    }
                    else
                    {
                        db.Entry(detallecategoria).State = EntityState.Modified;
                    }

                }

                await db.SaveChangesAsync();

                foreach (var item in vProceso.ProcesosEliminados)
                {
                    tblProcesoDetalle tblProcesoDetalle = db.tblProcesoDetalle.Find(item.lProcesoDetalle_id);
                    db.tblProcesoDetalle.Remove(tblProcesoDetalle);
                }

                db.SaveChanges();

                foreach (var item in vProceso.ProcesosCategoriaEliminados)
                {
                    tblProcesoDetalleCategoria tblProcesoDetalle = db.tblProcesoDetalleCategoria.Find(item.lProcesoDetalleCategoria_id);
                    db.tblProcesoDetalleCategoria.Remove(tblProcesoDetalle);
                }

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(tblProceso);
        }

        [Permiso(Permiso = RolesPermisos.OYM_procesos_puedeEliminar)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblProceso tblProceso = await db.tblProceso.Include(p => p.TipoProceso).FirstOrDefaultAsync(p=> p.lProceso_id == id);
            if (tblProceso == null)
            {
                return HttpNotFound();
            }
            return View(tblProceso);
        }

        // POST: tblProcesos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            tblProceso tblProceso = await db.tblProceso.FindAsync(id);
            db.tblProceso.Remove(tblProceso);
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
