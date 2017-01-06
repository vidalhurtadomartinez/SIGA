using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIGAA.Models;
using MvcFlash.Core.Extensions;
using MvcFlash.Core;
using SIGAA.Commons;
using SIGAA.ViewModels;
using SIGAA.Etiquetas;

namespace SIGAA.Controllers
{
    [Autenticado]
    public class UsuarioController : Controller
    {
        private SeguridadContext db = new SeguridadContext();

        [Permiso(Permiso = RolesPermisos.SEGU_usuario_puedeVerIndice)]
        public ActionResult Index(string criterio = null)
        {
            var usu = db.Usuario.Where(p => p.iEliminado_fl == 1).Include(g => g.Rol).ToList();
            var usuFil = usu.Where(t => criterio == null ||
                                   t.agd_codigo.ToLower().Contains(criterio.ToLower()) ||
                                   t.usr_login.ToString().ToLower().Contains(criterio.ToLower()) ||
                                   t.dtFechaVigencia.ToString().Contains(criterio.ToLower()) ||
                                   t.Rol.Nombre.ToLower().Contains(criterio.ToLower())
                                   ).ToList();
            var usuFilOr = usuFil.OrderBy(ef => ef.usr_login);
            if (Request.IsAjaxRequest())
            {
                if (!String.IsNullOrWhiteSpace(criterio))
                {
                    Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + usuFilOr.Count() + " registros con los criterios especificados.");
                }
                return PartialView("_Index", usuFilOr);
            }

            if (!String.IsNullOrWhiteSpace(criterio))
            {
                Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + usuFilOr.Count() + " registros con los criterios especificados.");
            }
            return View(usuFilOr);
        }

        [Permiso(Permiso = RolesPermisos.SEGU_usuario_puedeVerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                Flash.Instance.Error("ERROR", "El paramettro Id no puede ser nulo");
                return RedirectToAction("Index");
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                Flash.Instance.Warning("ERROR", "No Existe Usuario con ID "+id);
                return RedirectToAction("Index");
            }
            ViewBag.datos = new { nombreCompleto = usuario.Persona.NombreCompleto };
            return View(usuario);
        }

        [Permiso(Permiso = RolesPermisos.SEGU_usuario_puedeCrearNuevo)]
        public ActionResult Create()
        {
            Usuario usu = new Usuario();
            ViewBag.iRol_id = new SelectList(db.Rol, "iRol_id", "Nombre",usu.iRol_id);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "iUsuario_id,agd_codigo,usr_login,Contrasena,dtFechaVigencia,sObservacion,iRol_id,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] Usuario usuario)
        {
            try
            {
                usuario.iEstado_fl = true;
                usuario.iEliminado_fl = 1;
                usuario.sCreado_by = FrontUser.Get().usr_login;
                usuario.iConcurrencia_id = 1;

                if (ModelState.IsValid)
                {
                    db.Usuario.Add(usuario);
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO", "El dato se ha guradado correctamente.");
                    return RedirectToAction("Index");
                }
                Flash.Instance.Error("ERROR", "No se pudo Guardar el dato porque ha ocurrido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                ViewBag.iRol_id = new SelectList(db.Rol, "iRol_id", "Nombre", usuario.iRol_id);
                return View(usuario);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR", "No se pudo Guardar el dato porque ha ocurrido el siguiente error "+ex.Message);
                return RedirectToAction("Index");
            }         
        }

        [Permiso(Permiso = RolesPermisos.SEGU_usuario_puedeEditar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            ViewBag.datos = new { nombreCompleto = usuario.Persona.NombreCompleto };
            ViewBag.iRol_id = new SelectList(db.Rol, "iRol_id", "Nombre", usuario.iRol_id);
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "iUsuario_id,agd_codigo,usr_login,Contrasena,dtFechaVigencia,sObservacion,iRol_id,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] Usuario usuario)
        {
            try
            {
                usuario.iEliminado_fl = 1;
                usuario.sCreado_by = FrontUser.Get().usr_login;
                usuario.iConcurrencia_id += 1;

                if (ModelState.IsValid)
                {
                    db.Entry(usuario).State = EntityState.Modified;
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO", "El dato ha sido Modificado correctamente.");
                    return RedirectToAction("Index");
                }
                ViewBag.datos = new { nombreCompleto = usuario.Persona.NombreCompleto };
                ViewBag.iRol_id = new SelectList(db.Rol, "iRol_id", "Nombre", usuario.iRol_id);
                Flash.Instance.Error("ERROR", "No se pudo Modificar el dato porque ha ocurrido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                return View(usuario);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR", "No se pudo Modificar el dato proque ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }          
        }

        //// GET: Usuario/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Usuario usuario = db.Usuario.Find(id);
        //    if (usuario == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(usuario);
        //}

        //// POST: Usuario/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    using (var transaccion = db.Database.BeginTransaction()) {
        //        try
        //        {
        //            Usuario usuario = db.Usuario.Find(id);
        //            usuario.iEstado_fl = false;
        //            usuario.iEliminado_fl = 2;
        //            usuario.sCreado_by = FrontUser.Get().usr_login;
        //            usuario.iConcurrencia_id += 1;

        //            db.Entry(usuario).State = EntityState.Modified;
        //            db.SaveChanges();

        //            db.Usuario.Remove(usuario);
        //            db.SaveChanges();
        //            Flash.Instance.Success("CORRECTO", "El dato ha sido eliminado correctamente.");
        //            transaccion.Commit();
        //            return RedirectToAction("Index");
        //        }
        //        catch (Exception ex)
        //        {
        //            Flash.Instance.Error("ERROR", "No se pudo eliminar el dato porque ha ocurrido el siguiente error: "+ex.Message);
        //            transaccion.Rollback();
        //            return RedirectToAction("Index");
        //        }
        //    }       
        //}


        [Permiso(Permiso = RolesPermisos.SEGU_usuario_puedeCambiarContrasena)]
        [HttpGet]
        public ActionResult CambioDeContrasena() {
            CambioContrasenaWiewModel usuCambioContra = new CambioContrasenaWiewModel();
            int idUsuario = FrontUser.Get().iUsuario_id;
            var usu= db.Usuario.Find(idUsuario);
            if (usu == null) {
                return HttpNotFound("NO EXISTE USUARIO CON EL CODIGO ENVIADO");
            }
            ViewBag.datos = new { nombreCompleto = usu.Persona.NombreCompleto };
            usuCambioContra.iUsuario_id = usu.iUsuario_id;
            usuCambioContra.usr_login = usu.usr_login;
            return View(usuCambioContra);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CambioDeContrasena(CambioContrasenaWiewModel usuCambioContra)
        {
            //verificamos que la cuenta exista y corresponda al usuario
            var usuCambia = db.Usuario.Where(e => e.usr_login.Equals(usuCambioContra.usr_login) && e.Contrasena.Equals(usuCambioContra.ContrasenaActual) && usuCambioContra.ContrasenaNueva.Equals(usuCambioContra.ConfirmaContrasenaNueva)).Include(a =>a.Persona).FirstOrDefault();
            if(usuCambia != null) {
                Usuario usuario = new Usuario();
                try
                {
                    usuCambia.iEliminado_fl = 1;
                    usuCambia.sCreado_by = FrontUser.Get().usr_login;
                    usuCambia.iConcurrencia_id += 1;
                    usuCambia.Contrasena = usuCambioContra.ContrasenaNueva;

                    if (ModelState.IsValid)
                    {
                        db.Entry(usuCambia).State = EntityState.Modified;
                        db.SaveChanges();
                        Flash.Instance.Success("CORRECTO", "El dato ha sido Modificado correctamente.");
                        ViewBag.datos = new { nombreCompleto = usuCambia.Persona.NombreCompleto };
                        return View(usuCambioContra);
                    }                  
                    Flash.Instance.Error("ERROR", "No se puede modificar la Contraseña porque ha ocurrido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                    ViewBag.datos = new { nombreCompleto = usuCambia.Persona.NombreCompleto };
                    return View(usuCambioContra);
                }
                catch (Exception ex)
                {
                    Flash.Instance.Error("ERROR", "No se puede Modificar el dato proque ha ocurrido el siguiente error: " + ex.Message);
                    ViewBag.datos = new { nombreCompleto = usuCambia.Persona.NombreCompleto };
                    return View(usuCambioContra);
                }
            }
            else
            {
                Flash.Instance.Error("ERROR", "No se puede Modificar la contraseña por que los datos proporcionados son incorrectos");
                var user = db.Usuario.Where(u => u.iUsuario_id == usuCambioContra.iUsuario_id).Include(a => a.Persona).FirstOrDefault();
                ViewBag.datos = new { nombreCompleto = user.Persona.NombreCompleto};
                return View(usuCambioContra);
            }
        }

        public JsonResult ListarPersonas_Json(string term) // 
        {
            List<string> personas = new List<string>();
            var personasAgenda = (from a in db.Personas
                                    select new { codigoAgenda = a.agd_codigo, nombreCompletoAgenda = a.agd_nombres.Trim() + " " + a.agd_appaterno.Trim() + " " + a.agd_apmaterno.Trim() }).ToList();

            var personasUsuario = (from usu in db.Usuario
                                          where usu.iEliminado_fl == 1
                                          select new { codigoAgenda = usu.agd_codigo}).ToList();

            var personasNoUsuarios = (from perA in personasAgenda
                                         where !personasUsuario.Any(m => m.codigoAgenda == perA.codigoAgenda)                                         
                                         select new { codigoAgenda = perA.codigoAgenda, nombreComleto = perA.nombreCompletoAgenda, }).ToList();

            personas = (from p in personasNoUsuarios
                        where p.nombreComleto.ToLower().Contains(term.ToLower())
                        select p.nombreComleto).Take(10).ToList();

            return Json(personas, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TraerInformacionDePersona_Json(string term)
        {
            var infoAlumno = TraerInformacionDePersona(term);
            return Json(infoAlumno, JsonRequestBehavior.AllowGet);
        }

        private Object TraerInformacionDePersona(string id)
        {
            var resultado = (from p in db.Personas                         
                             select new
                             {
                                 codigoAgenda = p.agd_codigo,
                                 nombreCompleto = p.agd_nombres.Trim() + " " + p.agd_appaterno.Trim() + " " + p.agd_apmaterno.Trim(),
                                 usr_login = p.usr_login.Trim(),
                             }).ToList();

            var result = (from r in resultado
                          where r.nombreCompleto.ToLower().Contains(id.ToLower())
                          select r).FirstOrDefault();

            return result;
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
