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
    public class EntregaAlEstController : Controller
    {
        private EgresadosContext db = new EgresadosContext();
        #region METODOS ESTANDAR

        [Permiso(Permiso = RolesPermisos.EGRE_entregaTFGalAlumno_puedeVerIndice)]
        public ActionResult Index(string criterio = null)
        {
            var entregasAlEstudiante = db.EntregasAlEstudiante.Where(p => p.iEliminado_fl == 1).Include(g => g.Alumno).Include(g => g.Persona).Include(g => g.RecepcionTFG).ToList();
            var entregasAlEstudianteFiltrado = entregasAlEstudiante.Where(e => criterio == null ||
                                                                           e.Alumno.Persona.NombreCompleto.ToLower().Contains(criterio.ToLower()) ||
                                                                           e.lEstudiante_id.Contains(criterio.ToLower()) ||
                                                                           e.Persona.NombreCompleto.ToLower().Contains(criterio.ToLower()) ||
                                                                           e.Alumno.Carrera.crr_descripcion.ToLower().Contains(criterio.ToLower()) ||
                                                                           e.dtEntregaAlEst_dt.ToString().Contains(criterio.ToLower()) ||
                                                                           e.RecepcionTFG.EntregaTribunal.Tutor.sNombre_desc.ToLower().Contains(criterio.ToLower()) ||
                                                                           e.sEntregaAlEst_fl.ToString().ToLower().Contains(criterio.ToLower()) ||
                                                                           e.Persona.NombreCompleto.ToLower().Contains(criterio.ToLower())
                                                                            ).ToList();
            var entregasTFGFiltradasOrdenadas = entregasAlEstudianteFiltrado.OrderBy(ef => ef.Alumno.Persona.NombreCompleto).ThenBy(ef => ef.dtEntregaAlEst_dt);

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

        [Permiso(Permiso = RolesPermisos.EGRE_entregaTFGalAlumno_puedeVerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_EntregaAlEst EntregaAlEst = db.EntregasAlEstudiante.Find(id);
            if (EntregaAlEst == null)
            {
                return HttpNotFound();
            }
            var informacionAlumno = TraerInformacionDeAlumnoPorIdRecepcionTFG(EntregaAlEst.iRecepcionTFG_id);
            ViewBag.datos = informacionAlumno;
            return View(EntregaAlEst);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_entregaTFGalAlumno_puedeCrearNuevo)]
        public ActionResult Create()
        {
            gatbl_EntregaAlEst EntregaAlEst = new gatbl_EntregaAlEst();
            CargarViewBags(Tarea.NUEVO, EntregaAlEst);
            return View(EntregaAlEst);
        }

        //CREAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "iEntregaAlEst_id,sGestion_desc,sPeriodo_desc,sEntregaAlEst_fl,dtEntregaAlEst_dt,iRecepcionTFG_id,lEstudiante_id,lEntregadoPor_id,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_EntregaAlEst EntregaAlEst)
        {
            try
            {
                EntregaAlEst.iEstado_fl = true;
                EntregaAlEst.iEliminado_fl = 1;
                EntregaAlEst.sCreado_by = FrontUser.Get().usr_login;
                EntregaAlEst.iConcurrencia_id = 1;

                if (ModelState.IsValid)
                {
                    db.EntregasAlEstudiante.Add(EntregaAlEst);
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO   ", "El dato ha sido Guardado correctamente.");
                    return RedirectToAction("Index");
                }
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                CargarViewBags(Tarea.NUEVO, EntregaAlEst);
                return View(EntregaAlEst);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        [Permiso(Permiso = RolesPermisos.EGRE_entregaTFGalAlumno_puedeEditar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_EntregaAlEst EntregaAlEst = db.EntregasAlEstudiante.Find(id);
            if (EntregaAlEst == null)
            {
                return HttpNotFound();
            }
            var informacionAlumno = TraerInformacionDeAlumnoPorIdRecepcionTFG(EntregaAlEst.iRecepcionTFG_id);
            ViewBag.datos = informacionAlumno;
            CargarViewBags(Tarea.EDITAR, EntregaAlEst);
            return View(EntregaAlEst);
        }

        //EDITAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "iEntregaAlEst_id,sGestion_desc,sPeriodo_desc,sEntregaAlEst_fl,dtEntregaAlEst_dt,iRecepcionTFG_id,lEstudiante_id,lEntregadoPor_id,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_EntregaAlEst EntregaAlEst)
        {          
            try
            {
                EntregaAlEst.iEliminado_fl = 1;
                EntregaAlEst.sCreado_by = FrontUser.Get().usr_login;
                EntregaAlEst.iConcurrencia_id += 1;

                if (ModelState.IsValid)
                {
                    db.Entry(EntregaAlEst).State = EntityState.Modified;
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO   ", "El dato ha sido Modificado correctamente.");
                    return RedirectToAction("Index");
                }
                var informacionAlumno = TraerInformacionDeAlumnoPorIdRecepcionTFG(EntregaAlEst.iRecepcionTFG_id);
                ViewBag.datos = informacionAlumno;
                CargarViewBags(Tarea.EDITAR, EntregaAlEst);
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                return View(EntregaAlEst);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        [Permiso(Permiso = RolesPermisos.EGRE_entregaTFGalAlumno_puedeEliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_EntregaAlEst EntregaAlEst = db.EntregasAlEstudiante.Find(id);
            if (EntregaAlEst == null)
            {
                return HttpNotFound();
            }
            var informacionAlumno = TraerInformacionDeAlumnoPorIdRecepcionTFG(EntregaAlEst.iRecepcionTFG_id);
            ViewBag.datos = informacionAlumno;
            return View(EntregaAlEst);
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
                    gatbl_EntregaAlEst EntregaAlEst = db.EntregasAlEstudiante.Find(id);
                    EntregaAlEst.iEstado_fl = false;
                    EntregaAlEst.iEliminado_fl = 2;
                    EntregaAlEst.sCreado_by = FrontUser.Get().usr_login;
                    EntregaAlEst.iConcurrencia_id += 1;

                    db.Entry(EntregaAlEst).State = EntityState.Modified;
                    db.SaveChanges();

                    db.EntregasAlEstudiante.Remove(EntregaAlEst);
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO   ", "El dato ha sido Eliminado correctamente.");
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

        #endregion

        #region Metodos Auxiliares
        //OBTEBER DATOS DEL ALUMNO ASINCRONO
        [HttpGet]
        public ActionResult BuscarInformacionDeAlumno(string term = "")
        {
            int id = 0;
            id = int.Parse(term);
            var infoAlumno = TraerInformacionDeAlumnoPorIdRecepcionTFG(id);
            return Json(infoAlumno, JsonRequestBehavior.AllowGet);
        }

        //CARGA INFORMACION DEL ALUMNO EN BASE AL ID
        private Object TraerInformacionDeAlumnoPorIdRecepcionTFG(int id)
        {
            var result = (from rt in db.RecepcionesTFG
                          join et in db.EntregasTribunales on rt.iETribunal_id equals et.iETribunal_id
                          join e in db.EntregasTFG on et.iEntrega_id equals e.iEntrega_id
                          join p in db.Perfiles on e.iPerfil_id equals p.iPerfil_id
                          join al in db.Alumnos on p.lEstudiante_id equals al.alm_registro
                          join per in db.Personas on al.agd_codigo equals per.agd_codigo
                          join c in db.Carreras on al.crr_codigo equals c.crr_codigo
                          join f in db.Facultades on c.sca_codigo equals f.sca_codigo
                          join t in db.Tutorez on et.iTutuor_id equals t.iTutuor_id
                          where rt.iRecepcionTFG_id == id
                          select new { nombreAlumno = per.agd_nombres.Trim() + " " + per.agd_appaterno.Trim() + " " + per.agd_apmaterno.Trim(), carrera = c.crr_descripcion.Trim(), facultad = f.sca_descripcion.Trim(), TituloPerfil = p.sTitulo_tfg, iRecepcionTFG_id = rt.iRecepcionTFG_id, TipoGraduacion = p.TipoGraduacion.tttg_descripcion.Trim(), al.alm_registro, nombreTutor = t.sNombre_desc, estadoRecepcionDeTribunal = rt.sRecepcionTFG_fl.ToString(), ejemplar = e.sNumEjemplar.ToString() }).FirstOrDefault();
            return result;
        }

        //CONSTANTE ENUM DE TAREA
        //TEREAS
        private enum Tarea
        {
            NUEVO,
            EDITAR,
            ELIMINAR
        }

        //CLASE INTERNA
        private class RecepcionesDeTribunal
        {
            public int iRecepcionTFG_id { get; set; }
            public string nombreAlumno { get; set; }
            public string registro { get; set; }
        }

        //CARGA UNA LISTA PARA VIEWBAG DE ACUERDO A LA TAREA
        private List<RecepcionesDeTribunal> SeleccionarLista(Tarea tarea, int id)
        {

            List<RecepcionesDeTribunal> recepciones = new List<RecepcionesDeTribunal>();

            var recepcionesDeTribunales = from rt in db.RecepcionesTFG
                                          join et in db.EntregasTribunales on rt.iETribunal_id equals et.iETribunal_id
                                          join t in db.Tutorez on et.iTutuor_id equals t.iTutuor_id
                                          join eTFG in db.EntregasTFG on et.iEntrega_id equals eTFG.iEntrega_id
                                          join p in db.Perfiles on eTFG.iPerfil_id equals p.iPerfil_id
                                          join es in db.Alumnos on p.lEstudiante_id equals es.alm_registro
                                          join per in db.Personas on es.agd_codigo equals per.agd_codigo
                                          select new { iRecepcionTFG_id = rt.iRecepcionTFG_id, nombreAlumno = per.agd_nombres.Trim() + " " + per.agd_appaterno.Trim() + " " + per.agd_apmaterno.Trim() + " " + es.alm_registro + " (" + t.sNombre_desc + ")", registro = es.alm_registro };

            var entregasAlEstudiantes = from eest in db.EntregasAlEstudiante
                                        join rt in db.RecepcionesTFG on eest.iRecepcionTFG_id equals rt.iRecepcionTFG_id
                                        join et in db.EntregasTribunales on rt.iETribunal_id equals et.iETribunal_id
                                        join t in db.Tutorez on et.iTutuor_id equals t.iTutuor_id
                                        join eTFG in db.EntregasTFG on et.iEntrega_id equals eTFG.iEntrega_id
                                        join p in db.Perfiles on eTFG.iPerfil_id equals p.iPerfil_id
                                        join es in db.Alumnos on p.lEstudiante_id equals es.alm_registro
                                        join per in db.Personas on es.agd_codigo equals per.agd_codigo
                                        select new { iRecepcionTFG_id = eest.iRecepcionTFG_id, nombreAlumno = per.agd_nombres.Trim() + " " + per.agd_appaterno.Trim() + " " + per.agd_apmaterno.Trim() + " " + es.alm_registro + " (" + t.sNombre_desc + ")", registro = es.alm_registro };

            recepciones = (from rt in recepcionesDeTribunales
                           where !entregasAlEstudiantes.Any(m => m.iRecepcionTFG_id == rt.iRecepcionTFG_id)
                           select new RecepcionesDeTribunal { iRecepcionTFG_id = rt.iRecepcionTFG_id, nombreAlumno = rt.nombreAlumno, registro = rt.registro }).ToList();

            switch (tarea)
            {
                case Tarea.NUEVO:
                    return recepciones.ToList();
                case Tarea.EDITAR:
                    {
                        List<RecepcionesDeTribunal> entregaAEstudiante = new List<RecepcionesDeTribunal>();
                        entregaAEstudiante = (from ees in entregasAlEstudiantes
                                              where ees.iRecepcionTFG_id == id
                                              select new RecepcionesDeTribunal { iRecepcionTFG_id = ees.iRecepcionTFG_id, nombreAlumno = ees.nombreAlumno, registro = ees.registro }).ToList();
                        return recepciones.Union(entregaAEstudiante).ToList();
                    }
                default:
                    {
                        return recepciones.ToList();
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

        //CARGA VIEW BAGS
        private void CargarViewBags(Tarea tarea, gatbl_EntregaAlEst EntregaAlEst)
        {
            var recepcionesDeTrib = SeleccionarLista(tarea, EntregaAlEst.iRecepcionTFG_id);
            ViewBag.iRecepcionTFG_id = new SelectList(recepcionesDeTrib, "iRecepcionTFG_id", "nombreAlumno", EntregaAlEst.iRecepcionTFG_id);
            ViewBag.sPeriodo_desc = new SelectList(db.Semestres, "sem_codigo", "sem_codigo", EntregaAlEst.sPeriodo_desc);
            // ViewBag.lEntregadoPor_id = new SelectList(db.Personas, "agd_codigo", "NombreCompleto", EntregaAlEst.lEntregadoPor_id);
            if (tarea == Tarea.EDITAR)
            {
                //var receptor = TraerInformacionDePersonaPorCodigo(EntregaAlEst.lEntregadoPor_id);
                ViewBag.Receptor = TraerInformacionDePersonaPorCodigo(EntregaAlEst.lEntregadoPor_id); ;
            }
        }
        #endregion

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
