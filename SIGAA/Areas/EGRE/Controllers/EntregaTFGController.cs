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
    public class EntregaTFGController : Controller
    {
        private EgresadosContext db = new EgresadosContext();

        [Permiso(Permiso = RolesPermisos.EGRE_recepcionTFGdelAlumno_puedeVerIndice)]
        public ActionResult Index(string criterio = null)
        {
            var entregasTFG = db.EntregasTFG.Where(p => p.iEliminado_fl == 1).Include(g => g.Alumno).Include(g => g.Perfil).Include(g => g.Persona).ToList();
            var entregasTFGFiltradas = entregasTFG.Where(e => criterio == null ||
                                                        e.Alumno.Persona.NombreCompleto.ToLower().Contains(criterio.ToLower()) ||
                                                        e.lEstudiante_id.Contains(criterio.ToLower()) ||
                                                        e.Perfil.sTitulo_tfg.ToLower().Contains(criterio.ToLower()) ||
                                                        e.Alumno.Carrera.crr_descripcion.ToLower().Contains(criterio.ToLower()) ||
                                                        e.dtEntrega_dt.ToString().Contains(criterio.ToLower()) ||
                                                        e.sPeriodo_desc.ToLower().Contains(criterio.ToLower()) ||
                                                        e.sEntrega_fl.ToString().ToLower().Contains(criterio.ToLower())
                                                        ).ToList();
            var entregasTFGFiltradasOrdenadas = entregasTFGFiltradas.OrderBy(ef => ef.Alumno.Persona.NombreCompleto).ThenBy(ef => ef.dtEntrega_dt);
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

        [Permiso(Permiso = RolesPermisos.EGRE_recepcionTFGdelAlumno_puedeVerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_EntregaTFG EntregaTFG = db.EntregasTFG.Find(id);
            if (EntregaTFG == null)
            {
                return HttpNotFound();
            }
            var informacionAlumno = TraerInformacionDeAlumnoPorPerfil_id(EntregaTFG.iPerfil_id);
            ViewBag.datos = informacionAlumno;
            return View(EntregaTFG);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_recepcionTFGdelAlumno_puedeCrearNuevo)]
        public ActionResult Create()
        {
            gatbl_EntregaTFG EntregaTFG = new gatbl_EntregaTFG();
            CargarViewBags(Tarea.NUEVO, EntregaTFG);
            return View(EntregaTFG);
        }

        //CREAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "iEntrega_id,sGestion_desc,sPeriodo_desc,sEntrega_fl,dtEntrega_dt,sNumEjemplar,iPerfil_id,lEstudiante_id,lRecepciona_id,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_EntregaTFG EntregaTFG)
        {           
            try
            {
                EntregaTFG.iEstado_fl = true;
                EntregaTFG.iEliminado_fl = 1;
                EntregaTFG.sCreado_by = FrontUser.Get().usr_login;
                EntregaTFG.iConcurrencia_id = 1;

                if (ModelState.IsValid)
                {
                    db.EntregasTFG.Add(EntregaTFG);
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Guardado correctamente.");
                    return RedirectToAction("Index");
                }
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                CargarViewBags(Tarea.NUEVO, EntregaTFG);
                return View(EntregaTFG);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, ha ocurrido el siguiente error: " +ex.Message);
                return RedirectToAction("Index");
            }
        }

        [Permiso(Permiso = RolesPermisos.EGRE_recepcionTFGdelAlumno_puedeEditar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_EntregaTFG EntregaTFG = db.EntregasTFG.Find(id);
            if (EntregaTFG == null)
            {
                return HttpNotFound();
            }
            var informacionDeAlumno = TraerInformacionDeAlumnoPorPerfil_id(EntregaTFG.iPerfil_id);
            ViewBag.datos = informacionDeAlumno;
            CargarViewBags(Tarea.EDITAR, EntregaTFG);
            return View(EntregaTFG);
        }

        //EDITAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "iEntrega_id,sGestion_desc,sPeriodo_desc,sEntrega_fl,dtEntrega_dt,sNumEjemplar,iPerfil_id,lEstudiante_id,lRecepciona_id,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_EntregaTFG EntregaTFG)
        {            
            try
            {
                EntregaTFG.iEliminado_fl = 1;
                EntregaTFG.sCreado_by = FrontUser.Get().usr_login;
                EntregaTFG.iConcurrencia_id += 1;

                if (ModelState.IsValid)
                {
                    db.Entry(EntregaTFG).State = EntityState.Modified;
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Modificado correctamente.");
                    return RedirectToAction("Index");
                }
                var informacionDeAlumno = TraerInformacionDeAlumnoPorPerfil_id(EntregaTFG.iPerfil_id);
                ViewBag.datos = informacionDeAlumno;
                CargarViewBags(Tarea.EDITAR, EntregaTFG);
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                return View(EntregaTFG);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        [Permiso(Permiso = RolesPermisos.EGRE_recepcionTFGdelAlumno_puedeEliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_EntregaTFG EntregaTFG = db.EntregasTFG.Find(id);
            if (EntregaTFG == null)
            {
                return HttpNotFound();
            }
            var informacionAlumno = TraerInformacionDeAlumnoPorPerfil_id(EntregaTFG.iPerfil_id);
            ViewBag.datos = informacionAlumno;
            return View(EntregaTFG);
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
                    gatbl_EntregaTFG EntregaTFG = db.EntregasTFG.Find(id);
                    EntregaTFG.iEstado_fl = false;
                    EntregaTFG.iEliminado_fl = 2;
                    EntregaTFG.sCreado_by = FrontUser.Get().usr_login;
                    EntregaTFG.iConcurrencia_id += 1;

                    db.Entry(EntregaTFG).State = EntityState.Modified;
                    db.SaveChanges();

                    db.EntregasTFG.Remove(EntregaTFG);
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

        //TRAE INFORMACION DE ALUMNO DETERMINADO POR ID
        private Object TraerInformacionDeAlumnoPorPerfil_id(int id)
        {
            var result = (from p in db.Perfiles
                          join al in db.Alumnos on p.lEstudiante_id equals al.alm_registro
                          join per in db.Personas on al.agd_codigo equals per.agd_codigo
                          join c in db.Carreras on al.crr_codigo equals c.crr_codigo
                          join f in db.Facultades on c.sca_codigo equals f.sca_codigo
                          where p.iPerfil_id == id
                          select new { nombreAlumno = per.agd_nombres.Trim() + " " + per.agd_appaterno.Trim() + " " + per.agd_apmaterno.Trim(), carrera = c.crr_descripcion.Trim(), facultad = f.sca_descripcion.Trim(), TituloPerfil = p.sTitulo_tfg, TipoGraduacion = p.TipoGraduacion.tttg_descripcion.Trim(), registro = al.alm_registro, estadoPerfil = p.sPerfil_fl.ToString() }).FirstOrDefault();
            return result;
        }

        //DATOS DE ALUMNO POR REGISTRO
        public ActionResult BuscarInformacionDeAlumno(string term = "")
        {
            int id = 0;
            id = int.Parse(term);
            var result = TraerInformacionDeAlumnoPorPerfil_id(id);
            return Json(result, JsonRequestBehavior.AllowGet);
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
            public int iPerfil_id { get; set; }
            public string nombreAlumno { get; set; }
            public string registro { get; set; }
        }

        //LISTA SELECCIONA ENTREGAS A TRIBUNALES
        private List<RecepcionDeAlumno> SeleccionarLista(Tarea tarea, int id)
        {
            var modalidades = new string[] { "EXAMEN DE GRADO", "GRADUACION POR EXCELENCIA" };
            var Perfiles = from p in db.Perfiles
                           join al in db.Alumnos on p.lEstudiante_id equals al.alm_registro
                           join per in db.Personas on al.agd_codigo equals per.agd_codigo
                           where !modalidades.Contains(p.TipoGraduacion.tttg_descripcion.Trim())
                           select new { iPerfil_id = p.iPerfil_id, nombreAlumno = per.agd_nombres.Trim() + " " + per.agd_appaterno.Trim() + " " + per.agd_apmaterno.Trim() + " (" + al.alm_registro.Trim() + ")", registro = al.alm_registro };

            var RecepcionesDeAlumnos = from re in db.EntregasTFG
                                       join p in db.Perfiles on re.iPerfil_id equals p.iPerfil_id
                                       join al in db.Alumnos on p.lEstudiante_id equals al.alm_registro
                                       join per in db.Personas on al.agd_codigo equals per.agd_codigo
                                       select new { iPerfil_id = re.iPerfil_id, nombreAlumno = per.agd_nombres.Trim() + " " + per.agd_appaterno.Trim() + " " + per.agd_apmaterno.Trim() + " (" + al.alm_registro.Trim() + ")", registro = al.alm_registro };

            var perfiles = (from p in Perfiles
                            select new RecepcionDeAlumno { iPerfil_id = p.iPerfil_id, nombreAlumno = p.nombreAlumno, registro = p.registro }).ToList();
            switch (tarea)
            {
                case Tarea.NUEVO:
                    return perfiles.ToList();
                case Tarea.EDITAR:
                    {
                        return perfiles.ToList();
                        //    List<RecepcionDeAlumno> RecepcionDeAlumno = new List<RecepcionDeAlumno>();
                        //    RecepcionDeAlumno = (from re in RecepcionesDeAlumnos
                        //                         where re.iPerfil_id == id
                        //                         select new RecepcionDeAlumno { iPerfil_id = re.iPerfil_id, nombreAlumno = re.nombreAlumno, registro = re.registro }).ToList();
                        //    return perfiles.Union(RecepcionDeAlumno).ToList();
                    }
                default:
                    {
                        return perfiles.ToList();
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
        private void CargarViewBags(Tarea tarea, gatbl_EntregaTFG EntregaTFG)
        {
            var perfilesDeAlumnos = SeleccionarLista(tarea, EntregaTFG.iPerfil_id);
            ViewBag.iPerfil_id = new SelectList(perfilesDeAlumnos, "iPerfil_id", "nombreAlumno", EntregaTFG.iEntrega_id);
            ViewBag.sPeriodo_desc = new SelectList(db.Semestres, "sem_codigo", "sem_codigo", EntregaTFG.sPeriodo_desc);
            if (tarea == Tarea.EDITAR)
            {
                var receptor = TraerInformacionDePersonaPorCodigo(EntregaTFG.lRecepciona_id);
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
