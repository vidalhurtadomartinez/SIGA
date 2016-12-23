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
    public class ComunicacionEstController : Controller
    {
        private EgresadosContext db = new EgresadosContext();

    [Permiso(Permiso = RolesPermisos.EGRE_comunicacionAlAlumno_puedeVerIndice)]
    public ActionResult Index(string criterio = null)
        {
            var comunicacionesEstudiantes = db.ComunicacionesEstudiantes.Where(p => p.iEliminado_fl == 1).Include(g => g.Alumno).Include(g => g.Perfil).Include(g => g.Persona).ToList();
            var comunicacionesEstudiantesFiltrado = comunicacionesEstudiantes.Where(c => criterio == null ||
                                                                                    c.lEstudiante_id.Contains(criterio.ToLower()) ||
                                                                                    c.Alumno.Persona.NombreCompleto.ToLower().Contains(criterio.ToLower()) ||
                                                                                    c.Alumno.Carrera.crr_descripcion.ToLower().Contains(criterio.ToLower()) ||
                                                                                    c.dtComunicacionEst_dt.ToString().Contains(criterio.ToLower()) ||
                                                                                    c.sComunicacionEst_fl.ToString().ToLower().Contains(criterio.ToLower()) ||
                                                                                    c.Perfil.TipoGraduacion.tttg_descripcion.ToLower().Contains(criterio.ToLower())
                                                                                    ).ToList();

            var ccomunicacionesEstudiantesFiltradoOrdenadas = comunicacionesEstudiantesFiltrado.OrderBy(ef => ef.Alumno.Persona.NombreCompleto).ThenBy(ef => ef.dtComunicacionEst_dt);
            if (Request.IsAjaxRequest())
            {
                if (!String.IsNullOrWhiteSpace(criterio))
                {
                    Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + ccomunicacionesEstudiantesFiltradoOrdenadas.Count() + " registros con los criterios especificados.");
                }
                return PartialView("_Index", ccomunicacionesEstudiantesFiltradoOrdenadas);
            }

            if (!String.IsNullOrWhiteSpace(criterio))
            {
                Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + ccomunicacionesEstudiantesFiltradoOrdenadas.Count() + " registros con los criterios especificados.");
            }
            return View(ccomunicacionesEstudiantesFiltradoOrdenadas);
        }

    [Permiso(Permiso = RolesPermisos.EGRE_comunicacionAlAlumno_puedeVerDetalle)]
    public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_ComunicacionEst ComunicacionEst = db.ComunicacionesEstudiantes.Find(id);
            if (ComunicacionEst == null)
            {
                return HttpNotFound();
            }
            var informacionAlumno = TraerInformacionDeAlumno(ComunicacionEst.lEstudiante_id);
            ViewBag.datos = informacionAlumno;
            return View(ComunicacionEst);
        }

    [Permiso(Permiso = RolesPermisos.EGRE_comunicacionAlAlumno_puedeCrearNuevo)]
    public ActionResult Create()
        {
            gatbl_ComunicacionEst ComunicacionEst = new gatbl_ComunicacionEst();
            ComunicacionEst.iNumeracion_num = ObtenerNumeracion();
            CargarViewBags(Tarea.NUEVO, ComunicacionEst);
            return View(ComunicacionEst);
        }

        //CREAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "iComunicacionEst_id,sGestion_desc,dtDProgramada_dt,dtHProgramada_dt,sLugarDefensa_desc,dtSorteoArea_dt,dtHoraSorteo_dt,sLugarSorteo_desc,dtHoraEntrega_dt,sLugarEntrega_desc,sPeriodo_desc,iNumeracion_num,sComunicacionEst_fl,dtComunicacionEst_dt,sDescripcion_desc,sObs_desc,iPerfil_id,lEstudiante_id,lEntregadoPor_id,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_ComunicacionEst ComunicacionEst)
        {
            try
            {
                if (!esExamenDeGrado(ComunicacionEst.iPerfil_id))//si no es examen de grado los valres para fecha y hora de sorteo y areaDefensa valores equivalentes a nulo
                {
                    ComunicacionEst.dtSorteoArea_dt = DateTime.Parse("1900-01-01");
                    ComunicacionEst.dtHoraSorteo_dt = DateTime.Parse("1900-01-01 00:00");
                    ComunicacionEst.sLugarSorteo_desc = "s/n";
                    ComunicacionEst.dtHoraEntrega_dt = DateTime.Parse("1900-01-01 00:00"); ;
                    ComunicacionEst.sLugarEntrega_desc = "s/n";
                }
                var tipoDeComunicacion = ComunicacionEst.sComunicacionEst_fl.ToString();
                if (tipoDeComunicacion.Equals("AUT_HABILITACION"))
                {
                    ComunicacionEst.dtDProgramada_dt = DateTime.Parse("1900-01-01");
                    ComunicacionEst.dtHProgramada_dt = DateTime.Parse("1900-01-01 00:00");
                    ComunicacionEst.sLugarDefensa_desc = "s/n"; //"";
                    //verifica que el perfil esté como aprobado
                    if (!PerfilEstaAprobado(ComunicacionEst.iPerfil_id))
                    {
                        throw new Exception("El Perfil del alumno NO ESTÁ APROBADO, por favor verifique e intente nuevamente.");
                    }
                }

                ComunicacionEst.iEstado_fl = true;
                ComunicacionEst.iEliminado_fl = 1;
                ComunicacionEst.sCreado_by = FrontUser.Get().EmailUtepsa;
                ComunicacionEst.iConcurrencia_id = 1;

                if (ModelState.IsValid)
                {
                    db.ComunicacionesEstudiantes.Add(ComunicacionEst);
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Guardado correctamente.");
                    return RedirectToAction("Index");
                }
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                CargarViewBags(Tarea.NUEVO, ComunicacionEst);
                return View(ComunicacionEst);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

    [Permiso(Permiso = RolesPermisos.EGRE_comunicacionAlAlumno_puedeEditar)]
    public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_ComunicacionEst ComunicacionEst = db.ComunicacionesEstudiantes.Find(id);
            if (ComunicacionEst == null)
            {
                return HttpNotFound();
            }
            CargarViewBags(Tarea.EDITAR, ComunicacionEst);
            return View(ComunicacionEst);
        }

        //EDITAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "iComunicacionEst_id,sGestion_desc,dtDProgramada_dt,dtHProgramada_dt,sLugarDefensa_desc,dtSorteoArea_dt,dtHoraSorteo_dt,sLugarSorteo_desc,dtHoraEntrega_dt,sLugarEntrega_desc,sPeriodo_desc,iNumeracion_num,sComunicacionEst_fl,dtComunicacionEst_dt,sDescripcion_desc,sObs_desc,iPerfil_id,lEstudiante_id,lEntregadoPor_id,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_ComunicacionEst ComunicacionEst)
        {
            try
            {
                if (!esExamenDeGrado(ComunicacionEst.iPerfil_id))//si no es examen de grado los valres para fecha y hora de sorteo y areaDefensa  colocamos NULOS
                {
                    ComunicacionEst.dtSorteoArea_dt = DateTime.Parse("1900-01-01");
                    ComunicacionEst.dtHoraSorteo_dt = DateTime.Parse("1900-01-01 00:00");
                    ComunicacionEst.sLugarSorteo_desc = "s/n";
                    ComunicacionEst.dtHoraEntrega_dt = DateTime.Parse("1900-01-01 00:00"); ;
                    ComunicacionEst.sLugarEntrega_desc = "s/n";
                }
                var tipoDeComunicacion = ComunicacionEst.sComunicacionEst_fl.ToString();
                if (tipoDeComunicacion.Equals("AUT_HABILITACION"))
                {
                    ComunicacionEst.dtDProgramada_dt = DateTime.Parse("1900-01-01");
                    ComunicacionEst.dtHProgramada_dt = DateTime.Parse("1900-01-01 00:00");
                    ComunicacionEst.sLugarDefensa_desc = "s/n"; //"";
                    if (!PerfilEstaAprobado(ComunicacionEst.iPerfil_id))
                    {
                        throw new Exception("El Perfil del alumno NO ESTÁ APROBADO, por favor verifique e intente nuevamente.");
                    }
                }

                ComunicacionEst.iEliminado_fl = 1;
                ComunicacionEst.sCreado_by = FrontUser.Get().EmailUtepsa;
                ComunicacionEst.iConcurrencia_id += 1;

                if (ModelState.IsValid)
                {
                    db.Entry(ComunicacionEst).State = EntityState.Modified;
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Modificado correctamente.");
                    return RedirectToAction("Index");
                }
                CargarViewBags(Tarea.EDITAR, ComunicacionEst);
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                return View(ComunicacionEst);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

    [Permiso(Permiso = RolesPermisos.EGRE_comunicacionAlAlumno_puedeEliminar)]
    public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_ComunicacionEst ComunicacionEst = db.ComunicacionesEstudiantes.Find(id);
            if (ComunicacionEst == null)
            {
                return HttpNotFound();
            }
            var informacionAlumno = TraerInformacionDeAlumno(ComunicacionEst.lEstudiante_id);
            ViewBag.datos = informacionAlumno;
            return View(ComunicacionEst);
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
                    gatbl_ComunicacionEst ComunicacionEst = db.ComunicacionesEstudiantes.Find(id);
                    ComunicacionEst.iEstado_fl = false;
                    ComunicacionEst.iEliminado_fl = 2;
                    ComunicacionEst.sCreado_by = FrontUser.Get().EmailUtepsa;
                    ComunicacionEst.iConcurrencia_id += 1;
                    db.Entry(ComunicacionEst).State = EntityState.Modified;
                    db.SaveChanges();

                    db.ComunicacionesEstudiantes.Remove(ComunicacionEst);
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

        //BUSQUEDAS ASINCRONAS
        public JsonResult BuscarPorRegistro(string term)
        {
            List<string> personas = new List<string>();
            var personasAl = (from pr in db.Perfiles
                              join a in db.Alumnos on pr.lEstudiante_id equals a.alm_registro
                              join ag in db.Personas on a.agd_codigo equals ag.agd_codigo
                              where pr.iEliminado_fl == 1 && pr.sPerfil_fl.ToString() == "Aprobado" //validamos NoEsten Eliminados; No Examen de Grado=02; y el perfil este aprobado
                              select new { alm_registro = ag.agd_nombres.Trim() + " " + ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + " (" + a.alm_registro.Trim() + ")" }).ToList();

            personas = (from pa in personasAl
                        where pa.alm_registro.ToLower().Contains(term.ToLower())//validamos NoEsten Eliminados; No Examen de Grado=02; y el perfil este aprobado
                        select pa.alm_registro).Take(10).ToList();

            return Json(personas, JsonRequestBehavior.AllowGet);
        }

        //BUSQUEDA ASINCRONA
        public ActionResult Buscar(string term)
        {
            var infoAlumno = TraerInformacionDeAlumno(term);
            return Json(infoAlumno, JsonRequestBehavior.AllowGet);
        }

        private Object TraerInformacionDeAlumno(string id)
        {
            var datosL = (from a in db.Alumnos
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

            var datos = (from dl in datosL
                         where dl.nombreAlumno.ToLower().Contains(id.ToLower())
                         select dl).FirstOrDefault();
            return datos;
        }
       
        //VERIFICA MODALIDAD DE GRADUACION POR EXAMEN DE GRADO
        private bool esExamenDeGrado(int idPerfil)
        {
            var modalidadGraduacion = (from p in db.Perfiles
                                       where p.iPerfil_id == idPerfil
                                       select new { modalidad = p.TipoGraduacion.tttg_descripcion.Trim() }).FirstOrDefault();

            if (modalidadGraduacion.modalidad.Equals("EXAMEN DE GRADO"))
            {
                return true;
            }
            return false;
        }
       
        //VERIFICA SI EL PERFIL ESTA APROBADO
        private bool PerfilEstaAprobado(int idPerfil)
        {
            var modalidadGraduacion = (from p in db.Perfiles
                                       where p.iPerfil_id == idPerfil
                                       select new { estadoPerfil = p.sPerfil_fl.ToString().Trim() }).FirstOrDefault();

            if (modalidadGraduacion.estadoPerfil.Equals("APROBADO"))
            {
                return true;
            }
            return false;
        }
        //TEREAS
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

        //CARGA VIEWBAGS
        private void CargarViewBags(Tarea tarea, gatbl_ComunicacionEst ComunicacionEst)
        {
            ViewBag.sLugarDefensa_desc = new SelectList(db.Salas.Where(p => p.iEliminado_fl == 1), "sNombre_nm", "sNombre_nm", ComunicacionEst.sLugarDefensa_desc);
            ViewBag.sLugarSorteo_desc = new SelectList(db.Salas.Where(p => p.iEliminado_fl == 1), "sNombre_nm", "sNombre_nm", ComunicacionEst.sLugarSorteo_desc);
            ViewBag.sLugarEntrega_desc = new SelectList(db.Salas.Where(p => p.iEliminado_fl == 1), "sNombre_nm", "sNombre_nm", ComunicacionEst.sLugarEntrega_desc);
            ViewBag.sPeriodo_desc = new SelectList(db.Semestres.OrderBy(p => p.sem_codigo), "sem_codigo", "sem_codigo", ComunicacionEst.sPeriodo_desc);
            if (tarea == Tarea.EDITAR)
            {
                ViewBag.datos = TraerInformacionDeAlumno(ComunicacionEst.lEstudiante_id);
                ViewBag.EntregadoPor = TraerInformacionDePersonaPorCodigo(ComunicacionEst.lEntregadoPor_id);
            }
        }

    [Permiso(Permiso = RolesPermisos.EGRE_comunicacionAlAlumno_puedeImprimir)]
    public ActionResult ImprimirComunicacionAlEstudiante(int? id)
        {
            try
            {
                var res = from ce in db.ComunicacionesEstudiantes
                          join p in db.Perfiles on ce.iPerfil_id equals p.iPerfil_id
                          join a in db.Alumnos on ce.lEstudiante_id equals a.alm_registro
                          join ag in db.Personas on a.agd_codigo equals ag.agd_codigo
                          join pe in db.Personas on ce.lEntregadoPor_id equals pe.agd_codigo
                          join c in db.Carreras on a.crr_codigo equals c.crr_codigo
                          join s in db.Facultades on c.sca_codigo equals s.sca_codigo
                          where ce.iComunicacionEst_id == id
                          select new
                          {
                              iComunicacionEst_id = ce.iComunicacionEst_id,
                              sComunicacionEst_fl = ce.sComunicacionEst_fl.ToString(),
                              dtComunicacionEst_dt = ce.dtComunicacionEst_dt,
                              dtDProgramada_dt = ce.dtDProgramada_dt,
                              dtHProgramada_dt = ce.dtHProgramada_dt,
                              sLugarDefensa_desc = ce.sLugarDefensa_desc,
                              dtSorteoArea_dt = ce.dtSorteoArea_dt,
                              dtHoraSorteo_dt = ce.dtHoraSorteo_dt,
                              sLugarSorteo_desc = ce.sLugarSorteo_desc,
                              sLugarEntrega_desc = ce.sLugarEntrega_desc,
                              dtHoraEntrega_dt = ce.dtHoraEntrega_dt,
                              sDescripcion_desc = ce.sDescripcion_desc,
                              crr_descripcion = c.crr_descripcion,
                              sObs_desc = ce.sObs_desc,
                              sTitulo_tfg = p.sTitulo_tfg,
                              alumno = ag.agd_nombres.Trim() + " " + ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim(),
                              receptor = pe.agd_nombres.Trim() + " " + pe.agd_appaterno.Trim() + " " + pe.agd_apmaterno.Trim(),
                              alm_registro = a.alm_registro,
                              numeracion = ce.iNumeracion_num,
                              facultad = s.sca_descripcion.Trim(),

                              modalidad = p.TipoGraduacion.tttg_descripcion.Trim()
                          };

                ReportDocument rd = new ReportDocument();
                var modalidad = res.Select(e => e.modalidad.Trim()).FirstOrDefault();
                var tipoComInterna = res.Select(e => e.sComunicacionEst_fl.Trim()).FirstOrDefault();
                switch (tipoComInterna)
                {
                    case "AUT_HABILITACION":

                        {
                            switch (modalidad)
                            {
                                case "EXAMEN DE GRADO":
                                    {
                                        rd.Load(Path.Combine(Server.MapPath("~/Areas/EGRE/Reportes"), "ReporteNoDefinido.rpt"));
                                        break;
                                    }
                                case "GRADUACION POR EXCELENCIA":
                                    {
                                        rd.Load(Path.Combine(Server.MapPath("~/Areas/EGRE/Reportes"), "ReporteNoDefinido.rpt"));
                                        break;
                                    }
                                case "PRACTICA EMPRESARIAL":
                                    {
                                        rd.Load(Path.Combine(Server.MapPath("~/Areas/EGRE/Reportes"), "ComAlEstudiante_AutHabilitacionTG.rpt"));
                                        break;
                                    }
                                case "PROYECTO DE GRADO":
                                    {
                                        rd.Load(Path.Combine(Server.MapPath("~/Areas/EGRE/Reportes"), "ComAlEstudiante_AutHabilitacionTG.rpt"));
                                        break;
                                    }
                            }
                            break;
                        }
                    case "PREDEFENSA":
                        {
                            rd.Load(Path.Combine(Server.MapPath("~/Areas/EGRE/Reportes"), "ReporteNoDefinido.rpt"));
                            break;
                        }
                    case "DEFENSA_FINAL":
                        {
                            switch (modalidad)
                            {
                                case "EXAMEN DE GRADO":
                                    {
                                        rd.Load(Path.Combine(Server.MapPath("~/Areas/EGRE/Reportes"), "ComAlEstudiante_ActoDefensaEG.rpt"));
                                        break;
                                    }
                                case "GRADUACION POR EXCELENCIA":
                                    {
                                        rd.Load(Path.Combine(Server.MapPath("~/Areas/EGRE/Reportes"), "ComAlEstudiante_ActoDefensaExcelencia.rpt"));
                                        break;
                                    }
                                case "PRACTICA EMPRESARIAL":
                                    {
                                        rd.Load(Path.Combine(Server.MapPath("~/Areas/EGRE/Reportes"), "ComAlEstudiante_ActoDefensaTG.rpt"));
                                        break;
                                    }
                                case "PROYECTO DE GRADO":
                                    {
                                        rd.Load(Path.Combine(Server.MapPath("~/Areas/EGRE/Reportes"), "ComAlEstudiante_ActoDefensaPG.rpt"));
                                        break;
                                    }
                            }
                            break;
                        }
                }

                rd.SetDataSource(res.ToList());
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                //return File(stream, "application/pdf", "ComAlEstudiante.pdf");
                return File(stream, "application/pdf");
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Mostrar el reporte, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        private int ObtenerNumeracion()
        {
            int numMaximoMasUno = 0;
            var anioActual = DateTime.Today.Year;
            var comuEstudianteAnio = db.ComunicacionesEstudiantes.Where(d => d.dtComunicacionEst_dt.Year == anioActual).ToList();
            var cantidad = comuEstudianteAnio.Count();
            if (cantidad > 0)
            {
                var numeroMaximo = comuEstudianteAnio.Max(e => e.iNumeracion_num);
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
