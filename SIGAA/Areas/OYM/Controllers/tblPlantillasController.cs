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
    public class tblPlantillasController : Controller
    {
        private DataDb db = new DataDb();

        [Permiso(Permiso = RolesPermisos.OYM_plantillas_puedeVerIndice)]   
        public async Task<ActionResult> Index()
        {
            return View(await db.tblPlantilla.ToListAsync());
        }

        private string FilePahtRoot()
        {
            string path = System.Configuration.ConfigurationManager.ConnectionStrings["FilePath"].ConnectionString;

            return path;
        }

        [Permiso(Permiso = RolesPermisos.OYM_plantillas_puedeVerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPlantilla tblPlantilla = db.tblPlantilla.Find(id);
            if (tblPlantilla == null)
            {
                return HttpNotFound();
            }
            return View(tblPlantilla);
        }

        [Permiso(Permiso = RolesPermisos.OYM_plantillas_puedeCrearNuevo)]
        public ActionResult Create()
        {
            ViewModels.Plantilla plantilla = new ViewModels.Plantilla();

            plantilla.tblPlantilla = new tblPlantilla();
            plantilla.tblPlantillaMarcadores = new List<tblPlantillaMarcador>();
            plantilla.ItemPlantillaEliminados = new List<tblPlantillaMarcador>();
            
            Session["vPlantilla"] = plantilla;


            //ViewBag.lFormatoNumeracion_id = new SelectList(db.FormatoNumeracionPlantilla, "lFormatoNumeracion_id", "sDescripcion");
            //ViewBag.lLugarNumeracion = new SelectList(db.LugarNumeracionPlantilla, "lLugarNumeracion_id", "sDescripcion");
            //ViewBag.lUbicacion_id = new SelectList(db.UbicacionPlantilla, "lUbicacion_id", "sDescripcion");
            return View(new tblPlantilla());
        }

        // POST: tblPlantillas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(tblPlantilla tblPlantilla, HttpPostedFileBase Plantillaupload)
        {
            if (ModelState.IsValid)
            {
                var vPlantilla = (ViewModels.Plantilla)Session["vPlantilla"];

                tblPlantilla.iEstado_fl = "1";
                tblPlantilla.iEliminado_fl = "1";
                tblPlantilla.sCreated_by = DateTime.Now.ToString();
                tblPlantilla.iConcurrencia_id = 1;
                tblPlantilla.lUsuario_id = FrontUser.Get().iUsuario_id;

                if (Plantillaupload != null && Plantillaupload.ContentLength > 0)
                {
                    string strDate = DateTime.Now.ToString("ddMMyyyyHHmmss");
                    string fileName = string.Format("{0}_{1}", strDate, System.IO.Path.GetFileName(Plantillaupload.FileName));
                    //string pathFile = string.Format("{0}/Adjunto/Plantillas/{1}", Server.MapPath("~"), fileName);
                    string pathFile = string.Format(@"{0}\Plantillas\{1}", FilePahtRoot(), fileName);

                    //tblPlantilla.sPathFormato = string.Format("Adjunto/Plantillas/{0}", fileName);
                    tblPlantilla.sPathFormato = string.Format(@"Plantillas\{0}", fileName);
                    Plantillaupload.SaveAs(pathFile);
                }

                db.tblPlantilla.Add(tblPlantilla);
                await db.SaveChangesAsync();


                foreach (var item in vPlantilla.tblPlantillaMarcadores)
                {
                    var detalle = new tblPlantillaMarcador
                    {
                        lPlantillaMarcador_id = item.lPlantillaMarcador_id,
                        lDocumentoCaracteristica_id = item.lDocumentoCaracteristica_id,
                        sMarcador = item.sMarcador                        
                    };

                    if (detalle.lPlantillaMarcador_id < 0)
                    {
                        detalle.lPlantilla_id = tblPlantilla.lPlantilla_id;
                        db.tblPlantillaMarcador.Add(detalle);
                    }
                    else
                    {
                        db.Entry(detalle).State = EntityState.Modified;
                    }

                }

                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(tblPlantilla);
        }

        [Permiso(Permiso = RolesPermisos.OYM_plantillas_puedeEditar)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var plantilla = await db.tblPlantilla
                        .Where(p => p.lPlantilla_id == id)
                        .SingleAsync();

            if (plantilla == null)
            {
                return HttpNotFound();
            }

            ViewModels.Plantilla vmplantilla = new ViewModels.Plantilla();

            vmplantilla.tblPlantilla = plantilla;
            vmplantilla.tblPlantillaMarcadores = db.tblPlantillaMarcador.Include(t => t.tblDocumentoCaracteristica).Where(t => t.lPlantilla_id == id).ToList();
            vmplantilla.ItemPlantillaEliminados = new List<tblPlantillaMarcador>();

            Session["vPlantilla"] = vmplantilla;

            return View(plantilla);
        }

        // POST: tblPlantillas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(tblPlantilla tblPlantilla, HttpPostedFileBase Plantillaupload)
        {
            if (ModelState.IsValid)
            {
                var vPlantilla = (ViewModels.Plantilla)Session["vPlantilla"];
                var plantilla = db.tblPlantilla.FirstOrDefault(p => p.lPlantilla_id == tblPlantilla.lPlantilla_id);

                plantilla.Nombre = tblPlantilla.Nombre;
                plantilla.Descripcion = tblPlantilla.Descripcion;                                
                                
                if (Plantillaupload != null && Plantillaupload.ContentLength > 0)
                {
                    string strDate = DateTime.Now.ToString("ddMMyyyyHHmmss");
                    string fileName = string.Format("{0}_{1}", strDate, System.IO.Path.GetFileName(Plantillaupload.FileName));
                    string pathFile = string.Format("{0}/Areas/OYM/Adjunto/Plantillas/{1}", Server.MapPath("~"), fileName);
                    //string pathFile = string.Format(@"{0}\Plantillas\{1}", FilePahtRoot(), fileName);

                    plantilla.sPathFormato = string.Format("Areas/OYM/Adjunto/Plantillas/{0}", fileName);
                    //plantilla.sPathFormato = string.Format(@"Plantillas\{0}", fileName);
                    Plantillaupload.SaveAs(pathFile);
                }

                db.Entry(plantilla).State = EntityState.Modified;
                await db.SaveChangesAsync();

                foreach (var item in vPlantilla.tblPlantillaMarcadores)
                {
                    var detalle = new tblPlantillaMarcador
                    {
                        lPlantillaMarcador_id = item.lPlantillaMarcador_id,
                        lDocumentoCaracteristica_id = item.lDocumentoCaracteristica_id,
                        sMarcador = item.sMarcador,
                        lPlantilla_id = item.lPlantilla_id                        
                    };

                    if (detalle.lPlantillaMarcador_id < 0)
                    {
                        detalle.lPlantilla_id = plantilla.lPlantilla_id;
                        db.tblPlantillaMarcador.Add(detalle);
                    }
                    else
                    {
                        db.Entry(detalle).State = EntityState.Modified;
                    }

                }

                await db.SaveChangesAsync();

                foreach (var item in vPlantilla.ItemPlantillaEliminados)
                {
                    tblPlantillaMarcador tblPlantillaMarcador = db.tblPlantillaMarcador.Find(item.lPlantillaMarcador_id);
                    db.tblPlantillaMarcador.Remove(tblPlantillaMarcador);
                }

                await db.SaveChangesAsync();


                return RedirectToAction("Index");
            }
            //ViewBag.lFormatoNumeracion_id = new SelectList(db.FormatoNumeracionPlantilla, "lFormatoNumeracion_id", "sDescripcion", tblPlantilla.lFormatoNumeracion_id);
            //ViewBag.lLugarNumeracion = new SelectList(db.LugarNumeracionPlantilla, "lLugarNumeracion_id", "sDescripcion", tblPlantilla.lLugarNumeracion);
            //ViewBag.lUbicacion_id = new SelectList(db.UbicacionPlantilla, "lUbicacion_id", "sDescripcion", tblPlantilla.lUbicacion_id);
            return View(tblPlantilla);
        }

        [Permiso(Permiso = RolesPermisos.OYM_plantillas_puedeEliminar)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPlantilla tblPlantilla = await db.tblPlantilla.FirstOrDefaultAsync(p => p.lPlantilla_id == id);
            if (tblPlantilla == null)
            {
                return HttpNotFound();
            }
            return View(tblPlantilla);            
        }

        // POST: tblPlantillas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {            
            tblPlantilla tblPlantilla = await db.tblPlantilla.FindAsync(id);
            db.tblPlantilla.Remove(tblPlantilla);
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
