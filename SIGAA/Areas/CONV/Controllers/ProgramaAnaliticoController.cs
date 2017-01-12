using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SIGAA.Areas.CONV.Models;
using SIGAA.Areas.CONV.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Telerik.Reporting.Processing;
using SIGAA.Etiquetas;
using SIGAA.Commons;

namespace SIGAA.Areas.CONV.Controllers
{
    public class ProgramaAnaliticoCopia
    {
        public int old_id { get; set; }
        public int new_id { get; set; }
    }
    public class ProgramaAnaliticoController : Controller
    {
        private ConvalidacionesContext db = new ConvalidacionesContext();

        // GET: ProgramaAnalitico
        public ActionResult Index()
        {
            var programa = from p in db.gatbl_ProgramasAnaliticos
                                  select p;

            return View(programa.ToList());
        }
        

        public ActionResult Index_Read([DataSourceRequest] DataSourceRequest request)
        {
            var prog = from p in db.gatbl_ProgramasAnaliticos.Include(g => g.gatbl_Carreras).
                       Include(g => g.gatbl_Facultades).Include(g => g.gatbl_Universidades).
                       Include(g => g.Responsables).Include(g => g.UnidadNegocios).
                       Include(g => g.Pensum).Include(g => g.TipoCargaHoraria)
                       where p.iEliminado_fl != "2"
                       select p;                        
            return Json(prog.ToDataSourceResult(request, p => new
            {
                p.lProgramaAnalitico_id,
                p.dtRegistro_dt,
                p.sMateria_desc,
                p.lUNegocio_id,
                p.lUniversidad_id,
                p.lFacultad_id,
                p.lCarrera_id,
                p.lInstitucion_id,
                p.sInstitucion_desc,
                p.sCodigo_nro,
                p.sSigla_desc,
                p.sHorasPracticas_nro,
                p.sHorasTeoricas_nro,
                p.sHorasSociales_nro,
                p.sHorasAyudantia_nro,
                p.sCarga_Horaria,
                p.sCreditos_nro,
                p.sVersion_nro,
                p.lResponsable_id,
                p.sObs_desc,
                UnidadNegocios = new
                {
                    p.lUNegocio_id,
                    p.UnidadNegocios.sDescripcion
                },
                gatbl_Universidades = new
                {
                    p.lUniversidad_id,
                    p.gatbl_Universidades.sNombre_desc,
                    p.gatbl_Universidades.sDireccion_desc
                },
                gatbl_Facultades = new
                {
                    p.lFacultad_id,
                    p.gatbl_Facultades.sFacultad_nm,
                    p.gatbl_Facultades.sTelefono_desc
                },
                gatbl_Carreras = new
                {
                    p.lCarrera_id,
                    p.gatbl_Carreras.sCarrera_nm,
                    p.gatbl_Carreras.sTelefono_desc
                },
                TipoCargaHoraria = new
                {
                    p.lTipoCargaHoraria_fl,
                    p.TipoCargaHoraria.sDescripcion
                },
                Pensum = new
                {
                    p.lPensum_id,
                    p.Pensum.sDescripcion
                },
                Responsables = new
                {
                    p.lResponsable_id,
                    p.Responsables.NombreCompleto,
                    p.Responsables.agd_codigo
                }
            }));
        }

        public JsonResult ObtenerMaterias(string text)
        {
            //programa solicitud = Session["programa"] as programa;

            //if (string.IsNullOrEmpty(text) && programa.gatbl_PConvalidaciones.lResponsable_id != null)
            //{
            //    return Json((from ag in db.agendas
            //                 select new { agd_nombres = ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + ", " + ag.agd_nombres.Trim(), agd_codigo = ag.agd_codigo.Trim(), agd_docnro = ag.agd_docnro.Trim() }).Where(g => g.agd_codigo.Contains(programa.gatbl_PConvalidaciones.lResponsable_id)).OrderBy(g => g.agd_nombres), JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    return Json((from ag in db.agendas
            //                 select new { agd_nombres = ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + ", " + ag.agd_nombres.Trim(), agd_codigo = ag.agd_codigo.Trim(), agd_docnro = ag.agd_docnro.Trim() }).Where(g => g.agd_nombres.Contains(text)).OrderBy(g => g.agd_nombres), JsonRequestBehavior.AllowGet);
            //}

            return Json((from ma in db.gatbl_ProgramasAnaliticos
                         select new { lProgramaAnalitico_id = ma.lProgramaAnalitico_id, sMateria_desc = ma.sMateria_desc,
                             sCodigo_nro = ma.sCodigo_nro, sSigla_desc = ma.sSigla_desc, sHorasPracticas_nro = ma.sHorasPracticas_nro,
                             sHorasTeoricas_nro = ma.sHorasTeoricas_nro, sHorasSociales_nro = ma.sHorasSociales_nro,
                             sHorasAyudantia_nro = ma.sHorasAyudantia_nro, sCarga_Horaria = ma.sCarga_Horaria,
                             sCreditos_nro = ma.sCreditos_nro, sVersion_nro = ma.sVersion_nro }).Where(g => g.sMateria_desc.Contains(text)).OrderBy(g => g.sMateria_desc), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerResponsables(string text)
        {
            ProgramaAnaliticoViewModels programa = Session["programa"] as ProgramaAnaliticoViewModels;

            if (string.IsNullOrEmpty(text) && programa.gatbl_ProgramasAnaliticos.lResponsable_id != null)
            {
                return Json((from ag in db.agendas
                             select new { agd_nombres = ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + ", " + ag.agd_nombres.Trim(), agd_codigo = ag.agd_codigo.Trim(), agd_docnro = ag.agd_docnro.Trim() }).Where(g => g.agd_codigo.Contains(programa.gatbl_ProgramasAnaliticos.lResponsable_id)).OrderBy(g => g.agd_nombres), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json((from ag in db.agendas
                             select new { agd_nombres = ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + ", " + ag.agd_nombres.Trim(), agd_codigo = ag.agd_codigo.Trim(), agd_docnro = ag.agd_docnro.Trim() }).Where(g => g.agd_nombres.Contains(text)).OrderBy(g => g.agd_nombres), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult FacultadList(int id)
        {
            var facultades = from s in db.gatbl_Facultades
                             where s.lUniversidad_id == id                             
                             select s;

            return Json(new SelectList(facultades.ToArray(), "lFacultad_id", "sFacultad_nm"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CarreraList(int id)
        {
            var carreras = from s in db.gatbl_Carreras
                           where s.lFacultad_id == id
                           select s;

            return Json(new SelectList(carreras.ToArray(), "lCarrera_id", "sCarrera_nm"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult PensumList(int id)
        {
            var pensum = from s in db.Pensums
                           where s.lCarrera_id == id
                           select s;

            return Json(new SelectList(pensum.ToArray(), "lPensum_id", "sDescripcion"), JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult ReportProgram(int ID)
        {
            ReportProcessor reportProcessor = new ReportProcessor();
            Telerik.Reporting.InstanceReportSource instanceReportSource = new Telerik.Reporting.InstanceReportSource();

            ReporteConvalidaciones.rptProgramaAnalitico rpt = new ReporteConvalidaciones.rptProgramaAnalitico();
            rpt.ReportParameters["lprogramaID"].Value = ID;

            instanceReportSource.ReportDocument = rpt;
            RenderingResult result = reportProcessor.RenderReport("PDF", instanceReportSource, null);

            
            this.Response.Clear();
            this.Response.ContentType = result.MimeType;
            this.Response.Cache.SetCacheability(HttpCacheability.Private);
            this.Response.Expires = -1;
            this.Response.Buffer = true;

            

            this.Response.BinaryWrite(result.DocumentBytes);
            this.Response.End();

            return View();
        }

        // GET: ProgramaAnalitico/Details/5
        public ActionResult Details(int id)
        {
            var programa = new ProgramaAnaliticoViewModels();
            programa.gatbl_ProgramasAnaliticos = db.gatbl_ProgramasAnaliticos.Find(id);
            programa.gatbl_DetProgramasAnaliticos = (from t in db.gatbl_DetProgramasAnaliticos
                                                     where t.lProgramaAnalitico_id == id
                                                    select t).ToList();
            //programa.gatbl_DProgramasAnaliticos = programa.gatbl_ProgramasAnaliticos.gatbl_DProgramasAnaliticos.ToList();
            //programa.gatbl_DProgramasAnaliticosTemas = (from t in db.gatbl_DProgramasAnaliticosTemas
            //                                            join d in db.gatbl_DProgramasAnaliticos on t.lDProgramaAnalitico_id equals
            //                                            d.lDProgramaAnalitico_id
            //                                            where d.lProgramaAnalitico_id == id
            //                                           select t
            //                                           ).ToList();

            Session["programa"] = programa;

            return View(programa);            
        }

        public ActionResult Busqueda()
        {
            return View();
        }
        

        // GET: ProgramaAnalitico/Create
        public ActionResult Create()
        {
            ViewBag.lCarrera_id = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm");
            ViewBag.lFacultad_id = new SelectList(db.gatbl_Facultades, "lFacultad_id", "sFacultad_nm");
            ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc");
            ViewBag.sNivel_fl = new SelectList(db.NivelProgramaAnaliticos, "sNivel_fl", "sDescripcion");
            ViewBag.sOrigen_fl = new SelectList(db.OrigenProgramaAnaliticos, "sOrigen_fl", "sDescripcion");
            ViewBag.lResponsable_id = new SelectList(db.agendas, "agd_codigo", "NombreCompleto");
            ViewBag.lUNegocio_id = new SelectList(db.UnidadNegocios, "lUNegocio_id", "sDescripcion");
            ViewBag.lTipoCargaHoraria_fl = new SelectList(db.TipoCargaHorarias, "lTipoCargaHoraria_fl", "sDescripcion");
            ViewBag.UniversidadList = db.gatbl_Universidades;

            var programa = new ProgramaAnaliticoViewModels();
            programa.gatbl_ProgramasAnaliticos = new gatbl_ProgramasAnaliticos();
            programa.gatbl_DProgramasAnalitico = new gatbl_DProgramasAnaliticos();
            programa.gatbl_DProgramasAnaliticos = new List<gatbl_DProgramasAnaliticos>();
            programa.gatbl_DetProgramasAnaliticos = new List<gatbl_DetProgramasAnaliticos>();
            programa.gatbl_DetProgramasAnaliticosEliminados = new List<gatbl_DetProgramasAnaliticos>();
            programa.gatbl_DProgramasAnaliticosTema = new gatbl_DProgramasAnaliticosTemas();
            programa.gatbl_DProgramasAnaliticosTemas = new List<gatbl_DProgramasAnaliticosTemas>();

            Session["programa"] = programa;



            return View(programa);
        }

        // POST: ProgramaAnalitico/Create
        [HttpPost]
        public ActionResult Create(ProgramaAnaliticoViewModels programaAnalitico)
        {
            try
            {
                // TODO: Add insert logic here
                ProgramaAnaliticoViewModels programa = Session["programa"] as ProgramaAnaliticoViewModels;

                if(programaAnalitico.gatbl_ProgramasAnaliticos.lUniversidad_id == 0)
                {
                    ViewBag.Error = "El campo Universidad no puede ser nulo";
                }
                if (programaAnalitico.gatbl_ProgramasAnaliticos.lFacultad_id == 0)
                {
                    ViewBag.Error = "El campo Facultad no puede ser nulo";
                }
                if (programaAnalitico.gatbl_ProgramasAnaliticos.lCarrera_id == 0)
                {
                    ViewBag.Error = "El campo Carrera no puede ser nulo";
                }
                if (programaAnalitico.gatbl_ProgramasAnaliticos.lPensum_id == 0)
                {
                    ViewBag.Error = "El campo Pensum no puede ser nulo";
                }


                if(ViewBag.Error == null)
                {

                }
                programaAnalitico.gatbl_DProgramasAnaliticos = programa.gatbl_DProgramasAnaliticos;

                //ViewBag.lCarrera_id = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm");
                //ViewBag.lFacultad_id = new SelectList(db.gatbl_Facultades, "lFacultad_id", "sFacultad_nm");
                //ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc");
                //ViewBag.sNivel_fl = new SelectList(db.NivelProgramaAnaliticos, "sNivel_fl", "sDescripcion");
                //ViewBag.sOrigen_fl = new SelectList(db.OrigenProgramaAnaliticos, "sOrigen_fl", "sDescripcion");
                //ViewBag.lResponsable_id = new SelectList(db.agendas, "agd_codigo", "NombreCompleto");
                //ViewBag.lUNegocio_id = new SelectList(db.UnidadNegocios, "lUNegocio_id", "sDescripcion");

                ViewBag.lResponsable_id = new SelectList(db.agendas, "agd_codigo", "NombreCompleto", programaAnalitico.gatbl_ProgramasAnaliticos.lResponsable_id);
                ViewBag.lUNegocio_id = new SelectList(db.UnidadNegocios, "lUNegocio_id", "sDescripcion", programaAnalitico.gatbl_ProgramasAnaliticos.lUNegocio_id);
                ViewBag.lTipoCargaHoraria_fl = new SelectList(db.TipoCargaHorarias, "lTipoCargaHoraria_fl", "sDescripcion", programaAnalitico.gatbl_ProgramasAnaliticos.lTipoCargaHoraria_fl);
                ViewBag.lPensum_id = new SelectList(db.Pensums, "lPensum_id", "sDescripcion", programaAnalitico.gatbl_ProgramasAnaliticos.lPensum_id);

                ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", programaAnalitico.gatbl_ProgramasAnaliticos.lUniversidad_id);
                ViewBag.lFacultad_id = new SelectList(db.gatbl_Facultades.Where(f => f.lUniversidad_id == programaAnalitico.gatbl_ProgramasAnaliticos.lUniversidad_id), "lFacultad_id", "sFacultad_nm", programaAnalitico.gatbl_ProgramasAnaliticos.lFacultad_id);
                ViewBag.lCarrera_id = new SelectList(db.gatbl_Carreras.Where(c => c.lFacultad_id == programaAnalitico.gatbl_ProgramasAnaliticos.lFacultad_id), "lCarrera_id", "sCarrera_nm", programaAnalitico.gatbl_ProgramasAnaliticos.lCarrera_id);


                ViewBag.UniversidadList = db.gatbl_Universidades;

                programaAnalitico.gatbl_ProgramasAnaliticos.lUNegocio_id = Convert.ToString(Request["lUNegocio_id"]);
                //programaAnalitico.gatbl_ProgramasAnaliticos.lUniversidad_id = int.Parse(Request["lUniversidad_id"]);
                //programaAnalitico.gatbl_ProgramasAnaliticos.lFacultad_id = int.Parse(Request["lFacultad_id"]);
                //programaAnalitico.gatbl_ProgramasAnaliticos.lCarrera_id = int.Parse(Request["lCarrera_id"]);
                //programaAnalitico.gatbl_ProgramasAnaliticos.lResponsable_id = Convert.ToString(Request["lResponsable_id"]);

                programaAnalitico.gatbl_ProgramasAnaliticos.dtRegistro_dt = DateTime.Now;
                programaAnalitico.gatbl_ProgramasAnaliticos.lResponsable_id = "0000001138";
                programaAnalitico.gatbl_ProgramasAnaliticos.iEstado_fl = "1";
                programaAnalitico.gatbl_ProgramasAnaliticos.iEliminado_fl = "1";
                programaAnalitico.gatbl_ProgramasAnaliticos.sCreated_by = DateTime.Now.ToString();
                programaAnalitico.gatbl_ProgramasAnaliticos.iConcurrencia_id = 1;
                programaAnalitico.gatbl_ProgramasAnaliticos.lInstitucion_id = -1;
                programaAnalitico.gatbl_ProgramasAnaliticos.sInstitucion_desc = string.Empty;
                programaAnalitico.gatbl_ProgramasAnaliticos.sObs_desc = string.Empty;
                programaAnalitico.gatbl_ProgramasAnaliticos.lTipoCargaHoraria_fl = Convert.ToInt32(Request["lTipoCargaHoraria_fl"]);
                programaAnalitico.gatbl_ProgramasAnaliticos.lMateria_id = -1;
                programaAnalitico.gatbl_ProgramasAnaliticos.sVersion_nro += 1;
                //programaAnalitico.gatbl_ProgramasAnaliticos.sCarga_Horaria = programaAnalitico.gatbl_ProgramasAnaliticos.sHorasPracticas_nro +
                //    programaAnalitico.gatbl_ProgramasAnaliticos.sHorasTeoricas_nro + programaAnalitico.gatbl_ProgramasAnaliticos.sHorasAyudantia_nro +
                //    programaAnalitico.gatbl_ProgramasAnaliticos.sHorasSociales_nro;



                db.gatbl_ProgramasAnaliticos.Add(programaAnalitico.gatbl_ProgramasAnaliticos);
                db.SaveChanges();

                var ListaCopia = new List<ProgramaAnaliticoCopia>();

                var lProgramaAnalitico_id = db.gatbl_ProgramasAnaliticos.Select(p => p.lProgramaAnalitico_id).Max();

                foreach(var item in programa.gatbl_DetProgramasAnaliticos)
                {
                    int? Parent_id = ListaCopia.Count() > 0 && ListaCopia.Exists(t => t.old_id == item.lDetProgramaAnaliticoPadre_id)? ListaCopia.Find(t => t.old_id == item.lDetProgramaAnaliticoPadre_id).new_id: 0;
                    var detalle = new gatbl_DetProgramasAnaliticos
                    {
                        lDetProgramaAnalitico_id = item.lDetProgramaAnalitico_id,
                        sCodigo = item.sCodigo,
                        Nivel = item.Nivel,
                        sNumero = item.sNumero,
                        sDescripcion_desc = item.sDescripcion_desc,
                        sContenido_gral = item.sContenido_gral,
                        lDetProgramaAnaliticoPadre_id = Parent_id != 0? Parent_id:null,
                        iEstado_fl = item.iEstado_fl,
                        iEliminado_fl = item.iEliminado_fl,
                        sCreated_by = item.sCreated_by,
                        iConcurrencia_id = item.iConcurrencia_id                                    
                    };


                    if(detalle.lDetProgramaAnalitico_id > 0)
                    {
                        db.Entry(detalle).State = EntityState.Modified;                        
                    }                                          
                    else
                    {
                        detalle.lProgramaAnalitico_id = lProgramaAnalitico_id;
                        db.gatbl_DetProgramasAnaliticos.Add(detalle);
                    }

                    db.SaveChanges();

                    ListaCopia.Add(new ProgramaAnaliticoCopia() { old_id = item.lDetProgramaAnalitico_id, new_id=detalle.lDetProgramaAnalitico_id});
                }                

                //foreach (var item in programaAnalitico.gatbl_DProgramasAnaliticos)
                //{
                //    var detalle = new gatbl_DProgramasAnaliticos
                //    {
                //        lProgramaAnalitico_id = lProgramaAnalitico_id,
                //        sUnidad_nro = item.sUnidad_nro,
                //        sUnidad_desc = item.sUnidad_desc,
                //        sContenido_gral = item.sContenido_gral,                        
                //        iEstado_fl = item.iEstado_fl,
                //        iEliminado_fl = item.iEliminado_fl,
                //        sCreated_by = item.sCreated_by,
                //        iConcurrencia_id = item.iConcurrencia_id
                //    };
                //    db.gatbl_DProgramasAnaliticos.Add(detalle);
                //    db.SaveChanges();

                //    var lDProgramaAnalitico_id = db.gatbl_DProgramasAnaliticos.Select(p => p.lDProgramaAnalitico_id).Max();
                //    foreach (var tema in programa.gatbl_DProgramasAnaliticosTemas.Where(t=> t.lDProgramaAnalitico_id == item.lDProgramaAnalitico_id))
                //    {
                //        var detalleTema = new gatbl_DProgramasAnaliticosTemas
                //        {
                //            lDProgramaAnalitico_id = lDProgramaAnalitico_id,
                //            sTema_desc = tema.sTema_desc,
                //            sContenido_gral = tema.sContenido_gral,
                //            iEstado_fl = tema.iEstado_fl,
                //            iEliminado_fl = tema.iEliminado_fl,
                //            sCreated_by = tema.sCreated_by,
                //            iConcurrencia_id = tema.iConcurrencia_id
                //        };
                //        db.gatbl_DProgramasAnaliticosTemas.Add(detalleTema);
                //    }
                //    db.SaveChanges();
                //}


                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View(programaAnalitico);
            }
        }

        public ActionResult AdicionarUnidad()
        {            
            return View();
        }
        
        [HttpPost]
        public ActionResult AdicionarUnidad(ProgramaAnaliticoViewModels programaAnalitico)
        {
            var programa = Session["programa"] as ProgramaAnaliticoViewModels;

            //ViewBag.lCarrera_id = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm");
            //ViewBag.lFacultad_id = new SelectList(db.gatbl_Facultades, "lFacultad_id", "sFacultad_nm");
            //ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc");
            //ViewBag.sNivel_fl = new SelectList(db.NivelProgramaAnaliticos, "sNivel_fl", "sDescripcion");
            //ViewBag.sOrigen_fl = new SelectList(db.OrigenProgramaAnaliticos, "sOrigen_fl", "sDescripcion");
            //ViewBag.lResponsable_id = new SelectList(db.agendas, "agd_codigo", "NombreCompleto");
            //ViewBag.lUNegocio_id = new SelectList(db.UnidadNegocios, "lUNegocio_id", "sDescripcion");
            ViewBag.lResponsable_id = new SelectList(db.agendas, "agd_codigo", "NombreCompleto", programa.gatbl_ProgramasAnaliticos.lResponsable_id);
            ViewBag.lUNegocio_id = new SelectList(db.UnidadNegocios, "lUNegocio_id", "sDescripcion", programa.gatbl_ProgramasAnaliticos.lUNegocio_id);
            ViewBag.lTipoCargaHoraria_fl = new SelectList(db.TipoCargaHorarias, "lTipoCargaHoraria_fl", "sDescripcion", programa.gatbl_ProgramasAnaliticos.lTipoCargaHoraria_fl);
            ViewBag.lPensum_id = new SelectList(db.Pensums.Where(p=>p.lCarrera_id == programa.gatbl_ProgramasAnaliticos.lCarrera_id), "lPensum_id", "sDescripcion", programa.gatbl_ProgramasAnaliticos.lPensum_id);

            ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", programa.gatbl_ProgramasAnaliticos.lUniversidad_id);
            ViewBag.lFacultad_id = new SelectList(db.gatbl_Facultades.Where(f => f.lUniversidad_id == programa.gatbl_ProgramasAnaliticos.lUniversidad_id), "lFacultad_id", "sFacultad_nm", programa.gatbl_ProgramasAnaliticos.lFacultad_id);
            ViewBag.lCarrera_id = new SelectList(db.gatbl_Carreras.Where(c => c.lFacultad_id == programa.gatbl_ProgramasAnaliticos.lFacultad_id), "lCarrera_id", "sCarrera_nm", programa.gatbl_ProgramasAnaliticos.lCarrera_id);


            ViewBag.UniversidadList = db.gatbl_Universidades;

            programaAnalitico.gatbl_DProgramasAnalitico.lDProgramaAnalitico_id = -(programa.gatbl_DProgramasAnaliticos.Count() + 1);
            programaAnalitico.gatbl_DProgramasAnalitico.sUnidad_nro = Convert.ToString(programa.gatbl_DProgramasAnaliticos.Count() + 1);
            programaAnalitico.gatbl_DProgramasAnalitico.iEstado_fl = "1";
            programaAnalitico.gatbl_DProgramasAnalitico.iEliminado_fl = "1";
            programaAnalitico.gatbl_DProgramasAnalitico.sCreated_by = DateTime.Now.ToString();
            programaAnalitico.gatbl_DProgramasAnalitico.iConcurrencia_id = 1;            

            if(programaAnalitico.gatbl_DProgramasAnalitico.sUnidad_nro == null)
            {
                return View(programaAnalitico);
            }
            if (programaAnalitico.gatbl_DProgramasAnalitico.sUnidad_desc == null)
            {
                return View(programaAnalitico);
            }
            if (programaAnalitico.gatbl_DProgramasAnalitico.sContenido_gral == null)
            {
                return View(programaAnalitico);
            }

            programa.gatbl_DProgramasAnalitico = new gatbl_DProgramasAnaliticos
            {
                lDProgramaAnalitico_id = programaAnalitico.gatbl_DProgramasAnalitico.lDProgramaAnalitico_id,
                sUnidad_nro = programaAnalitico.gatbl_DProgramasAnalitico.sUnidad_nro,
                sUnidad_desc = programaAnalitico.gatbl_DProgramasAnalitico.sUnidad_desc,
                sContenido_gral = programaAnalitico.gatbl_DProgramasAnalitico.sContenido_gral,
                iEstado_fl = programaAnalitico.gatbl_DProgramasAnalitico.iEstado_fl,
                iEliminado_fl = programaAnalitico.gatbl_DProgramasAnalitico.iEliminado_fl,
                sCreated_by = programaAnalitico.gatbl_DProgramasAnalitico.sCreated_by,
                iConcurrencia_id = 1
            };
            
            programa.gatbl_DProgramasAnaliticos.Add(programa.gatbl_DProgramasAnalitico);

            string sController = "Create";
            if (programa.gatbl_ProgramasAnaliticos.lProgramaAnalitico_id != 0)
            {
                sController = "Edit";
            }

            return View(sController, programa);

        }

        public ActionResult AdicionarTema()
        {
            var programa = Session["programa"] as ProgramaAnaliticoViewModels;

            ViewBag.lDProgramaAnalitico_id = new SelectList(programa.gatbl_DProgramasAnaliticos, "lDProgramaAnalitico_id", "sUnidad_desc");

            return View();
        }

        [HttpPost]
        public ActionResult AdicionarTema(ProgramaAnaliticoViewModels programaAnalitico)
        {
            var programa = Session["programa"] as ProgramaAnaliticoViewModels;

            ViewBag.lDProgramaAnalitico_id = new SelectList(programa.gatbl_DProgramasAnaliticos, "lDProgramaAnalitico_id", "sUnidad_desc", programaAnalitico.gatbl_DProgramasAnaliticosTema.lDProgramaAnalitico_id);

            ViewBag.lResponsable_id = new SelectList(db.agendas, "agd_codigo", "NombreCompleto", programa.gatbl_ProgramasAnaliticos.lResponsable_id);
            ViewBag.lUNegocio_id = new SelectList(db.UnidadNegocios, "lUNegocio_id", "sDescripcion", programa.gatbl_ProgramasAnaliticos.lUNegocio_id);
            ViewBag.lTipoCargaHoraria_fl = new SelectList(db.TipoCargaHorarias, "lTipoCargaHoraria_fl", "sDescripcion", programa.gatbl_ProgramasAnaliticos.lTipoCargaHoraria_fl);
            ViewBag.lPensum_id = new SelectList(db.Pensums.Where(p => p.lCarrera_id == programa.gatbl_ProgramasAnaliticos.lCarrera_id), "lPensum_id", "sDescripcion", programa.gatbl_ProgramasAnaliticos.lPensum_id);

            ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", programa.gatbl_ProgramasAnaliticos.lUniversidad_id);
            ViewBag.lFacultad_id = new SelectList(db.gatbl_Facultades.Where(f => f.lUniversidad_id == programa.gatbl_ProgramasAnaliticos.lUniversidad_id), "lFacultad_id", "sFacultad_nm", programa.gatbl_ProgramasAnaliticos.lFacultad_id);
            ViewBag.lCarrera_id = new SelectList(db.gatbl_Carreras.Where(c => c.lFacultad_id == programa.gatbl_ProgramasAnaliticos.lFacultad_id), "lCarrera_id", "sCarrera_nm", programa.gatbl_ProgramasAnaliticos.lCarrera_id);
            ViewBag.UniversidadList = db.gatbl_Universidades;

            string contenido = HttpUtility.HtmlDecode(Request["editor"]);


            programa.gatbl_DProgramasAnaliticosTema = new gatbl_DProgramasAnaliticosTemas
            {
                lDProgramaAnalitico_id = Convert.ToInt32(Request["lDProgramaAnalitico_id"]),
                sTema_desc = programaAnalitico.gatbl_DProgramasAnaliticosTema.sTema_desc,
                sContenido_gral = contenido,
                iEstado_fl = "1",
                iEliminado_fl = "1",
                sCreated_by = DateTime.Now.ToString(),
                iConcurrencia_id = 1
            };

            programa.gatbl_DProgramasAnaliticosTemas.Add(programa.gatbl_DProgramasAnaliticosTema);

            string sController = "Create";
            if (programa.gatbl_ProgramasAnaliticos.lProgramaAnalitico_id != 0)
            {
                sController = "Edit";
            }

            return View(sController, programa);
        }

        public ActionResult EditarTema(int? id)
        {
            var programa = Session["programa"] as ProgramaAnaliticoViewModels;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            gatbl_DProgramasAnaliticosTemas tema = new gatbl_DProgramasAnaliticosTemas();

            tema = db.gatbl_DProgramasAnaliticosTemas.Find(id);
            if (tema == null)
            {
                return HttpNotFound();
            }

            programa.gatbl_DProgramasAnaliticosTema = tema;

            ViewBag.lDProgramaAnalitico_id = new SelectList(programa.gatbl_DProgramasAnaliticos, "lDProgramaAnalitico_id", "sUnidad_desc", tema.lDProgramaAnalitico_id);
            
            return View(tema);
        }

        [HttpPost]
        public ActionResult EditarTema(gatbl_DProgramasAnaliticosTemas tema)
        {
            var programaAnalitico = Session["programa"] as ProgramaAnaliticoViewModels;
            
            ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", programaAnalitico.gatbl_ProgramasAnaliticos.lUniversidad_id);
            ViewBag.lFacultad_id = new SelectList(db.gatbl_Facultades.Where(f => f.lUniversidad_id == programaAnalitico.gatbl_ProgramasAnaliticos.lUniversidad_id), "lFacultad_id", "sFacultad_nm", programaAnalitico.gatbl_ProgramasAnaliticos.lFacultad_id);
            ViewBag.lCarrera_id = new SelectList(db.gatbl_Carreras.Where(c => c.lFacultad_id == programaAnalitico.gatbl_ProgramasAnaliticos.lFacultad_id), "lCarrera_id", "sCarrera_nm", programaAnalitico.gatbl_ProgramasAnaliticos.lCarrera_id);


            ViewBag.lResponsable_id = new SelectList(db.agendas, "agd_codigo", "NombreCompleto", programaAnalitico.gatbl_ProgramasAnaliticos.lResponsable_id);
            ViewBag.lUNegocio_id = new SelectList(db.UnidadNegocios, "lUNegocio_id", "sDescripcion", programaAnalitico.gatbl_ProgramasAnaliticos.lUNegocio_id);
            ViewBag.lTipoCargaHoraria_fl = new SelectList(db.TipoCargaHorarias, "lTipoCargaHoraria_fl", "sDescripcion", programaAnalitico.gatbl_ProgramasAnaliticos.lTipoCargaHoraria_fl);
            ViewBag.lPensum_id = new SelectList(db.Pensums.Where(p => p.lCarrera_id == programaAnalitico.gatbl_ProgramasAnaliticos.lCarrera_id), "lPensum_id", "sDescripcion", programaAnalitico.gatbl_ProgramasAnaliticos.lPensum_id);
            ViewBag.UniversidadList = db.gatbl_Universidades;

            tema.iEstado_fl = "1";
            tema.iEliminado_fl = "1";
            tema.sCreated_by = DateTime.Now.ToString();
            tema.iConcurrencia_id = 1;

            string contenido = HttpUtility.HtmlDecode(Request["editor"]);

            var temaactual = programaAnalitico.gatbl_DProgramasAnaliticosTemas.First(d => d.lDProgramaAnaliticoTema_id == programaAnalitico.gatbl_DProgramasAnaliticosTema.lDProgramaAnaliticoTema_id);
            if (temaactual != null)
            {
                temaactual.lDProgramaAnaliticoTema_id = programaAnalitico.gatbl_DProgramasAnaliticosTema.lDProgramaAnaliticoTema_id;
                temaactual.lDProgramaAnalitico_id = programaAnalitico.gatbl_DProgramasAnaliticosTema.lDProgramaAnalitico_id;
                temaactual.sTema_desc = tema.sTema_desc;
                temaactual.sContenido_gral = contenido;
                temaactual.iEstado_fl = tema.iEstado_fl;
                temaactual.iEliminado_fl = tema.iEliminado_fl;
                temaactual.sCreated_by = tema.sCreated_by;
                temaactual.iConcurrencia_id = tema.iConcurrencia_id;
            }


            string sController = "Create";
            if (programaAnalitico.gatbl_ProgramasAnaliticos.lProgramaAnalitico_id != 0)
            {
                sController = "Edit";
            }

            return View(sController, programaAnalitico);
        }

        public ActionResult EditarUnidad(int? id)
        {
            var programa = Session["programa"] as ProgramaAnaliticoViewModels;            

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            gatbl_DProgramasAnaliticos unidad = new gatbl_DProgramasAnaliticos();

            unidad = db.gatbl_DProgramasAnaliticos.Find(id);
            if (unidad == null)
            {
                return HttpNotFound();
            }

            programa.gatbl_DProgramasAnalitico = unidad;

           
            return View(programa);
        }

        [HttpPost]
        public ActionResult EditarUnidad(ProgramaAnaliticoViewModels programa)
        {
            var programaAnalitico = Session["programa"] as ProgramaAnaliticoViewModels;

            //ViewBag.lCarrera_id = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm", programaAnalitico.gatbl_ProgramasAnaliticos.lCarrera_id);
            //ViewBag.lFacultad_id = new SelectList(db.gatbl_Facultades, "lFacultad_id", "sFacultad_nm", programaAnalitico.gatbl_ProgramasAnaliticos.lFacultad_id);
            //ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", programaAnalitico.gatbl_ProgramasAnaliticos.lUniversidad_id);

            ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", programaAnalitico.gatbl_ProgramasAnaliticos.lUniversidad_id);
            ViewBag.lFacultad_id = new SelectList(db.gatbl_Facultades.Where(f => f.lUniversidad_id == programaAnalitico.gatbl_ProgramasAnaliticos.lUniversidad_id), "lFacultad_id", "sFacultad_nm", programaAnalitico.gatbl_ProgramasAnaliticos.lFacultad_id);
            ViewBag.lCarrera_id = new SelectList(db.gatbl_Carreras.Where(c => c.lFacultad_id == programaAnalitico.gatbl_ProgramasAnaliticos.lFacultad_id), "lCarrera_id", "sCarrera_nm", programaAnalitico.gatbl_ProgramasAnaliticos.lCarrera_id);


            ViewBag.lResponsable_id = new SelectList(db.agendas, "agd_codigo", "NombreCompleto", programaAnalitico.gatbl_ProgramasAnaliticos.lResponsable_id);
            ViewBag.lUNegocio_id = new SelectList(db.UnidadNegocios, "lUNegocio_id", "sDescripcion", programaAnalitico.gatbl_ProgramasAnaliticos.lUNegocio_id);
            ViewBag.lTipoCargaHoraria_fl = new SelectList(db.TipoCargaHorarias, "lTipoCargaHoraria_fl", "sDescripcion", programaAnalitico.gatbl_ProgramasAnaliticos.lTipoCargaHoraria_fl);
            ViewBag.lPensum_id = new SelectList(db.Pensums.Where(p => p.lCarrera_id == programaAnalitico.gatbl_ProgramasAnaliticos.lCarrera_id), "lPensum_id", "sDescripcion", programaAnalitico.gatbl_ProgramasAnaliticos.lPensum_id);
            ViewBag.UniversidadList = db.gatbl_Universidades;

            programa.gatbl_DProgramasAnalitico.iEstado_fl = "1";
            programa.gatbl_DProgramasAnalitico.iEliminado_fl = "1";
            programa.gatbl_DProgramasAnalitico.sCreated_by = DateTime.Now.ToString();
            programa.gatbl_DProgramasAnalitico.iConcurrencia_id = 1;
           
            var unidad = programaAnalitico.gatbl_DProgramasAnaliticos.First(d => d.lDProgramaAnalitico_id == programaAnalitico.gatbl_DProgramasAnalitico.lDProgramaAnalitico_id);
            if (unidad != null)
            {
                unidad.lDProgramaAnalitico_id = programaAnalitico.gatbl_DProgramasAnalitico.lDProgramaAnalitico_id;
                unidad.sUnidad_nro = programa.gatbl_DProgramasAnalitico.sUnidad_nro;
                unidad.sUnidad_desc = programa.gatbl_DProgramasAnalitico.sUnidad_desc;
                unidad.sContenido_gral = programa.gatbl_DProgramasAnalitico.sContenido_gral;
                unidad.iEstado_fl = programa.gatbl_DProgramasAnalitico.iEstado_fl;
                unidad.iEliminado_fl = programa.gatbl_DProgramasAnalitico.iEliminado_fl;
                unidad.sCreated_by = programa.gatbl_DProgramasAnalitico.sCreated_by;   
                unidad.iConcurrencia_id = programa.gatbl_DProgramasAnalitico.iConcurrencia_id;
            }


            string sController = "Create";
            if (programaAnalitico.gatbl_ProgramasAnaliticos.lProgramaAnalitico_id != 0)
            {
                sController = "Edit";
            }

            return View(sController, programaAnalitico);
        }

        // GET: ProgramaAnalitico/Edit/5
        public ActionResult Edit(int? id)
        {
            //ViewBag.lCarrera_id = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm");
            //ViewBag.lFacultad_id = new SelectList(db.gatbl_Facultades, "lFacultad_id", "sFacultad_nm");
            //ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc");
            //ViewBag.sNivel_fl = new SelectList(db.NivelProgramaAnaliticos, "sNivel_fl", "sDescripcion");
            //ViewBag.sOrigen_fl = new SelectList(db.OrigenProgramaAnaliticos, "sOrigen_fl", "sDescripcion");
            //ViewBag.lResponsable_id = new SelectList(db.agendas, "agd_codigo", "NombreCompleto");
            //ViewBag.lUNegocio_id = new SelectList(db.UnidadNegocios, "lUNegocio_id", "sDescripcion");
            ViewBag.UniversidadList = db.gatbl_Universidades;

            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProgramaAnaliticoViewModels programa = new ProgramaAnaliticoViewModels();
            
            gatbl_ProgramasAnaliticos gatbl_ProgramasAnaliticos = db.gatbl_ProgramasAnaliticos.Find(id);
            if (gatbl_ProgramasAnaliticos == null)
            {
                return HttpNotFound();
            }

            ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", gatbl_ProgramasAnaliticos.lUniversidad_id);
            ViewBag.lFacultad_id = new SelectList(db.gatbl_Facultades.Where(f => f.lUniversidad_id == gatbl_ProgramasAnaliticos.lUniversidad_id), "lFacultad_id", "sFacultad_nm", gatbl_ProgramasAnaliticos.lFacultad_id);            
            ViewBag.lCarrera_id = new SelectList(db.gatbl_Carreras.Where(c => c.lFacultad_id == gatbl_ProgramasAnaliticos.lFacultad_id), "lCarrera_id", "sCarrera_nm", gatbl_ProgramasAnaliticos.lCarrera_id);

            ViewBag.lResponsable_id = new SelectList(db.agendas, "agd_codigo", "NombreCompleto", gatbl_ProgramasAnaliticos.lResponsable_id);
            ViewBag.lUNegocio_id = new SelectList(db.UnidadNegocios, "lUNegocio_id", "sDescripcion", gatbl_ProgramasAnaliticos.lUNegocio_id);
            ViewBag.lTipoCargaHoraria_fl = new SelectList(db.TipoCargaHorarias, "lTipoCargaHoraria_fl", "sDescripcion", gatbl_ProgramasAnaliticos.lTipoCargaHoraria_fl);
            ViewBag.lPensum_id = new SelectList(db.Pensums.Where(p=>p.lCarrera_id == gatbl_ProgramasAnaliticos.lCarrera_id), "lPensum_id", "sDescripcion", gatbl_ProgramasAnaliticos.lPensum_id);


            programa.gatbl_ProgramasAnaliticos = gatbl_ProgramasAnaliticos;
            programa.gatbl_DetProgramasAnaliticos = (from t in db.gatbl_DetProgramasAnaliticos
                                                     where t.lProgramaAnalitico_id == id
                                                     select t).ToList();
            programa.gatbl_DetProgramasAnaliticosEliminados = new List<gatbl_DetProgramasAnaliticos>();
            //programa.gatbl_DProgramasAnaliticos = gatbl_ProgramasAnaliticos.gatbl_DProgramasAnaliticos.ToList();
            //programa.gatbl_DProgramasAnaliticosTemas = (from t in db.gatbl_DProgramasAnaliticosTemas
            //                                            join d in db.gatbl_DProgramasAnaliticos on t.lDProgramaAnalitico_id equals
            //                                            d.lDProgramaAnalitico_id
            //                                            where d.lProgramaAnalitico_id == id
            //                                            select t
            //                                           ).ToList();

            Session["programa"] = programa;

            return View(programa);
        }

        // POST: ProgramaAnalitico/Edit/5
        [HttpPost]
        public ActionResult Edit(ProgramaAnaliticoViewModels programaAnalitico)
        {
            try
            {
                // TODO: Add update logic here

                var programa = Session["programa"] as ProgramaAnaliticoViewModels;

                //ViewBag.lCarrera_id = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm");
                //ViewBag.lFacultad_id = new SelectList(db.gatbl_Facultades, "lFacultad_id", "sFacultad_nm");
                //ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc");

                ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", programa.gatbl_ProgramasAnaliticos.lUniversidad_id);
                ViewBag.lFacultad_id = new SelectList(db.gatbl_Facultades.Where(f => f.lUniversidad_id == programa.gatbl_ProgramasAnaliticos.lUniversidad_id), "lFacultad_id", "sFacultad_nm", programa.gatbl_ProgramasAnaliticos.lFacultad_id);
                ViewBag.lCarrera_id = new SelectList(db.gatbl_Carreras.Where(c => c.lFacultad_id == programa.gatbl_ProgramasAnaliticos.lFacultad_id), "lCarrera_id", "sCarrera_nm", programa.gatbl_ProgramasAnaliticos.lCarrera_id);


                //ViewBag.lResponsable_id = new SelectList(db.agendas, "agd_codigo", "NombreCompleto", programa.gatbl_ProgramasAnaliticos.lResponsable_id);
                ViewBag.lUNegocio_id = new SelectList(db.UnidadNegocios, "lUNegocio_id", "sDescripcion", programa.gatbl_ProgramasAnaliticos.lUNegocio_id);
                ViewBag.lTipoCargaHoraria_fl = new SelectList(db.TipoCargaHorarias, "lTipoCargaHoraria_fl", "sDescripcion", programa.gatbl_ProgramasAnaliticos.lTipoCargaHoraria_fl);
                ViewBag.lPensum_id = new SelectList(db.Pensums.Where(p => p.lCarrera_id == programa.gatbl_ProgramasAnaliticos.lCarrera_id), "lPensum_id", "sDescripcion", programa.gatbl_ProgramasAnaliticos.lPensum_id);
                ViewBag.UniversidadList = db.gatbl_Universidades;

                programaAnalitico.gatbl_ProgramasAnaliticos.lProgramaAnalitico_id = programa.gatbl_ProgramasAnaliticos.lProgramaAnalitico_id;

                programaAnalitico.gatbl_ProgramasAnaliticos.lUNegocio_id = Convert.ToString(Request["lUNegocio_id"]);
                programaAnalitico.gatbl_ProgramasAnaliticos.lUniversidad_id = int.Parse(Request["lUniversidad_id"]);
                programaAnalitico.gatbl_ProgramasAnaliticos.lFacultad_id = int.Parse(Request["lFacultad_id"]);
                programaAnalitico.gatbl_ProgramasAnaliticos.lCarrera_id = int.Parse(Request["lCarrera_id"]);
                //programaAnalitico.gatbl_ProgramasAnaliticos.lResponsable_id = Convert.ToString(Request["lResponsable_id"]);

                programaAnalitico.gatbl_ProgramasAnaliticos.dtRegistro_dt = DateTime.Now;
                programaAnalitico.gatbl_ProgramasAnaliticos.lResponsable_id = "0000001138";
                programaAnalitico.gatbl_ProgramasAnaliticos.iEstado_fl = "1";
                programaAnalitico.gatbl_ProgramasAnaliticos.iEliminado_fl = "1";
                programaAnalitico.gatbl_ProgramasAnaliticos.sCreated_by = DateTime.Now.ToString();
                programaAnalitico.gatbl_ProgramasAnaliticos.iConcurrencia_id = 1;
                programaAnalitico.gatbl_ProgramasAnaliticos.lInstitucion_id = -1;
                programaAnalitico.gatbl_ProgramasAnaliticos.sInstitucion_desc = string.Empty;
                programaAnalitico.gatbl_ProgramasAnaliticos.sObs_desc = string.Empty;
                programaAnalitico.gatbl_ProgramasAnaliticos.lMateria_id = -1;
                programaAnalitico.gatbl_ProgramasAnaliticos.lTipoCargaHoraria_fl = Convert.ToInt32(Request["lTipoCargaHoraria_fl"]);
                programaAnalitico.gatbl_ProgramasAnaliticos.lPensum_id = Convert.ToInt32(Request["lPensum_id"]);
                programaAnalitico.gatbl_ProgramasAnaliticos.CopiaDe = programa.gatbl_ProgramasAnaliticos.CopiaDe;
                programaAnalitico.gatbl_ProgramasAnaliticos.sVersion_nro = programa.gatbl_ProgramasAnaliticos.sVersion_nro;
                programaAnalitico.gatbl_ProgramasAnaliticos.sCarga_Horaria = programaAnalitico.gatbl_ProgramasAnaliticos.sHorasPracticas_nro +
                    programaAnalitico.gatbl_ProgramasAnaliticos.sHorasTeoricas_nro + programaAnalitico.gatbl_ProgramasAnaliticos.sHorasAyudantia_nro +
                    programaAnalitico.gatbl_ProgramasAnaliticos.sHorasSociales_nro;


                db.Entry(programaAnalitico.gatbl_ProgramasAnaliticos).State = EntityState.Modified;
                db.SaveChanges();

                var ListaCopia = new List<ProgramaAnaliticoCopia>();
                
                foreach (var item in programa.gatbl_DetProgramasAnaliticos)
                {
                    int? Parent_id = ListaCopia.Count() > 0 && ListaCopia.Exists(t => t.old_id == item.lDetProgramaAnaliticoPadre_id) ? ListaCopia.Find(t => t.old_id == item.lDetProgramaAnaliticoPadre_id).new_id : 0;
                    var detalle = new gatbl_DetProgramasAnaliticos
                    {
                        lDetProgramaAnalitico_id = item.lDetProgramaAnalitico_id,
                        lProgramaAnalitico_id = item.lProgramaAnalitico_id,
                        sCodigo = item.sCodigo,
                        sNumero = item.sNumero,
                        Nivel = item.Nivel,
                        sDescripcion_desc = item.sDescripcion_desc,
                        sContenido_gral = item.sContenido_gral,
                        lDetProgramaAnaliticoPadre_id = Parent_id != 0 ? Parent_id : null,
                        iEstado_fl = item.iEstado_fl,
                        iEliminado_fl = item.iEliminado_fl,
                        sCreated_by = item.sCreated_by,
                        iConcurrencia_id = item.iConcurrencia_id
                    };


                    if (detalle.lDetProgramaAnalitico_id > 0)
                    {
                        db.Entry(detalle).State = EntityState.Modified;
                    }
                    else
                    {
                        detalle.lProgramaAnalitico_id = programaAnalitico.gatbl_ProgramasAnaliticos.lProgramaAnalitico_id;
                        db.gatbl_DetProgramasAnaliticos.Add(detalle);
                    }

                    db.SaveChanges();

                    ListaCopia.Add(new ProgramaAnaliticoCopia() { old_id = item.lDetProgramaAnalitico_id, new_id = detalle.lDetProgramaAnalitico_id });
                }


                foreach (var it in programa.gatbl_DetProgramasAnaliticosEliminados)
                {
                    var itEliminado = db.gatbl_DetProgramasAnaliticos.Find(it.lDetProgramaAnalitico_id);

                    if (itEliminado != null)
                    {
                        db.gatbl_DetProgramasAnaliticos.Remove(itEliminado);
                        db.SaveChanges();
                    }                        
                }


                //foreach (var item in programa.gatbl_DProgramasAnaliticos)
                //{
                //    var detalle = new gatbl_DProgramasAnaliticos
                //    {
                //        lDProgramaAnalitico_id = item.lDProgramaAnalitico_id,
                //        lProgramaAnalitico_id = lProgramaAnalitico_id,
                //        sUnidad_nro = item.sUnidad_nro,
                //        sUnidad_desc = item.sUnidad_desc,
                //        sContenido_gral = item.sContenido_gral,
                //        iEstado_fl = item.iEstado_fl,
                //        iEliminado_fl = item.iEliminado_fl,
                //        sCreated_by = item.sCreated_by,
                //        iConcurrencia_id = item.iConcurrencia_id
                //    };

                //    if (detalle.lDProgramaAnalitico_id > 0)
                //        db.Entry(detalle).State = EntityState.Modified;
                //    else
                //        db.gatbl_DProgramasAnaliticos.Add(detalle);

                //    db.SaveChanges();

                //    var lDProgramaAnalitico_id = item.lDProgramaAnalitico_id < 0? db.gatbl_DProgramasAnaliticos.Select(p => p.lDProgramaAnalitico_id).Max(): item.lDProgramaAnalitico_id;
                //    foreach (var tema in programa.gatbl_DProgramasAnaliticosTemas.Where(t => t.lDProgramaAnalitico_id == item.lDProgramaAnalitico_id))
                //    {
                //        var detalleTema = new gatbl_DProgramasAnaliticosTemas
                //        {
                //            lDProgramaAnaliticoTema_id = tema.lDProgramaAnaliticoTema_id,
                //            lDProgramaAnalitico_id = lDProgramaAnalitico_id,
                //            sTema_desc = tema.sTema_desc,
                //            sContenido_gral = tema.sContenido_gral,
                //            iEstado_fl = tema.iEstado_fl,
                //            iEliminado_fl = tema.iEliminado_fl,
                //            sCreated_by = tema.sCreated_by,
                //            iConcurrencia_id = tema.iConcurrencia_id
                //        };
                //        //db.gatbl_DProgramasAnaliticosTemas.Add(detalleTema);
                //        if (detalleTema.lDProgramaAnaliticoTema_id > 0)
                //            db.Entry(detalleTema).State = EntityState.Modified;
                //        else
                //            db.gatbl_DProgramasAnaliticosTemas.Add(detalleTema);
                //    }
                //    db.SaveChanges();
                //}


                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {                
                return View();
            }
        }

        // GET: ProgramaAnalitico/Duplicate/5
        public ActionResult Duplicate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_ProgramasAnaliticos gatbl_ProgramasAnaliticos = db.gatbl_ProgramasAnaliticos.Find(id);
            if (gatbl_ProgramasAnaliticos == null)
            {
                return HttpNotFound();
            }

            var programa = new ProgramaAnaliticoViewModels();
            programa.gatbl_ProgramasAnaliticos = gatbl_ProgramasAnaliticos;
            programa.gatbl_DetProgramasAnaliticos = (from t in db.gatbl_DetProgramasAnaliticos
                                                     where t.lProgramaAnalitico_id == id
                                                     select t).ToList();
            
            Session["programa"] = programa;

            return View(programa);
        }

        // POST: ProgramaAnalitico/Duplicate/5        
        [HttpPost, ActionName("Duplicate")]
        [ValidateAntiForgeryToken]
        public ActionResult DuplicateConfirmed(int id)
        {
            try
            {
                // TODO: Add duplicate logic here
                ProgramaAnaliticoViewModels programa = Session["programa"] as ProgramaAnaliticoViewModels;

                var programaanalitico = new gatbl_ProgramasAnaliticos
                {
                    dtRegistro_dt = programa.gatbl_ProgramasAnaliticos.dtRegistro_dt,
                    lMateria_id = programa.gatbl_ProgramasAnaliticos.lMateria_id,
                    sMateria_desc = string.Format("{0} - (Copia)", programa.gatbl_ProgramasAnaliticos.sMateria_desc),
                    lUNegocio_id = programa.gatbl_ProgramasAnaliticos.lUNegocio_id,
                    lUniversidad_id = programa.gatbl_ProgramasAnaliticos.lUniversidad_id,
                    lFacultad_id = programa.gatbl_ProgramasAnaliticos.lFacultad_id,
                    lCarrera_id = programa.gatbl_ProgramasAnaliticos.lCarrera_id,
                    lInstitucion_id = programa.gatbl_ProgramasAnaliticos.lInstitucion_id,
                    sInstitucion_desc = programa.gatbl_ProgramasAnaliticos.sInstitucion_desc,
                    sCodigo_nro = programa.gatbl_ProgramasAnaliticos.sCodigo_nro,
                    sSigla_desc = programa.gatbl_ProgramasAnaliticos.sSigla_desc,
                    sHorasPracticas_nro = programa.gatbl_ProgramasAnaliticos.sHorasPracticas_nro,
                    sHorasTeoricas_nro = programa.gatbl_ProgramasAnaliticos.sHorasTeoricas_nro,
                    sHorasAyudantia_nro = programa.gatbl_ProgramasAnaliticos.sHorasAyudantia_nro,
                    sHorasSociales_nro = programa.gatbl_ProgramasAnaliticos.sHorasSociales_nro,
                    sCarga_Horaria = programa.gatbl_ProgramasAnaliticos.sCarga_Horaria,
                    sCreditos_nro = programa.gatbl_ProgramasAnaliticos.sCreditos_nro,
                    sVersion_nro = programa.gatbl_ProgramasAnaliticos.sVersion_nro,
                    lResponsable_id = programa.gatbl_ProgramasAnaliticos.lResponsable_id,
                    sObs_desc = programa.gatbl_ProgramasAnaliticos.sObs_desc,
                    CopiaDe = id,
                    lTipoCargaHoraria_fl = programa.gatbl_ProgramasAnaliticos.lTipoCargaHoraria_fl,
                    lPensum_id = programa.gatbl_ProgramasAnaliticos.lPensum_id,
                    iEstado_fl = programa.gatbl_ProgramasAnaliticos.iEstado_fl,
                    iEliminado_fl = programa.gatbl_ProgramasAnaliticos.iEliminado_fl,
                    sCreated_by = DateTime.Now.ToString(),
                    iConcurrencia_id = 1
                };

                db.gatbl_ProgramasAnaliticos.Add(programaanalitico);
                db.SaveChanges();

                foreach (var item in programa.gatbl_DetProgramasAnaliticos)
                {
                    var detalle = new gatbl_DetProgramasAnaliticos
                    {
                        lDetProgramaAnalitico_id = item.lDetProgramaAnalitico_id,
                        lProgramaAnalitico_id = programaanalitico.lProgramaAnalitico_id,
                        sCodigo = item.sCodigo,
                        Nivel = item.Nivel,
                        sNumero = item.sNumero,
                        sDescripcion_desc = item.sDescripcion_desc,
                        sContenido_gral = item.sContenido_gral,
                        lDetProgramaAnaliticoPadre_id = item.lDetProgramaAnaliticoPadre_id,
                        iEstado_fl = item.iEstado_fl,
                        iEliminado_fl = item.iEliminado_fl,
                        sCreated_by = item.sCreated_by,
                        iConcurrencia_id = item.iConcurrencia_id
                    };

                    db.gatbl_DetProgramasAnaliticos.Add(detalle);                    
                }
                db.SaveChanges();

                //var lProgramaAnalitico_id = db.gatbl_ProgramasAnaliticos.Select(p => p.lProgramaAnalitico_id).Max();
                //foreach (var item in programa.gatbl_DProgramasAnaliticos)
                //{
                //    var detalle = new gatbl_DProgramasAnaliticos
                //    {
                //        lProgramaAnalitico_id = lProgramaAnalitico_id,
                //        sUnidad_nro = item.sUnidad_nro,
                //        sUnidad_desc = item.sUnidad_desc,
                //        sContenido_gral = item.sContenido_gral,
                //        iEstado_fl = item.iEstado_fl,
                //        iEliminado_fl = item.iEliminado_fl,
                //        sCreated_by = item.sCreated_by,
                //        iConcurrencia_id = item.iConcurrencia_id
                //    };
                //    db.gatbl_DProgramasAnaliticos.Add(detalle);
                //    db.SaveChanges();

                //    var lDProgramaAnalitico_id = db.gatbl_DProgramasAnaliticos.Select(p => p.lDProgramaAnalitico_id).Max();
                //    foreach (var tema in programa.gatbl_DProgramasAnaliticosTemas.Where(t => t.lDProgramaAnalitico_id == item.lDProgramaAnalitico_id))
                //    {
                //        var detalleTema = new gatbl_DProgramasAnaliticosTemas
                //        {
                //            lDProgramaAnalitico_id = lDProgramaAnalitico_id,
                //            sTema_desc = tema.sTema_desc,
                //            sContenido_gral = tema.sContenido_gral,
                //            iEstado_fl = tema.iEstado_fl,
                //            iEliminado_fl = tema.iEliminado_fl,
                //            sCreated_by = tema.sCreated_by,
                //            iConcurrencia_id = tema.iConcurrencia_id
                //        };
                //        db.gatbl_DProgramasAnaliticosTemas.Add(detalleTema);
                //    }
                //    db.SaveChanges();
                //}

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: ProgramaAnalitico/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_ProgramasAnaliticos gatbl_ProgramasAnaliticos = db.gatbl_ProgramasAnaliticos.Find(id);
            if (gatbl_ProgramasAnaliticos == null)
            {
                return HttpNotFound();
            }

            var programa = new ProgramaAnaliticoViewModels();
            programa.gatbl_ProgramasAnaliticos = gatbl_ProgramasAnaliticos;
            programa.gatbl_DetProgramasAnaliticos = (from t in db.gatbl_DetProgramasAnaliticos
                                                     where t.lProgramaAnalitico_id == id
                                                     select t).ToList();
           
            Session["programa"] = programa;

            return View(programa);
        }

        // POST: ProgramaAnalitico/Delete/5        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                // TODO: Add delete logic here
                gatbl_ProgramasAnaliticos gatbl_ProgramasAnaliticos = db.gatbl_ProgramasAnaliticos.Find(id);

                gatbl_ProgramasAnaliticos.iEliminado_fl = "2";

                db.Entry(gatbl_ProgramasAnaliticos).State = EntityState.Modified;
                db.SaveChanges();

                //var temas = from t in db.gatbl_DProgramasAnaliticosTemas
                //            join d in db.gatbl_DProgramasAnaliticos on t.lDProgramaAnalitico_id equals
                //            d.lDProgramaAnalitico_id
                //              where d.lProgramaAnalitico_id == id
                //              select t;

                //foreach (var item in temas)
                //{

                //    db.gatbl_DProgramasAnaliticosTemas.Remove(item);
                //}

                //var detalle = from d in db.gatbl_DProgramasAnaliticos
                //              where d.lProgramaAnalitico_id == id
                //              select d;

                //foreach (var item in detalle)
                //{

                //    db.gatbl_DProgramasAnaliticos.Remove(item);
                //}

                //db.gatbl_ProgramasAnaliticos.Remove(gatbl_ProgramasAnaliticos);
                //db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
