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
    public class ComunicacionExtUnivPublicaController : Controller
    {
        private EgresadosContext db = new EgresadosContext();

        [Permiso(Permiso = RolesPermisos.EGRE_comunicacionExternaUnivPublica_puedeVerIndice)]
        public ActionResult Index(string criterio = null)
        {
            var comunicacionesExternas = db.ComExternasUniversidadesPublicas.Where(p => p.iEliminado_fl == 1).Include(g => g.Alumno).Include(g => g.Perfil).Include(g => g.Persona).Include(g => g.RectorUniversidadPublica).ToList();
            var comunicacionesExternasFiltradas = comunicacionesExternas.Where(c => criterio == null ||
                                                                         c.RectorUniversidadPublica.Universidad.unv_descripcion.ToLower().Contains(criterio.ToLower()) ||
                                                                         c.RectorUniversidadPublica.sNombreCompleto_desc.ToLower().Contains(criterio.ToLower()) ||
                                                                         c.iNumeracion_num.ToString().Contains(criterio.ToLower()) ||
                                                                         c.dtComunicacionExt_dt.ToString().Contains(criterio.ToLower()) ||
                                                                         c.sTipoCom_fl.ToString().ToLower().Contains(criterio.ToLower()) ||
                                                                         c.Alumno.Persona.NombreCompleto.ToLower().Contains(criterio.ToLower()) ||
                                                                          c.lEstudiante_id.Contains(criterio.ToLower()) ||
                                                                         c.Perfil.TipoGraduacion.tttg_descripcion.ToLower().Contains(criterio.ToLower())
                                                                         ).ToList();
            var comunicacionesExternasFiltradasOrdenadas = comunicacionesExternasFiltradas.OrderBy(ef => ef.Alumno.Persona.NombreCompleto).ThenBy(ef => ef.dtComunicacionExt_dt);

            if (Request.IsAjaxRequest())
            {
                if (!String.IsNullOrWhiteSpace(criterio))
                {
                    Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + comunicacionesExternasFiltradasOrdenadas.Count() + " registros con los criterios especificados.");
                }
                return PartialView("_Index", comunicacionesExternasFiltradasOrdenadas);
            }

            if (!String.IsNullOrWhiteSpace(criterio))
            {
                Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + comunicacionesExternasFiltradasOrdenadas.Count() + " registros con los criterios especificados.");
            }
            return View(comunicacionesExternasFiltradasOrdenadas);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_comunicacionExternaUnivPublica_puedeVerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_ComunicacionExtUnivPublica ComunicacionExt = db.ComExternasUniversidadesPublicas.Find(id);
            if (ComunicacionExt == null)
            {
                return HttpNotFound();
            }
            ViewBag.datos = TraerInformacionDeAlumno(ComunicacionExt.lEstudiante_id);
            return View(ComunicacionExt);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_comunicacionExternaUnivPublica_puedeCrearNuevo)]
        public ActionResult Create()
        {
            gatbl_ComunicacionExtUnivPublica ComunicacionExt = new gatbl_ComunicacionExtUnivPublica();
            ComunicacionExt.iNumeracion_num = ObtenerNumeracion();
            CargarViewBags(Tarea.NUEVO, ComunicacionExt);
            return View(ComunicacionExt);
        }

        //CREAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "iComExtUnivPublica_id,sGestion_desc,sPeriodo_desc,iNumeracion_num,sTipoCom_fl,dtComunicacionExt_dt,dtSorteo_dt,dtHora_dt,dtDefensa_dt,dtHoraDefensa_dt,sLugar_desc,iPerfil_id,lEstudiante_id,lEntregadoPor_id,iRectorUnivPublica_id,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_ComunicacionExtUnivPublica ComunicacionExt)
        {
            try
            {
                if (!esExamenDeGrado(ComunicacionExt.iPerfil_id))//si no es examen de grado los valres para fecha y hora de sorteo y areaDefensa equivalentes a nulos
                {
                    ComunicacionExt.dtSorteo_dt = DateTime.Parse("1900-01-01");
                    ComunicacionExt.dtHora_dt = DateTime.Parse("1900-01-01 00:00");
                }
                ComunicacionExt.iEstado_fl = true;
                ComunicacionExt.iEliminado_fl = 1;
                ComunicacionExt.sCreado_by = FrontUser.Get().EmailUtepsa;
                ComunicacionExt.iConcurrencia_id = 1;

                if (ModelState.IsValid)
                {
                    db.ComExternasUniversidadesPublicas.Add(ComunicacionExt);
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Guardado correctamente.");
                    return RedirectToAction("Index");
                }
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                CargarViewBags(Tarea.NUEVO, ComunicacionExt);
                return View(ComunicacionExt);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        [Permiso(Permiso = RolesPermisos.EGRE_comunicacionExternaUnivPublica_puedeEditar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_ComunicacionExtUnivPublica ComunicacionExt = db.ComExternasUniversidadesPublicas.Find(id);
            if (ComunicacionExt == null)
            {
                return HttpNotFound();
            }
            CargarViewBags(Tarea.EDITAR, ComunicacionExt);
            return View(ComunicacionExt);
        }

        //EDITAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "iComExtUnivPublica_id,sGestion_desc,sPeriodo_desc,iNumeracion_num,sTipoCom_fl,dtComunicacionExt_dt,dtSorteo_dt,dtHora_dt,dtDefensa_dt,dtHoraDefensa_dt,sLugar_desc,iPerfil_id,lEstudiante_id,lEntregadoPor_id,iRectorUnivPublica_id,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_ComunicacionExtUnivPublica ComunicacionExt)
        {           
            try
            {
                if (!esExamenDeGrado(ComunicacionExt.iPerfil_id))//si no es examen de grado los valres para fecha y hora de sorteo y areaDefensa  colocamos NULOS
                {
                    ComunicacionExt.dtSorteo_dt = DateTime.Parse("1900-01-01");
                    ComunicacionExt.dtHora_dt = DateTime.Parse("1900-01-01 00:00");
                }

                ComunicacionExt.iEliminado_fl = 1;
                ComunicacionExt.sCreado_by = FrontUser.Get().EmailUtepsa;
                ComunicacionExt.iConcurrencia_id += 1;

                if (ModelState.IsValid)
                {
                    db.Entry(ComunicacionExt).State = EntityState.Modified;
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Modificado correctamente.");
                    return RedirectToAction("Index");
                }
                CargarViewBags(Tarea.EDITAR, ComunicacionExt);
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                return View(ComunicacionExt);
            }
            catch (Exception ex)
            {               
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        [Permiso(Permiso = RolesPermisos.EGRE_comunicacionExternaUnivPublica_puedeEliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_ComunicacionExtUnivPublica ComunicacionExt = db.ComExternasUniversidadesPublicas.Find(id);
            if (ComunicacionExt == null)
            {
                return HttpNotFound();
            }

            ViewBag.datos = TraerInformacionDeAlumno(ComunicacionExt.lEstudiante_id);
            return View(ComunicacionExt);
        }

        //ELIMINAR POST:
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {            
            using (var transaccion = db.Database.BeginTransaction())
            {
                try
                {
                    gatbl_ComunicacionExtUnivPublica ComunicacionExt = db.ComExternasUniversidadesPublicas.Find(id);
                    ComunicacionExt.iEstado_fl = false;
                    ComunicacionExt.iEliminado_fl = 2;
                    ComunicacionExt.sCreado_by = FrontUser.Get().EmailUtepsa;
                    ComunicacionExt.iConcurrencia_id += 1;

                    db.Entry(ComunicacionExt).State = EntityState.Modified;
                    db.SaveChanges();

                    db.ComExternasUniversidadesPublicas.Remove(ComunicacionExt);
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

        [Permiso(Permiso = RolesPermisos.EGRE_comunicacionExternaUnivPublica_puedeImprimir)]
        public ActionResult ImprimirComunicacionExterna(int? id)
        {
            var res = from ce in db.ComExternasUniversidadesPublicas
                      join p in db.Perfiles on ce.iPerfil_id equals p.iPerfil_id
                      join t in db.Tutorez on p.iTutuor_id equals t.iTutuor_id
                      join a in db.Alumnos on ce.lEstudiante_id equals a.alm_registro
                      join ag in db.Personas on a.agd_codigo equals ag.agd_codigo
                      join c in db.Carreras on a.crr_codigo equals c.crr_codigo
                      join na in db.NivelesAcademicos on c.nva_codigo equals na.Nva_codigo
                      join r in db.RectoresUniversidadesPublicas on ce.iRectorUnivPublica_id equals r.iRectorUnivPublica_id
                      join u in db.Universidades on r.unv_codigo equals u.unv_codigo
                      join age in db.Personas on ce.lEntregadoPor_id equals age.agd_codigo
                      where ce.iComExtUnivPublica_id == id
                      select new
                      {
                          iComExtUnivPublica_id = ce.iComExtUnivPublica_id,//cambiar esto en el procedimiento almacenado
                          dtComunicacionExt_dt = ce.dtComunicacionExt_dt,
                          alm_registro = a.alm_registro,
                          alumno = ag.agd_nombres.Trim() + " " + ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim(),
                          Universidad = u.unv_descripcion,
                          rectorUniv = r.sNombreCompleto_desc,
                          realizadoPor = age.agd_prefijo.Trim() + " " + age.agd_nombres.Trim() + " " + age.agd_appaterno.Trim() + " " + age.agd_apmaterno.Trim(),
                          SiglaUniv = u.unv_sigla,
                          sLugar_desc = ce.sLugar_desc,
                          Carrera = c.crr_descripcion,
                          NivelAcademico = na.Nva_descripcion,
                          TituloTFG = p.sTitulo_tfg,
                          dtSorteo_dt = ce.dtSorteo_dt,
                          dtHora_dt = ce.dtHora_dt,
                          dtDefensa_dt = ce.dtDefensa_dt,
                          dtHoraDefensa_dt = ce.dtHoraDefensa_dt,
                          tutorGuia = t.sNombre_desc,
                          numero = ce.iNumeracion_num,

                          modalidad = p.TipoGraduacion.tttg_descripcion.Trim(),
                          tipoComExterna = ce.sTipoCom_fl.ToString()
                      };

            ReportDocument rd = new ReportDocument();
            var contador = res.Count();
            var modalidad = res.Select(e => e.modalidad).FirstOrDefault();
            var tipoComunicacion = res.Select(c => c.tipoComExterna).FirstOrDefault();


            switch (modalidad)
            {
                case "EXAMEN DE GRADO":
                    {
                        rd.Load(Path.Combine(Server.MapPath("~/Areas/EGRE/Reportes"), "ComExterna_DefensaFinalUnivPubEG.rpt"));
                        break;
                    }
                case "GRADUACION POR EXCELENCIA":
                    {
                        rd.Load(Path.Combine(Server.MapPath("~/Areas/EGRE/Reportes"), "ComExterna_DefensaFinalUnivPubExcelencia.rpt"));
                        break;
                    }
                case "PRACTICA EMPRESARIAL":
                    {
                        rd.Load(Path.Combine(Server.MapPath("~/Areas/EGRE/Reportes"), "ComExterna_DefensaFinalUnivPubPG.rpt"));
                        break;
                    }
                case "PROYECTO DE GRADO":
                    {
                        rd.Load(Path.Combine(Server.MapPath("~/Areas/EGRE/Reportes"), "ComExterna_DefensaFinalUnivPubPG.rpt"));
                        break;
                    }
            }

            rd.SetDataSource(res.ToList());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                //return File(stream, "application/pdf", "ComunicacionExterna.pdf");
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

        //LISTA REGISTROS PARA EL AUTOCOMPLETE DE ALUMNO *1
        public JsonResult BuscarPorRegistro(string term)
        {
            List<string> personas = new List<string>();

            var personasAl = (from pr in db.Perfiles
                              join a in db.Alumnos on pr.lEstudiante_id equals a.alm_registro
                              join ag in db.Personas on a.agd_codigo equals ag.agd_codigo
                              where pr.iEliminado_fl == 1 && pr.sPerfil_fl.ToString() == "Aprobado" //validamos NoEsten Eliminados; No Examen de Grado=02; y el perfil 
                              select new { alm_registro = ag.agd_nombres.Trim() + " " + ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + " (" + a.alm_registro.Trim() + ")" }).ToList();

            personas = (from pa in personasAl
                        where pa.alm_registro.ToLower().Contains(term.ToLower()) //validamos NoEsten Eliminados; No Examen de Grado=02; y el perfil este aprobado
                        select pa.alm_registro).Take(10).ToList();

            return Json(personas, JsonRequestBehavior.AllowGet);
        }

        //BUSCAR DATOS CON EL ITEM SELECCIONADO DEL AUTOCOMPLETE DE ALUMNO *2  Y DEVUELVE EN FORMATO JSON
        public ActionResult Buscar(string term)
        {
            var informacionAlumno = TraerInformacionDeAlumno(term);
            return Json(informacionAlumno, JsonRequestBehavior.AllowGet);
        }
        //BUSCAR DATOS CON EL ITEM SELECCIONADO DEL AUTOCOMPLETE DE ALUMNO *3
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

        //LISTA PERSONAS PARA EL AUTOCOMPLETE DE RECEPCIONADO POR *1
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

        //BUSCAR DATOS CON EL ITEM SELECCIONADO DEL AUTOCOMPLETE DE LA PERSONA *2  Y DEVUELVE EN FORMATO JSON
        public ActionResult TraerInformacionDePersona_Json(string term)
        {
            var infoAlumno = TraerInformacionDePersonaPorNombre(term);
            return Json(infoAlumno, JsonRequestBehavior.AllowGet);
        }

        //BUSCAR DATOS CON EL ITEM SELECCIONADO DEL AUTOCOMPLETE DE LA PERSONA *3
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
        private void CargarViewBags(Tarea tarea, gatbl_ComunicacionExtUnivPublica ComunicComunicacionExt)
        {
            ViewBag.sLugar_desc = new SelectList(db.Salas.Where(s => s.iEliminado_fl == 1), "sNombre_nm", "sNombre_nm", ComunicComunicacionExt.sLugar_desc);
            ViewBag.iRectorUnivPublica_id = new SelectList(db.RectoresUniversidadesPublicas.Where(s => s.iEliminado_fl == 1), "iRectorUnivPublica_id", "sNombreCompleto_desc", ComunicComunicacionExt.iRectorUnivPublica_id);
            ViewBag.sPeriodo_desc = new SelectList(db.Semestres.OrderBy(p => p.sem_codigo), "sem_codigo", "sem_codigo", ComunicComunicacionExt.sPeriodo_desc);

            if (tarea == Tarea.EDITAR)
            {
                ViewBag.datos = TraerInformacionDeAlumno(ComunicComunicacionExt.lEstudiante_id);
                ViewBag.EntregadoPor = TraerInformacionDePersonaPorCodigo(ComunicComunicacionExt.lEntregadoPor_id);
            }
        }

        //TEREAS
        private enum Tarea
        {
            NUEVO,
            EDITAR,
            ELIMINAR
        }

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

        private int ObtenerNumeracion()
        {
            int numMaximoMasUno = 0;
            var anioActual = DateTime.Today.Year;
            var comuExtAnio = db.ComExternasUniversidadesPublicas.Where(d => d.dtComunicacionExt_dt.Year == anioActual).ToList();
            var cantidad = comuExtAnio.Count();
            if (cantidad > 0)
            {
                var numeroMaximo = comuExtAnio.Max(e => e.iNumeracion_num);
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
