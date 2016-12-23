using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
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
    public class ActaDefensaFinalController : Controller
    {
        private EgresadosContext db = new EgresadosContext();

        [Permiso(Permiso = RolesPermisos.EGRE_actaDeDefensaFinal_puedeVerIndice)]
        public ActionResult Index(string criterio = null)
        {
            var actasDefensasFinales = db.ActasDefensasFinales.Where(p => p.iEliminado_fl == 1).Include(g => g.Alumno).Include(g => g.Evaluador1E).Include(g => g.Evaluador1I).Include(g => g.Evaluador2E).Include(g => g.Evaluador2I).Include(g => g.Perfil).Include(g => g.Presidente).Include(g => g.Secretario).Include(g => g.Representante).ToList();
            var actasDefensasFinalesFiltrados = actasDefensasFinales.Where(a => criterio == null ||
                                                                            a.alm_registro.Contains(criterio.ToLower()) ||
                                                                            a.Alumno.Persona.NombreCompleto.ToLower().Contains(criterio.ToLower()) ||
                                                                            a.Alumno.Carrera.crr_descripcion.ToLower().Contains(criterio.ToLower()) ||
                                                                            a.Perfil.TipoGraduacion.tttg_descripcion.ToLower().Contains(criterio.ToLower()) ||
                                                                            a.dtDefensa_dt.ToString().Contains(criterio.ToLower()) ||
                                                                            a.dtHoraDefensa_dt.ToString().Contains(criterio.ToLower()) ||
                                                                            a.sCalificacion_desc.ToString().ToLower().Contains(criterio.ToLower()) ||
                                                                            a.sResultadoDefensa_fl.ToString().ToLower().Contains(criterio.ToLower())
                                                                            ).ToList();
            if (Request.IsAjaxRequest())
            {
                if (!String.IsNullOrWhiteSpace(criterio))
                {
                    Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + actasDefensasFinalesFiltrados.Count() + " registros con los criterios especificados.");
                }
                return PartialView("_Index", actasDefensasFinalesFiltrados);
            }

            if (!String.IsNullOrWhiteSpace(criterio))
            {
                Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + actasDefensasFinalesFiltrados.Count() + " registros con los criterios especificados.");
            }
            return View(actasDefensasFinalesFiltrados);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_actaDeDefensaFinal_puedeVerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_ActaDefensaFinal ActaDefensaFinal = db.ActasDefensasFinales.Find(id);
            if (ActaDefensaFinal == null)
            {
                return HttpNotFound();
            }
            var informacionAlumno = TraerInformacionDeAlumno(ActaDefensaFinal.alm_registro);
            ViewBag.datos = informacionAlumno;
            return View(ActaDefensaFinal);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_actaDeDefensaFinal_puedeCrearNuevo)]
        public ActionResult Create()
        {
            gatbl_ActaDefensaFinal ActaDefensaFina = new gatbl_ActaDefensaFinal();
            ActaDefensaFina.iNumeracion_num = ObtenerNumeracion();
            CargarViewBags(Tarea.NUEVO, ActaDefensaFina);
            return View(ActaDefensaFina);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "lRepresentante_id,iActaDefensaFinal_id,sGestion_desc,sPeriodo_desc,iNumeracion_num,dtSorteo_dt,dtHora_dt,dtDefensa_dt,dtHoraDefensa_dt,sLugar_desc,dtFinalizacionDefensa_dt,dtHoraFinalizacion_dt,sResultadoDefensa_fl,sCalificacion_desc,sObs_desc,bActa_digital,iPerfil_id,alm_registro,lPresidente_id,lSecretario_id,lEvaluador1I_id,lEvaluador2I_id,lEvaluador1E_id,lEvaluador2E_id,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_ActaDefensaFinal ActaDefensaFinal, HttpPostedFileBase image)
        {
            try
            {
                if (!esExamenDeGrado(ActaDefensaFinal.iPerfil_id))
                {
                    ActaDefensaFinal.dtSorteo_dt = DateTime.Parse("1900-01-01");
                    ActaDefensaFinal.dtHora_dt = DateTime.Parse("1900-01-01 00:00");
                }
                //verificamos que temgamos imagen cargado
                if (image != null)
                {
                    var contettype = image.ContentType;
                    int length = image.ContentLength;
                    byte[] buffer = new byte[length];
                    image.InputStream.Read(buffer, 0, length);
                    ActaDefensaFinal.bActa_digital = buffer;
                }
                else {
                    ActaDefensaFinal.bActa_digital = null;
                }

                ActaDefensaFinal.iEstado_fl = true;
                ActaDefensaFinal.iEliminado_fl = 1;
                ActaDefensaFinal.sCreado_by = FrontUser.Get().EmailUtepsa;
                ActaDefensaFinal.iConcurrencia_id = 1;
                if (ModelState.IsValid)
                {
                    db.ActasDefensasFinales.Add(ActaDefensaFinal);
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Guardado correctamente.");
                    return RedirectToAction("Index");
                }
                Flash.Instance.Error("ERROR:   ", "No se pudo Guardar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                CargarViewBags(Tarea.NUEVO, ActaDefensaFinal);
                return View(ActaDefensaFinal);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:   ", "No se pudo Guardar el dato, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        [Permiso(Permiso = RolesPermisos.EGRE_actaDeDefensaFinal_puedeEditar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_ActaDefensaFinal ActaDefensaFinal = db.ActasDefensasFinales.Find(id);
            if (ActaDefensaFinal == null)
            {
                return HttpNotFound();
            }
            CargarViewBags(Tarea.EDITAR, ActaDefensaFinal);
            return View(ActaDefensaFinal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lRepresentante_id,iActaDefensaFinal_id,sGestion_desc,sPeriodo_desc,iNumeracion_num,dtSorteo_dt,dtHora_dt,dtDefensa_dt,dtHoraDefensa_dt,sLugar_desc,dtFinalizacionDefensa_dt,dtHoraFinalizacion_dt,sResultadoDefensa_fl,sCalificacion_desc,sObs_desc,bActa_digital,iPerfil_id,alm_registro,lPresidente_id,lSecretario_id,lEvaluador1I_id,lEvaluador2I_id,lEvaluador1E_id,lEvaluador2E_id,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_ActaDefensaFinal ActaDefensaFinal, HttpPostedFileBase image)
        {
            try
            {
                if (!esExamenDeGrado(ActaDefensaFinal.iPerfil_id))
                {
                    ActaDefensaFinal.dtSorteo_dt = DateTime.Parse("1900-01-01");
                    ActaDefensaFinal.dtHora_dt = DateTime.Parse("1900-01-01 00:00");
                }
                if (image != null)
                {
                    var contettype = image.ContentType;
                    int length = image.ContentLength;
                    byte[] buffer = new byte[length];
                    image.InputStream.Read(buffer, 0, length);
                    ActaDefensaFinal.bActa_digital = buffer;
                }
                else {
                    //verificamos si existe imaten en la BD
                    var imagen = db.ActasDefensasFinales.Where(a => a.iActaDefensaFinal_id == ActaDefensaFinal.iActaDefensaFinal_id).Select(i => i.bActa_digital).FirstOrDefault();
                    if (imagen != null)
                    {
                        ActaDefensaFinal.bActa_digital = imagen;
                    }
                    else {
                        ActaDefensaFinal.bActa_digital = null;
                    }
                }

                ActaDefensaFinal.iEliminado_fl = 1;
                ActaDefensaFinal.sCreado_by = FrontUser.Get().EmailUtepsa;
                ActaDefensaFinal.iConcurrencia_id += 1;

                if (ModelState.IsValid)
                {
                    db.Entry(ActaDefensaFinal).State = EntityState.Modified;
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Modificado correctamente.");
                    return RedirectToAction("Index");
                }
                CargarViewBags(Tarea.EDITAR, ActaDefensaFinal);
                Flash.Instance.Error("ERROR:   ", "No se pudo Modificar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                return View(ActaDefensaFinal);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:   ", "No se pudo Modificar el dato, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        [Permiso(Permiso = RolesPermisos.EGRE_actaDeDefensaFinal_puedeEliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_ActaDefensaFinal ActaDefensaFinal = db.ActasDefensasFinales.Find(id);
            if (ActaDefensaFinal == null)
            {
                return HttpNotFound();
            }
            var informacionAlumno = TraerInformacionDeAlumno(ActaDefensaFinal.alm_registro);
            ViewBag.datos = informacionAlumno;
            return View(ActaDefensaFinal);
        }

        //ELIMINAR POSST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var transaccion = db.Database.BeginTransaction())
            {
                try
                {
                    gatbl_ActaDefensaFinal ActaDefensaFinal = db.ActasDefensasFinales.Find(id);
                    ActaDefensaFinal.iEstado_fl = false;
                    ActaDefensaFinal.iEliminado_fl = 2;
                    ActaDefensaFinal.sCreado_by = FrontUser.Get().EmailUtepsa;
                    ActaDefensaFinal.iConcurrencia_id += 1;

                    db.Entry(ActaDefensaFinal).State = EntityState.Modified;
                    db.SaveChanges();

                    db.ActasDefensasFinales.Remove(ActaDefensaFinal);
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Eliminado correctamente.");
                    transaccion.Commit();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Flash.Instance.Error("ERROR:   ", "No se pudo Eliminar el dato, ha ocurrido el siguiente error: " + ex.Message);
                    transaccion.Rollback();
                    return RedirectToAction("Index");
                }
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


        //PARA BUSQUEDAS ASINCRONAS
        public JsonResult BuscarPorRegistro(string term)
        {
            List<string> personas = new List<string>();
            var personasAl = (from pr in db.Perfiles
                              join a in db.Alumnos on pr.lEstudiante_id equals a.alm_registro
                              join ag in db.Personas on a.agd_codigo equals ag.agd_codigo
                              where pr.iEliminado_fl == 1 && pr.sPerfil_fl.ToString() == "Aprobado" //validamos NoEsten Eliminados;  y el perfil este aprobado                                                                                                  
                              select new { alm_registro = ag.agd_nombres.Trim() + " " + ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + " (" + a.alm_registro.Trim() + ")" }).ToList();

            personas = (from pa in personasAl
                        where pa.alm_registro.ToLower().Contains(term.ToLower())//validamos NoEsten Eliminados; No Examen de Grado=02; y el perfil este aprobado
                        select pa.alm_registro).Take(10).ToList();

            return Json(personas, JsonRequestBehavior.AllowGet);
        }
        //CARGA INFORMACION DEL ALUMNO EN BASE A ID
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
        //PARA BUSQUEDAS ASINCRONAS
        public ActionResult Buscar(string term)
        {
            var infoAlumno = TraerInformacionDeAlumno(term);
            return Json(infoAlumno, JsonRequestBehavior.AllowGet);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_actaDeDefensaFinal_puedeImprimir)]
        public ActionResult ImprimirActaDefensaFinal(int? id)
        {
            try
            {
                var res = from df in db.ActasDefensasFinales
                          join p in db.Perfiles on df.iPerfil_id equals p.iPerfil_id
                          join a in db.Alumnos on p.lEstudiante_id equals a.alm_registro
                          join ag in db.Personas on a.agd_codigo equals ag.agd_codigo
                          join pr in db.Personas on df.lPresidente_id equals pr.agd_codigo
                          join se in db.Personas on df.lSecretario_id equals se.agd_codigo
                          join ji1 in db.Personas on df.lEvaluador1I_id equals ji1.agd_codigo
                          join ji2 in db.Personas on df.lEvaluador2I_id equals ji2.agd_codigo
                          join je1 in db.Personas on df.lEvaluador1E_id equals je1.agd_codigo
                          join je2 in db.Personas on df.lEvaluador2E_id equals je2.agd_codigo
                          join re in db.Personas on df.lRepresentante_id equals re.agd_codigo
                          join c in db.Carreras on a.crr_codigo equals c.crr_codigo
                        
                          where df.iActaDefensaFinal_id == id
                          select new
                          {
                              iActaDefensaFinal_id = df.iActaDefensaFinal_id,
                              sObs_desc = df.sObs_desc,
                              sTitulo_tfg = p.sTitulo_tfg,
                              dtHoraDefensa_dt = df.dtHoraDefensa_dt,
                              dtDefensa_dt = df.dtDefensa_dt,
                              sLugar_desc = df.sLugar_desc,
                              dtHora_dt = df.dtHora_dt,
                              alumno = ag.agd_nombres.Trim() + " " + ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim(),
                              alm_registro = a.alm_registro,
                              crr_descripcion = c.crr_descripcion,
                              presidente = pr.agd_prefijo.Trim() + " " + pr.agd_nombres.Trim() + " " + pr.agd_appaterno.Trim() + " " + pr.agd_apmaterno.Trim(),
                              secretaria = se.agd_prefijo.Trim() + " " + se.agd_nombres.Trim() + " " + se.agd_appaterno.Trim() + " " + se.agd_apmaterno.Trim(),
                              juradoInterno1 = ji1.agd_prefijo.Trim() + " " + ji1.agd_nombres.Trim() + " " + ji1.agd_appaterno.Trim() + " " + ji1.agd_apmaterno.Trim(),
                              juradoInterno2 = ji2.agd_prefijo.Trim() + " " + ji2.agd_nombres.Trim() + " " + ji2.agd_appaterno.Trim() + " " + ji2.agd_apmaterno.Trim(),
                              juradoExterno1 = je1.agd_prefijo.Trim() + " " + je1.agd_nombres.Trim() + " " + je1.agd_appaterno.Trim() + " " + je1.agd_apmaterno.Trim(),
                              juradoExterno2 = je2.agd_prefijo.Trim() + " " + je2.agd_nombres.Trim() + " " + je2.agd_appaterno.Trim() + " " + je2.agd_apmaterno.Trim(),
                              Representante = re.agd_prefijo.Trim() + " " + re.agd_nombres.Trim() + " " + re.agd_appaterno.Trim() + " " + re.agd_apmaterno.Trim(),
                              sResultadoDefensa_fl = (int)df.sResultadoDefensa_fl,
                              sCalificacion_desc = df.sCalificacion_desc,
                              dtHoraFinalizacion_dt = df.dtHoraFinalizacion_dt,
                              numeracion = df.iNumeracion_num,

                              modalidad = p.TipoGraduacion.tttg_descripcion.Trim()
                          };
                ReportDocument rd = new ReportDocument();

                var modalidad = res.Select(e => e.modalidad.Trim()).FirstOrDefault();
                switch (modalidad)
                {
                    case "EXAMEN DE GRADO":
                        {
                            rd.Load(Path.Combine(Server.MapPath("~/Areas/EGRE/Reportes"), "ActaDefensaFinalEG.rpt"));
                            break;
                        }
                    case "GRADUACION POR EXCELENCIA":
                        {
                            rd.Load(Path.Combine(Server.MapPath("~/Areas/EGRE/Reportes"), "ActaDefensaFinalExcelencia.rpt"));
                            break;
                        }
                    case "PRACTICA EMPRESARIAL":
                        {
                            rd.Load(Path.Combine(Server.MapPath("~/Areas/EGRE/Reportes"), "ActaDefensaFinalTG.rpt"));
                            break;
                        }
                    case "PROYECTO DE GRADO":
                        {
                            rd.Load(Path.Combine(Server.MapPath("~/Areas/EGRE/Reportes"), "ActaDefensaFinalPG.rpt"));
                            break;
                        }
                }
                rd.SetDataSource(res.ToList());
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                //return File(stream, "application/pdf", "ActaDefensaFinal.pdf");//guarda direcrametne en disco
                return File(stream, "application/pdf");//preViaualiza en otra ventana
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:   ", "No se pudo Mostrar el reporte, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        [Permiso(Permiso = RolesPermisos.EGRE_actaDeDefensaFinal_puedeVerActaDigitalizada)]
        public FileContentResult GetImage(int id)
        {
            var actaFinal = db.ActasDefensasFinales.FirstOrDefault(c => c.iActaDefensaFinal_id == id);
            if (actaFinal != null)
            {
                return File(actaFinal.bActa_digital, "image/jpeg");
            }
            else
            {
                return null;
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
                                 select new { codigoPersona = p.agd_codigo, nombrePersona = p.agd_nombres.Trim() + " " + p.agd_appaterno.Trim() + " " + p.agd_apmaterno.Trim() }).ToList();

            var datosPersona = (from f in listaPersonas
                                where f.codigoPersona.ToLower().Contains(codigo.ToLower())
                                select f).FirstOrDefault();
            return datosPersona;
        }

        private enum Tarea
        {
            NUEVO,
            EDITAR,
            ELIMINAR
        }
        //CARGA vIEWBAGS
        private void CargarViewBags(Tarea tarea, gatbl_ActaDefensaFinal ActaDefensaFinal)
        {
            ViewBag.sLugar_desc = new SelectList(db.Salas.Where(p => p.iEliminado_fl == 1).OrderBy(p => p.sNombre_nm), "sNombre_nm", "sNombre_nm", ActaDefensaFinal.sLugar_desc);
            ViewBag.sPeriodo_desc = new SelectList(db.Semestres.OrderBy(p => p.sem_codigo), "sem_codigo", "sem_codigo", ActaDefensaFinal.sPeriodo_desc);
            if (tarea == Tarea.EDITAR)
            {
                ViewBag.datos = TraerInformacionDeAlumno(ActaDefensaFinal.alm_registro);
                ViewBag.Presidente = TraerInformacionDePersonaPorCodigo(ActaDefensaFinal.lPresidente_id);
                ViewBag.Secretario = TraerInformacionDePersonaPorCodigo(ActaDefensaFinal.lSecretario_id);
                ViewBag.Evaluador1I = TraerInformacionDePersonaPorCodigo(ActaDefensaFinal.lEvaluador1I_id);
                ViewBag.Evaluador2I = TraerInformacionDePersonaPorCodigo(ActaDefensaFinal.lEvaluador2I_id);
                ViewBag.Evaluador1E = TraerInformacionDePersonaPorCodigo(ActaDefensaFinal.lEvaluador1E_id);
                ViewBag.Evaluador2E = TraerInformacionDePersonaPorCodigo(ActaDefensaFinal.lEvaluador2E_id);
                ViewBag.Representante = TraerInformacionDePersonaPorCodigo(ActaDefensaFinal.lRepresentante_id);
            }
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

        private int ObtenerNumeracion()
        {
            int numMaximoMasUno = 0;
            var anioActual = DateTime.Today.Year;
            var actaDefensaFinalAnio = db.ActasDefensasFinales.Where(d => d.dtDefensa_dt.Year == anioActual).ToList();
            var cantidad = actaDefensaFinalAnio.Count();
            if (cantidad > 0)
            {
                var numeroMaximo = actaDefensaFinalAnio.Max(e => e.iNumeracion_num);
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
