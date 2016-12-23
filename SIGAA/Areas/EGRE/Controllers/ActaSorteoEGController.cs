using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using MvcFlash.Core;
using MvcFlash.Core.Extensions;
using SIGAA.Areas.EGRE.Models;
using SIGAA.Commons;
using SIGAA.Etiquetas;

namespace SIGAA.Areas.EGRE.Controllers
{
    [Autenticado]
    public class ActaSorteoEGController : Controller
    {
        private EgresadosContext db = new EgresadosContext();

        [Permiso(Permiso = RolesPermisos.EGRE_actaDeSorteo_puedeVerIndice)]
        public ActionResult Index(string criterio = null)
        {
            var actasSorteosEG = db.ActasSorteosEG.Where(p => p.iEliminado_fl == 1).Include(g => g.Alumno).Include(g => g.Evaluador1E).Include(g => g.Evaluador1I).Include(g => g.Evaluador2E).Include(g => g.Evaluador2I).Include(g => g.Perfil).Include(g => g.Presidente).Include(g => g.RealizadoPor).Include(g => g.Secretario).ToList();
            var actasSorteosEGFiltrado = actasSorteosEG.Where(a => criterio == null ||
                                                              a.alm_registro.Contains(criterio.ToLower()) ||
                                                              a.Alumno.Persona.NombreCompleto.ToLower().Contains(criterio.ToLower()) ||
                                                              a.Alumno.Carrera.crr_descripcion.ToLower().Contains(criterio.ToLower()) ||
                                                              a.dtSorteo_dt.ToString().Contains(criterio.ToLower()) ||
                                                              a.dtHora_dt.ToString().Contains(criterio.ToLower()) ||
                                                              a.sBolos_desc.ToLower().Contains(criterio.ToLower())
                                                                ).ToList();
            var actasSorteosEGFiltradoOrdenadas = actasSorteosEGFiltrado.OrderBy(ef => ef.Alumno.Persona.NombreCompleto).ThenBy(ef => ef.dtSorteo_dt);
            if (Request.IsAjaxRequest())
            {
                if (!String.IsNullOrWhiteSpace(criterio))
                {
                    Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + actasSorteosEGFiltradoOrdenadas.Count() + " registros con los criterios especificados.");
                }
                return PartialView("_Index", actasSorteosEGFiltradoOrdenadas);
            }

            if (!String.IsNullOrWhiteSpace(criterio))
            {
                Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + actasSorteosEGFiltradoOrdenadas.Count() + " registros con los criterios especificados.");
            }
            return View(actasSorteosEGFiltradoOrdenadas);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_actaDeSorteo_puedeVerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_ActaSorteoEG ActaSorteoEG = db.ActasSorteosEG.Find(id);
            if (ActaSorteoEG == null)
            {
                return HttpNotFound();
            }
            var informacionAlumno = TraerInformacionDeAlumno(ActaSorteoEG.alm_registro);
            ViewBag.datos = informacionAlumno;
            return View(ActaSorteoEG);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_actaDeSorteo_puedeCrearNuevo)]
        public ActionResult Create()
        {
            gatbl_ActaSorteoEG ActaSorteoEG = new gatbl_ActaSorteoEG();
            ActaSorteoEG.iNumeracion_num = ObtenerNumeracion();
            CargarViewBags(Tarea.NUEVO, ActaSorteoEG);
            return View(ActaSorteoEG);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "iActaSorteoEG_id,sGestion_desc,sPeriodo_desc,iNumeracion_num,dtSorteo_dt,dtHora_dt,sBolos_desc,sArea_desc,dtHoraFinalizacion_dt,sLugar_desc,sObs_desc,iPerfil_id,alm_registro,lPresidente_id,lSecretario_id,lEvaluador1I_id,lEvaluador2I_id,lEvaluador1E_id,lEvaluador2E_id,lRealizadoPor_id,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_ActaSorteoEG ActaSorteoEG)
        {
            try
            {
                ActaSorteoEG.iEstado_fl = true;
                ActaSorteoEG.iEliminado_fl = 1;
                ActaSorteoEG.sCreado_by = FrontUser.Get().EmailUtepsa;
                ActaSorteoEG.iConcurrencia_id = 1;

                if (ModelState.IsValid)
                {
                    db.ActasSorteosEG.Add(ActaSorteoEG);
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato se ha Guardado correctamente.");
                    return RedirectToAction("Index");
                }

                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                CargarViewBags(Tarea.NUEVO, ActaSorteoEG);
                return View(ActaSorteoEG);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        [Permiso(Permiso = RolesPermisos.EGRE_actaDeSorteo_puedeEditar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_ActaSorteoEG ActaSorteoEG = db.ActasSorteosEG.Find(id);
            if (ActaSorteoEG == null)
            {
                return HttpNotFound();
            }
            CargarViewBags(Tarea.EDITAR, ActaSorteoEG);
            return View(ActaSorteoEG);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "iActaSorteoEG_id,sGestion_desc,sPeriodo_desc,iNumeracion_num,dtSorteo_dt,dtHora_dt,sBolos_desc,sArea_desc,dtHoraFinalizacion_dt,sLugar_desc,sObs_desc,iPerfil_id,alm_registro,lPresidente_id,lSecretario_id,lEvaluador1I_id,lEvaluador2I_id,lEvaluador1E_id,lEvaluador2E_id,lRealizadoPor_id,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_ActaSorteoEG ActaSorteoEG)
        {
            try
            {
                ActaSorteoEG.iEliminado_fl = 1;
                ActaSorteoEG.sCreado_by = FrontUser.Get().EmailUtepsa;
                ActaSorteoEG.iConcurrencia_id += 1;
                if (ModelState.IsValid)
                {
                    db.Entry(ActaSorteoEG).State = EntityState.Modified;
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Modificado correctamente.");
                    return RedirectToAction("Index");

                }
                CargarViewBags(Tarea.EDITAR, ActaSorteoEG);
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                return View(ActaSorteoEG);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        [Permiso(Permiso = RolesPermisos.EGRE_actaDeSorteo_puedeEliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_ActaSorteoEG ActaSorteoEG = db.ActasSorteosEG.Find(id);
            if (ActaSorteoEG == null)
            {
                return HttpNotFound();
            }
            var informacionAlumno = TraerInformacionDeAlumno(ActaSorteoEG.alm_registro);
            ViewBag.datos = informacionAlumno;
            return View(ActaSorteoEG);
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
                    gatbl_ActaSorteoEG ActaSorteoEG = db.ActasSorteosEG.Find(id);
                    ActaSorteoEG.iEstado_fl = false;
                    ActaSorteoEG.iEliminado_fl = 2;
                    ActaSorteoEG.sCreado_by = FrontUser.Get().EmailUtepsa;
                    ActaSorteoEG.iConcurrencia_id += 1;

                    db.Entry(ActaSorteoEG).State = EntityState.Modified;
                    db.SaveChanges();

                    db.ActasSorteosEG.Remove(ActaSorteoEG);
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

        //BUSQUEDA DE REGISTRO ASINCRONO
        public JsonResult BuscarPorRegistro(string term)
        {
            List<string> personas = new List<string>();
            var personasAl = (from pr in db.Perfiles
                              join a in db.Alumnos on pr.lEstudiante_id equals a.alm_registro
                              join ag in db.Personas on a.agd_codigo equals ag.agd_codigo
                              where pr.iEliminado_fl == 1 && pr.sModalidad_fl == "02" && pr.sPerfil_fl.ToString() == "Aprobado" //validamos NoEsten Eliminados; No Examen de Grado=02; y el perfil este aprobado
                              select new { alm_registro = ag.agd_nombres.Trim() + " " + ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + " (" + a.alm_registro.Trim() + ")" }).ToList();

            personas = (from pa in personasAl
                        where pa.alm_registro.ToLower().Contains(term.ToLower()) //validamos NoEsten Eliminados; No Examen de Grado=02; y el perfil este aprobado
                        select pa.alm_registro).Take(10).ToList();

            return Json(personas, JsonRequestBehavior.AllowGet);
        }
        //TRAE INFORMACION DE ALUMNO EN BASE A SU ID
        private object TraerInformacionDeAlumno(string id)
        {
            var resultL = (from a in db.Alumnos
                           join p in db.Personas on a.agd_codigo equals p.agd_codigo
                           join c in db.Carreras on a.crr_codigo equals c.crr_codigo
                           join f in db.Facultades on c.sca_codigo equals f.sca_codigo
                           join per in db.Perfiles on a.alm_registro equals per.lEstudiante_id
                           select new
                           {
                               nombreCompletoAlumno = p.agd_nombres.Trim() + " " + p.agd_appaterno.Trim() + " " + p.agd_apmaterno.Trim() + " (" + a.alm_registro.Trim() + ")",
                               nombreAlumno = p.agd_nombres.Trim() + " " + p.agd_appaterno.Trim() + " " + p.agd_apmaterno.Trim() + " (" + a.alm_registro.Trim() + ")",
                               codigoAlumno = a.alm_registro.Trim(),
                               carrera = c.crr_descripcion.Trim(),
                               facultad = f.sca_descripcion.Trim(),
                               TituloPerfil = per.sTitulo_tfg,
                               iPerfil_id = per.iPerfil_id,
                               TipoGraduacion = per.TipoGraduacion.tttg_descripcion.Trim()
                           }).ToList();

            var result = (from dl in resultL
                          where dl.nombreAlumno.ToLower().Contains(id.ToLower())
                          select dl).FirstOrDefault();
            return result;
        }
        //BUSQUEDA DE DATOS DE ALUMO ASINCRONO
        public ActionResult Buscar(string term)
        {
            var informacionAlumno = TraerInformacionDeAlumno(term);
            return Json(informacionAlumno, JsonRequestBehavior.AllowGet);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_actaDeSorteo_puedeImprimir)]
        public ActionResult ImprimirActaDeSorteo(int id)
        {
            try
            {
                var res = from df in db.ActasSorteosEG
                          join p in db.Perfiles on df.iPerfil_id equals p.iPerfil_id
                          join a in db.Alumnos on p.lEstudiante_id equals a.alm_registro
                          join ag in db.Personas on a.agd_codigo equals ag.agd_codigo
                          join pr in db.Personas on df.lPresidente_id equals pr.agd_codigo
                          join se in db.Personas on df.lSecretario_id equals se.agd_codigo
                          join ji1 in db.Personas on df.lEvaluador1I_id equals ji1.agd_codigo
                          join ji2 in db.Personas on df.lEvaluador2I_id equals ji2.agd_codigo
                          join c in db.Carreras on a.crr_codigo equals c.crr_codigo
                          where df.iActaSorteoEG_id == id
                          select new
                          {
                              iActaSorteoEG_id = df.iActaSorteoEG_id,
                              sTitulo_tfg = p.sTitulo_tfg,
                              dtHora_dt = df.dtHora_dt,
                              dtSorteo_dt = df.dtSorteo_dt,
                              sLugar_desc = df.sLugar_desc,
                              alumno = ag.agd_nombres.Trim() + " " + ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim(),
                              alm_registro = a.alm_registro,
                              crr_descripcion = c.crr_descripcion,
                              presidente = pr.agd_prefijo.Trim() + " " + pr.agd_nombres.Trim() + " " + pr.agd_appaterno.Trim() + " " + pr.agd_apmaterno.Trim(),
                              secretaria = se.agd_prefijo.Trim() + " " + se.agd_nombres.Trim() + " " + se.agd_appaterno.Trim() + " " + se.agd_apmaterno.Trim(),
                              juradoInterno1 = ji1.agd_prefijo.Trim() + " " + ji1.agd_nombres.Trim() + " " + ji1.agd_appaterno.Trim() + " " + ji1.agd_apmaterno.Trim(),
                              juradoInterno2 = ji2.agd_prefijo.Trim() + " " + ji2.agd_nombres.Trim() + " " + ji2.agd_appaterno.Trim() + " " + ji2.agd_apmaterno.Trim(),
                              dtHoraFinalizacion_dt = df.dtHoraFinalizacion_dt,
                              sArea_desc = df.sArea_desc,
                              sBolos_desc = df.sBolos_desc,
                              sObs_desc = df.sObs_desc,
                              numeracion = df.iNumeracion_num,

                              modalidad = p.TipoGraduacion.tttg_descripcion.Trim()
                          };
                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Areas/EGRE/Reportes"), "ActaDeSorteoEG.rpt"));
                rd.SetDataSource(res.ToList());
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
               // return File(stream, "application/pdf", "ActaSorteoExamenDeGrado.pdf");
                return File(stream, "application/pdf");
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Mostrar el reporte, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
        }


        //OBTIENE INFORMACION DE FECHAS Y LUGAR DE DEFENSA FINAL, DE ACUERDO A LA FECHAS ESTABLECIDA EN LA COMUNICACION AL ALUMNO EN FORMATO JSON
        public ActionResult TraerInformacionDeFechaYlugarDeDefensaFinal_Json(int idPerfil)
        {
            var informacionAlumno = TraerInformacionDeFechaYlugarDeDefensaFinal(idPerfil);
            return Json(informacionAlumno, JsonRequestBehavior.AllowGet);
        }

        //OBTIENE INFORMACION DE FECHAS Y LUGAR DE DEFENSA FINAL, DE ACUERDO A LA FECHAS ESTABLECIDA EN LA COMUNICACION AL ALUMNO
        private Object TraerInformacionDeFechaYlugarDeDefensaFinal(int idPerfil)
        {
            var datosFechasYlugaresT = (from p in db.Perfiles
                                        join ce in db.ComunicacionesEstudiantes on p.iPerfil_id equals ce.iPerfil_id
                                        where p.iPerfil_id == idPerfil && ce.sComunicacionEst_fl.ToString().Equals("DEFENSA_FINAL")
                                        select new
                                        {
                                            fechaDefensaProgramada = ce.dtDProgramada_dt,
                                            horaDefensaProgramada = ce.dtHProgramada_dt,
                                            lugarDefensaProgramada = ce.sLugarDefensa_desc,
                                            fechaSorteo = ce.dtSorteoArea_dt,
                                            horaSorteo = ce.dtHoraSorteo_dt,
                                            lugarSorteo = ce.sLugarSorteo_desc,
                                            horaEntregaCaso = ce.dtHoraEntrega_dt,
                                            lugarEntregaCaso = ce.sLugarEntrega_desc
                                        }).FirstOrDefault();
            return datosFechasYlugaresT;
        }


        private enum Tarea
        {
            NUEVO,
            EDITAR,
            ELIMINAR
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
                                 select new { codigoPersona = p.agd_codigo, nombrePersona = p.agd_nombres.Trim() + " " + p.agd_appaterno.Trim() + " " + p.agd_apmaterno.Trim() }).ToList();

            var datosPersona = (from f in listaPersonas
                                where f.codigoPersona.ToLower().Contains(codigo.ToLower())
                                select f).FirstOrDefault();
            return datosPersona;
        }

        //CARGA VIEW BAGS
        private void CargarViewBags(Tarea tarea, gatbl_ActaSorteoEG ActaSorteoEG)
        {
            ViewBag.sLugar_desc = new SelectList(db.Salas.Where(p => p.iEliminado_fl == 1).OrderBy(p => p.sNombre_nm), "sNombre_nm", "sNombre_nm", ActaSorteoEG.sLugar_desc);
            ViewBag.sBolos_desc = new SelectList(db.AreasDefensas.OrderBy(p => p.dsc_descripcion), "dsc_descripcion", "dsc_descripcion", ActaSorteoEG.sBolos_desc);
            ViewBag.sPeriodo_desc = new SelectList(db.Semestres.OrderBy(p => p.sem_codigo), "sem_codigo", "sem_codigo", ActaSorteoEG.sPeriodo_desc);
            if (tarea == Tarea.EDITAR)
            {
                ViewBag.datos = TraerInformacionDeAlumno(ActaSorteoEG.alm_registro);
                ViewBag.Presidente = TraerInformacionDePersonaPorCodigo(ActaSorteoEG.lPresidente_id);
                ViewBag.Secretario = TraerInformacionDePersonaPorCodigo(ActaSorteoEG.lSecretario_id);
                ViewBag.Evaluador1I = TraerInformacionDePersonaPorCodigo(ActaSorteoEG.lEvaluador1I_id);
                ViewBag.Evaluador2I = TraerInformacionDePersonaPorCodigo(ActaSorteoEG.lEvaluador2I_id);
                ViewBag.RealizadoPor = TraerInformacionDePersonaPorCodigo(ActaSorteoEG.lRealizadoPor_id);
            }
        }

        private int ObtenerNumeracion()
        {
            int numMaximoMasUno = 0;
            var anioActual = DateTime.Today.Year;
            var actaSorteo = db.ActasSorteosEG.Where(d => d.dtSorteo_dt.Year == anioActual).ToList();
            var cantidad = actaSorteo.Count();
            if (cantidad > 0)
            {
                var numeroMaximo = actaSorteo.Max(e => e.iNumeracion_num);
                numMaximoMasUno = numeroMaximo + 1;
            }
            else
            {
                numMaximoMasUno = +1;
            }
            return numMaximoMasUno;
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
