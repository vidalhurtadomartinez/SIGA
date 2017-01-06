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
using SIGAA.Areas.EGRE.ViewModel;
using SIGAA.Commons;
using SIGAA.Etiquetas;

namespace SIGAA.Areas.EGRE.Controllers
{
    [Autenticado]
    public class PerfilController : Controller
    {
        private EgresadosContext db = new EgresadosContext();

        [Permiso(Permiso = RolesPermisos.EGRE_perfil_puedeVerIndice)]
        public ActionResult Index(string criterio = null)
        {
            var perfiles = db.Perfiles.Where(p => p.iEliminado_fl == 1).Include(g => g.Alumno)
                .Include(g => g.RecepcionadoPor).Include(g => g.TipoGraduacion)
                .ToList();
            var comEstudiantesDefensaFinal = db.ComunicacionesEstudiantes.Where(c => c.sComunicacionEst_fl == ParEstadoComunicacionEst.DEFENSA_FINAL).ToList();
            List<gatbl_PerfilesView> perfilesView = new List<gatbl_PerfilesView>();
            foreach (var perfil in perfiles)
            {
                var fechaDefensaFinalProgramada = comEstudiantesDefensaFinal.Where(d => d.iPerfil_id == perfil.iPerfil_id).Select(e => e.dtDProgramada_dt).FirstOrDefault();
                if (string.IsNullOrEmpty(fechaDefensaFinalProgramada.ToString()))
                {
                    //fechaDefensaFinalProgramada = "Sin Definir";
                }
                perfilesView.Add(new gatbl_PerfilesView
                {
                    iPerfil_id = perfil.iPerfil_id,
                    sGestion_desc = perfil.sGestion_desc,
                    sPeriodo_desc = perfil.sPeriodo_desc,
                    dtSolicitud_dt = perfil.dtSolicitud_dt,
                    sTitulo_tfg = perfil.sTitulo_tfg,
                    sPerfil_fl = perfil.sPerfil_fl,
                    dtRevision_dt = perfil.dtRevision_dt,
                    sObsRevision_desc = perfil.sObsRevision_desc,
                    dtEntregado_dt = perfil.dtEntregado_dt,
                    sModalidad_fl = perfil.sModalidad_fl,
                    iTutuor_id = perfil.iTutuor_id,
                    lEstudiante_id = perfil.lEstudiante_id,
                    lEntregaForm_id = perfil.lEntregaForm_id,
                    lRecepciona_id = perfil.lRecepciona_id,
                    iEstado_fl = perfil.iEstado_fl,
                    iEliminado_fl = perfil.iEliminado_fl,
                    sCreado_by = perfil.sCreado_by,
                    iConcurrencia_id = perfil.iConcurrencia_id,

                    dtDProgramada_dt = fechaDefensaFinalProgramada,
                    NombreCompleto = perfil.Alumno.Persona.NombreCompleto.Trim(),
                    Carrera = perfil.Alumno.Carrera.crr_descripcion.Trim(),
                    TipoGraduacionS = perfil.TipoGraduacion.tttg_descripcion.Trim(),
                });
            }

            var perfilesViewFiltrados = perfilesView.Where(p => criterio == null ||
                                                    p.NombreCompleto.ToLower().Contains(criterio.ToLower()) ||
                                                    p.lEstudiante_id.ToLower().Contains(criterio.ToLower()) ||
                                                    p.Carrera.ToLower().Contains(criterio.ToLower()) ||
                                                    p.dtDProgramada_dt.ToString().ToLower().Contains(criterio.ToLower()) ||
                                                    p.dtEntregado_dt.ToString().ToLower().Contains(criterio.ToLower()) ||
                                                    p.TipoGraduacionS.ToLower().Contains(criterio.ToLower()) ||
                                                    p.sPerfil_fl.ToString().ToLower().Contains(criterio.ToLower())
                                                    ).ToList();

            var eperfilesFiltradosOrdenadas = perfilesViewFiltrados.OrderBy(ef => ef.NombreCompleto);
            if (Request.IsAjaxRequest())
            {
                if (!String.IsNullOrWhiteSpace(criterio))
                {
                    Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + eperfilesFiltradosOrdenadas.Count() + " registros con los criterios especificados.");
                }           
                return PartialView("_Index", eperfilesFiltradosOrdenadas);
            }

            if (!String.IsNullOrWhiteSpace(criterio))
            {
                Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + eperfilesFiltradosOrdenadas.Count() + " registros con los criterios especificados.");
            }
            return View(eperfilesFiltradosOrdenadas);
        }


        [HttpPost]
        public ActionResult BuscarPorRangoDeFechas(DateTime fechaInicio, DateTime fechaFinal)
        {
            var perfiles = db.Perfiles.Where(p => p.iEliminado_fl == 1).Include(g => g.Alumno)
                .Include(g => g.RecepcionadoPor).Include(g => g.TipoGraduacion)
                .ToList();
            var comEstudiantesDefensaFinal = db.ComunicacionesEstudiantes.Where(c => c.sComunicacionEst_fl == ParEstadoComunicacionEst.DEFENSA_FINAL).ToList();
            List<gatbl_PerfilesView> perfilesView = new List<gatbl_PerfilesView>();
            foreach (var perfil in perfiles)
            {
                var fechaDefensaFinalProgramada = comEstudiantesDefensaFinal.Where(d => d.iPerfil_id == perfil.iPerfil_id).Select(e => e.dtDProgramada_dt).FirstOrDefault();
                if (string.IsNullOrEmpty(fechaDefensaFinalProgramada.ToString()))
                {
                    //fechaDefensaFinalProgramada = "Sin Definir";
                }
                perfilesView.Add(new gatbl_PerfilesView
                {
                    iPerfil_id = perfil.iPerfil_id,
                    sGestion_desc = perfil.sGestion_desc,
                    sPeriodo_desc = perfil.sPeriodo_desc,
                    dtSolicitud_dt = perfil.dtSolicitud_dt,
                    sTitulo_tfg = perfil.sTitulo_tfg,
                    sPerfil_fl = perfil.sPerfil_fl,
                    dtRevision_dt = perfil.dtRevision_dt,
                    sObsRevision_desc = perfil.sObsRevision_desc,
                    dtEntregado_dt = perfil.dtEntregado_dt,
                    sModalidad_fl = perfil.sModalidad_fl,
                    iTutuor_id = perfil.iTutuor_id,
                    lEstudiante_id = perfil.lEstudiante_id,
                    lEntregaForm_id = perfil.lEntregaForm_id,
                    lRecepciona_id = perfil.lRecepciona_id,
                    iEstado_fl = perfil.iEstado_fl,
                    iEliminado_fl = perfil.iEliminado_fl,
                    sCreado_by = perfil.sCreado_by,
                    iConcurrencia_id = perfil.iConcurrencia_id,

                    dtDProgramada_dt = fechaDefensaFinalProgramada,
                    NombreCompleto = perfil.Alumno.Persona.NombreCompleto.Trim(),
                    Carrera = perfil.Alumno.Carrera.crr_descripcion.Trim(),
                    TipoGraduacionS = perfil.TipoGraduacion.tttg_descripcion.Trim(),
                });
            }

            var perfilesViewFiltrados = perfilesView.Where(p => p.dtDProgramada_dt >= fechaInicio && p.dtDProgramada_dt <= fechaFinal).ToList();

            var eperfilesFiltradosOrdenadas = perfilesViewFiltrados.OrderBy(ef => ef.NombreCompleto);
            if (Request.IsAjaxRequest())
            {
                Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado "+eperfilesFiltradosOrdenadas.Count()+" registros con los criterios especificados.");
                return PartialView("_Index", eperfilesFiltradosOrdenadas);
            }
            Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + eperfilesFiltradosOrdenadas.Count() + " registros con los criterios especificados.");
            return View(eperfilesFiltradosOrdenadas);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_perfil_puedeVerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            gatbl_Perfiles Perfil = db.Perfiles.Find(id);
            if (Perfil == null)
            {
                return HttpNotFound();
            }
            ViewBag.datos = TraerInformacionDeAlumno(Perfil.lEstudiante_id);
            return View(Perfil);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_perfil_puedeCrearNuevo)]
        public ActionResult Create()
        {
            gatbl_Perfiles Perfil = new gatbl_Perfiles();
            cargarViewBag(Perfil);
            return View(Perfil);
        }

        //CREAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "iPerfil_id,sGestion_desc,sPeriodo_desc,dtSolicitud_dt,sTitulo_tfg,sObsRevision_desc,sPerfil_fl,dtRevision_dt,dtEntregado_dt,sModalidad_fl,iTutuor_id,lEstudiante_id,lEntregaForm_id,lRecepciona_id,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_Perfiles Perfil)
        {
            try
            {
                if ((Perfil.sModalidad_fl.Trim() == "02") || (Perfil.sModalidad_fl.Trim() == "03"))
                {
                    var idTutorVacio = db.Tutorez.Where(n => n.sNombre_desc.Trim() == "S/N").Select(n => n.iTutuor_id).FirstOrDefault();
                    Perfil.sTitulo_tfg = "";
                    Perfil.iTutuor_id = idTutorVacio;
                    Perfil.dtRevision_dt = DateTime.Parse("1900-01-01");
                    Perfil.sObsRevision_desc = "";
                    Perfil.dtSolicitud_dt = DateTime.Parse("1900-01-01");
                    Perfil.lEntregaForm_id = null;
                }

                Perfil.iEstado_fl = true;
                Perfil.iEliminado_fl = 1;
                Perfil.sCreado_by = FrontUser.Get().usr_login;
                Perfil.iConcurrencia_id = 1;

                if (ModelState.IsValid)
                {
                    db.Perfiles.Add(Perfil);
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Guardado correctamente.");
                    return RedirectToAction("Index");
                }
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                cargarViewBag(Perfil);
                return View(Perfil);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        [Permiso(Permiso = RolesPermisos.EGRE_perfil_puedeEditar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_Perfiles Perfil = db.Perfiles.Find(id);
            if (Perfil == null)
            {
                return HttpNotFound();
            }
            var informacionAlumno = TraerInformacionDeAlumno(Perfil.lEstudiante_id);
            ViewBag.datos = informacionAlumno;

            var infoPersonaForm = TraerInformacionDePersonaPorCodigo(Perfil.lEntregaForm_id);
            ViewBag.datosForm = infoPersonaForm;
            var infoPersonaRecepciona = TraerInformacionDePersonaPorCodigo(Perfil.lRecepciona_id);
            ViewBag.datosRecep = infoPersonaRecepciona;
            cargarViewBag(Perfil);

            return View(Perfil);
        }

        //EDITAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "iPerfil_id,sGestion_desc,sPeriodo_desc,dtSolicitud_dt,sTitulo_tfg,sObsRevision_desc,sPerfil_fl,dtRevision_dt,dtEntregado_dt,sModalidad_fl,iTutuor_id,lEstudiante_id,lEntregaForm_id,lRecepciona_id,iEstado_fl,iEliminado_fl,sCreado_by,iConcurrencia_id")] gatbl_Perfiles Perfil)
        {
            try
            {
                if ((Perfil.sModalidad_fl.Trim() == "02") || (Perfil.sModalidad_fl.Trim() == "03"))
                {
                    var idTutorVacio = db.Tutorez.Where(n => n.sNombre_desc.Trim() == "S/N").Select(n => n.iTutuor_id).FirstOrDefault();
                    Perfil.sTitulo_tfg = "";
                    Perfil.iTutuor_id = idTutorVacio;
                    Perfil.dtRevision_dt = DateTime.Parse("1900-01-01");
                    Perfil.sObsRevision_desc = "";
                    Perfil.dtSolicitud_dt = DateTime.Parse("1900-01-01");
                    Perfil.lEntregaForm_id = null;
                }


                Perfil.iEliminado_fl = 1;
                Perfil.sCreado_by = FrontUser.Get().usr_login;
                Perfil.iConcurrencia_id += 1;

                if (ModelState.IsValid)
                {
                    db.Entry(Perfil).State = EntityState.Modified;
                    db.SaveChanges();
                    Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Modificado correctamente.");
                    return RedirectToAction("Index");
                }

                var informacionAlumno = TraerInformacionDeAlumno(Perfil.lEstudiante_id);
                ViewBag.datos = informacionAlumno;
                var infoPersonaForm = TraerInformacionDePersonaPorCodigo(Perfil.lEntregaForm_id);
                ViewBag.datosForm = infoPersonaForm;
                var infoPersonaRecepciona = TraerInformacionDePersonaPorCodigo(Perfil.lRecepciona_id);
                ViewBag.datosRecep = infoPersonaRecepciona;
                cargarViewBag(Perfil);
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, se ha porducido un error al validar el modelo, por favor verifique que los campos estén correctamente llenados.");
                return View(Perfil);
            }
            catch (Exception ex)
            {
                Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, ha ocurrido el siguiente error: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        [Permiso(Permiso = RolesPermisos.EGRE_perfil_puedeEliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_Perfiles Perfil = db.Perfiles.Find(id);
            if (Perfil == null)
            {
                return HttpNotFound();
            }
            ViewBag.datos = TraerInformacionDeAlumno(Perfil.lEstudiante_id);
            return View(Perfil);
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
                    gatbl_Perfiles Perfil = db.Perfiles.Find(id);

                    Perfil.iEstado_fl = false;
                    Perfil.iEliminado_fl = 2;
                    Perfil.sCreado_by = FrontUser.Get().usr_login;
                    Perfil.iConcurrencia_id += 1;

                    db.Entry(Perfil).State = EntityState.Modified;
                    db.SaveChanges();

                    db.Perfiles.Remove(Perfil);
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

        //RETORNA LISTA REGISTROS DE ACUERDO A UN CRITERIO EN FORMATO JSON, ES UTILIZADO PARA EL AUTOCOMPLETE
        public JsonResult ListarRegistrosDeAlumnos_Json(string term) // 
        {
            List<string> personas = new List<string>();

            var alumnosEgresados = (from a in db.Alumnos
                                    join c in db.Carreras on a.crr_codigo equals c.crr_codigo
                                    join e in db.TiposEstados on a.tstd_codigo equals e.tstd_codigo
                                    //where a.tstd_codigo == "02" && c.nva_codigo == "03"// egresados = "02", y solo licenciaturas = "03"
                                    where c.nva_codigo == "03"// egresados = "02", y solo licenciaturas = "03"
                                    select new { registroAlumno = a.alm_registro, codigoPersona = a.agd_codigo, estadoAlumno = e.tstd_descripcion }).ToList();

            var alumnosInscritosPerfil = (from ap in db.Perfiles //crea una lista con los registros de los alumos que tienen registrados su perfil
                                          where ap.iEliminado_fl == 1
                                          select new { registroAlumnoPerfil = ap.lEstudiante_id }).ToList();

            var resultadoListaAlumnos = (from alumno in alumnosEgresados //en base a las 2 lista se hace una seleccion solo de los alumnos que no tengan registrado su perfil
                                         join ag in db.Personas on alumno.codigoPersona equals ag.agd_codigo////////
                                         where !alumnosInscritosPerfil.Any(m => m.registroAlumnoPerfil == alumno.registroAlumno)
                                         select new { registroAlumno = ag.agd_nombres.Trim() + " " + ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + " (" + alumno.registroAlumno.Trim() + ") " + alumno.estadoAlumno.Trim() }).ToList();

            personas = (from p in resultadoListaAlumnos //en la lista filtrada busca el registro que viene por parametro, asi evitamos registrar 2 perfiles a un solo estudiante
                        where p.registroAlumno.ToLower().Contains(term.ToLower())
                        select p.registroAlumno).Take(10).ToList();

            return Json(personas, JsonRequestBehavior.AllowGet);
        }

        //OBTIENE INFORMACION DE ALUMNO DE ACUERDO A UN REGISTRO EN FORMATO JSON, UTILIZADO PARA EL EVENTO SELECTED DEL AUTOCOPLETE
        public ActionResult TraerInformacionDeAlumno_Json(string term)
        {
            var infoAlumno = TraerInformacionDeAlumno(term);
            return Json(infoAlumno, JsonRequestBehavior.AllowGet);
        }

        //OBTIENE INFORMACION DE ALUMNO DE ACUERDO A UN REGISTRO, PARA LOS VIEWBAGS DE EDITAR, ELIMININAR, DETALLE
        private Object TraerInformacionDeAlumno(string id)
        {
            var resultado = (from a in db.Alumnos
                             join p in db.Personas on a.agd_codigo equals p.agd_codigo
                             join c in db.Carreras on a.crr_codigo equals c.crr_codigo
                             join f in db.Facultades on c.sca_codigo equals f.sca_codigo
                             join e in db.TiposEstados on a.tstd_codigo equals e.tstd_codigo
                             select new
                             {
                                 nombreCompletoAlumno = p.agd_nombres.Trim() + " " + p.agd_appaterno.Trim() + " " + p.agd_apmaterno.Trim() + " (" + a.alm_registro.Trim() + ") " + e.tstd_descripcion.Trim(),
                                 nombreAlumno = p.agd_nombres.Trim() + " " + p.agd_appaterno.Trim() + " " + p.agd_apmaterno.Trim() + " (" + a.alm_registro.Trim() + ") " + e.tstd_descripcion.Trim(),
                                 codigoAlumno = a.alm_registro.Trim(),
                                 carrera = c.crr_descripcion.Trim(),
                                 facultad = f.sca_descripcion.Trim()
                             }).ToList();

            var result = (from r in resultado
                          where r.nombreAlumno.ToLower().Contains(id.ToLower())
                          select r).FirstOrDefault();

            return result;
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
            //  var datosPersona;
            if (string.IsNullOrEmpty(codigo))
            {
                var listaPersonasN = (from p in db.Personas
                                      select new { codigoPersona = "", nombrePersona = "" });
                var datosPersona = (from f in listaPersonasN
                                    select f).FirstOrDefault();
                return datosPersona;
            }
            else {
                var listaPersonas = (from p in db.Personas
                                     select new { codigoPersona = p.agd_codigo, nombrePersona = p.agd_prefijo.Trim() + " " + p.agd_nombres.Trim() + " " + p.agd_appaterno.Trim() + " " + p.agd_apmaterno.Trim() }).ToList();

                var datosPersona = (from f in listaPersonas
                                    where f.codigoPersona.ToLower().Contains(codigo.ToLower())
                                    select f).FirstOrDefault();
                return datosPersona;
            }
        }

        //CARGA VIEWBAGS
        private void cargarViewBag(gatbl_Perfiles Perfil)
        {
            ViewBag.sPeriodo_desc = new SelectList(db.Semestres, "sem_codigo", "sem_codigo", Perfil.sPeriodo_desc);
            ViewBag.sModalidad_fl = new SelectList(db.tipoGraduacion.Where(g => g.nva_codigo == "03").OrderByDescending(p => p.tttg_descripcion), "tttg_codigo", "tttg_descripcion", Perfil.sModalidad_fl);
            ViewBag.iTutuor_id = new SelectList(db.Tutorez.Where(p => p.iEliminado_fl == 1).OrderBy(p => p.sNombre_desc), "iTutuor_id", "sNombre_desc", Perfil.iTutuor_id);
        }

        //TEREAS
        private enum Tarea
        {
            NUEVO,
            EDITAR,
            ELIMINAR
        }

        //listado de solo poryectos de grado
        public ActionResult IndexTFG(string criterio = null)
        {
            PerfilTFGView vistaPerfilTFG = new PerfilTFGView();
            List<gatbl_Perfiles> perfilesTFG = db.Perfiles.Where(p1 => p1.iEliminado_fl == 1 && (p1.TipoGraduacion.tttg_descripcion.Trim() == "PRACTICA EMPRESARIAL" || p1.TipoGraduacion.tttg_descripcion.Trim() == "PROYECTO DE GRADO" || p1.TipoGraduacion.tttg_descripcion.Trim() == "TESIS")).ToList();
            gatbl_Perfiles perfilTFG;
            if (criterio == null || criterio == "")
            {
                perfilTFG = perfilesTFG.LastOrDefault();
            }
            else
            {
                var alumnosNombreRegistro = (from ag in perfilesTFG
                                             select new { nombreYregistro = ag.Alumno.Persona.NombreCompleto.ToUpper() + " (" + ag.lEstudiante_id.Trim() + ")", registroAlumno = ag.lEstudiante_id.Trim() }).ToList();

                var alumnoPFG = (from p in alumnosNombreRegistro
                                 where p.nombreYregistro.ToLower().Contains(criterio.ToLower())
                                 select p).FirstOrDefault();
                if (alumnoPFG != null)
                {
                    perfilTFG = perfilesTFG.Where(pr => pr.lEstudiante_id == alumnoPFG.registroAlumno).FirstOrDefault();
                }
                else
                {
                    perfilTFG = perfilesTFG.Last();
                }
            }

            if (perfilTFG != null)//existe perfilTFG
            {
                vistaPerfilTFG.Perfil = perfilTFG;//agrego al viewModel
                var recepcionAlum = db.EntregasTFG.Where(e => e.iPerfil_id == perfilTFG.iPerfil_id).ToList();
                if (recepcionAlum.Count > 0)//exite recepcion del alumno
                {
                    //verifica a que EJEMPLAR corresponde 
                    var RecepcionesAluEjemplar1 = recepcionAlum.Where(pr => pr.sNumEjemplar.ToString().Equals("PRIMERO")).ToList();
                    var RecepcionesAluEjemplar2 = recepcionAlum.Where(pr => pr.sNumEjemplar.ToString().Equals("SEGUNDO")).ToList();

                    if (RecepcionesAluEjemplar1.Count > 0) //si existe es ejemplar 1
                    {
                        vistaPerfilTFG.RecepcionesEstudianteEjemplar1 = RecepcionesAluEjemplar1;//agrego al ViewModel
                        foreach (var item in RecepcionesAluEjemplar1)
                        {
                            var entregaTribu = db.EntregasTribunales.Where(et => et.iEntrega_id == item.iEntrega_id).FirstOrDefault();
                            if (entregaTribu != null)//si existe entregaATribunal
                            {
                                vistaPerfilTFG.EntregaTribuanalEjemplar1.Add(entregaTribu);//agrego al ViewModel
                                var recepcionTribu = db.RecepcionesTFG.Where(rt => rt.iETribunal_id == entregaTribu.iETribunal_id).FirstOrDefault();
                                if (recepcionTribu != null)//si existe recepcionDeTribunal
                                {
                                    vistaPerfilTFG.RecepcionTribunalEjemplar1.Add(recepcionTribu);
                                    var entregaAlu = db.EntregasAlEstudiante.Where(ea => ea.iRecepcionTFG_id == recepcionTribu.iRecepcionTFG_id).FirstOrDefault();
                                    if (entregaAlu != null)
                                    {
                                        vistaPerfilTFG.EntregaEstudianteEjemplar1.Add(entregaAlu);
                                    }
                                }
                            }
                        }
                    }

                    if (RecepcionesAluEjemplar2.Count > 0) //si existe es ejemplar 2
                    {
                        vistaPerfilTFG.RecepcionesEstudianteEjemplar2 = RecepcionesAluEjemplar2;
                        foreach (var item in RecepcionesAluEjemplar2)
                        {
                            var entregaTribu2 = db.EntregasTribunales.Where(et => et.iEntrega_id == item.iEntrega_id).FirstOrDefault();
                            if (entregaTribu2 != null)//si existe entregaATribunal
                            {
                                vistaPerfilTFG.EntregaTribuanalEjemplar2.Add(entregaTribu2);//agrego al ViewModel
                                var recepcionTribu2 = db.RecepcionesTFG.Where(rt => rt.iETribunal_id == entregaTribu2.iETribunal_id).FirstOrDefault();
                                if (recepcionTribu2 != null)//si existe recepcionDeTribunal
                                {
                                    vistaPerfilTFG.RecepcionTribunalEjemplar2.Add(recepcionTribu2);
                                    var entregaAlu2 = db.EntregasAlEstudiante.Where(ea => ea.iRecepcionTFG_id == recepcionTribu2.iRecepcionTFG_id).FirstOrDefault();
                                    if (entregaAlu2 != null)
                                    {
                                        vistaPerfilTFG.EntregaEstudianteEjemplar2.Add(entregaAlu2);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else //Con criterio, pero no encontro 
            {

            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_IndexTFG", vistaPerfilTFG);
            }
            return View(vistaPerfilTFG);
        }

        //se utiliza par el autocomplete de la pantalla seguimiento de ejemplar
        public JsonResult ListarAlumnosTFG_Json(string term) // 
        {
            List<string> alumnoPFG = new List<string>();

            var perfilesTFG = db.Perfiles.Where(p1 => p1.iEliminado_fl == 1 && (p1.TipoGraduacion.tttg_descripcion.Trim() == "PRACTICA EMPRESARIAL" || p1.TipoGraduacion.tttg_descripcion.Trim() == "PROYECTO DE GRADO" || p1.TipoGraduacion.tttg_descripcion.Trim() == "TESIS")).ToList();
            var alumnosNombreRegistro = (from ag in perfilesTFG
                                         select new { nombreYregistro = ag.Alumno.Persona.NombreCompleto.ToUpper() + " (" + ag.lEstudiante_id.Trim() + ")" }).ToList();

            alumnoPFG = (from p in alumnosNombreRegistro
                         where p.nombreYregistro.ToLower().Contains(term.ToLower())
                         select p.nombreYregistro).Take(10).ToList();
            return Json(alumnoPFG, JsonRequestBehavior.AllowGet);
        }

        //DISPOSE
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
