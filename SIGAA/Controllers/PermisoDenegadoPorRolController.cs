using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIGAA.Models;
using SIGAA.Commons;
using MvcFlash.Core;
using MvcFlash.Core.Extensions;
using SIGAA.Etiquetas;

namespace SIGAA.Controllers
{
    [Autenticado]
    public class PermisoDenegadoPorRolController : Controller
    {
        private SeguridadContext db = new SeguridadContext();

        [Permiso(Permiso = RolesPermisos.SEGU_permisoDenegadoPorRol_puedeVerIndice)]
        public ActionResult Index(string criterio = null)
        {
            var pdr = db.PermisoDenegadoPorRol.Where(p => p.iEliminado_fl == 1).Include(g => g.Rol).ToList();
            var pdrFil = pdr.Where(t => criterio == null ||
                                   t.Rol.Nombre.ToLower().Contains(criterio.ToLower()) ||
                                   t.Permiso.Modulo.ToLower().Contains(criterio.ToLower()) ||
                                   t.Permiso.Descripcion.ToString().ToLower().Contains(criterio.ToLower())
                                   ).ToList();
            var pdrFilFilOr = pdrFil.OrderBy(ef => ef.Rol.Nombre);
            if (Request.IsAjaxRequest())
            {
                if (!String.IsNullOrWhiteSpace(criterio))
                {
                    Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + pdrFilFilOr.Count() + " registros con los criterios especificados.");
                }
                return PartialView("_Index", pdrFilFilOr);
            }

            if (!String.IsNullOrWhiteSpace(criterio))
            {
                Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + pdrFilFilOr.Count() + " registros con los criterios especificados.");
            }
            return View(pdrFilFilOr);
        }

        [Permiso(Permiso = RolesPermisos.SEGU_permisoDenegadoPorRol_puedeVerDetalle)]
        public ActionResult Details(int Rol_id = 0, int Permiso_id = 0)
        {
            
            if (Rol_id == 0 && Permiso_id == 0)
            {
                Flash.Instance.Error("ERROR", "El paramettro Rol_id ó Permiso_id  no puede ser nulo");
                return RedirectToAction("Index");
            }
            RolesPermisos permiso = (RolesPermisos)Enum.ToObject(typeof(RolesPermisos), Permiso_id);//convierte a un enum en base a su ID
            PermisoDenegadoPorRol permisoDenegadoPorRol = db.PermisoDenegadoPorRol.Find(Rol_id, permiso);
            if (permisoDenegadoPorRol == null)
            {
                Flash.Instance.Error("ERROR", "No existe un permiso dengado por rol con Ids"+Rol_id+" ,"+Permiso_id);
                return RedirectToAction("Index");
            }
            return View(permisoDenegadoPorRol);
        }

        [Permiso(Permiso = RolesPermisos.SEGU_permisoDenegadoPorRol_puedeCrearNuevo)]
        public ActionResult Create()
        {
            PermisoDenegadoPorRol pdr = new PermisoDenegadoPorRol();
            Rol rol = db.Rol.FirstOrDefault();

            //List<PermisoDenegadoPorRol> pdrs = db.PermisoDenegadoPorRol.Where(pd => pd.iRol_id == rol.iRol_id).ToList();
            //List<Permiso> permisos = db.Permiso.ToList();

            //var permisoFiltradoPorRol = (from perm in permisos
            //                             where !pdrs.Any(m => m.iPermiso_id == perm.iPermiso_id)
            //                             select perm
            //                             ).ToList();
            var permisoFiltrado = filtrarPermisoDenegadoPorRol(rol.iRol_id);

            ViewBag.iPermiso_id = new SelectList(permisoFiltrado, "iPermiso_id", "Descripcion");
            ViewBag.iRol_id = new SelectList(db.Rol, "iRol_id", "Nombre");
            return View(pdr);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "iRol_id,iPermiso_id,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] PermisoDenegadoPorRol permisoDenegadoPorRol)
        {
            try
            {
                permisoDenegadoPorRol.iEstado_fl = true;
                permisoDenegadoPorRol.iEliminado_fl = 1;
                permisoDenegadoPorRol.sCreado_by = FrontUser.Get().EmailUtepsa;
                permisoDenegadoPorRol.iConcurrencia_id = 1;

                if (ModelState.IsValid)
                {
                    db.PermisoDenegadoPorRol.Add(permisoDenegadoPorRol);
                    db.SaveChanges();
                    ModelState.Clear();
                    permisoDenegadoPorRol = null;
                    Flash.Instance.Success("CORRECTO", "El dato se ha guradado correctamente.");
                    return RedirectToAction("Index");
                }

                ViewBag.iPermiso_id = new SelectList(db.Permiso, "iPermiso_id", "Descripcion", permisoDenegadoPorRol.iPermiso_id);
                ViewBag.iRol_id = new SelectList(db.Rol, "iRol_id", "Nombre", permisoDenegadoPorRol.iRol_id);
                Flash.Instance.Error("ERROR", "No se pudo Guardar el dato porque ha ocurrido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                return View(permisoDenegadoPorRol);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR", "No se pudo Guardar el dato porque ha ocurrido el siguiente error " + ex.Message);
                return RedirectToAction("Index");
            }            
        }

        [Permiso(Permiso = RolesPermisos.SEGU_permisoDenegadoPorRol_puedeEditar)]
        public ActionResult Edit(int Rol_id = 0, int Permiso_id = 0)
        {
            if (Rol_id == 0 && Permiso_id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest,"LOS PARAMETROS NO SON CORRECTOS");
            }
            RolesPermisos permiso = (RolesPermisos)Enum.ToObject(typeof(RolesPermisos), Permiso_id);//convierte a un enum en base a su ID
            PermisoDenegadoPorRol permisoDenegadoPorRol = db.PermisoDenegadoPorRol.Find(Rol_id, permiso);
            if (permisoDenegadoPorRol == null)
            {
                return HttpNotFound();
            }

            Session["permisoId"] = (int)Permiso_id;//envio a session para luego remplazar por el nuevo valor
            //List<PermisoDenegadoPorRol> pdrs = db.PermisoDenegadoPorRol.Where(pd => pd.iRol_id == Rol_id).ToList();
            //List<Permiso> permisos = db.Permiso.ToList();

            //var permisoFiltradoPorRol = (from perm in permisos
            //                             where !pdrs.Any(m => m.iPermiso_id == perm.iPermiso_id)
            //                             select perm
            //                             ).ToList();

            var permisoFiltrados = filtrarPermisoDenegadoPorRol(Rol_id);

            ViewBag.iPermiso_id = new SelectList(permisoFiltrados, "iPermiso_id", "Descripcion", permisoDenegadoPorRol.iPermiso_id);
            ViewBag.iRol_id = new SelectList(db.Rol, "iRol_id", "Nombre", Rol_id);
            return View(permisoDenegadoPorRol);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "iRol_id,iPermiso_id,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] PermisoDenegadoPorRol permisoDenegadoPorRol)
        {
            try
            {
                int permisoIDanterior = (int)Session["permisoId"];
                RolesPermisos permisoAnterior = (RolesPermisos)Enum.ToObject(typeof(RolesPermisos), permisoIDanterior);//convierte a un enum en base a su ID
                if (ModelState.IsValid)
                {
                    var PermisosDenegos = db.PermisoDenegadoPorRol.Where(a => a.iRol_id == permisoDenegadoPorRol.iRol_id && a.iPermiso_id == permisoAnterior).ToList();

                    foreach (var s in PermisosDenegos)
                       {
                            db.PermisoDenegadoPorRol.Remove(s);
                       }
                    permisoDenegadoPorRol.iEliminado_fl = 1;
                    permisoDenegadoPorRol.sCreado_by = FrontUser.Get().EmailUtepsa;
                    permisoDenegadoPorRol.iConcurrencia_id += 1;

                    db.PermisoDenegadoPorRol.Add(permisoDenegadoPorRol);
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (Exception en)
                        {
                        Flash.Instance.Error("ERROR", "No se pudo Modificar el dato proque ha ocurrido el siguiente error: " + en.Message);
                        return RedirectToAction("Index");
                        }

                    //db.Entry(permisoDenegadoPorRol).State = EntityState.Modified;
                    //db.SaveChanges();
                    Flash.Instance.Success("CORRECTO", "El dato ha sido Modificado correctamente.");
                    return RedirectToAction("Index");
                }
                ViewBag.iPermiso_id = new SelectList(db.Permiso, "iPermiso_id", "Descripcion", permisoDenegadoPorRol.iPermiso_id);
                ViewBag.iRol_id = new SelectList(db.Rol, "iRol_id", "Nombre", permisoDenegadoPorRol.iRol_id);
                Flash.Instance.Error("ERROR", "No se pudo Modificar el dato porque ha ocurrido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                return View(permisoDenegadoPorRol);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR", "No se pudo Modificar el dato proque ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }           
        }

        [Permiso(Permiso = RolesPermisos.SEGU_permisoDenegadoPorRol_puedeEliminar)]
        public ActionResult Delete(int Rol_id = 0, int Permiso_id = 0)
        {
            if (Rol_id == 0 && Permiso_id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "LOS PARAMETROS NO SON CORRECTOS");
            }
            RolesPermisos permiso = (RolesPermisos)Enum.ToObject(typeof(RolesPermisos), Permiso_id);//convierte a un enum en base a su ID
            PermisoDenegadoPorRol permisoDenegadoPorRol = db.PermisoDenegadoPorRol.Find(Rol_id, permiso);
            if (permisoDenegadoPorRol == null)
            {
                return HttpNotFound();
            }
            return View(permisoDenegadoPorRol);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int Rol_id = 0, int Permiso_id = 0)
        {
            using (var transaccion = db.Database.BeginTransaction()) {
                try
                {
                    RolesPermisos permiso = (RolesPermisos)Enum.ToObject(typeof(RolesPermisos), Permiso_id);//convierte a un enum en base a su ID
                    PermisoDenegadoPorRol permisoDenegadoPorRol = db.PermisoDenegadoPorRol.Find(Rol_id, permiso);
                    permisoDenegadoPorRol.iEstado_fl = false;
                    permisoDenegadoPorRol.iEliminado_fl = 2;
                    permisoDenegadoPorRol.sCreado_by = FrontUser.Get().EmailUtepsa;
                    permisoDenegadoPorRol.iConcurrencia_id += 1;

                    db.Entry(permisoDenegadoPorRol).State = EntityState.Modified;
                    db.SaveChanges();

                    db.PermisoDenegadoPorRol.Remove(permisoDenegadoPorRol);
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO", "El dato ha sido eliminado correctamente.");
                    transaccion.Commit();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Flash.Instance.Error("ERROR", "No se pudo eliminar el dato porque ha ocurrido el siguiente error: " + ex.Message);
                    transaccion.Rollback();
                    return RedirectToAction("Index");
                }
            }
        }

        private List<Permiso> filtrarPermisoDenegadoPorRol(int rol_id) {
            List<Permiso> resultado = new List<Permiso>();
            List<Permiso> permisosTodos = db.Permiso.ToList();
            if (rol_id > 0) { 
                var pdrs = db.PermisoDenegadoPorRol.Where(pd => pd.iRol_id == rol_id).ToList();
                if (pdrs.Count > 0)
                {
                    resultado = (from perm in permisosTodos
                                 where !pdrs.Any(m => m.iPermiso_id == perm.iPermiso_id)
                                 select perm).ToList();
                }
                else {
                    resultado = (from perm in permisosTodos
                                 select perm
                                                 ).ToList();
                }
            }
            return resultado;
        }

        [HttpPost]
        public JsonResult PermisosPorRol(int rol_id)
        {
            var permisos = filtrarPermisoDenegadoPorRol(rol_id).Select(a => new { iPermiso_id = a.iPermiso_id.ToString(), Descripcion = a.Descripcion } ).ToList();
            //var resultado = (from perm in permisos
            //                 select new { iPermiso_id = perm.iPermiso_id.ToString(), Descripcion=perm.Descripcion }).ToList();
            return Json(permisos, JsonRequestBehavior.AllowGet);
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
