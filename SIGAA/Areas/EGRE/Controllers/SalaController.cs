using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MvcFlash.Core;
using MvcFlash.Core.Extensions;
using SIGAA.Areas.EGRE.Models;
using SIGAA.Commons;
using SIGAA.Etiquetas;

namespace SIGAA.Areas.EGRE.Controllers
{
    [Autenticado]
    public class SalaController : Controller
    {
        private EgresadosContext db = new EgresadosContext();
        private List<gatbl_Salas> PaginarDeDiezEnDiez(int indicePaginaActual)
        {
            var saltar = (indicePaginaActual - 1) * 10;

            List<gatbl_Salas> salasPagina = new List<gatbl_Salas>();
            var cantidadPaginas = (db.Salas.Count() / 10) + 1;
            salasPagina = (from sa in db.Salas
                           orderby sa.sNombre_nm
                           select sa).Skip(saltar).Take(10).ToList();

            return salasPagina;
        }

        //public ActionResult Index(int id = 1)
        //{
        //    gatbl_Salas sala = new gatbl_Salas();
        //    int cantidadPaginas = 0;
        //    List<gatbl_Salas> salas = sala.Paginar(id,5,out cantidadPaginas);
        //    ViewBag.indicePagina = id;
        //    ViewBag.cantidadPaginas = cantidadPaginas;
        //    if (Request.IsAjaxRequest())
        //    {           
        //        return PartialView("_Index", salas);
        //    }

        //    return View(salas);
        //}

        // INDICE Y BUSQUEDA ASINCRONA

        [Permiso(Permiso = RolesPermisos.EGRE_aulaSala_puedeVerIndice)]
        public ActionResult Index(string criterio = null)
        {
            var salas = db.Salas.Where(p => p.iEliminado_fl == 1).ToList();
            var salasFiltradas = salas.Where(p => criterio == null ||
                                             p.sNombre_nm.ToLower().Contains(criterio.ToLower()) ||
                                             p.sUbicacion_desc.ToLower().Contains(criterio.ToLower()) ||
                                             p.sTelefono_desc.ToLower().Contains(criterio.ToLower()) ||
                                             p.sEncargado_nm.ToLower().Contains(criterio.ToLower())
                                             ).ToList();
            var salasFiltradasOrdenadas = salasFiltradas.OrderBy(ef => ef.sNombre_nm);

            if (Request.IsAjaxRequest())
            {
                if (!String.IsNullOrWhiteSpace(criterio))
                {
                    Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + salasFiltradasOrdenadas.Count() + " registros con los criterios especificados.");
                }

                return PartialView("_Index", salasFiltradasOrdenadas);
            }
            if (!String.IsNullOrWhiteSpace(criterio))
            {
                Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + salasFiltradasOrdenadas.Count() + " registros con los criterios especificados.");
            }
            return View(salasFiltradasOrdenadas);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_aulaSala_puedeVerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_Salas Sala = db.Salas.Find(id);
            if (Sala == null)
            {
                return HttpNotFound();
            }
            return View(Sala);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_aulaSala_puedeCrearNuevo)]
        public ActionResult Create()
        {
            gatbl_Salas Sala = new gatbl_Salas();
            return View(Sala);
        }

        //CREAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "iSala_id,sNombre_nm,sUbicacion_desc,sTelefono_desc,sEncargado_nm,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_Salas Sala)
        {
            try
            {
                Sala.iEstado_fl = true;
                Sala.iEliminado_fl = 1;
                Sala.sCreado_by = FrontUser.Get().EmailUtepsa;
                Sala.iConcurrencia_id = 1;

                if (ModelState.IsValid)
                {
                    db.Salas.Add(Sala);
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Guardado correctamente.");
                    return RedirectToAction("Index");
                }
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                return View(Sala);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        [Permiso(Permiso = RolesPermisos.EGRE_aulaSala_puedeEditar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_Salas Sala = db.Salas.Find(id);
            if (Sala == null)
            {
                return HttpNotFound();
            }
            return View(Sala);
        }

        //EDITAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "iSala_id,sNombre_nm,sUbicacion_desc,sTelefono_desc,sEncargado_nm,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_Salas Sala)
        {
            try
            {
                Sala.iEliminado_fl = 1;
                Sala.sCreado_by = FrontUser.Get().EmailUtepsa;
                Sala.iConcurrencia_id += 1;

                if (ModelState.IsValid)
                {
                    db.Entry(Sala).State = EntityState.Modified;
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Modificado correctamente.");
                    return RedirectToAction("Index");
                }
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                return View(Sala);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        [Permiso(Permiso = RolesPermisos.EGRE_aulaSala_puedeEliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_Salas Sala = db.Salas.Find(id);
            if (Sala == null)
            {
                return HttpNotFound();
            }
            return View(Sala);
        }

        //ELIMINAR POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var transaccion = db.Database.BeginTransaction())
            {
                try
                {
                    gatbl_Salas Sala = db.Salas.Find(id);
                    Sala.iEstado_fl = false;
                    Sala.iEliminado_fl = 5;
                    Sala.sCreado_by = FrontUser.Get().EmailUtepsa;
                    Sala.iConcurrencia_id += 5;

                    db.Entry(Sala).State = EntityState.Modified;
                    db.SaveChanges();

                    db.Salas.Remove(Sala);
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Eliminado correctamente.");
                    transaccion.Commit();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Flash.Instance.Error("ERROR:    ", "No se pudo Eliminar el dato, ha ocurrido el siguiente error: " + ex.Message);
                    transaccion.Rollback();
                    return RedirectToAction("Index");
                }
            }
        }


        //TEREAS
        private enum Tarea
        {
            NUEVO,
            EDITAR,
            ELIMINAR
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
