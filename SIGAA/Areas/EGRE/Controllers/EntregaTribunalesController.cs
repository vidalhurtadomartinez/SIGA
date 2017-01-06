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
    public class EntregaTribunalesController : Controller
    {
        private EgresadosContext db = new EgresadosContext();

        [Permiso(Permiso = RolesPermisos.EGRE_entregaTFGalTribunal_puedeVerIndice)]
        public ActionResult Index(string criterio = null)
        {
            var entregasTribunales = db.EntregasTribunales.Where(p => p.iEliminado_fl == 1).Include(g => g.Alumno).Include(g => g.EntregaTFG).Include(g => g.Persona).Include(g => g.Tutor).ToList();
            var entragasTribunalesFiltrados = entregasTribunales.Where(e => criterio == null ||
                                                                        e.Alumno.Persona.NombreCompleto.ToLower().Contains(criterio.ToLower()) ||
                                                                        e.lEstudiante_id.Contains(criterio.ToLower()) ||
                                                                        e.Tutor.sNombre_desc.ToLower().Contains(criterio.ToLower()) ||
                                                                        e.sPeriodo_desc.ToLower().Contains(criterio.ToLower()) ||
                                                                        e.dtEntregaTribunal_dt.ToString().Contains(criterio.ToLower()) ||
                                                                        e.sEntregaTribunal_fl.ToString().ToLower().Contains(criterio.ToLower()) ||
                                                                        e.Persona.NombreCompleto.ToLower().Contains(criterio.ToLower())
                                                                        ).ToList();
            var entregasTFGFiltradasOrdenadas = entragasTribunalesFiltrados.OrderBy(ef => ef.Alumno.Persona.NombreCompleto).ThenBy(ef => ef.dtEntregaTribunal_dt);

            if (Request.IsAjaxRequest())
            {
                if (!String.IsNullOrWhiteSpace(criterio))
                {
                    Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + entregasTFGFiltradasOrdenadas.Count() + " registros con los criterios especificados.");
                }
                return PartialView("_Index", entregasTFGFiltradasOrdenadas);
            }

            if (!String.IsNullOrWhiteSpace(criterio))
            {
                Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + entregasTFGFiltradasOrdenadas.Count() + " registros con los criterios especificados.");
            }
            return View(entregasTFGFiltradasOrdenadas);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_entregaTFGalTribunal_puedeVerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_EntregaTribunales EntregaTribunal = db.EntregasTribunales.Find(id);
            if (EntregaTribunal == null)
            {
                return HttpNotFound();
            }
            var informacionAlumno = TraerInformacionDeAlumnoPorEntrega_id(EntregaTribunal.iEntrega_id);
            ViewBag.datos = informacionAlumno;
            return View(EntregaTribunal);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_entregaTFGalTribunal_puedeCrearNuevo)]
        public ActionResult Create()
        {
            gatbl_EntregaTribunales EntregaTribunal = new gatbl_EntregaTribunales();
            CargarViewBags(Tarea.NUEVO, EntregaTribunal);
            return View(EntregaTribunal);
        }

        //CREAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "iETribunal_id,sGestion_desc,sPeriodo_desc,sEntregaTribunal_fl,dtEntregaTribunal_dt,iTutuor_id,iEntrega_id,lEstudiante_id,lRecepciona_id,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_EntregaTribunales EntregaTribunal)
        {
            try
            {
                EntregaTribunal.iEstado_fl = true;
                EntregaTribunal.iEliminado_fl = 1;
                EntregaTribunal.sCreado_by = FrontUser.Get().usr_login;
                EntregaTribunal.iConcurrencia_id = 1;

                if (ModelState.IsValid)
                {
                    db.EntregasTribunales.Add(EntregaTribunal);
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Guardado correctamente.");
                    return RedirectToAction("Index");
                }
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                CargarViewBags(Tarea.NUEVO, EntregaTribunal);
                return View(EntregaTribunal);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, ha ocurrido el siguiente error: " + ex.InnerException.InnerException.Message);
                return RedirectToAction("Index");
            }
        }

        [Permiso(Permiso = RolesPermisos.EGRE_entregaTFGalTribunal_puedeEditar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_EntregaTribunales EntregaTribunal = db.EntregasTribunales.Find(id);
            if (EntregaTribunal == null)
            {
                return HttpNotFound();
            }
            var informacionAlumno = TraerInformacionDeAlumnoPorEntrega_id(EntregaTribunal.iEntrega_id);
            ViewBag.datos = informacionAlumno;
            CargarViewBags(Tarea.EDITAR, EntregaTribunal);
            return View(EntregaTribunal);
        }

        //EDITAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "iETribunal_id,sGestion_desc,sPeriodo_desc,sEntregaTribunal_fl,dtEntregaTribunal_dt,iTutuor_id,iEntrega_id,lEstudiante_id,lRecepciona_id,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_EntregaTribunales EntregaTribunal)
        {
            try
            {
                EntregaTribunal.iEliminado_fl = 1;
                EntregaTribunal.sCreado_by = FrontUser.Get().usr_login;
                EntregaTribunal.iConcurrencia_id += 1;

                if (ModelState.IsValid)
                {
                    db.Entry(EntregaTribunal).State = EntityState.Modified;
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Modificado correctamente.");
                    return RedirectToAction("Index");
                }
                var informacionAlumno = TraerInformacionDeAlumnoPorEntrega_id(EntregaTribunal.iEntrega_id);
                ViewBag.datos = informacionAlumno;
                CargarViewBags(Tarea.EDITAR, EntregaTribunal);
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                return View(EntregaTribunal);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        [Permiso(Permiso = RolesPermisos.EGRE_entregaTFGalTribunal_puedeEliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_EntregaTribunales EntregaTribunal = db.EntregasTribunales.Find(id);
            if (EntregaTribunal == null)
            {
                return HttpNotFound();
            }
            var informacionAlumno = TraerInformacionDeAlumnoPorEntrega_id(EntregaTribunal.iEntrega_id);
            ViewBag.datos = informacionAlumno;
            return View(EntregaTribunal);
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
                    gatbl_EntregaTribunales EntregaTribunal = db.EntregasTribunales.Find(id);
                    EntregaTribunal.iEstado_fl = false;
                    EntregaTribunal.iEliminado_fl = 2;
                    EntregaTribunal.sCreado_by = FrontUser.Get().usr_login;
                    EntregaTribunal.iConcurrencia_id += 1;

                    db.Entry(EntregaTribunal).State = EntityState.Modified;
                    db.SaveChanges();

                    db.EntregasTribunales.Remove(EntregaTribunal);
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

        //BUSCA INFORMACION EN BASE A UN CRITERIO
        public ActionResult BuscarInformacionDeAlumno(string term = "")
        {
            int id = 0;
            id = int.Parse(term);
            var infoAlumno = TraerInformacionDeAlumnoPorEntrega_id(id);
            return Json(infoAlumno, JsonRequestBehavior.AllowGet);
        }

        //CARGA INFORMACION DE ALUMNO POR DETERMINADO ID
        private Object TraerInformacionDeAlumnoPorEntrega_id(int id)
        {
            var result = (from e in db.EntregasTFG
                          join p in db.Perfiles on e.iPerfil_id equals p.iPerfil_id
                          join al in db.Alumnos on p.lEstudiante_id equals al.alm_registro
                          join per in db.Personas on al.agd_codigo equals per.agd_codigo
                          join c in db.Carreras on al.crr_codigo equals c.crr_codigo
                          join f in db.Facultades on c.sca_codigo equals f.sca_codigo
                          where e.iEntrega_id == id
                          select new { nombreAlumno = per.agd_nombres.Trim() + " " + per.agd_appaterno.Trim() + " " + per.agd_apmaterno.Trim(), carrera = c.crr_descripcion.Trim(), facultad = f.sca_descripcion.Trim(), TituloPerfil = p.sTitulo_tfg, TipoGraduacion = p.TipoGraduacion.tttg_descripcion.Trim(), registro = al.alm_registro, estadoRecepcionDelAlumno = e.sEntrega_fl.ToString(), ejemplar = e.sNumEjemplar.ToString() }).FirstOrDefault();
            return result;
        }

        //TEREAS
        private enum Tarea
        {
            NUEVO,
            EDITAR,
            ELIMINAR
        }

        //CLASE INTERNA
        private class RecepcionDeAlumno
        {
            public int iEntrega_id { get; set; }
            public string nombreAlumno { get; set; }
            public string registro { get; set; }

        }

        //LISTA SELECCIONA ENTREGAS A TRIBUNALES
        private List<RecepcionDeAlumno> SeleccionarLista(Tarea tarea, int id)
        {
            var RecepcionesDeAlumnos = from re in db.EntregasTFG
                                       join p in db.Perfiles on re.iPerfil_id equals p.iPerfil_id
                                       join al in db.Alumnos on p.lEstudiante_id equals al.alm_registro
                                       join per in db.Personas on al.agd_codigo equals per.agd_codigo
                                       select new { iEntrega_id = re.iEntrega_id, nombreAlumno = per.agd_nombres.Trim() + " " + per.agd_appaterno.Trim() + " " + per.agd_apmaterno.Trim() + " (" + al.alm_registro.Trim() + ")", registro = al.alm_registro };

            var EntregaATribunales = from et in db.EntregasTribunales
                                     join re in db.EntregasTFG on et.iEntrega_id equals re.iEntrega_id
                                     join p in db.Perfiles on re.iPerfil_id equals p.iPerfil_id
                                     join al in db.Alumnos on p.lEstudiante_id equals al.alm_registro
                                     join per in db.Personas on al.agd_codigo equals per.agd_codigo
                                     select new { iEntrega_id = et.iEntrega_id, nombreAlumno = per.agd_nombres.Trim() + " " + per.agd_appaterno.Trim() + " " + per.agd_apmaterno.Trim() + " (" + al.alm_registro.Trim() + ")", registro = al.alm_registro };

            var Recepciones = (from ra in RecepcionesDeAlumnos
                               where !EntregaATribunales.Any(m => m.iEntrega_id == ra.iEntrega_id)
                               select new RecepcionDeAlumno { iEntrega_id = ra.iEntrega_id, nombreAlumno = ra.nombreAlumno, registro = ra.registro }).ToList();
            switch (tarea)
            {
                case Tarea.NUEVO:
                    return Recepciones.ToList();
                case Tarea.EDITAR:
                    {
                        List<RecepcionDeAlumno> RecepcionDeAlumno = new List<RecepcionDeAlumno>();
                        RecepcionDeAlumno = (from et in EntregaATribunales
                                             where et.iEntrega_id == id
                                             select new RecepcionDeAlumno { iEntrega_id = et.iEntrega_id, nombreAlumno = et.nombreAlumno, registro = et.registro }).ToList();
                        return Recepciones.Union(RecepcionDeAlumno).ToList();
                    }
                default:
                    {
                        return Recepciones.ToList();
                    }
            }
        }

        //RETORNA LISTA NOMBRES DE ACUERDO A UN CRITERIO EN FORMATO JSON, ES UTILIZADO PARA EL AUTOCOMPLETE
        public JsonResult ListarNombresDePersonas_Json(string term)
        {
            List<string> nombresPersonas = new List<string>();

            var listaPersonas = (from p in db.Personas
                                 select new { nombrePersona = p.agd_prefijo.Trim() + " " + p.agd_nombres.Trim() + " " + p.agd_appaterno.Trim() + " " + p.agd_apmaterno.Trim() }).ToList();

            nombresPersonas = (from p in listaPersonas
                               where p.nombrePersona.ToLower().Contains(term.ToLower())
                               select p.nombrePersona).Take(10).ToList();

            return Json(nombresPersonas, JsonRequestBehavior.AllowGet);
        }

        //OBTIENE INFORMACION DE PERSONA DE ACUERDO A UN NOMBRE EN FORMATO JSON, UTILIZADO PARA EL EVENTO SELECTED DEL AUTOCOPLETE
        public ActionResult TraerInformacionDePersona_Json(string term)
        {
            var infoAlumno = TraerInformacionDePersonaPorNombre(term);
            return Json(infoAlumno, JsonRequestBehavior.AllowGet);
        }

        //OBTIENE INFORMACION DE PERSONA DE ACUERDO A UN NOMBRE, 
        private Object TraerInformacionDePersonaPorNombre(string nombre)
        {
            var listaPersonas = (from p in db.Personas
                                 select new { codigoPersona = p.agd_codigo, nombrePersona = p.agd_prefijo.Trim() + " " + p.agd_nombres.Trim() + " " + p.agd_appaterno.Trim() + " " + p.agd_apmaterno.Trim() }).ToList();

            var datosPersona = (from f in listaPersonas
                                where f.nombrePersona.ToLower().Contains(nombre.ToLower())
                                select f).FirstOrDefault();
            return datosPersona;
        }

        //OBTIENE INFORMACION DE PERSONA DE ACUERDO A UN NOMBRE, PARA LOS VIEWBAGS DE EDITAR, ELIMININAR, DETALLE
        private Object TraerInformacionDePersonaPorCodigo(string codigo)
        {
            var listaPersonas = (from p in db.Personas
                                 select new { codigoPersona = p.agd_codigo, nombrePersona = p.agd_prefijo.Trim() + " " + p.agd_nombres.Trim() + " " + p.agd_appaterno.Trim() + " " + p.agd_apmaterno.Trim() }).ToList();

            var datosPersona = (from f in listaPersonas
                                where f.codigoPersona.ToLower().Contains(codigo.ToLower())
                                select f).FirstOrDefault();
            return datosPersona;
        }


        //CARGA VIEWBAGS
        private void CargarViewBags(Tarea tarea, gatbl_EntregaTribunales EntregaTribunal)
        {
            var recepcionDeAlumno = SeleccionarLista(tarea, EntregaTribunal.iEntrega_id);
            ViewBag.iEntrega_id = new SelectList(recepcionDeAlumno, "iEntrega_id", "nombreAlumno", EntregaTribunal.iEntrega_id);
            ViewBag.sPeriodo_desc = new SelectList(db.Semestres, "sem_codigo", "sem_codigo", EntregaTribunal.sPeriodo_desc);
            ViewBag.iTutuor_id = new SelectList(db.Tutorez.Where(p => p.iEliminado_fl == 1).OrderBy(p => p.sNombre_desc), "iTutuor_id", "sNombre_desc", EntregaTribunal.iTutuor_id);
            if (tarea == Tarea.EDITAR)
            {
                var receptor = TraerInformacionDePersonaPorCodigo(EntregaTribunal.lRecepciona_id);
                ViewBag.Receptor = receptor;
            }
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
