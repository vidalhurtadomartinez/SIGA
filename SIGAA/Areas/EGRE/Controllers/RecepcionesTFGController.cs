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
    public class RecepcionesTFGController : Controller
    {
        private EgresadosContext db = new EgresadosContext();

        [Permiso(Permiso = RolesPermisos.EGRE_recepcionTFGdelTribunal_puedeVerIndice)]
        public ActionResult Index(string criterio = null)
        {
            var recepcionesTFG = db.RecepcionesTFG.Where(p => p.iEliminado_fl == 1).Include(g => g.Alumno).Include(g => g.EntregaTribunal).Include(g => g.Persona).ToList();
            var recepcionesTGFFiltrados = recepcionesTFG.Where(r => criterio == null ||
                                                                r.Alumno.Persona.NombreCompleto.ToLower().Contains(criterio.ToLower()) ||
                                                                r.lEstudiante_id.Contains(criterio.ToLower()) ||
                                                                r.Alumno.Carrera.crr_descripcion.ToLower().Contains(criterio.ToLower()) ||
                                                                r.sPeriodo_desc.ToLower().Contains(criterio.ToLower()) ||
                                                                r.dtRecepcion_dt.ToString().Contains(criterio.ToLower()) ||
                                                                r.sRecepcionTFG_fl.ToString().ToLower().Contains(criterio.ToLower()) ||
                                                                r.sEstadoEntrega_fl.ToString().ToLower().Contains(criterio.ToLower()) ||
                                                                r.Persona.NombreCompleto.ToLower().Contains(criterio.ToLower())
                                                                ).ToList();
            var recepcionesTFGFiltradasOrdenadas = recepcionesTGFFiltrados.OrderBy(ef => ef.Alumno.Persona.NombreCompleto).ThenBy(ef => ef.dtRecepcion_dt);

            if (Request.IsAjaxRequest())
            {
                if (!String.IsNullOrWhiteSpace(criterio))
                {
                    Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + recepcionesTFGFiltradasOrdenadas.Count() + " registros con los criterios especificados.");
                }
                return PartialView("_Index", recepcionesTFGFiltradasOrdenadas);
            }
            if (!String.IsNullOrWhiteSpace(criterio))
            {
                Flash.Instance.Success("RESULTADO DE BUSQUEDA", "Se han encontrado " + recepcionesTFGFiltradasOrdenadas.Count() + " registros con los criterios especificados.");
            }
            return View(recepcionesTFGFiltradasOrdenadas);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_recepcionTFGdelTribunal_puedeVerDetalle)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_RecepcionesTFG RecepcionesTFG = db.RecepcionesTFG.Find(id);

            if (RecepcionesTFG == null)
            {
                return HttpNotFound();
            }
            var listaDetalleRecepciones = db.DRecepcionesTFG.Where(d => d.iRecepcionTFG_id == id).ToList();
            RecepcionTFGView vistaRecepcion = new RecepcionTFGView();
            vistaRecepcion.RecepcionTFG = RecepcionesTFG;
            vistaRecepcion.DetalleRecepcionesTFG = listaDetalleRecepciones;

            var informacionAlumno = TraerInfoDeAlumnoPorIdEntregaATribunal(RecepcionesTFG.iETribunal_id);
            ViewBag.datos = informacionAlumno;
            return View(vistaRecepcion);
        }

        [Permiso(Permiso = RolesPermisos.EGRE_recepcionTFGdelTribunal_puedeCrearNuevo)]
        public ActionResult Create()
        {
            var recepcionTFGView = new RecepcionTFGView();
            CargarViewBags(Tarea.NUEVO, recepcionTFGView);
            Session["recepcionTFGView"] = recepcionTFGView;//crea variable de session
            return View(recepcionTFGView);
        }

        //CREAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RecepcionTFGView viewModel)
        {
            using (var transacion = db.Database.BeginTransaction()) //Tranascciones
            {
                try
                {
                    db.RecepcionesTFG.Add(viewModel.RecepcionTFG);
                    db.SaveChanges();
                    var cantidadLista = viewModel.DetalleRecepcionesTFG.Count;
                    if (cantidadLista > 0)
                    {
                        int recepcionIdMAX = 0;
                        recepcionIdMAX = db.RecepcionesTFG.ToList().Select(r => r.iRecepcionTFG_id).Max();
                        //guardo la lista de detalles de observaciones
                        foreach (var item in viewModel.DetalleRecepcionesTFG)
                        {
                            var observacion = new gatbl_DRecepcionesTFG
                            {
                                iRecepcionTFG_id = recepcionIdMAX,
                                iObs_nro = item.iObs_nro,
                                sTipoObs_fl = item.sTipoObs_fl,
                                sObsCorta = item.sObsCorta,
                                sObsDetallada = item.sObsDetallada,
                                sSugerencias = item.sSugerencias,
                                iEstado_fl = true,
                                iEliminado_fl = 1,
                                sCreado_by = FrontUser.Get().EmailUtepsa,
                            iConcurrencia_id = 1
                            };
                            db.DRecepcionesTFG.Add(observacion);
                        }
                        db.SaveChanges();
                    }
                    transacion.Commit();
                }
                catch (Exception ex)
                {
                    transacion.Rollback();
                    CargarViewBags(Tarea.NUEVO, viewModel);
                    Flash.Instance.Error("ERROR:    ", "No se pudo Guardar el dato, ha ocurrido el siguiente error: " + ex.Message);
                    return View(viewModel);
                }
            }//fin Transacion
            Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Guardado correctamente.");
            return RedirectToAction("Index");// reenvio a la vista 
        }

        [Permiso(Permiso = RolesPermisos.EGRE_recepcionTFGdelTribunal_puedeEditar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //var recepcionTFGView = Session["recepcionTFGView"] as RecepcionTFGView;
            gatbl_RecepcionesTFG RecepcionesTFG = db.RecepcionesTFG.Find(id);
            if (RecepcionesTFG == null)
            {
                return HttpNotFound();
            }

            RecepcionTFGView recepcionTFGView = new RecepcionTFGView();
            recepcionTFGView.RecepcionTFG = RecepcionesTFG;

            var DRecepcionesTFG = db.DRecepcionesTFG.Where(d => d.iRecepcionTFG_id == id).ToList();
            foreach (var item in DRecepcionesTFG)
            {
                var observacion = new gatbl_DRecepcionesTFG
                {
                    iDRecepcionTFG_id = item.iDRecepcionTFG_id,
                    iRecepcionTFG_id = item.iRecepcionTFG_id,
                    iObs_nro = item.iObs_nro,
                    sTipoObs_fl = item.sTipoObs_fl,
                    sObsCorta = item.sObsCorta,
                    sObsDetallada = item.sObsDetallada,
                    sSugerencias = item.sSugerencias,
                    iEstado_fl = true,
                    iEliminado_fl = 1,
                    sCreado_by = string.Format("Computador: {0}, Usuario: {1}, Fecha {2}, Accion: {3}", Environment.MachineName, Environment.UserName, DateTime.Now.ToString(), Tarea.EDITAR.ToString()),
                    iConcurrencia_id = 1
                };
                recepcionTFGView.DetalleRecepcionesTFG.Add(observacion);
            }

            var informacionAlumno = TraerInfoDeAlumnoPorIdEntregaATribunal(recepcionTFGView.RecepcionTFG.iETribunal_id);
            ViewBag.datos = informacionAlumno;
            CargarViewBags(Tarea.EDITAR, recepcionTFGView);
            Session["recepcionTFGView"] = recepcionTFGView;
            return View(recepcionTFGView);
        }

        //EDITAR POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RecepcionTFGView recepcionTFGView)
        {
            //enviamos a persistencia los datos
            using (var transacion = db.Database.BeginTransaction()) //Tranascciones
            {
                try
                {
                    var recepcionTFG = new gatbl_RecepcionesTFG
                    {
                        iRecepcionTFG_id = recepcionTFGView.RecepcionTFG.iRecepcionTFG_id,
                        iETribunal_id = recepcionTFGView.RecepcionTFG.iETribunal_id,
                        sGestion_desc = recepcionTFGView.RecepcionTFG.sGestion_desc,
                        sPeriodo_desc = recepcionTFGView.RecepcionTFG.sPeriodo_desc,
                        lEstudiante_id = recepcionTFGView.RecepcionTFG.lEstudiante_id,
                        sRecepcionTFG_fl = recepcionTFGView.RecepcionTFG.sRecepcionTFG_fl,
                        sEstadoEntrega_fl = recepcionTFGView.RecepcionTFG.sEstadoEntrega_fl,
                        dtRecepcion_dt = recepcionTFGView.RecepcionTFG.dtRecepcion_dt,
                        lRecepciona_id = recepcionTFGView.RecepcionTFG.lRecepciona_id,
                        iEstado_fl = true,
                        iEliminado_fl = 1,
                        sCreado_by = FrontUser.Get().EmailUtepsa,
                    iConcurrencia_id = 1
                    };

                    if (!ModelState.IsValid)
                    {
                        throw new System.Exception("El modelo RecepcionTFG no es Valido");
                    }
                    db.Entry(recepcionTFG).State = EntityState.Modified;
                    db.SaveChanges();


                    var detallesAnterior = db.DRecepcionesTFG.Where(d => d.iRecepcionTFG_id == recepcionTFGView.RecepcionTFG.iRecepcionTFG_id).ToList();
                    if (detallesAnterior.Count > 0) //elimina detalle anterior si existen 
                    {
                        foreach (var detallePaEliminar in detallesAnterior)
                        {
                            detallePaEliminar.iEstado_fl = false;
                            detallePaEliminar.iEliminado_fl = 2;
                            detallePaEliminar.sCreado_by = FrontUser.Get().EmailUtepsa;
                            detallePaEliminar.iConcurrencia_id += 1;
                            db.Entry(detallePaEliminar).State = EntityState.Modified;
                            db.SaveChanges();

                            db.DRecepcionesTFG.Remove(detallePaEliminar);
                            db.SaveChanges();
                        }
                        //db.SaveChanges();
                    }

                    var cantidadDetalleActual = recepcionTFGView.DetalleRecepcionesTFG.Count;
                    if (cantidadDetalleActual > 0)//inserta detalle actual si existen
                    {
                        foreach (var item in recepcionTFGView.DetalleRecepcionesTFG)
                        {
                            var observacion = new gatbl_DRecepcionesTFG
                            {
                                iRecepcionTFG_id = recepcionTFGView.RecepcionTFG.iRecepcionTFG_id,
                                iObs_nro = item.iObs_nro,
                                sTipoObs_fl = item.sTipoObs_fl,
                                sObsCorta = item.sObsCorta,
                                sObsDetallada = item.sObsDetallada,
                                sSugerencias = item.sSugerencias,
                                iEstado_fl = true,
                                iEliminado_fl = 1,
                                sCreado_by = FrontUser.Get().EmailUtepsa,
                            iConcurrencia_id = 1
                            };
                            db.DRecepcionesTFG.Add(observacion);
                        }
                        db.SaveChanges();
                        transacion.Commit();
                    }
                }
                catch (Exception ex)
                {
                    transacion.Rollback();
                    CargarViewBags(Tarea.EDITAR, recepcionTFGView);
                    Flash.Instance.Error("ERROR:    ", "No se pudo Modificar el dato, ha ocurrido el siguiente error: " + ex.Message);
                    return View(recepcionTFGView);
                }
            }//fin Transacion
            Flash.Instance.Success("CORRECTO:    ", "El dato ha sido Modificado correctamente.");
            return RedirectToAction("Index");// reenvio a la vista 
        }

        [Permiso(Permiso = RolesPermisos.EGRE_recepcionTFGdelTribunal_puedeEliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_RecepcionesTFG RecepcionesTFG = db.RecepcionesTFG.Find(id);
            if (RecepcionesTFG == null)
            {
                return HttpNotFound();
            }

            var listaDetalleRecepciones = db.DRecepcionesTFG.Where(d => d.iRecepcionTFG_id == id).ToList();
            RecepcionTFGView vistaRecepcion = new RecepcionTFGView();
            vistaRecepcion.RecepcionTFG = RecepcionesTFG;
            vistaRecepcion.DetalleRecepcionesTFG = listaDetalleRecepciones;

            var informacionAlumno = TraerInfoDeAlumnoPorIdEntregaATribunal(RecepcionesTFG.iETribunal_id);
            ViewBag.datos = informacionAlumno;
            return View(vistaRecepcion);
            //return View(RecepcionesTFG);
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
                    //eliminar detalles
                    var DRecepcionesTFGs = db.DRecepcionesTFG.Where(d => d.iRecepcionTFG_id == id).ToList();
                    if (DRecepcionesTFGs.Count > 0)
                    {
                        foreach (var item in DRecepcionesTFGs)
                        {
                            gatbl_DRecepcionesTFG DRecepcionesTFG = db.DRecepcionesTFG.Find(item.iDRecepcionTFG_id);
                            DRecepcionesTFG.iEstado_fl = false;
                            DRecepcionesTFG.iEliminado_fl = 2;
                            DRecepcionesTFG.sCreado_by = FrontUser.Get().EmailUtepsa;
                            DRecepcionesTFG.iConcurrencia_id += 1;
                            db.Entry(DRecepcionesTFG).State = EntityState.Modified;
                            db.SaveChanges();

                            db.DRecepcionesTFG.Remove(DRecepcionesTFG);
                            db.SaveChanges();
                        }
                    }

                    //luego eliminar recepcion
                    gatbl_RecepcionesTFG RecepcionesTFG = db.RecepcionesTFG.Find(id);
                    RecepcionesTFG.iEstado_fl = false;
                    RecepcionesTFG.iEliminado_fl = 2;
                    RecepcionesTFG.sCreado_by = FrontUser.Get().EmailUtepsa;
                    RecepcionesTFG.iConcurrencia_id += 1;
                    db.Entry(RecepcionesTFG).State = EntityState.Modified;
                    db.SaveChanges();

                    db.RecepcionesTFG.Remove(RecepcionesTFG);
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
        
        //AGREGAR DETALLE RECEPCIONES GET
        public ActionResult CrearDetalleRecepcionTFG()//BOTON AGREGAR OBSERVACION
        {
            var recepcionTFGView = Session["recepcionTFGView"] as RecepcionTFGView;
            gatbl_DRecepcionesTFG dRecepcionTFG = new gatbl_DRecepcionesTFG();
            return View(dRecepcionTFG);
        }

        //AGREGAR DETALLE RECEPCION POST
        [HttpPost]
        public ActionResult CrearDetalleRecepcionTFG(gatbl_DRecepcionesTFG dRecepcionTFG)
        {
            var recepcionTFGView = Session["recepcionTFGView"] as RecepcionTFGView;

            if (!ModelState.IsValid)
            {
                return View(dRecepcionTFG); //  
            }
            var detRecepTFG1 = new gatbl_DRecepcionesTFG
            {
                iObs_nro = recepcionTFGView.DetalleRecepcionesTFG.Count() + 1,
                sTipoObs_fl = dRecepcionTFG.sTipoObs_fl,
                sObsCorta = dRecepcionTFG.sObsCorta,
                sObsDetallada = dRecepcionTFG.sObsDetallada,
                sSugerencias = dRecepcionTFG.sSugerencias
            };
            recepcionTFGView.DetalleRecepcionesTFG.Add(detRecepTFG1);//agrego a la lista que esta en sesion

            CargarViewBags(Tarea.NUEVO, recepcionTFGView);
            return PartialView("_Create", recepcionTFGView);
        }

        //EDITAR DETALLE RECEPCIONES GET
        public ActionResult AgregarEditarDetalleRecepcionTFG()//BOTON AGREGAR OBSERVACION
        {
            var recepcionTFGView = Session["recepcionTFGView"] as RecepcionTFGView;
            gatbl_DRecepcionesTFG dRecepcionTFG = new gatbl_DRecepcionesTFG();
            return View(dRecepcionTFG);
        }

        //EDITAR DETALLE RECEPCION POST
        [HttpPost]
        public ActionResult AgregarEditarDetalleRecepcionTFG(gatbl_DRecepcionesTFG dRecepcionTFG)
        {
            var recepcionTFGView = Session["recepcionTFGView"] as RecepcionTFGView;

            if (!ModelState.IsValid) //verifico el estado del modelo en lado del servidor
            {
                return View(dRecepcionTFG); //  
            }
            var detRecepTFG1 = new gatbl_DRecepcionesTFG
            {
                iObs_nro = recepcionTFGView.DetalleRecepcionesTFG.Count() + 1,
                sTipoObs_fl = dRecepcionTFG.sTipoObs_fl,
                sObsCorta = dRecepcionTFG.sObsCorta,
                sObsDetallada = dRecepcionTFG.sObsDetallada,
                sSugerencias = dRecepcionTFG.sSugerencias
            };
            recepcionTFGView.DetalleRecepcionesTFG.Add(detRecepTFG1);//agrego a la lista que esta en sesion

            CargarViewBags(Tarea.NUEVO, recepcionTFGView);
            return RedirectToAction(string.Format("Edit/{0}", recepcionTFGView.RecepcionTFG.iRecepcionTFG_id));
        }
        
        //ELIMINA DETALLE
        public ActionResult DeleteDetailObservation(int id)
        {
            var detalleObservacion = db.DRecepcionesTFG.Find(id);
            if (detalleObservacion != null)
            {
                db.DRecepcionesTFG.Remove(detalleObservacion);
                db.SaveChanges();
            }
            return RedirectToAction(string.Format("Edit/{0}", detalleObservacion.iRecepcionTFG_id));
        }

        //busca 
        [HttpGet]
        public ActionResult BuscarInformacionDeAlumno(string term = "")
        {
            int id = 0;
            id = int.Parse(term);
            var infoAlumno = TraerInfoDeAlumnoPorIdEntregaATribunal(id);
            return Json(infoAlumno, JsonRequestBehavior.AllowGet);
        }

        //CARGA INFORMACION DE ALUMNO POR ID 
        private Object TraerInfoDeAlumnoPorIdEntregaATribunal(int id)
        {
            var result = (from et in db.EntregasTribunales
                          join e in db.EntregasTFG on et.iEntrega_id equals e.iEntrega_id
                          join p in db.Perfiles on e.iPerfil_id equals p.iPerfil_id
                          join al in db.Alumnos on p.lEstudiante_id equals al.alm_registro
                          join per in db.Personas on al.agd_codigo equals per.agd_codigo
                          join c in db.Carreras on al.crr_codigo equals c.crr_codigo
                          join f in db.Facultades on c.sca_codigo equals f.sca_codigo
                          join t in db.Tutorez on et.iTutuor_id equals t.iTutuor_id
                          where et.iETribunal_id == id
                          select new { nombreAlumno = per.agd_nombres.Trim() + " " + per.agd_appaterno.Trim() + " " + per.agd_apmaterno.Trim(), carrera = c.crr_descripcion.Trim(), facultad = f.sca_descripcion.Trim(), TituloPerfil = p.sTitulo_tfg, iETribunal_id = et.iETribunal_id, TipoGraduacion = p.TipoGraduacion.tttg_descripcion.Trim(), al.alm_registro, nombreTutor = t.sNombre_desc, estadoEntregaATribunal = et.sEntregaTribunal_fl.ToString(), ejemplar = e.sNumEjemplar.ToString() }).FirstOrDefault();

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
        private class EntregaATribunal
        {
            public int iETribunal_id { get; set; }
            public string nombreTutor { get; set; }

        }

        //LISTA SELECCIONA ENTREGAS A TRIBUNALES
        private List<EntregaATribunal> SeleccionarLista(Tarea tarea, int id)
        {
            var entregaTribunales = from et in db.EntregasTribunales
                                    join t in db.Tutorez on et.iTutuor_id equals t.iTutuor_id
                                    join a in db.Alumnos on et.lEstudiante_id equals a.alm_registro
                                    join p in db.Personas on a.agd_codigo equals p.agd_codigo
                                    select new { iETribunal_id = et.iETribunal_id, nombreTutor = t.sNombre_desc + " (" + p.agd_nombres.Trim() + " " + p.agd_appaterno.Trim() + " " + p.agd_apmaterno.Trim() + " " + a.alm_registro.Trim() + ")" };

            var recepcionDeTribunales = from rt in db.RecepcionesTFG
                                        join et in db.EntregasTribunales on rt.iETribunal_id equals et.iETribunal_id
                                        join t in db.Tutorez on et.iTutuor_id equals t.iTutuor_id
                                        join a in db.Alumnos on et.lEstudiante_id equals a.alm_registro
                                        join p in db.Personas on a.agd_codigo equals p.agd_codigo
                                        select new { iETribunal_id = rt.iETribunal_id, nombreTutor = t.sNombre_desc + " (" + p.agd_nombres.Trim() + " " + p.agd_appaterno.Trim() + " " + p.agd_apmaterno.Trim() + " " + a.alm_registro.Trim() + ")" };

            var entregas = (from et in entregaTribunales
                            where !recepcionDeTribunales.Any(m => m.iETribunal_id == et.iETribunal_id)
                            orderby et.nombreTutor
                            select new EntregaATribunal { iETribunal_id = et.iETribunal_id, nombreTutor = et.nombreTutor }).ToList();
            switch (tarea)
            {
                case Tarea.NUEVO:
                    return entregas.ToList();
                case Tarea.EDITAR:
                    {
                        List<EntregaATribunal> entregaATribunal = new List<EntregaATribunal>();
                        entregaATribunal = (from rt in recepcionDeTribunales
                                            where rt.iETribunal_id == id
                                            select new EntregaATribunal { iETribunal_id = rt.iETribunal_id, nombreTutor = rt.nombreTutor }).ToList();
                        return entregas.Union(entregaATribunal).ToList();
                    }
                default:
                    {
                        return entregas.ToList();
                    }
            }
        }

        //RETORNA LISTA NOMBRES DE ACUERDO A UN CRITERIO EN FORMATO JSON, ES UTILIZADO PARA EL AUTOCOMPLETE
        public JsonResult ListarNombresDePersonas_Json(string term)
        {
            List<string> nombresPersonas = new List<string>();

            var listaPersonas = (from p in db.Personas
                                     // select new { nombrePersona = p.agd_prefijo.Trim() + " " + p.agd_nombres.Trim() + " " + p.agd_appaterno.Trim() + " " + p.agd_apmaterno.Trim() + " (" + p.agd_codigo.Trim() + ")" }).ToList();
                                 select new { nombrePersona = p.agd_prefijo.Trim() + " " + p.agd_nombres.Trim() + " " + p.agd_appaterno.Trim() + " " + p.agd_apmaterno.Trim()}).ToList();

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
                                     // select new { codigoPersona = p.agd_codigo, nombrePersona = p.agd_prefijo.Trim() + " " + p.agd_nombres.Trim() + " " + p.agd_appaterno.Trim() + " " + p.agd_apmaterno.Trim() + " (" + p.agd_codigo.Trim() + ")" }).ToList();
                                 select new { codigoPersona = p.agd_codigo, nombrePersona = p.agd_prefijo.Trim() + " " + p.agd_nombres.Trim() + " " + p.agd_appaterno.Trim() + " " + p.agd_apmaterno.Trim()}).ToList();

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

        //CARGAR VIEWBAGS
        private void CargarViewBags(Tarea tarea, RecepcionTFGView recepcionTFGView)
        {
            var entregaATrib = SeleccionarLista(tarea, recepcionTFGView.RecepcionTFG.iETribunal_id);
            ViewBag.iETribunal_id = new SelectList(entregaATrib, "iETribunal_id", "nombreTutor", recepcionTFGView.RecepcionTFG.iETribunal_id);
            ViewBag.sPeriodo_desc = new SelectList(db.Semestres, "sem_codigo", "sem_codigo", recepcionTFGView.RecepcionTFG.sPeriodo_desc);
            if (tarea == Tarea.EDITAR)
            {
                var receptor = TraerInformacionDePersonaPorCodigo(recepcionTFGView.RecepcionTFG.lRecepciona_id);
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
