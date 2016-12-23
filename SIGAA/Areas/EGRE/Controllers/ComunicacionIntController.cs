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
    public class ComunicacionIntController : Controller
    {
        private EgresadosContext db = new EgresadosContext();

        [Permiso(Permiso = RolesPermisos.EGRE_comunicacionInterna_puedeVerIndice)]
        public ActionResult Index(string criterio = null)
        {
            var comunicacionesInternas = db.ComunicacionesInternas.Where(p => p.iEliminado_fl == 1).Include(g => g.Alumno).Include(g => g.AreaAdministrativa).Include(g => g.Perfil).Include(g => g.Persona).ToList();
            var comunicacionesInternasFiltrado = comunicacionesInternas.Where(c => criterio == null ||
                                                                              c.AreaAdministrativa.sNombre_nm.ToLower().Contains(criterio.ToLower()) ||
                                                                              c.dtComunicacionInt_dt.ToString().Contains(criterio.ToLower()) ||
                                                                              c.sTipoCom_fl.ToString().ToLower().Contains(criterio.ToLower()) ||
                                                                              c.lEstudiante_id.Contains(criterio.ToLower()) ||
                                                                              c.Alumno.Persona.NombreCompleto.ToLower().Contains(criterio.ToLower()) ||
                                                                              c.lEstudiante_id.ToLower().Contains(criterio.ToLower()) ||
                                                                              c.Perfil.TipoGraduacion.tttg_descripcion.ToLower().Contains(criterio.ToLower())
                                                                              ).ToList();
            var comunicacionesInternasFiltradoOrdenado = comunicacionesInternasFiltrado.OrderBy(ef => ef.Alumno.Persona.NombreCompleto).ThenBy(ef => ef.dtComunicacionInt_dt);

            if (Request.IsAjaxRequest())
            {
                if (!String.IsNullOrWhiteSpace(criterio))
                {
                    Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + comunicacionesInternasFiltradoOrdenado.Count() + " registros con los criterios especificados.");
                }
                return PartialView("_Index", comunicacionesInternasFiltradoOrdenado);
            }

            if (!String.IsNullOrWhiteSpace(criterio))
            {
                Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + comunicacionesInternasFiltradoOrdenado.Count() + " registros con los criterios especificados.");
            }
            return View(comunicacionesInternasFiltradoOrdenado);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_comunicacionInterna_puedeVerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_ComunicacionInt ComunicacionInt = db.ComunicacionesInternas.Find(id);
            if (ComunicacionInt == null)
            {
                return HttpNotFound();
            }
            ViewBag.datos = TraerInformacionDeAlumno(ComunicacionInt.lEstudiante_id);
            return View(ComunicacionInt);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_comunicacionInterna_puedeCrearNuevo)]
        public ActionResult Create()
        {
            gatbl_ComunicacionInt ComunicacionInt = new gatbl_ComunicacionInt();
            ComunicacionInt.iNumeracion_num = ObtenerNumeracion();
            CargarViewBags(Tarea.NUEVO, ComunicacionInt);
            return View(ComunicacionInt);
        }

        //CREAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "iComunicacionInt_id,sGestion_desc,sCopia1_nm,sCopia2_nm,sCopia3_nm,sCopia4_nm,sCopia5_nm,sPeriodo_desc,iNumeracion_num,sTipoCom_fl,dtComunicacionInt_dt,dtSorteo_dt,dtHora_dt,dtDefensa_dt,dtHoraEntregaCaso_dt,dtHoraDefensa_dt,sLugar_desc,sDescripcion_desc,sObs_desc,iAreaAdministrativa_id,iPerfil_id,lEstudiante_id,lEntregadoPor_id,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_ComunicacionInt ComunicacionInt)
        {
            try
            {
                if (!esExamenDeGrado(ComunicacionInt.iPerfil_id))//si no es examen de grado los valres para fecha y hora de sorteo y areaDefensa  colocamos NULOS
                {
                    ComunicacionInt.dtSorteo_dt = DateTime.Parse("1900-01-01");
                    ComunicacionInt.dtHora_dt = DateTime.Parse("1900-01-01 00:00");
                    ComunicacionInt.dtHoraEntregaCaso_dt = DateTime.Parse("1900-01-01 00:00");
                }
                var tipoDeComunicacion = ComunicacionInt.sTipoCom_fl.ToString();
                if (tipoDeComunicacion.Equals("DESIG_TRIB_INT_REVISION"))
                {
                    ComunicacionInt.dtDefensa_dt = DateTime.Parse("1900-01-01");
                    ComunicacionInt.dtHoraDefensa_dt = DateTime.Parse("1900-01-01 00:00");
                    ComunicacionInt.dtHoraEntregaCaso_dt = DateTime.Parse("1900-01-01 00:00");
                    ComunicacionInt.sLugar_desc = "s/n"; //"";
                }
                if (!tipoDeComunicacion.Equals("DESIG_TRIB_INT_REVISION"))
                {
                    //verifica que este aprobado
                    if (!PerfilEstaAprobado(ComunicacionInt.iPerfil_id))
                    {
                        throw new Exception("El perfil del alumno NO ESTÁ APROBADO, por favor verifique e intente nuevamente.");
                    }
                    //tambien verifica que se haya realizado la comunicacion al estudiante de su defensa final
                    if (!alumnoTieneComunicacionDeDefensaFinalDePerfil(ComunicacionInt.iPerfil_id, ComunicacionInt.lEstudiante_id))
                    {
                        throw new Exception("No se realizó la cumunicacion al alumno de su de Defensa Final, por favor verifique e intente nuevamente.");
                    }
                }

                ComunicacionInt.iEstado_fl = true;
                ComunicacionInt.iEliminado_fl = 1;
                ComunicacionInt.sCreado_by = FrontUser.Get().EmailUtepsa;
                ComunicacionInt.iConcurrencia_id = 1;

                if (ModelState.IsValid)
                {
                    db.ComunicacionesInternas.Add(ComunicacionInt);
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Guardado correctamente.");
                    return RedirectToAction("Index");
                }
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                CargarViewBags(Tarea.NUEVO, ComunicacionInt);
                return View(ComunicacionInt);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        [Permiso(Permiso = RolesPermisos.EGRE_comunicacionInterna_puedeEditar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_ComunicacionInt ComunicacionInt = db.ComunicacionesInternas.Find(id);
            if (ComunicacionInt == null)
            {
                return HttpNotFound();
            }
            CargarViewBags(Tarea.EDITAR, ComunicacionInt);
            return View(ComunicacionInt);
        }

        //EDITAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "iComunicacionInt_id,sGestion_desc,sCopia1_nm,sCopia2_nm,sCopia3_nm,sCopia4_nm,sCopia5_nm,sPeriodo_desc,iNumeracion_num,sTipoCom_fl,dtComunicacionInt_dt,dtSorteo_dt,dtHora_dt,dtDefensa_dt,tHoraEntregaCaso_dt,dtHoraDefensa_dt,sLugar_desc,sDescripcion_desc,sObs_desc,iAreaAdministrativa_id,iPerfil_id,lEstudiante_id,lEntregadoPor_id,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_ComunicacionInt ComunicacionInt)
        {
            try
            {
                if (!esExamenDeGrado(ComunicacionInt.iPerfil_id))//si no es examen de grado los valres para fecha y hora de sorteo y areaDefensa  colocamos NULOS
                {
                    ComunicacionInt.dtSorteo_dt = DateTime.Parse("1900-01-01");
                    ComunicacionInt.dtHora_dt = DateTime.Parse("1900-01-01 00:00");
                    ComunicacionInt.dtHoraEntregaCaso_dt = DateTime.Parse("1900-01-01 00:00");
                }
                var tipoDeComunicacion = ComunicacionInt.sTipoCom_fl.ToString();
                if (tipoDeComunicacion.Equals("DESIG_TRIB_INT_REVISION"))
                {
                    ComunicacionInt.dtDefensa_dt = DateTime.Parse("1900-01-01");
                    ComunicacionInt.dtHoraDefensa_dt = DateTime.Parse("1900-01-01 00:00");
                    ComunicacionInt.dtHoraEntregaCaso_dt = DateTime.Parse("1900-01-01 00:00");
                    ComunicacionInt.sLugar_desc = "s/n"; //"";
                }
                if (!tipoDeComunicacion.Equals("DESIG_TRIB_INT_REVISION"))
                {
                    //verifica que este aprobado
                    if (!PerfilEstaAprobado(ComunicacionInt.iPerfil_id))
                    {
                        throw new Exception("El perfil del alumno NO ESTÁ APROBADO, por favor verifique e intente nuevamente.");
                    }
                    //tambien verifica que se haya realizado la comunicacion al estudiante de su defensa final
                    if (!alumnoTieneComunicacionDeDefensaFinalDePerfil(ComunicacionInt.iPerfil_id, ComunicacionInt.lEstudiante_id))
                    {
                        throw new Exception("No se realizó la cumunicacion al alumno de su de Defensa Final, por favor verifique e intente nuevamente.");
                    }
                }

                ComunicacionInt.iEliminado_fl = 1;
                ComunicacionInt.sCreado_by = FrontUser.Get().EmailUtepsa;
                ComunicacionInt.iConcurrencia_id += 1;

                if (ModelState.IsValid)
                {
                    db.Entry(ComunicacionInt).State = EntityState.Modified;
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Modificado correctamente.");
                    return RedirectToAction("Index");
                }
                CargarViewBags(Tarea.EDITAR, ComunicacionInt);
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                return View(ComunicacionInt);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        [Permiso(Permiso = RolesPermisos.EGRE_comunicacionInterna_puedeEliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_ComunicacionInt ComunicacionInt = db.ComunicacionesInternas.Find(id);
            if (ComunicacionInt == null)
            {
                return HttpNotFound();
            }
            ViewBag.datos = TraerInformacionDeAlumno(ComunicacionInt.lEstudiante_id);
            return View(ComunicacionInt);
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
                    gatbl_ComunicacionInt ComunicacionInt = db.ComunicacionesInternas.Find(id);
                    ComunicacionInt.iEstado_fl = false;
                    ComunicacionInt.iEliminado_fl = 2;
                    ComunicacionInt.sCreado_by = FrontUser.Get().EmailUtepsa;
                    ComunicacionInt.iConcurrencia_id += 1;

                    db.Entry(ComunicacionInt).State = EntityState.Modified;
                    db.SaveChanges();

                    db.ComunicacionesInternas.Remove(ComunicacionInt);
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Eliminado correctamente.");
                    transaccion.Commit();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    Flash.Instance.Error("ERROR:    ", "No se pudo Eliminar el dato, ha ocurrido el siguiente error: " + ex.Message);
                    return RedirectToAction("Index");
                }
            }
        }

        //BUSCAR ASINCRONO, LISTA REGISTROS DE ACUERDO A UN CRITERIO EN FORMATO JSON, ES UTILIZADO PARA EL AUTOCOMPLETE
       
        public JsonResult BuscarPorRegistro(string term)
        {
            List<string> personas = new List<string>();

            var personasAl = (from pr in db.Perfiles
                              join a in db.Alumnos on pr.lEstudiante_id equals a.alm_registro
                              join ag in db.Personas on a.agd_codigo equals ag.agd_codigo
                              where pr.iEliminado_fl == 1 && !pr.sPerfil_fl.ToString().Equals("RECHAZADO") && !pr.sPerfil_fl.ToString().Equals("ANULADO") //validamos NoEsten Eliminados; No Examen de Grado=02; y el perfil este
                              select new { alm_registro = ag.agd_nombres.Trim() + " " + ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + " (" + a.alm_registro.Trim() + ")" }).ToList();

            personas = (from pa in personasAl
                        where pa.alm_registro.ToLower().Contains(term.ToLower()) //validamos NoEsten Eliminados; No Examen de Grado=02; y el perfil este aprobado
                        select pa.alm_registro).Take(10).ToList();

            return Json(personas, JsonRequestBehavior.AllowGet);
        }

        //BUSCAR DATOS ALUMNO ASINCRONO
        public ActionResult Buscar(string term)
        {
            var informacionAlumno = TraerInformacionDeAlumno(term);
            return Json(informacionAlumno, JsonRequestBehavior.AllowGet);
        }

        //OBTIENE INFORMACION DEL ALUMNO
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

        //VERIFICA SI SE COMUNICÓ AL ESTUDIANTE DE SU DEFENSA FINAL
        private bool alumnoTieneComunicacionDeDefensaFinalDePerfil(int idPerfil, string registroAlumno)
        {
            var modalidadGraduacion = (from p in db.Perfiles
                                       join ce in db.ComunicacionesEstudiantes on p.iPerfil_id equals ce.iPerfil_id
                                       where p.iPerfil_id == idPerfil && ce.lEstudiante_id.Equals(registroAlumno) && ce.sComunicacionEst_fl.ToString().Equals("DEFENSA_FINAL")
                                       select p).ToList();

            if (modalidadGraduacion.Count > 0)
            {
                return true;
            }
            return false;
        }

        [Permiso(Permiso = RolesPermisos.EGRE_comunicacionInterna_puedeImprimir)]
        public ActionResult ImprimirComunicacionInterna(int? id)
        {
            try
            {
                var res = from ci in db.ComunicacionesInternas
                          join p in db.Perfiles on ci.iPerfil_id equals p.iPerfil_id
                          join t in db.Tutorez on p.iTutuor_id equals t.iTutuor_id
                          join a in db.Alumnos on p.lEstudiante_id equals a.alm_registro
                          join ag in db.Personas on a.agd_codigo equals ag.agd_codigo
                          join c in db.Carreras on a.crr_codigo equals c.crr_codigo
                          join aa in db.AreasAdministrativas on ci.iAreaAdministrativa_id equals aa.iAreaAdministrativa_id
                          join s in db.Facultades on c.sca_codigo equals s.sca_codigo
                          where ci.iComunicacionInt_id == id
                          select new
                          {
                              iComunicacionInt_id = ci.iComunicacionInt_id,
                              dtComunicacionInt_dt = ci.dtComunicacionInt_dt,
                              alm_registro = a.alm_registro,
                              alumno = ag.agd_nombres.Trim() + " " + ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim(),
                              dtDefensa_dt = ci.dtDefensa_dt,
                              dtHoraDefensa_dt = ci.dtHoraDefensa_dt,
                              sLugar_desc = ci.sLugar_desc,
                              crr_descripcion = c.crr_descripcion,
                              sTitulo_tfg = p.sTitulo_tfg,
                              areaAdministrativa = aa.sNombre_nm,
                              sCoordinador_nm = aa.sCoordinador_nm,
                              sCopia1_nm = ci.sCopia1_nm,
                              sCopia2_nm = ci.sCopia2_nm,
                              sCopia3_nm = ci.sCopia3_nm,
                              sCopia4_nm = ci.sCopia4_nm,
                              sCopia5_nm = ci.sCopia5_nm,
                              dtHoraEntregaCaso_dt = ci.dtHoraEntregaCaso_dt,
                              dtSorteo_dt = ci.dtSorteo_dt,
                              dtHora_dt = ci.dtHora_dt,
                              tutor = t.sNombre_desc,
                              numeracion = ci.iNumeracion_num,
                              facultad = s.sca_descripcion.Trim(),

                              modalidad = p.TipoGraduacion.tttg_descripcion.Trim(),
                              tipoComInterna = ci.sTipoCom_fl.ToString()
                          };

                ReportDocument rd = new ReportDocument();
                var modalidad = res.Select(e => e.modalidad).FirstOrDefault();
                var tipoComunicacion = res.Select(c => c.tipoComInterna).FirstOrDefault();

                switch (tipoComunicacion)
                {
                    case "DESIG_TRIB_INT_REVISION":
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
                                        rd.Load(Path.Combine(Server.MapPath("~/Areas/EGRE/Reportes"), "ComInterna_Desig_TribInterno_P_Revision.rpt"));
                                        break;
                                    }
                                case "PROYECTO DE GRADO":
                                    {
                                        rd.Load(Path.Combine(Server.MapPath("~/Areas/EGRE/Reportes"), "ComInterna_Desig_TribInterno_P_Revision.rpt"));
                                        break;
                                    }
                            }
                            break;
                        }
                    case "DESIG_TRIB_INT_DEFENSA_FINAL":
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
                                        rd.Load(Path.Combine(Server.MapPath("~/Areas/EGRE/Reportes"), "ComInterna_Desig_TribInterno_P_defensaFinal.rpt"));
                                        break;
                                    }
                                case "PROYECTO DE GRADO":
                                    {
                                        rd.Load(Path.Combine(Server.MapPath("~/Areas/EGRE/Reportes"), "ComInterna_Desig_TribInterno_P_defensaFinal.rpt"));
                                        break;
                                    }
                            }
                            break;
                        }
                    case "DEFENSA_FINAL":
                        {
                            switch (modalidad)
                            {
                                case "EXAMEN DE GRADO":
                                    {
                                        rd.Load(Path.Combine(Server.MapPath("~/Areas/EGRE/Reportes"), "ComInterna_ActoDefensaFinalEG.rpt"));
                                        break;
                                    }
                                case "GRADUACION POR EXCELENCIA":
                                    {
                                        rd.Load(Path.Combine(Server.MapPath("~/Areas/EGRE/Reportes"), "ComInterna_ActoDefensaFinalExcelencia.rpt"));
                                        break;
                                    }
                                case "PRACTICA EMPRESARIAL":
                                    {
                                        rd.Load(Path.Combine(Server.MapPath("~/Areas/EGRE/Reportes"), "ComInterna_ActoDefensaFinalTG.rpt"));
                                        break;
                                    }
                                case "PROYECTO DE GRADO":
                                    {
                                        rd.Load(Path.Combine(Server.MapPath("~/Areas/EGRE/Reportes"), "ComInterna_ActoDefensaFinalPG.rpt"));
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
                //return File(stream, "application/pdf", "ComunicacionInterna.pdf");
                return File(stream, "application/pdf");
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Mostrar el reporte, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
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
        private void CargarViewBags(Tarea tarea, gatbl_ComunicacionInt ComunicacionInt)
        {
            ViewBag.sLugar_desc = new SelectList(db.Salas.Where(p => p.iEliminado_fl == 1).OrderBy(p => p.sNombre_nm), "sNombre_nm", "sNombre_nm", ComunicacionInt.sLugar_desc);
            //ViewBag.sDefenderArea_desc = new SelectList(db.AreasDefensas.OrderBy(p => p.dsc_descripcion), "dsc_descripcion", "dsc_descripcion", ComunicacionInt.sDefenderArea_desc);
            ViewBag.sPeriodo_desc = new SelectList(db.Semestres.OrderBy(p => p.sem_codigo), "sem_codigo", "sem_codigo", ComunicacionInt.sPeriodo_desc);
            ViewBag.iAreaAdministrativa_id = new SelectList(db.AreasAdministrativas.Where(p => p.iEliminado_fl == 1).OrderBy(p => p.sNombre_nm), "iAreaAdministrativa_id", "sNombre_nm", ComunicacionInt.iAreaAdministrativa_id);

            if (tarea == Tarea.EDITAR)
            {
                ViewBag.datos = TraerInformacionDeAlumno(ComunicacionInt.lEstudiante_id);
                ViewBag.EntregadoPor = TraerInformacionDePersonaPorCodigo(ComunicacionInt.lEntregadoPor_id);
            }
        }

        private int ObtenerNumeracion()
        {
            int numMaximoMasUno = 0;
            var anioActual = DateTime.Today.Year;
            var comuEntAnio = db.ComunicacionesInternas.Where(d => d.dtComunicacionInt_dt.Year == anioActual).ToList();
            var cantidad = comuEntAnio.Count();
            if (cantidad > 0)
            {
                var numeroMaximo = comuEntAnio.Max(e => e.iNumeracion_num);
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
