using SIGAA.Areas.CONV.Models;
using SIGAA.Areas.CONV.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIGAA.Etiquetas;
using SIGAA.Commons;

namespace SIGAA.Areas.CONV.Controllers
{
    public class ConvalidacionExternaController : Controller
    {
        private ConvalidacionesContext db = new ConvalidacionesContext();
        // GET: ConvalidacionExterna
        public ActionResult Index()
        {            
            return View(db.gatbl_ConvalidacionesExternasPA.ToList());
        }

        public JsonResult ObtenerPostulantes(string text)
        {
            ConvalidacionExternaViewModels convalidacionExterna = Session["convalidacionExterna"] as ConvalidacionExternaViewModels;

            if (string.IsNullOrEmpty(text) && convalidacionExterna.gatbl_ConvalidacionesExternasPA.lEstudiante_id != null)
            {
                return Json((from p in db.gatbl_Postulantes
                             join es in db.gatbl_PConvalidaciones on p.lPostulante_id equals es.lPostulante_id
                             //join ca in db.carreras on p.crr_codigo equals ca.crr_codigo
                             //join sa in db.secciones_academicas on ca.sca_codigo equals sa.sca_codigo
                             where p.sDocumento_nro == convalidacionExterna.gatbl_ConvalidacionesExternasPA.lEstudiante_id
                             orderby p.sNombre_desc ascending
                             select new {
                                 sNombre_desc = p.sNombre_desc,
                                 agd_docnro = p.sDocumento_nro
                                 //,
                                 //crr_descripcion = ca.crr_descripcion,
                                 //sca_descripcion = sa.sca_descripcion
                             }), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json((from p in db.gatbl_Postulantes
                             join es in db.gatbl_PConvalidaciones on p.lPostulante_id equals es.lPostulante_id
                             //join ca in db.carreras on p.crr_codigo equals ca.crr_codigo
                             //join sa in db.secciones_academicas on ca.sca_codigo equals sa.sca_codigo
                             where p.sNombre_desc.Contains(text)
                             orderby p.sNombre_desc ascending
                             select new
                             {
                                 sNombre_desc = p.sNombre_desc,
                                 agd_docnro = p.sDocumento_nro
                                 //,
                                 //crr_descripcion = ca.crr_descripcion,
                                 //sca_descripcion = sa.sca_descripcion
                             }), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ObtenerResponsables(string text)
        {
            ConvalidacionExternaViewModels convalidacionExterna = Session["convalidacionExterna"] as ConvalidacionExternaViewModels;

            if (string.IsNullOrEmpty(text) && convalidacionExterna.gatbl_ConvalidacionesExternasPA.lResponsable_id != null)
            {
                return Json((from ag in db.agendas
                             select new { agd_nombres = ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + ", " + ag.agd_nombres.Trim(), agd_codigo = ag.agd_codigo.Trim(), agd_docnro = ag.agd_docnro.Trim() }).Where(g => g.agd_codigo.Contains(convalidacionExterna.gatbl_ConvalidacionesExternasPA.lResponsable_id)).OrderBy(g => g.agd_nombres), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json((from ag in db.agendas
                             select new { agd_nombres = ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + ", " + ag.agd_nombres.Trim(), agd_codigo = ag.agd_codigo.Trim(), agd_docnro = ag.agd_docnro.Trim() }).Where(g => g.agd_nombres.Contains(text)).OrderBy(g => g.agd_nombres), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UniversidadList()
        {
            //var universidades = from s in db.gatbl_Universidades
            //                    join es in db.gatbl_EscalaCalificaciones on s.lUniversidad_id equals es.lUniversidad_id
            //                    join te in db.TipoEscalaEvaluacions on es.sTipoEscala_fl equals te.sTipoEscala_fl
            //                    join a in
            //                    (
            //                        from an in db.gatbl_AnalisisConvalidaciones
            //                        select new { lUniversidad_origen = an.lUniversidad_origen}
            //                    ).Distinct() on s.lUniversidad_id equals a.lUniversidad_origen
            //                    //join a in db.gatbl_AnalisisConvalidaciones on s.lUniversidad_id equals a.lUniversidad_origen
            //               //where s.lUniversidad_id == id
            //               select new { lUniversidad_id = s.lUniversidad_id, sNombre_desc = s.sNombre_desc, TipoEscalaDesc = te.sDescripcion};

            //return Json(universidades, JsonRequestBehavior.AllowGet);

            return Json(db.gatbl_Universidades, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CarreraList(int? id)
        {
            //var carreras = from c in db.gatbl_Carreras
            //               join a in
            //                    (
            //                        from an in db.gatbl_AnalisisConvalidaciones
            //                        select new { lCarrera_Origen = an.lCarrera_Origen }
            //                    ).Distinct() on c.lCarrera_id equals a.lCarrera_Origen
            //               where c.lUniversidad_id == id
            //               select new { lCarrera_id = c.lCarrera_id, sCarrera_nm = c.sCarrera_nm};

            //return Json(carreras, JsonRequestBehavior.AllowGet);

            return Json(db.gatbl_Carreras, JsonRequestBehavior.AllowGet);
        }        

        public JsonResult MateriaList(int id)
        {
            //var materias = from p in db.gatbl_ProgramasAnaliticos
            //               join m in
            //               (
            //                    from da in db.gatbl_DAnalisisConvalidaciones
            //                    where da.gatbl_AnalisisConvalidaciones.lCarrera_Origen == id && da.sOrigenMateria_fl == "01"
            //                    select new { lMateria_id = da.lMateria_id }
            //               ).Distinct() on p.lProgramaAnalitico_id equals m.lMateria_id                                                     
            //               select new { lProgramaAnalitico_id = p.lProgramaAnalitico_id, sMateria_desc = p.sMateria_desc};

            //return Json(materias, JsonRequestBehavior.AllowGet);

            return Json(db.gatbl_ProgramasAnaliticos, JsonRequestBehavior.AllowGet);
        }

        public List<gatbl_ProgramasAnaliticos> ListaProgramas()
        {
            var convalidacionExterna = Session["convalidacionExterna"] as ConvalidacionExternaViewModels;

            var convalidacion = convalidacionExterna.gatbl_MateriasConvalidadasOrigen;

            List<gatbl_ProgramasAnaliticos> ListaMaterias = new List<gatbl_ProgramasAnaliticos>();
            foreach (var uni in convalidacion.ToList())
            {
                var detprogramas = from d in db.gatbl_DAnalisisConvalidaciones
                                   where d.sOrigenMateria_fl == "01" &&
                                   d.lMateria_id == uni.lMateria_id
                                   select d;

                foreach (var item in detprogramas.ToList())
                {
                    var progDestino = from a in db.gatbl_DAnalisisConvalidaciones
                                      where a.sOrigenMateria_fl == "02" &&
                                      a.lAnalisisConvalidacion_id == item.lAnalisisConvalidacion_id
                                      select a;

                    foreach (var mat in progDestino.ToList())
                    {
                        if (ListaMaterias.FindIndex(a => a.lProgramaAnalitico_id == mat.lMateria_id) == -1)
                        {
                            gatbl_ProgramasAnaliticos prog = db.gatbl_ProgramasAnaliticos.Find(mat.lMateria_id);
                            ListaMaterias.Add(prog);
                        }
                    }

                }
            }



            return ListaMaterias;
        }

        public List<gatbl_ProgramasAnaliticos> ListaProgramas(int? id)
        {
            var convalidacion = from m in db.gatbl_MateriasConvalidadas
                                join d in db.gatbl_DConvalidacionesExternasPA on m.lDConvalidacionExternaPA_id equals d.lDConvalidacionExternaPA_id
                                where d.lConvalidacionExternaPA_id == id
                                select m;

            List<gatbl_ProgramasAnaliticos> ListaMaterias = new List<gatbl_ProgramasAnaliticos>();
            foreach (var uni in convalidacion.ToList())
            {
                var detprogramas = from d in db.gatbl_DAnalisisConvalidaciones
                                   where d.sOrigenMateria_fl == "01" &&
                                   d.lMateria_id == uni.lMateria_id
                                   select d;
                
                foreach (var item in detprogramas.ToList())
                {
                    var progDestino = from a in db.gatbl_DAnalisisConvalidaciones
                                      where a.sOrigenMateria_fl == "02" &&
                                      a.lAnalisisConvalidacion_id == item.lAnalisisConvalidacion_id
                                      select a;

                    foreach (var mat in progDestino.ToList())
                    {
                        if (ListaMaterias.FindIndex(a => a.lProgramaAnalitico_id == mat.lMateria_id) == -1)
                        {
                            gatbl_ProgramasAnaliticos prog = db.gatbl_ProgramasAnaliticos.Find(mat.lMateria_id);
                            ListaMaterias.Add(prog);
                        }
                    }

                }
            }

            

            return ListaMaterias;
        }

        // GET: ConvalidacionExterna/Details/5
        public ActionResult Details(int id)
        {                        
            var convalidacionExterna = new ConvalidacionExternaViewModels();

            convalidacionExterna.gatbl_ConvalidacionesExternasPA = db.gatbl_ConvalidacionesExternasPA.Find(id);
            convalidacionExterna.gatbl_DConvalidacionesExternasPA = convalidacionExterna.gatbl_ConvalidacionesExternasPA.gatbl_DConvalidacionesExternasPA.ToList();
            convalidacionExterna.gatbl_MateriasConvalidadasOrigen = (from m in db.gatbl_MateriasConvalidadas
                                                                     join d in db.gatbl_DConvalidacionesExternasPA on m.lDConvalidacionExternaPA_id equals d.lDConvalidacionExternaPA_id
                                                                     where d.lConvalidacionExternaPA_id == id
                                                                    select m).ToList();
            //convalidacionExterna.gatbl_MateriasConvalidadasDestino = new List<gatbl_MateriasConvalidadas>();
            convalidacionExterna.gatbl_ProgramasAnaliticos = ListaProgramas(id);

            return View(convalidacionExterna);
        }

        // GET: ConvalidacionExterna/Create
        public ActionResult Create()
        {
            ViewBag.lEstudiante_id = new SelectList(db.agendas, "agd_codigo", "NombreCompleto");
            ViewBag.lResponsable_id = new SelectList(db.agendas, "agd_codigo", "NombreCompleto");

            var convalidacionExterna = new ConvalidacionExternaViewModels();
            convalidacionExterna.gatbl_ConvalidacionesExternasPA = new gatbl_ConvalidacionesExternasPA();
            convalidacionExterna.gatbl_MateriaConvalidada = new gatbl_MateriasConvalidadas();
            convalidacionExterna.gatbl_DConvalidacionesExternasPA = new List<gatbl_DConvalidacionesExternasPA>();
            convalidacionExterna.gatbl_MateriasConvalidadasOrigen = new List<gatbl_MateriasConvalidadas>();
            convalidacionExterna.gatbl_ProgramasAnaliticos = new List<gatbl_ProgramasAnaliticos>();

            Session["convalidacionExterna"] = convalidacionExterna;

            return View(convalidacionExterna);            
        }

        // POST: ConvalidacionExterna/Create
        [HttpPost]
        public ActionResult Create(ConvalidacionExternaViewModels convalidacionExterna)
        {
            try
            {
                // TODO: Add insert logic here
                var convalidacion = Session["convalidacionExterna"] as ConvalidacionExternaViewModels;

                convalidacionExterna.gatbl_ConvalidacionesExternasPA.iEstado_fl = "1";
                convalidacionExterna.gatbl_ConvalidacionesExternasPA.iEliminado_fl = "1";
                convalidacionExterna.gatbl_ConvalidacionesExternasPA.sCreated_by = DateTime.Now.ToString();
                convalidacionExterna.gatbl_ConvalidacionesExternasPA.iConcurrencia_id = 1;

                carreras car = (from ag in db.alumnos_agenda
                               join c in db.carreras on ag.crr_codigo equals c.crr_codigo
                               where ag.agd_codigo == convalidacionExterna.gatbl_ConvalidacionesExternasPA.lEstudiante_id
                                select c).FirstOrDefault();

                convalidacion.gatbl_ConvalidacionesExternasPA = new gatbl_ConvalidacionesExternasPA
                {
                    lEstudiante_id = convalidacionExterna.gatbl_ConvalidacionesExternasPA.lEstudiante_id,
                    crr_codigo = car.crr_codigo,
                    sca_codigo = car.sca_codigo,
                    dtConvalidacionExterna_dt = convalidacionExterna.gatbl_ConvalidacionesExternasPA.dtConvalidacionExterna_dt,
                    sFormulario_nro = convalidacionExterna.gatbl_ConvalidacionesExternasPA.sFormulario_nro,
                    sInforme_nro = convalidacionExterna.gatbl_ConvalidacionesExternasPA.sInforme_nro,
                    lResponsable_id = convalidacionExterna.gatbl_ConvalidacionesExternasPA.lResponsable_id,
                    sObs_desc = convalidacionExterna.gatbl_ConvalidacionesExternasPA.sObs_desc,
                    iEstado_fl = convalidacionExterna.gatbl_ConvalidacionesExternasPA.iEstado_fl,
                    iEliminado_fl = convalidacionExterna.gatbl_ConvalidacionesExternasPA.iEliminado_fl,
                    sCreated_by = convalidacionExterna.gatbl_ConvalidacionesExternasPA.sCreated_by,
                    iConcurrencia_id = convalidacionExterna.gatbl_ConvalidacionesExternasPA.iConcurrencia_id,
                };

                db.gatbl_ConvalidacionesExternasPA.Add(convalidacion.gatbl_ConvalidacionesExternasPA);
                db.SaveChanges();

                var lConvalidacionExternaPA_id = db.gatbl_ConvalidacionesExternasPA.Select(c => c.lConvalidacionExternaPA_id).Max();
                foreach (var item in convalidacion.gatbl_DConvalidacionesExternasPA)
                {
                    var detUniversidad = new gatbl_DConvalidacionesExternasPA
                    {
                        lConvalidacionExternaPA_id = lConvalidacionExternaPA_id,
                        lUniversidad_id = item.lUniversidad_id,
                        lCarrera_id = item.lCarrera_id,
                        lEscalaCalificacion_id = item.lEscalaCalificacion_id,
                        sObs_desc = item.sObs_desc,
                        iEstado_fl = item.iEstado_fl,
                        iEliminado_fl = item.iEliminado_fl,
                        sCreated_by = item.sCreated_by,
                        iConcurrencia_id = item.iConcurrencia_id
                    };

                    db.gatbl_DConvalidacionesExternasPA.Add(detUniversidad);
                    db.SaveChanges();

                    var lDConvalidacionExternaPA_id = db.gatbl_DConvalidacionesExternasPA.Select(c => c.lDConvalidacionExternaPA_id).Max();

                    List<gatbl_MateriasConvalidadas> ListaMateria = new List<gatbl_MateriasConvalidadas>();
                    foreach (var mat in convalidacion.gatbl_MateriasConvalidadasOrigen)
                    {
                        if(mat.gatbl_DConvalidacionesExternasPA.lUniversidad_id == detUniversidad.lUniversidad_id
                            && mat.gatbl_DConvalidacionesExternasPA.lCarrera_id == detUniversidad.lCarrera_id)
                        {
                            ListaMateria.Add(mat);
                        }
                    }

                    foreach (var mt in ListaMateria)
                    {
                        var detmateria = new gatbl_MateriasConvalidadas
                        {
                            lDConvalidacionExternaPA_id = lDConvalidacionExternaPA_id,
                            sOrigenMateria_fl = mt.sOrigenMateria_fl,
                            lMateria_id = mt.lMateria_id,
                            sNota_Origen = mt.sNota_Origen,
                            sNota_destino = mt.sNota_destino,
                            sObs_desc = mt.sObs_desc,
                            iEstado_fl = mt.iEstado_fl,
                            iEliminado_fl = mt.iEliminado_fl,
                            sCreated_by = mt.sCreated_by,
                            iConcurrencia_id = mt.iConcurrencia_id
                        };

                        db.gatbl_MateriasConvalidadas.Add(detmateria);
                    }
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        public ActionResult AdicionarMateria()
        {
            //var convalidacion = Session["convalidacionExterna"] as ConvalidacionExternaViewModels;

            ViewBag.lCarrera_id = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm");
            ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc");
            ViewBag.lMateria_id = new SelectList(db.gatbl_ProgramasAnaliticos, "lProgramaAnalitico_id", "sMateria_desc");
            ViewBag.lEscalaCalificacion_id = new SelectList(db.gatbl_EscalaCalificaciones, "lEscalaCalificacion_id", "sEscala_desc");
            ViewBag.UniversidadList = db.gatbl_Universidades;
            
            return View();
        }        

        [HttpPost]
        public ActionResult AdicionarMateria(FormCollection convalidacionExterna)
        {
            var convalidacion = Session["convalidacionExterna"] as ConvalidacionExternaViewModels;

            ViewBag.lCarrera_id = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm");
            ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc");
            ViewBag.lMateria_id = new SelectList(db.gatbl_ProgramasAnaliticos, "lProgramaAnalitico_id", "sMateria_desc");
            ViewBag.lEscalaCalificacion_id = new SelectList(db.gatbl_EscalaCalificaciones, "lEscalaCalificacion_id", "sEscala_desc");
            ViewBag.UniversidadList = db.gatbl_Universidades;
            
            
            if (string.IsNullOrEmpty(Request.Form["lUniversidad_id"]))
            {
                ViewBag.Error = "Debe seleccionar una Universidad.";
                return View(convalidacion);
            }

            int UniversidadID = int.Parse(Request.Form["lUniversidad_id"]);            
            gatbl_EscalaCalificaciones escala = db.gatbl_EscalaCalificaciones.FirstOrDefault(e => e.lUniversidad_id == UniversidadID);
            
            convalidacion.gatbl_DConvalidacionExternaPA = new gatbl_DConvalidacionesExternasPA
            {
                lUniversidad_id = UniversidadID,
                sObs_desc = Request.Form["gatbl_MateriaConvalidada.sObs_desc"],
                gatbl_EscalaCalificaciones = escala
                
            };

            convalidacion.gatbl_MateriaConvalidada = new gatbl_MateriasConvalidadas
            {
                sNota_Origen = Request.Form["gatbl_MateriaConvalidada.sNota_Origen"]                
            };

            
            if (string.IsNullOrEmpty(Request.Form["lCarrera_id"]))
            {
                ViewBag.Error = "Debe seleccionar una Carrera.";
                return View(convalidacion);
            }

            int CarreraID = int.Parse(Request.Form["lCarrera_id"]);
            convalidacion.gatbl_DConvalidacionExternaPA.lCarrera_id = CarreraID;
            
            if (string.IsNullOrEmpty(Request.Form["lMateria_id"]))
            {
                ViewBag.Error = "Debe seleccionar una Materia.";
                return View(convalidacion);
            }

            int MateriaID = int.Parse(Request.Form["lMateria_id"]);
            convalidacion.gatbl_MateriaConvalidada.lMateria_id = MateriaID;                      
                        
            

            if(escala != null)
            {
                convalidacion.gatbl_DConvalidacionExternaPA = new gatbl_DConvalidacionesExternasPA
                {
                    lUniversidad_id = Convert.ToInt32(UniversidadID),                        
                    lCarrera_id = Convert.ToInt32(CarreraID),
                    lEscalaCalificacion_id = escala.lEscalaCalificacion_id,                    
                    iEstado_fl = "1",
                    iEliminado_fl = "1",
                    sCreated_by = DateTime.Now.ToString(),
                    iConcurrencia_id = 1
                };

                convalidacion.gatbl_DConvalidacionExternaPA.gatbl_Universidades = db.gatbl_Universidades.Find(convalidacion.gatbl_DConvalidacionExternaPA.lUniversidad_id);
                convalidacion.gatbl_DConvalidacionExternaPA.gatbl_Carreras = db.gatbl_Carreras.Find(convalidacion.gatbl_DConvalidacionExternaPA.lCarrera_id);
                convalidacion.gatbl_DConvalidacionExternaPA.gatbl_EscalaCalificaciones = escala;

                convalidacion.gatbl_MateriaConvalidada = new gatbl_MateriasConvalidadas
                {
                    sOrigenMateria_fl = "01",
                    lMateria_id = Convert.ToInt32(MateriaID),
                    sNota_Origen = Convert.ToString(Request.Form["gatbl_MateriaConvalidada.sNota_Origen"]),
                    sNota_destino = Convert.ToString(escala.sEquivalencia_destino),
                    sObs_desc = Convert.ToString(Request.Form["gatbl_MateriaConvalidada.sObs_desc"]),
                    iEstado_fl = "1",
                    iEliminado_fl = "1",
                    sCreated_by = DateTime.Now.ToString(),
                    iConcurrencia_id = 1
                };

                int carreraID = convalidacion.gatbl_DConvalidacionExternaPA.lCarrera_id;
                int materiaID = convalidacion.gatbl_MateriaConvalidada.lMateria_id;

                int Exist = convalidacion.gatbl_MateriasConvalidadasOrigen.Where(m => m.gatbl_DConvalidacionesExternasPA.lUniversidad_id == UniversidadID &&
                m.gatbl_DConvalidacionesExternasPA.lCarrera_id == carreraID && m.lMateria_id == materiaID).Count();


                if(Exist > 0)
                {
                    ViewBag.Error = "Este registro ya existe en la lista.";
                    return View(convalidacion);
                }

                convalidacion.gatbl_MateriaConvalidada.gatbl_ProgramasAnaliticos = db.gatbl_ProgramasAnaliticos.Find(convalidacion.gatbl_MateriaConvalidada.lMateria_id);

                int ExistUniversidad = convalidacion.gatbl_DConvalidacionesExternasPA.Where(m => m.lUniversidad_id == UniversidadID &&
                m.lCarrera_id == carreraID).Count();

                if(ExistUniversidad == 0)
                {
                    convalidacion.gatbl_DConvalidacionesExternasPA.Add(convalidacion.gatbl_DConvalidacionExternaPA);
                }

                convalidacion.gatbl_MateriaConvalidada.gatbl_DConvalidacionesExternasPA = convalidacion.gatbl_DConvalidacionExternaPA;
                    
                convalidacion.gatbl_MateriasConvalidadasOrigen.Add(convalidacion.gatbl_MateriaConvalidada);
                convalidacion.gatbl_ProgramasAnaliticos = ListaProgramas();
            }                
            
            

            string sController = "Create";
            if (convalidacion.gatbl_ConvalidacionesExternasPA.lConvalidacionExternaPA_id != 0)
            {
                sController = "Edit";
            }

            return View(sController, convalidacion);
        }

        public ActionResult EditarMateria(int? id)
        {
            var convalidacion = Session["convalidacionExterna"] as ConvalidacionExternaViewModels;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            gatbl_MateriasConvalidadas materia = db.gatbl_MateriasConvalidadas.Find(id);
            materia.gatbl_DConvalidacionesExternasPA = db.gatbl_DConvalidacionesExternasPA.Find(id);

            if (materia == null)
            {
                return HttpNotFound();
            }

            convalidacion.gatbl_MateriaConvalidada = materia;
            

            return View(convalidacion);
        }

        // GET: ConvalidacionExterna/Edit/5
        public ActionResult Edit(int? id)
        {            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_ConvalidacionesExternasPA gatbl_ConvalidacionesExternasPA = db.gatbl_ConvalidacionesExternasPA.Find(id);
            if (gatbl_ConvalidacionesExternasPA == null)
            {
                return HttpNotFound();
            }

            var convalidacionExterna = new ConvalidacionExternaViewModels();

            convalidacionExterna.gatbl_ConvalidacionesExternasPA = gatbl_ConvalidacionesExternasPA;
            convalidacionExterna.gatbl_DConvalidacionesExternasPA = gatbl_ConvalidacionesExternasPA.gatbl_DConvalidacionesExternasPA.ToList();
            convalidacionExterna.gatbl_MateriasConvalidadasOrigen = (from m in db.gatbl_MateriasConvalidadas
                                                                     join d in db.gatbl_DConvalidacionesExternasPA on m.lDConvalidacionExternaPA_id equals d.lDConvalidacionExternaPA_id
                                                                     where d.lConvalidacionExternaPA_id == id
                                                                     select m).ToList();
            convalidacionExterna.gatbl_ProgramasAnaliticos = ListaProgramas(id);

            Session["convalidacionExterna"] = convalidacionExterna;

            return View(convalidacionExterna);
        }

        // POST: ConvalidacionExterna/Edit/5
        [HttpPost]
        public ActionResult Edit(ConvalidacionExternaViewModels convalidacionExterna)
        {
            try
            {
                // TODO: Add update logic here

                var convalidacion = Session["convalidacionExterna"] as ConvalidacionExternaViewModels;

                convalidacion.gatbl_ConvalidacionesExternasPA.sCreated_by = DateTime.Now.ToString();
                
                carreras car = (from ag in db.alumnos_agenda
                                join c in db.carreras on ag.crr_codigo equals c.crr_codigo
                                where ag.agd_codigo == convalidacion.gatbl_ConvalidacionesExternasPA.lEstudiante_id
                                select c).FirstOrDefault();

                convalidacion.gatbl_ConvalidacionesExternasPA.dtConvalidacionExterna_dt = convalidacionExterna.gatbl_ConvalidacionesExternasPA.dtConvalidacionExterna_dt;
                convalidacion.gatbl_ConvalidacionesExternasPA.sFormulario_nro = convalidacionExterna.gatbl_ConvalidacionesExternasPA.sFormulario_nro;
                convalidacion.gatbl_ConvalidacionesExternasPA.sInforme_nro = convalidacionExterna.gatbl_ConvalidacionesExternasPA.sInforme_nro;
                convalidacion.gatbl_ConvalidacionesExternasPA.lResponsable_id = convalidacionExterna.gatbl_ConvalidacionesExternasPA.lResponsable_id;
                convalidacion.gatbl_ConvalidacionesExternasPA.sObs_desc = convalidacionExterna.gatbl_ConvalidacionesExternasPA.sObs_desc;

                convalidacion.gatbl_ConvalidacionesExternasPA.carreras = car;
                convalidacion.gatbl_ConvalidacionesExternasPA.secciones_academicas = db.secciones_academicas.Find(car.sca_codigo);
                convalidacion.gatbl_ConvalidacionesExternasPA.agenda = db.agendas.Find(convalidacion.gatbl_ConvalidacionesExternasPA.lEstudiante_id);
                convalidacion.gatbl_ConvalidacionesExternasPA.Responsable = db.agendas.Find(convalidacion.gatbl_ConvalidacionesExternasPA.lResponsable_id);

                db.Entry(convalidacion.gatbl_ConvalidacionesExternasPA).State = EntityState.Modified;
                db.SaveChanges();

                foreach (var item in convalidacion.gatbl_DConvalidacionesExternasPA)
                {                    
                    if (item.lDConvalidacionExternaPA_id != 0)
                        db.Entry(item).State = EntityState.Modified;
                    else
                    {
                        item.lConvalidacionExternaPA_id = convalidacion.gatbl_ConvalidacionesExternasPA.lConvalidacionExternaPA_id;
                        var detUniversidad = new gatbl_DConvalidacionesExternasPA
                        {
                            lConvalidacionExternaPA_id = item.lConvalidacionExternaPA_id,
                            lUniversidad_id = item.lUniversidad_id,
                            lCarrera_id = item.lCarrera_id,
                            lEscalaCalificacion_id = item.lEscalaCalificacion_id,
                            sObs_desc = item.sObs_desc,
                            iEstado_fl = item.iEstado_fl,
                            iEliminado_fl = item.iEliminado_fl,
                            sCreated_by = item.sCreated_by,
                            iConcurrencia_id = item.iConcurrencia_id
                        };

                        db.gatbl_DConvalidacionesExternasPA.Add(detUniversidad);
                    }
                        
                    
                    db.SaveChanges();

                    
                    List<gatbl_MateriasConvalidadas> ListaMateria = new List<gatbl_MateriasConvalidadas>();
                    foreach (var mat in convalidacion.gatbl_MateriasConvalidadasOrigen)
                    {
                        if (mat.gatbl_DConvalidacionesExternasPA.lUniversidad_id == item.lUniversidad_id
                            && mat.gatbl_DConvalidacionesExternasPA.lCarrera_id == item.lCarrera_id)
                        {
                            ListaMateria.Add(mat);
                        }
                    }

                    foreach (var mt in ListaMateria)
                    {                        
                        if (mt.lMateriaConvalidada_id != 0)
                            db.Entry(mt).State = EntityState.Modified;
                        else
                        {
                            var lDConvalidacionExternaPA_id = db.gatbl_DConvalidacionesExternasPA.Select(c => c.lDConvalidacionExternaPA_id).Max();
                            mt.lDConvalidacionExternaPA_id = item.lDConvalidacionExternaPA_id != 0? item.lDConvalidacionExternaPA_id: lDConvalidacionExternaPA_id;

                            var detmateria = new gatbl_MateriasConvalidadas
                            {
                                lDConvalidacionExternaPA_id = item.lDConvalidacionExternaPA_id != 0 ? item.lDConvalidacionExternaPA_id : lDConvalidacionExternaPA_id,
                                sOrigenMateria_fl = mt.sOrigenMateria_fl,
                                lMateria_id = mt.lMateria_id,
                                sNota_Origen = mt.sNota_Origen,
                                sNota_destino = mt.sNota_destino,
                                sObs_desc = mt.sObs_desc,
                                iEstado_fl = mt.iEstado_fl,
                                iEliminado_fl = mt.iEliminado_fl,
                                sCreated_by = mt.sCreated_by,
                                iConcurrencia_id = mt.iConcurrencia_id
                            };

                            db.gatbl_MateriasConvalidadas.Add(detmateria);
                        }

                        db.SaveChanges();
                    }
                    
                }

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: ConvalidacionExterna/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_ConvalidacionesExternasPA gatbl_ConvalidacionesExternasPA = db.gatbl_ConvalidacionesExternasPA.Find(id);
            if (gatbl_ConvalidacionesExternasPA == null)
            {
                return HttpNotFound();
            }

            var convalidacionExterna = new ConvalidacionExternaViewModels();

            convalidacionExterna.gatbl_ConvalidacionesExternasPA = gatbl_ConvalidacionesExternasPA;
            convalidacionExterna.gatbl_DConvalidacionesExternasPA = gatbl_ConvalidacionesExternasPA.gatbl_DConvalidacionesExternasPA.ToList();
            convalidacionExterna.gatbl_MateriasConvalidadasOrigen = (from m in db.gatbl_MateriasConvalidadas
                                                                     join d in db.gatbl_DConvalidacionesExternasPA on m.lDConvalidacionExternaPA_id equals d.lDConvalidacionExternaPA_id
                                                                     where d.lConvalidacionExternaPA_id == id
                                                                     select m).ToList();
            convalidacionExterna.gatbl_ProgramasAnaliticos = ListaProgramas(id);

            return View(convalidacionExterna);
        }

        // POST: ConvalidacionExterna/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                // TODO: Add delete logic here
                gatbl_ConvalidacionesExternasPA gatbl_ConvalidacionesExternasPA = db.gatbl_ConvalidacionesExternasPA.Find(id);

                var materias = from m in db.gatbl_MateriasConvalidadas
                               join d in db.gatbl_DConvalidacionesExternasPA on m.lDConvalidacionExternaPA_id equals
                               d.lDConvalidacionExternaPA_id                               
                               where d.lConvalidacionExternaPA_id == id
                               select m;
                foreach (var mat in materias)
                {
                    db.gatbl_MateriasConvalidadas.Remove(mat);
                }

                var detalle = from d in db.gatbl_DConvalidacionesExternasPA
                              where d.lConvalidacionExternaPA_id == id
                              select d;

                foreach (var item in detalle)
                {
                    
                    db.gatbl_DConvalidacionesExternasPA.Remove(item);                    
                }

                db.gatbl_ConvalidacionesExternasPA.Remove(gatbl_ConvalidacionesExternasPA);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
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
