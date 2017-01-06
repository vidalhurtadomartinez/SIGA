using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIGAA.Areas.CONV.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Telerik.Reporting.Processing;
using System.IO;

namespace SIGAA.Areas.CONV.Controllers
{
    public class gatbl_AnalisisConvalidacionesController : Controller
    {       
        private ConvalidacionesContext db = new ConvalidacionesContext();

        // GET: gatbl_AnalisisConvalidaciones
        public ActionResult Index()
        {
            var gatbl_AnalisisConvalidaciones = db.gatbl_AnalisisConvalidaciones.Include(g => g.gatbl_AnalisisPreConvalidaciones).Include(g => g.Responsables);
            return View(gatbl_AnalisisConvalidaciones.ToList());
        }

        public ActionResult Index_Realizado()
        {            
            return View();
        }

        public ActionResult Index_Pendiente()
        {
            return View();
        }

        public ActionResult Index_Read([DataSourceRequest] DataSourceRequest request)
        {
            var pos = from po in db.gatbl_AnalisisConvalidaciones.Include(g => g.gatbl_AnalisisPreConvalidaciones).Include(g => g.Responsables)
                      join post in db.gatbl_Postulantes on po.gatbl_AnalisisPreConvalidaciones.gatbl_PConvalidaciones.lPostulante_id equals post.lPostulante_id
                      join uo in db.gatbl_Universidades on po.gatbl_AnalisisPreConvalidaciones.gatbl_PConvalidaciones.lUniversidadOrigen_id equals uo.lUniversidad_id
                      join ud in db.gatbl_Universidades on po.gatbl_AnalisisPreConvalidaciones.gatbl_PConvalidaciones.lUniversidadDestino_id equals ud.lUniversidad_id
                      join co in db.gatbl_Carreras on po.gatbl_AnalisisPreConvalidaciones.gatbl_PConvalidaciones.lCarreraOrigen_id equals co.lCarrera_id
                      join cd in db.gatbl_Carreras on po.gatbl_AnalisisPreConvalidaciones.gatbl_PConvalidaciones.lCarreraDestino_id equals cd.lCarrera_id
                      join pe in db.Pensums on po.gatbl_AnalisisPreConvalidaciones.gatbl_PConvalidaciones.lPensum_id equals pe.lPensum_id

                      select new
                      {                          
                          lAnalisisConvalidacion_id = po.lAnalisisConvalidacion_id,
                          lAnalisisPreConvalidacion_id = po.lAnalisisPreConvalidacion_id,
                          lPConvalidacion_id = po.gatbl_AnalisisPreConvalidaciones.lPConvalidacion_id,
                          sVersion_nro = po.gatbl_AnalisisPreConvalidaciones.sVersion_nro,
                          dtAnalisisConvalidacion_dt = po.dtAnalisisConvalidacion_dt,
                          lResponsable_id = po.lResponsable_id,
                          sObs_desc = po.sObs_desc,
                          gatbl_PConvalidaciones = po.gatbl_AnalisisPreConvalidaciones.gatbl_PConvalidaciones,
                          Responsables = po.gatbl_AnalisisPreConvalidaciones.gatbl_PConvalidaciones.Responsables,
                          gatbl_Postulantes = post,
                          gatbl_UniversidadesOrigen = uo,
                          gatbl_UniversidadesDestino = ud,
                          gatbl_CarrerasOrigen = co,
                          gatbl_CarrerasDestino = cd,
                          Pensum = pe

                      };
            return Json(pos.ToDataSourceResult(request, p => new
            {
                p.lAnalisisConvalidacion_id,
                p.lPConvalidacion_id,
                p.sVersion_nro,
                p.dtAnalisisConvalidacion_dt,
                p.lResponsable_id,
                p.sObs_desc,
                gatbl_PConvalidaciones = new
                {
                    p.lPConvalidacion_id,
                    p.gatbl_PConvalidaciones.lPostulante_id,
                    p.gatbl_PConvalidaciones.dtPostulacion_dt,
                    p.gatbl_PConvalidaciones.lResponsable_id,
                    p.gatbl_PConvalidaciones.lUniversidadOrigen_id,
                    p.gatbl_PConvalidaciones.lCarreraOrigen_id,
                    p.gatbl_PConvalidaciones.lUniversidadDestino_id,
                    p.gatbl_PConvalidaciones.lCarreraDestino_id,
                    p.gatbl_PConvalidaciones.sObs_desc,
                    p.gatbl_PConvalidaciones.lNro_solucitud,

                    gatbl_Postulantes = new
                    {
                        p.gatbl_Postulantes.lPostulante_id,
                        p.gatbl_Postulantes.NombreCompleto,
                        p.gatbl_Postulantes.sNombre_desc,
                        p.gatbl_Postulantes.sDocumento_nro,
                        p.gatbl_Postulantes.sDireccion_desc
                    },

                    gatbl_UniversidadesOrigen = new
                    {
                        p.gatbl_PConvalidaciones.lUniversidadOrigen_id,
                        p.gatbl_UniversidadesOrigen.sNombre_desc,
                        p.gatbl_UniversidadesOrigen.sDireccion_desc
                    },
                    gatbl_UniversidadesDestino = new
                    {
                        p.gatbl_PConvalidaciones.lUniversidadDestino_id,
                        p.gatbl_UniversidadesDestino.sNombre_desc,
                        p.gatbl_UniversidadesDestino.sDireccion_desc
                    },
                    gatbl_CarrerasOrigen = new
                    {
                        p.gatbl_PConvalidaciones.lCarreraOrigen_id,
                        p.gatbl_CarrerasOrigen.sCarrera_nm,
                        p.gatbl_CarrerasOrigen.sTelefono_desc
                    },
                    gatbl_CarrerasDestino = new
                    {
                        p.gatbl_PConvalidaciones.lCarreraDestino_id,
                        p.gatbl_CarrerasDestino.sCarrera_nm,
                        p.gatbl_CarrerasDestino.sTelefono_desc
                    },
                    Pensum = new
                    {
                        p.gatbl_PConvalidaciones.lPensum_id,
                        p.Pensum.sDescripcion
                    }
                },
                Responsables = new
                {
                    p.lResponsable_id,
                    p.Responsables.NombreCompleto,
                    p.Responsables.agd_codigo
                }
            }));
        }

        public ActionResult Index_Read_Pendiente([DataSourceRequest] DataSourceRequest request)
        {
            var pos = from po in db.gatbl_AnalisisPreConvalidaciones.Include(g => g.gatbl_PConvalidaciones).Include(g => g.Responsables)
                      join post in db.gatbl_Postulantes on po.gatbl_PConvalidaciones.lPostulante_id equals post.lPostulante_id
                      join uo in db.gatbl_Universidades on po.gatbl_PConvalidaciones.lUniversidadOrigen_id equals uo.lUniversidad_id
                      join ud in db.gatbl_Universidades on po.gatbl_PConvalidaciones.lUniversidadDestino_id equals ud.lUniversidad_id
                      join co in db.gatbl_Carreras on po.gatbl_PConvalidaciones.lCarreraOrigen_id equals co.lCarrera_id
                      join cd in db.gatbl_Carreras on po.gatbl_PConvalidaciones.lCarreraDestino_id equals cd.lCarrera_id
                      join pe in db.Pensums on po.gatbl_PConvalidaciones.lPensum_id equals pe.lPensum_id
                      where
                      db.gatbl_DetAnalisisPreConvalidaciones.Where(d=>d.iEstado_fl == "1").Any(m => m.lAnalisisPreConvalidacion_id == po.lAnalisisPreConvalidacion_id) &&
                      //!db.gatbl_AnalisisConvalidaciones.Any(m => m.lAnalisisPreConvalidacion_id == po.lAnalisisPreConvalidacion_id)
                      //&& 
                      po.iEstado_fl == "1"
                      
                      select new
                      {
                          lAnalisisPreConvalidacion_id = po.lAnalisisPreConvalidacion_id,
                          lPConvalidacion_id = po.lPConvalidacion_id,
                          sVersion_nro = po.sVersion_nro,
                          dtAnalisisConvalidacion_dt = po.dtAnalisisConvalidacion_dt,
                          lResponsable_id = po.lResponsable_id,
                          sObs_desc = po.sObs_desc,
                          gatbl_PConvalidaciones = po.gatbl_PConvalidaciones,
                          Responsables = po.gatbl_PConvalidaciones.Responsables,
                          gatbl_Postulantes = post,
                          gatbl_UniversidadesOrigen = uo,
                          gatbl_UniversidadesDestino = ud,
                          gatbl_CarrerasOrigen = co,
                          gatbl_CarrerasDestino = cd,
                          Pensum = pe

                      };
            
            return Json(pos.ToDataSourceResult(request, p => new
            {
                p.lAnalisisPreConvalidacion_id,
                p.lPConvalidacion_id,
                p.sVersion_nro,
                p.dtAnalisisConvalidacion_dt,
                p.lResponsable_id,
                p.sObs_desc,
                gatbl_PConvalidaciones = new
                {
                    p.lPConvalidacion_id,
                    p.gatbl_PConvalidaciones.lPostulante_id,
                    p.gatbl_PConvalidaciones.dtPostulacion_dt,
                    p.gatbl_PConvalidaciones.lResponsable_id,
                    p.gatbl_PConvalidaciones.lUniversidadOrigen_id,
                    p.gatbl_PConvalidaciones.lCarreraOrigen_id,
                    p.gatbl_PConvalidaciones.lUniversidadDestino_id,
                    p.gatbl_PConvalidaciones.lCarreraDestino_id,
                    p.gatbl_PConvalidaciones.sObs_desc,
                    p.gatbl_PConvalidaciones.lNro_solucitud,

                    gatbl_Postulantes = new
                    {
                        p.gatbl_Postulantes.lPostulante_id,
                        p.gatbl_Postulantes.NombreCompleto,
                        p.gatbl_Postulantes.sNombre_desc,
                        p.gatbl_Postulantes.sDocumento_nro,
                        p.gatbl_Postulantes.sDireccion_desc
                    },

                    gatbl_UniversidadesOrigen = new
                    {
                        p.gatbl_PConvalidaciones.lUniversidadOrigen_id,
                        p.gatbl_UniversidadesOrigen.sNombre_desc,
                        p.gatbl_UniversidadesOrigen.sDireccion_desc
                    },
                    gatbl_UniversidadesDestino = new
                    {
                        p.gatbl_PConvalidaciones.lUniversidadDestino_id,
                        p.gatbl_UniversidadesDestino.sNombre_desc,
                        p.gatbl_UniversidadesDestino.sDireccion_desc
                    },
                    gatbl_CarrerasOrigen = new
                    {
                        p.gatbl_PConvalidaciones.lCarreraOrigen_id,
                        p.gatbl_CarrerasOrigen.sCarrera_nm,
                        p.gatbl_CarrerasOrigen.sTelefono_desc
                    },
                    gatbl_CarrerasDestino = new
                    {
                        p.gatbl_PConvalidaciones.lCarreraDestino_id,
                        p.gatbl_CarrerasDestino.sCarrera_nm,
                        p.gatbl_CarrerasDestino.sTelefono_desc
                    },
                    Pensum = new
                    {
                        p.gatbl_PConvalidaciones.lPensum_id,
                        p.Pensum.sDescripcion
                    }
                },
                Responsables = new
                {
                    p.lResponsable_id,
                    p.Responsables.NombreCompleto,
                    p.Responsables.agd_codigo
                }
            }));
        }

        public ActionResult Index_Read_Realizado([DataSourceRequest] DataSourceRequest request)
        {
            var pos = from po in db.gatbl_AnalisisPreConvalidaciones.Include(g => g.gatbl_PConvalidaciones).Include(g => g.Responsables)
                      join dp in db.gatbl_DetAnalisisPreConvalidaciones.Include(g => g.gatbl_AnalisisPreConvalidaciones) on po.lAnalisisPreConvalidacion_id equals dp.lAnalisisPreConvalidacion_id
                      join post in db.gatbl_Postulantes on po.gatbl_PConvalidaciones.lPostulante_id equals post.lPostulante_id
                      join uo in db.gatbl_Universidades on po.gatbl_PConvalidaciones.lUniversidadOrigen_id equals uo.lUniversidad_id
                      join ud in db.gatbl_Universidades on po.gatbl_PConvalidaciones.lUniversidadDestino_id equals ud.lUniversidad_id
                      join co in db.gatbl_Carreras on po.gatbl_PConvalidaciones.lCarreraOrigen_id equals co.lCarrera_id
                      join cd in db.gatbl_Carreras on po.gatbl_PConvalidaciones.lCarreraDestino_id equals cd.lCarrera_id
                      join pe in db.Pensums on po.gatbl_PConvalidaciones.lPensum_id equals pe.lPensum_id
                      where po.iEstado_fl == "1" && dp.iEstado_fl == "3"

                      select new
                      {
                          lAnalisisPreConvalidacion_id = po.lAnalisisPreConvalidacion_id,
                          lDetAnalisisPreConvalidacion_id = dp.lDetAnalisisPreConvalidacion_id,
                          lPConvalidacion_id = po.lPConvalidacion_id,
                          sMateriaOrigen_id = dp.sMateriaOrigen_id,
                          sMateriaDestino_id = dp.sMateriaDestino_id,
                          sMateriaOrigen = dp.sMateriaOrigen,
                          sMateriaDestino = dp.sMateriaDestino,
                          sVersion_nro = po.sVersion_nro,
                          dtAnalisisConvalidacion_dt = po.dtAnalisisConvalidacion_dt,
                          lResponsable_id = po.lResponsable_id,
                          sObs_desc = po.sObs_desc,
                          gatbl_PConvalidaciones = po.gatbl_PConvalidaciones,
                          Responsables = po.gatbl_PConvalidaciones.Responsables,
                          gatbl_Postulantes = post,
                          gatbl_UniversidadesOrigen = uo,
                          gatbl_UniversidadesDestino = ud,
                          gatbl_CarrerasOrigen = co,
                          gatbl_CarrerasDestino = cd,
                          gatbl_DetAnalisisPreConvalidaciones = dp,
                          gatbl_AnalisisPreConvalidaciones = dp.gatbl_AnalisisPreConvalidaciones,
                          Pensum = pe

                      };

            return Json(pos.ToDataSourceResult(request, p => new
            {                
                p.lDetAnalisisPreConvalidacion_id,
                p.sMateriaOrigen_id,
                p.sMateriaDestino_id,
                p.sMateriaOrigen,
                p.sMateriaDestino,
                gatbl_AnalisisPreConvalidaciones = new
                {
                    p.gatbl_AnalisisPreConvalidaciones.lAnalisisPreConvalidacion_id,
                    p.gatbl_AnalisisPreConvalidaciones.lPConvalidacion_id,
                    p.gatbl_AnalisisPreConvalidaciones.sVersion_nro,
                    //p.gatbl_AnalisisPreConvalidaciones.dtAnalisisConvalidacion_dt,
                    p.gatbl_AnalisisPreConvalidaciones.lResponsable_id,
                    p.gatbl_AnalisisPreConvalidaciones.sObs_desc,
                    p.dtAnalisisConvalidacion_dt,
                    gatbl_PConvalidaciones = new
                    {
                        p.lPConvalidacion_id,
                        p.gatbl_PConvalidaciones.lPostulante_id,
                        p.gatbl_PConvalidaciones.dtPostulacion_dt,
                        p.gatbl_PConvalidaciones.lResponsable_id,
                        p.gatbl_PConvalidaciones.lUniversidadOrigen_id,
                        p.gatbl_PConvalidaciones.lCarreraOrigen_id,
                        p.gatbl_PConvalidaciones.lUniversidadDestino_id,
                        p.gatbl_PConvalidaciones.lCarreraDestino_id,
                        p.gatbl_PConvalidaciones.sObs_desc,
                        p.gatbl_PConvalidaciones.lNro_solucitud,

                        gatbl_Postulantes = new
                        {
                            p.gatbl_Postulantes.lPostulante_id,
                            p.gatbl_Postulantes.NombreCompleto,
                            p.gatbl_Postulantes.sNombre_desc,
                            p.gatbl_Postulantes.sDocumento_nro,
                            p.gatbl_Postulantes.sDireccion_desc
                        },

                        gatbl_UniversidadesOrigen = new
                        {
                            p.gatbl_PConvalidaciones.lUniversidadOrigen_id,
                            p.gatbl_UniversidadesOrigen.sNombre_desc,
                            p.gatbl_UniversidadesOrigen.sDireccion_desc
                        },
                        gatbl_UniversidadesDestino = new
                        {
                            p.gatbl_PConvalidaciones.lUniversidadDestino_id,
                            p.gatbl_UniversidadesDestino.sNombre_desc,
                            p.gatbl_UniversidadesDestino.sDireccion_desc
                        },
                        gatbl_CarrerasOrigen = new
                        {
                            p.gatbl_PConvalidaciones.lCarreraOrigen_id,
                            p.gatbl_CarrerasOrigen.sCarrera_nm,
                            p.gatbl_CarrerasOrigen.sTelefono_desc
                        },
                        gatbl_CarrerasDestino = new
                        {
                            p.gatbl_PConvalidaciones.lCarreraDestino_id,
                            p.gatbl_CarrerasDestino.sCarrera_nm,
                            p.gatbl_CarrerasDestino.sTelefono_desc
                        },
                        Pensum = new
                        {
                            p.gatbl_PConvalidaciones.lPensum_id,
                            p.Pensum.sDescripcion
                        }
                    },
                    Responsables = new
                    {
                        p.lResponsable_id,
                        p.Responsables.NombreCompleto,
                        p.Responsables.agd_codigo
                    }
                }                
                
            }));
        }

        public JsonResult Tooltip(int? id)
        {
            //var materias = from m in db.gatbl_ProgramasAnaliticos
            //               where (id.HasValue ? m.lCarrera_id == id : m.lCarrera_id == 0)
            //               select new
            //               {
            //                   id = m.lProgramaAnalitico_id,
            //                   Name = m.sMateria_desc,
            //                   hasChildren = m.gatbl_DProgramasAnaliticos.Any()
            //               };

            return Json("Hola Mundo", JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult ReportsConvalidation(int ID)
        {
            return View(new rptInformeConvalidacion() { Id = ID, dtFechaSolicitud_dt = DateTime.Now, dtFechaConvalidacion_dt = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult ReportsConvalidation(rptInformeConvalidacion rptInformeConvalidacion)
        {
            //if (ModelState.IsValid)
            //{
            //    ReportProcessor reportProcessor = new ReportProcessor();
            //    Telerik.Reporting.InstanceReportSource instanceReportSource = new Telerik.Reporting.InstanceReportSource();

            //    ConvalidacionesReports.rptInformeFinal rpt = new ConvalidacionesReports.rptInformeFinal();
            //    rpt.ReportParameters["ID"].Value = rptInformeConvalidacion.Id;
            //    rpt.ReportParameters["FechaConvalidacion"].Value = rptInformeConvalidacion.dtFechaConvalidacion_dt.ToString("dd/MM/yyyy");

            //    instanceReportSource.ReportDocument = rpt;
            //    RenderingResult result = reportProcessor.RenderReport("PDF", instanceReportSource, null);

            //    this.Response.Clear();
            //    this.Response.ContentType = result.MimeType;
            //    this.Response.Cache.SetCacheability(HttpCacheability.Private);
            //    this.Response.Expires = -1;
            //    this.Response.Buffer = true;


            //    this.Response.BinaryWrite(result.DocumentBytes);
            //    this.Response.End();

            //    return View();
            //}

            //return View(rptInformeConvalidacion);

            ReportProcessor reportProcessor = new ReportProcessor();
            Telerik.Reporting.InstanceReportSource instanceReportSource = new Telerik.Reporting.InstanceReportSource();

            ConvalidacionesReports.rptInformeFinal rpt = new ConvalidacionesReports.rptInformeFinal();
            rpt.ReportParameters["ID"].Value = rptInformeConvalidacion.Id;
            rpt.ReportParameters["FechaSolicitud"].Value = rptInformeConvalidacion.dtFechaSolicitud_dt.ToString("dd/MM/yyyy");
            rpt.ReportParameters["FechaConvalidacion"].Value = rptInformeConvalidacion.dtFechaConvalidacion_dt.ToString("dd/MM/yyyy");

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

        public virtual ActionResult ReportConvalidation(int ID)
        {
            ReportProcessor reportProcessor = new ReportProcessor();
            Telerik.Reporting.InstanceReportSource instanceReportSource = new Telerik.Reporting.InstanceReportSource();

            ConvalidacionesReports.rptInformeFinal rpt = new ConvalidacionesReports.rptInformeFinal();
            rpt.ReportParameters["ID"].Value = ID;
            rpt.ReportParameters["FechaConvalidacion"].Value = Convert.ToDateTime(Request["dtFechaConvalidacion_dt"]).ToString("dd/MM/yyyy");

            instanceReportSource.ReportDocument = rpt;
            RenderingResult result = reportProcessor.RenderReport("PDF", instanceReportSource, null);

            //string path = Server.MapPath("Informe");

            //using (FileStream fs = new FileStream(path, FileMode.Create))
            //{
            //    fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
            //}

            //var reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();
            //var typeReportSource = new Telerik.Reporting.TypeReportSource();

            //// reportToExport is the Assembly Qualified Name of the report

            ////typeReportSource = ConvalidacionesReports.rptInformeConvalidacion;
            //typeReportSource.TypeName = "ConvalidacionesReports.rptInformeConvalidacion";

            //var result = reportProcessor.RenderReport("PDF", typeReportSource, null);

            this.Response.Clear();
            this.Response.ContentType = result.MimeType;
            this.Response.Cache.SetCacheability(HttpCacheability.Private);
            this.Response.Expires = -1;
            this.Response.Buffer = true;

            /* Uncomment to handle the file as attachment
             Response.AddHeader("Content-Disposition",
                            string.Format("{0};FileName=\"{1}\"",
                                            "attachment",
                                            fileName));
             */

            this.Response.BinaryWrite(result.DocumentBytes);
            this.Response.End();

            return View();
        }
        

        public JsonResult CarreraList(int id)
        {
            var carreras = from s in db.gatbl_Carreras
                             where s.lUniversidad_id == id
                             select s;

            return Json(new SelectList(carreras.ToArray(), "lCarrera_id", "sCarrera_nm"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Materias(int? id)
        {
            var materias = from m in db.gatbl_ProgramasAnaliticos
                            where (id.HasValue ? m.lCarrera_id == id : m.lCarrera_id == 0)
                            select new
                            {
                                id = m.lProgramaAnalitico_id,
                                Name = m.sMateria_desc,
                                hasChildren = m.gatbl_DProgramasAnaliticos.Any()
                            };

            return Json(materias, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Unidades(int? id)
        {
            var materias = from m in db.gatbl_DProgramasAnaliticos
                           where (id.HasValue ? m.lProgramaAnalitico_id == id : m.lProgramaAnalitico_id == 0)
                           select new
                           {
                               id = m.lDProgramaAnalitico_id,
                               Name = m.sUnidad_desc + " - " + m.sUnidad_desc,
                               hasChildren = false
                           };

            return Json(materias, JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<TreeViewItemModel> GetDefaultInlineData(int? CarreraId)
        {
            List<TreeViewItemModel> inlineDefault = new List<TreeViewItemModel>();

            var materias = db.gatbl_ProgramasAnaliticos.Where(m => m.lCarrera_id == CarreraId).ToList();

            //var materias = db.gatbl_ProgramasAnaliticos.ToList();

            foreach (var item in materias)
            {
                TreeViewItemModel itmateria = new TreeViewItemModel();
                itmateria.Text = item.sMateria_desc;
                itmateria.Id = item.lProgramaAnalitico_id.ToString();

                var unidades = db.gatbl_DProgramasAnaliticos.Where(u => u.lProgramaAnalitico_id == item.lProgramaAnalitico_id).ToList();
                foreach (var itemm in unidades)
                {
                    TreeViewItemModel itunidad = new TreeViewItemModel();
                    itunidad.Text = string.Format("{0} - {1}", itemm.sUnidad_nro, itemm.sUnidad_desc);
                    itunidad.Id = itemm.lDProgramaAnalitico_id.ToString();

                    itmateria.Items.Add(itunidad);
                }

                inlineDefault.Add(itmateria);                
            }
            
            return inlineDefault;
        }

        private IEnumerable<TreeViewItemModel> mostrarNodos(ref List<TreeViewItemModel> inlineDefault, int MateriaID, int? id, TreeViewItemModel pad)
        {            
            var lista = GetUnitByProgram(MateriaID, id);
            foreach(var item in lista.ToList())
            {
                TreeViewItemModel nodo = new TreeViewItemModel();
                nodo.Text = item.sDescripcion_desc;
                nodo.Id = item.lDetProgramaAnalitico_id.ToString();
                pad.Items.Add(nodo);
                mostrarNodos(ref inlineDefault, MateriaID, item.lDetProgramaAnalitico_id, nodo);
            }

            return inlineDefault;
        }

        private IQueryable<gatbl_DetProgramasAnaliticos> GetUnitByProgram(int MateriaID, int? padreID)
        {
            var detalle = from d in db.gatbl_DetProgramasAnaliticos
                          where d.lProgramaAnalitico_id == MateriaID && d.lDetProgramaAnaliticoPadre_id == padreID                          
                          select d;

            return detalle;
        }

        private IEnumerable<TreeViewItemModel> GetEditDetailProgramDataUnit(int? id, int programaID)
        {
            List<TreeViewItemModel> inlineDefault = new List<TreeViewItemModel>();

            //var Unidades = from u in db.gatbl_DetProgramasAnaliticos
            //               where u.lDetProgramaAnalitico_id == id && u.Nivel == 0
            //               select u;                       

            string sql = string.Format("Exec sp_ReporteProgramaAnalitico {0}", programaID);

            var programa = db.Database.SqlQuery<gatbl_DetProgramasAnaliticos>(sql).ToList<gatbl_DetProgramasAnaliticos>();

            var prog = db.gatbl_ProgramasAnaliticos.Find(programaID);

            TreeViewItemModel itMateria = new TreeViewItemModel();
            itMateria.Text = prog.sMateria_desc;
            itMateria.Id = prog.lProgramaAnalitico_id.ToString();


            foreach (var itemu in programa.Where(p=>p.lDetProgramaAnalitico_id == id && p.Nivel == 0).ToList())
            {
                TreeViewItemModel itunidad = new TreeViewItemModel();
                itunidad.Text = string.Format("{0} - {1}", itemu.sCodigo, itemu.sDescripcion_desc);
                itunidad.Id = itemu.lDetProgramaAnalitico_id.ToString();
                

                //var temas = from t in db.gatbl_DetProgramasAnaliticos
                //            where t.lProgramaAnalitico_id == itemu.lProgramaAnalitico_id
                //            && t.Nivel > 0 && t.sCodigo.StartsWith(itemu.sCodigo)
                //            orderby t.sCodigo
                //            select t;

                foreach (var itemt in programa.Where(p=>p.Nivel > 0 && p.sCodigo.StartsWith(itemu.sCodigo)))
                {
                    TreeViewItemModel ittema = new TreeViewItemModel();
                    ittema.Text = string.Format("{0} - {1}", itemt.sCodigo, itemt.sDescripcion_desc);
                    ittema.Id = itemt.lDetProgramaAnalitico_id.ToString();

                    ittema.ImageUrl = "~/Content/Web/img/allow_list2_32.png";
                    //ittema.Checked = true;

                    //ittema.Checked = unitchecked.ToList().FindIndex(m => m.lUnidad_nro == itemu.lDProgramaAnalitico_id) != -1;

                    itunidad.Items.Add(ittema);
                }


                itMateria.Items.Add(itunidad);
                inlineDefault.Add(itMateria);
            }

            return inlineDefault;
        }

        private IEnumerable<TreeViewItemModel> GetEditDetailProgramDataAll(int? id)
        {
            List<TreeViewItemModel> inlineDefault = new List<TreeViewItemModel>();

            var materias = from m in db.gatbl_ProgramasAnaliticos
                           join d in db.gatbl_AnalisisPreConvalidacionesMateria 
                           on m.lProgramaAnalitico_id equals d.lProgramaAnalitico_id
                           where d.lDetAnalisisPreConvalidacion_id == id && d.lOrigen == 1
                           orderby m.sMateria_desc
                           select m;

            foreach (var itm in materias.ToList())
            {
                TreeViewItemModel itmateria = new TreeViewItemModel();
                itmateria.Text = itm.sMateria_desc;
                itmateria.Id = itm.lProgramaAnalitico_id.ToString();               

                string sql = string.Format("Exec sp_ReporteProgramaAnalitico {0}", itm.lProgramaAnalitico_id);

                var programa = db.Database.SqlQuery<gatbl_DetProgramasAnaliticos>(sql).ToList<gatbl_DetProgramasAnaliticos>();


                foreach (var itemu in programa.Where(p => p.Nivel == 0).OrderBy(p => p.sNumero).ToList())
                {
                    TreeViewItemModel itunidad = new TreeViewItemModel();
                    itunidad.Text = string.Format("{0} - {1}", itemu.sCodigo, itemu.sDescripcion_desc);
                    itunidad.Id = itemu.lDetProgramaAnalitico_id.ToString();


                    foreach (var itemt in programa.Where(p => p.Nivel > 0 && p.sCodigo.StartsWith(itemu.sCodigo)))
                    {
                        TreeViewItemModel ittema = new TreeViewItemModel();
                        ittema.Text = string.Format("{0} - {1}", itemt.sCodigo, itemt.sDescripcion_desc);
                        ittema.Id = itemt.lDetProgramaAnalitico_id.ToString();

                        ittema.Checked = false;

                        //ittema.Checked = unitchecked.ToList().FindIndex(m => m.lUnidad_nro == itemu.lDProgramaAnalitico_id) != -1;

                        itunidad.Items.Add(ittema);
                    }

                    itmateria.Items.Add(itunidad);                                        
                }

                inlineDefault.Add(itmateria);
            }

            

            return inlineDefault;
        }

        private IEnumerable<TreeViewItemModel> GetEditDetailProgramDataCheckedAll(int? id)
        {
            List<TreeViewItemModel> inlineDefault = new List<TreeViewItemModel>();

            string sql = string.Format("Exec sp_UnidadesAnalisis {0}", id);

            var programa = db.Database.SqlQuery<gatbl_DetProgramasAnaliticos>(sql).ToList<gatbl_DetProgramasAnaliticos>();

            var materias = from m in db.gatbl_ProgramasAnaliticos
                           join d in db.gatbl_AnalisisPreConvalidacionesMateria
                           on m.lProgramaAnalitico_id equals d.lProgramaAnalitico_id
                           join a in db.gatbl_AnalisisConvalidacionesUnidad
                           on d.lDetAnalisisPreConvalidacion_id equals a.lDetAnalisisPreConvalidacion_id
                           where a.lAnalisisConvalidacionUnidad_id == id && d.lOrigen == 1
                           orderby m.sMateria_desc
                           select m;

            foreach (var itm in materias.ToList())
            {
                TreeViewItemModel itmateria = new TreeViewItemModel();
                itmateria.Text = itm.sMateria_desc;
                itmateria.Id = itm.lProgramaAnalitico_id.ToString();

                foreach (var itemu in programa.Where(p => p.lProgramaAnalitico_id == itm.lProgramaAnalitico_id && p.Nivel == 0).ToList())
                {
                    TreeViewItemModel itunidad = new TreeViewItemModel();
                    itunidad.Text = string.Format("{0} - {1}", itemu.sCodigo, itemu.sDescripcion_desc);
                    itunidad.Id = itemu.lDetProgramaAnalitico_id.ToString();


                    foreach (var itemt in programa.Where(p => p.lProgramaAnalitico_id == itm.lProgramaAnalitico_id && p.Nivel > 0 && p.sCodigo.StartsWith(itemu.sCodigo)))
                    {
                        TreeViewItemModel ittema = new TreeViewItemModel();
                        ittema.Text = string.Format("{0} - {1}", itemt.sCodigo, itemt.sDescripcion_desc);
                        ittema.Id = itemt.lDetProgramaAnalitico_id.ToString();

                        ittema.Checked = false;

                        //ittema.Checked = unitchecked.ToList().FindIndex(m => m.lUnidad_nro == itemu.lDProgramaAnalitico_id) != -1;

                        itunidad.Items.Add(ittema);
                    }

                    itmateria.Items.Add(itunidad);

                    
                }

                inlineDefault.Add(itmateria);
            }            

            return inlineDefault;
        }


        private IEnumerable<TreeViewItemModel> GetEditDetailProgramData(int? id, string source)
        {
            List<TreeViewItemModel> inlineDefault = new List<TreeViewItemModel>();

            var materias = from c in db.gatbl_DAnalisisPreConvalidaciones
                           join p in db.gatbl_ProgramasAnaliticos on c.lMateria_id equals p.lProgramaAnalitico_id
                           where c.lAnalisisPreConvalidacion_id == id && c.sOrigenMateria_fl == source
                           select p;

            foreach (var item in materias.ToList())
            {                
                TreeViewItemModel itmateria = new TreeViewItemModel();
                itmateria.Text = item.sMateria_desc;
                itmateria.Id = item.lProgramaAnalitico_id.ToString();
                //itmateria.Checked = MateriasChecked.ToList().FindIndex(m => m.lProgramaAnalitico_id == item.lProgramaAnalitico_id) != -1;

                //var unidades = from u in db.gatbl_DProgramasAnaliticos
                //               where  u.lProgramaAnalitico_id == item.lProgramaAnalitico_id
                //               select u;


                var unidades = from u in db.gatbl_DetProgramasAnaliticos
                               where u.lProgramaAnalitico_id == item.lProgramaAnalitico_id
                               && u.Nivel == 0
                               orderby u.sCodigo
                               select u;

                foreach (var itemu in unidades.ToList())
                {
                    TreeViewItemModel itunidad = new TreeViewItemModel();
                    itunidad.Text = string.Format("{0} - {1}", itemu.sCodigo, itemu.sDescripcion_desc);
                    itunidad.Id = itemu.lDetProgramaAnalitico_id.ToString();
                    //itunidad.Id = itemu.lDProgramaAnalitico_id.ToString();                    
                    //itunidad.Checked = unitchecked.ToList().FindIndex(m => m.lUnidad_nro == itemu.lDProgramaAnalitico_id) != -1;

                    //var temas = from t in db.gatbl_DProgramasAnaliticosTemas
                    //            where t.lDProgramaAnalitico_id == itemu.lDProgramaAnalitico_id
                    //            select t;

                    var temas = from t in db.gatbl_DetProgramasAnaliticos
                                where t.lProgramaAnalitico_id == item.lProgramaAnalitico_id
                                && t.Nivel > 0 && t.sCodigo.StartsWith(itemu.sCodigo)
                                orderby t.sCodigo
                                select t;

                    foreach (var itemt in temas)
                    {
                        TreeViewItemModel ittema = new TreeViewItemModel();
                        ittema.Text = string.Format("{0} - {1}", itemt.sCodigo, itemt.sDescripcion_desc);
                        ittema.Id = itemt.lDetProgramaAnalitico_id.ToString();
                        if (source.Equals("01"))
                        {
                            ittema.ImageUrl = "~/Content/Web/img/allow_list2_32.png";
                            ittema.Checked = true;                            
                        }
                        
                        //ittema.Checked = unitchecked.ToList().FindIndex(m => m.lUnidad_nro == itemu.lDProgramaAnalitico_id) != -1;

                        itunidad.Items.Add(ittema);
                    }

                    itmateria.Items.Add(itunidad);
                }

                inlineDefault.Add(itmateria);
            }

            return inlineDefault;
        }

        private IEnumerable<TreeViewItemModel> GetEditDetailUnitData(int? id, int carreraID)
        {
            List<TreeViewItemModel> inlineDefault = new List<TreeViewItemModel>();

            var materias = db.gatbl_ProgramasAnaliticos.Where(m => m.lCarrera_id == carreraID).ToList();

            //var materias = from m in db.gatbl_ProgramasAnaliticos
            //               join dm in db.gatbl_DAnalisisConvalidaciones on m.lProgramaAnalitico_id equals dm.lMateria_id
            //               where dm.lAnalisisConvalidacion_id == id && dm.sOrigenMateria_fl.Equals(source)
            //               select new
            //               {
            //                   lProgramaAnalitico_id = m.lProgramaAnalitico_id,
            //                   sMateria_desc = m.sMateria_desc,
            //                   lDAnalisisConvalidacion_id = dm.lDAnalisisConvalidacion_id
            //               };

            foreach (var item in materias)
            {
                TreeViewItemModel itmateria = new TreeViewItemModel();
                itmateria.Text = item.sMateria_desc;
                itmateria.Id = item.lProgramaAnalitico_id.ToString();

                var unidades = db.gatbl_DProgramasAnaliticos.Where(u => u.lProgramaAnalitico_id == item.lProgramaAnalitico_id).ToList();

                //var unitchecked = from m in db.gatbl_DAnalisisConvalidaciones
                //                 join dm in db.gatbl_UnidadesConvalidadas on m.lDAnalisisConvalidacion_id equals dm.lDAnalisisConvalidacion_id
                //                 where m.lMateria_id == item.lProgramaAnalitico_id
                //                 select dm;

                var unitchecked = from u in db.gatbl_DProgramasAnaliticos                                                                    
                                  join du in db.gatbl_UnidadesConvalidadas on u.lDProgramaAnalitico_id equals du.lUnidad_nro 
                                  join da in db.gatbl_DAnalisisConvalidaciones on du.lDAnalisisConvalidacion_id equals da.lDAnalisisConvalidacion_id
                                  join ac in db.gatbl_AnalisisConvalidaciones on da.lAnalisisConvalidacion_id equals ac.lAnalisisConvalidacion_id                                 
                                  where u.lProgramaAnalitico_id == item.lProgramaAnalitico_id
                                  && ac.lAnalisisConvalidacion_id == id                                  
                                  select du;

                foreach (var itemu in unidades)
                {
                    TreeViewItemModel itunidad = new TreeViewItemModel();
                    itunidad.Text = string.Format("{0} - {1}", itemu.sUnidad_nro, itemu.sUnidad_desc);
                    itunidad.Id = itemu.lDProgramaAnalitico_id.ToString();
                    itunidad.Checked = unitchecked.ToList().FindIndex(m => m.lUnidad_nro == itemu.lDProgramaAnalitico_id)!= -1;

                    itmateria.Items.Add(itunidad);
                }

                inlineDefault.Add(itmateria);
            }

            return inlineDefault;
        }

        private IEnumerable<TreeViewItemModel> GetDetailUnitData(int? id, string source)
        {
            List<TreeViewItemModel> inlineDefault = new List<TreeViewItemModel>();

            var materias = from p in db.gatbl_ProgramasAnaliticos
                               //join dm in db.gatbl_DAnalisisConvalidaciones on m.lProgramaAnalitico_id equals dm.lMateria_id
                               //where dm.lAnalisisConvalidacion_id == id && dm.sOrigenMateria_fl.Equals(source)  
                           where db.gatbl_DAnalisisConvalidaciones.Any(m => m.lMateria_id == p.lProgramaAnalitico_id && m.sOrigenMateria_fl.Equals(source) 
                           && m.lAnalisisConvalidacion_id == id)                          
                           select new { lProgramaAnalitico_id = p.lProgramaAnalitico_id, sMateria_desc = p.sMateria_desc};
           
            foreach (var item in materias.ToList())
            {
                TreeViewItemModel itmateria = new TreeViewItemModel();
                itmateria.Text = item.sMateria_desc;
                itmateria.Id = item.lProgramaAnalitico_id.ToString();

                var unidades = from u in db.gatbl_DProgramasAnaliticos
                               //join du in db.gatbl_DAnalisisConvalidaciones on u.lDProgramaAnalitico_id equals du.lUnidad_id
                               where u.lProgramaAnalitico_id == item.lProgramaAnalitico_id
                               && db.gatbl_DAnalisisConvalidaciones.Any(m => m.lUnidad_id == u.lDProgramaAnalitico_id
                               && m.lAnalisisConvalidacion_id == id)                               
                               select u;                    
                foreach (var itemm in unidades.ToList())
                {
                    TreeViewItemModel itunidad = new TreeViewItemModel();
                    itunidad.Text = string.Format("{0} - {1}", itemm.sUnidad_nro, itemm.sUnidad_desc);
                    itunidad.Id = itemm.lDProgramaAnalitico_id.ToString();

                    var temas = from t in db.gatbl_DProgramasAnaliticosTemas
                                       //join du in db.gatbl_DAnalisisConvalidaciones on u.lDProgramaAnalitico_id equals du.lUnidad_id
                                   where t.lDProgramaAnalitico_id == itemm.lDProgramaAnalitico_id
                                   && db.gatbl_DAnalisisConvalidaciones.Any(m => m.lTema_id == t.lDProgramaAnaliticoTema_id
                                   && m.lAnalisisConvalidacion_id == id)
                                   select t;
                    foreach (var itemt in temas.ToList())
                    {
                        TreeViewItemModel ittema = new TreeViewItemModel();
                        ittema.Text = itemt.sTema_desc;
                        ittema.Id = itemm.lDProgramaAnalitico_id.ToString();

                        itunidad.Items.Add(ittema);
                    }

                    itmateria.Items.Add(itunidad);
                }

                inlineDefault.Add(itmateria);
            }

            return inlineDefault;
        }

        public JsonResult ObtenerResponsables(string text)
        {
            gatbl_AnalisisConvalidaciones analisis = Session["analisis"] as gatbl_AnalisisConvalidaciones;

            if (string.IsNullOrEmpty(text) && analisis.lResponsable_id != null)
            {
                return Json((from ag in db.agendas
                             select new { agd_nombres = ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + ", " + ag.agd_nombres.Trim(), agd_codigo = ag.agd_codigo.Trim(), agd_docnro = ag.agd_docnro.Trim() }).Where(g => g.agd_codigo.Contains(analisis.lResponsable_id)).OrderBy(g => g.agd_nombres), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json((from ag in db.agendas
                             select new { agd_nombres = ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + ", " + ag.agd_nombres.Trim(), agd_codigo = ag.agd_codigo.Trim(), agd_docnro = ag.agd_docnro.Trim() }).Where(g => g.agd_nombres.Contains(text)).OrderBy(g => g.agd_nombres), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult MateriasAConvalidar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_AnalisisPreConvalidaciones gatbl_AnalisisPreConvalidaciones = db.gatbl_AnalisisPreConvalidaciones.Find(id);
            if (gatbl_AnalisisPreConvalidaciones == null)
            {
                return HttpNotFound();
            }

            //gatbl_AnalisisConvalidaciones gatbl_AnalisisConvalidaciones = new gatbl_AnalisisConvalidaciones();
            //gatbl_AnalisisConvalidaciones.gatbl_AnalisisPreConvalidaciones = gatbl_AnalisisPreConvalidaciones;
            //gatbl_AnalisisConvalidaciones.lAnalisisPreConvalidacion_id = Convert.ToInt32(id);
            
            return View(gatbl_AnalisisPreConvalidaciones);
        }        

        public ActionResult Analisis(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            gatbl_DetAnalisisPreConvalidaciones detallepreanalisis = db.gatbl_DetAnalisisPreConvalidaciones.Find(id);
            gatbl_AnalisisPreConvalidaciones gatbl_AnalisisPreConvalidaciones = db.gatbl_AnalisisPreConvalidaciones.Find(detallepreanalisis.lAnalisisPreConvalidacion_id);
            if (gatbl_AnalisisPreConvalidaciones == null)
            {
                return HttpNotFound();
            }

            gatbl_AnalisisConvalidaciones gatbl_AnalisisConvalidaciones = new gatbl_AnalisisConvalidaciones();
            gatbl_AnalisisConvalidaciones.gatbl_AnalisisPreConvalidaciones = gatbl_AnalisisPreConvalidaciones;
            gatbl_AnalisisConvalidaciones.lAnalisisPreConvalidacion_id = Convert.ToInt32(id);

            ViewBag.MateriasOrigen = GetEditDetailProgramData(id, "01");
            ViewBag.MateriasDestino = GetEditDetailProgramData(id, "02");

            return View(gatbl_AnalisisConvalidaciones);
        }

        public ActionResult VerAnalisisUnidad(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            
            var analisis = db.gatbl_AnalisisConvalidacionesUnidad.Find(id);
            var detalle = db.gatbl_DetAnalisisPreConvalidaciones.Find(analisis.lDetAnalisisPreConvalidacion_id);
            var detprog = db.gatbl_DetProgramasAnaliticos.Find(analisis.lDetProgramaAnalitico_id);


            ViewBag.MateriasDestino = GetEditDetailProgramDataUnit(detprog.lDetProgramaAnalitico_id, Convert.ToInt32(detprog.lProgramaAnalitico_id));
            ViewBag.MateriasOrigen = GetEditDetailProgramDataCheckedAll(analisis.lAnalisisConvalidacionUnidad_id);

            gatbl_AnalisisConvalidacionesUnidad gatbl_AnalisisConvalidacionesUnidad = analisis;
            gatbl_AnalisisConvalidacionesUnidad.gatbl_DetAnalisisPreConvalidaciones = detalle;
            gatbl_AnalisisConvalidacionesUnidad.lDetAnalisisPreConvalidacion_id = detalle.lDetAnalisisPreConvalidacion_id;
            //gatbl_AnalisisConvalidacionesUnidad.lDetProgramaAnalitico_id = Convert.ToInt32(id);

            return View(gatbl_AnalisisConvalidacionesUnidad);
        }

        [HttpPost, ActionName("VerUnidades")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmarAnalisisUnidad(int id)
        {
            var preanalisis = db.gatbl_DetAnalisisPreConvalidaciones.Find(id);            
            
            preanalisis.iEstado_fl = "3";

            db.Entry(preanalisis).State = EntityState.Modified;            
            db.SaveChanges();

            return RedirectToAction("MateriasAConvalidar", "gatbl_AnalisisConvalidaciones", new { id = preanalisis.lAnalisisPreConvalidacion_id });
        }

        public ActionResult EliminarAnalisisUnidad(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var analisis = db.gatbl_AnalisisConvalidacionesUnidad.Find(id);
            var detalle = db.gatbl_DetAnalisisPreConvalidaciones.Find(analisis.lDetAnalisisPreConvalidacion_id);
            var detprog = db.gatbl_DetProgramasAnaliticos.Find(analisis.lDetProgramaAnalitico_id);

            
            ViewBag.MateriasDestino = GetEditDetailProgramDataUnit(detprog.lDetProgramaAnalitico_id, Convert.ToInt32(detprog.lProgramaAnalitico_id));
            ViewBag.MateriasOrigen = GetEditDetailProgramDataCheckedAll(analisis.lAnalisisConvalidacionUnidad_id);

            gatbl_AnalisisConvalidacionesUnidad gatbl_AnalisisConvalidacionesUnidad = analisis;
            gatbl_AnalisisConvalidacionesUnidad.gatbl_DetAnalisisPreConvalidaciones = detalle;
            gatbl_AnalisisConvalidacionesUnidad.lDetAnalisisPreConvalidacion_id = detalle.lDetAnalisisPreConvalidacion_id;
            
            return View(gatbl_AnalisisConvalidacionesUnidad);
        }
       
        [HttpPost, ActionName("EliminarAnalisisUnidad")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmedEliminarAnalisisUnidad(int id)
        {            
            var analisis = db.gatbl_AnalisisConvalidacionesUnidad.Find(id);
            var detalle = from d in db.gatbl_DAnalisisConvalidacionesUnidad
                          where d.lAnalisisConvalidacionUnidad_id == id
                          select d;
            var ID = analisis.lDetAnalisisPreConvalidacion_id;

            foreach (var item in detalle)
            {
                db.gatbl_DAnalisisConvalidacionesUnidad.Remove(item);
            }

            db.SaveChanges();

            db.gatbl_AnalisisConvalidacionesUnidad.Remove(analisis);
            db.SaveChanges();

            return RedirectToAction("VerUnidades", "gatbl_AnalisisConvalidaciones", new { id=ID});
        }

        public ActionResult AnalisisUnidad(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var detalleanalisis = ((ViewModels.PromedioEquivalenciaMateria)Session["vAnalisis"]).gatbl_DetAnalisisPreConvalidaciones;
            var detalle = db.gatbl_DetProgramasAnaliticos.Find(id);

            ViewBag.MateriasDestino = GetEditDetailProgramDataUnit(id, Convert.ToInt32(detalle.lProgramaAnalitico_id));
            ViewBag.MateriasOrigen = GetEditDetailProgramDataAll(Convert.ToInt32(detalleanalisis.lDetAnalisisPreConvalidacion_id));

            gatbl_AnalisisConvalidacionesUnidad gatbl_AnalisisConvalidacionesUnidad = new gatbl_AnalisisConvalidacionesUnidad();
            gatbl_AnalisisConvalidacionesUnidad.gatbl_DetAnalisisPreConvalidaciones = detalleanalisis;
            gatbl_AnalisisConvalidacionesUnidad.lDetAnalisisPreConvalidacion_id = detalleanalisis.lDetAnalisisPreConvalidacion_id;
            gatbl_AnalisisConvalidacionesUnidad.lDetProgramaAnalitico_id = Convert.ToInt32(id);

            return View(gatbl_AnalisisConvalidacionesUnidad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AnalisisUnidad(gatbl_AnalisisConvalidacionesUnidad gatbl_AnalisisConvalidacionesUnidad)
        {
            try
            {                
                string TemaListOrigen = gatbl_AnalisisConvalidacionesUnidad.sCreated_by;

                var analisis = new gatbl_AnalisisConvalidacionesUnidad
                {
                    lDetProgramaAnalitico_id = gatbl_AnalisisConvalidacionesUnidad.lDetProgramaAnalitico_id,
                    lDetAnalisisPreConvalidacion_id = gatbl_AnalisisConvalidacionesUnidad.lDetAnalisisPreConvalidacion_id,
                    dtAnalisisConvalidacionUnidad_dt = DateTime.Now,
                    sObs_desc = gatbl_AnalisisConvalidacionesUnidad.sObs_desc,
                    dEquivalencia_Unidad = gatbl_AnalisisConvalidacionesUnidad.dEquivalencia_Unidad,
                    iEstado_fl = "1",
                    iEliminado_fl = "1",
                    sCreated_by = DateTime.Now.ToString(),
                    iConcurrencia_id = 1,
                    lResponsable_id = "0000001138"
                };


                db.gatbl_AnalisisConvalidacionesUnidad.Add(analisis);
                db.SaveChanges();
                
                //Materias Origen
                foreach (string TemaID in TemaListOrigen.Split(','))
                {
                    int tID = Convert.ToInt32(TemaID);
                    
                    var DAnalisis = new gatbl_DAnalisisConvalidacionesUnidad
                    {
                        lDetProgramaAnalitico_id = tID,
                        lAnalisisConvalidacionUnidad_id = analisis.lAnalisisConvalidacionUnidad_id
                    };

                    db.gatbl_DAnalisisConvalidacionesUnidad.Add(DAnalisis);
                }

                db.SaveChanges();
                

                return RedirectToAction("VerUnidades", "gatbl_AnalisisConvalidaciones", new { id=analisis.lDetAnalisisPreConvalidacion_id});
            }
            catch (Exception ex)
            {
                //ViewBag.MateriasOrigen = GetEditDetailProgramData(gatbl_AnalisisConvalidaciones.gatbl_AnalisisPreConvalidaciones.lAnalisisPreConvalidacion_id, "01");
                //ViewBag.MateriasDestino = GetEditDetailProgramData(gatbl_AnalisisConvalidaciones.gatbl_AnalisisPreConvalidaciones.lAnalisisPreConvalidacion_id, "02");

                return View(gatbl_AnalisisConvalidacionesUnidad);
            }
        }

        public ActionResult VerUnidades(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            gatbl_DetAnalisisPreConvalidaciones detallepreanalisis = db.gatbl_DetAnalisisPreConvalidaciones.Find(id);
            gatbl_AnalisisPreConvalidaciones gatbl_AnalisisPreConvalidaciones = db.gatbl_AnalisisPreConvalidaciones.Find(detallepreanalisis.lAnalisisPreConvalidacion_id);
            if (detallepreanalisis == null)
            {
                return HttpNotFound();
            }


            string sql = string.Format("Exec sp_PromedioUnidadesAnalizadas {0}", detallepreanalisis.lDetAnalisisPreConvalidacion_id);

            var unidadequivalencia = db.Database.SqlQuery<gatbl_AnalisisConvalidacionesComplemento>(sql).ToList<gatbl_AnalisisConvalidacionesComplemento>();

            ViewModels.PromedioEquivalenciaMateria promedio = new ViewModels.PromedioEquivalenciaMateria();
            promedio.gatbl_DetAnalisisPreConvalidaciones = detallepreanalisis;
            promedio.gatbl_AnalisisConvalidacionesComplemento = unidadequivalencia.FirstOrDefault();

            Session["vAnalisis"] = promedio;

            return View(promedio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Analisis(gatbl_AnalisisConvalidaciones gatbl_AnalisisConvalidaciones)
        {            
            try
            {
                string TemaListOrigen = gatbl_AnalisisConvalidaciones.sEquivalencia_porc;
                string TemaListDestino = gatbl_AnalisisConvalidaciones.sCreated_by;

                decimal promedio = Convert.ToDecimal(Request["dEquivalencia_Promedio"]);

                var analisis = new gatbl_AnalisisConvalidaciones
                {
                    lAnalisisPreConvalidacion_id = gatbl_AnalisisConvalidaciones.lAnalisisPreConvalidacion_id,
                    dtAnalisisConvalidacion_dt = DateTime.Now,
                    sEquivalencia_porc = "0",                    
                    sObs_desc = gatbl_AnalisisConvalidaciones.sObs_desc,
                    dEquivalencia_Promedio = promedio,
                    iEstado_fl = "1",
                    iEliminado_fl = "1",
                    sCreated_by = DateTime.Now.ToString(),
                    iConcurrencia_id = 1,
                    lResponsable_id = "0000001138"
                };


                db.gatbl_AnalisisConvalidaciones.Add(analisis);
                db.SaveChanges();

                var AnalisisID = db.gatbl_AnalisisConvalidaciones.Select(p => p.lAnalisisConvalidacion_id).Max();

                //Origen
                //foreach(string TemaID in TemaListOrigen.Split(','))
                //{
                //    int tID = Convert.ToInt32(TemaID.Split(':')[0]);

                //    decimal nota = 0;
                //    if (TemaID.Split(':').Count() > 1)
                //    {
                //        nota = Convert.ToDecimal(TemaID.Split(':')[1]);
                //    }


                //    var tema = db.gatbl_DProgramasAnaliticosTemas.Find(tID);
                //    var Unidad = db.gatbl_DProgramasAnaliticos.Find(tema.lDProgramaAnalitico_id);
                //    var materia = db.gatbl_ProgramasAnaliticos.Find(Unidad.lProgramaAnalitico_id);

                //    var DAnalisis = new gatbl_DAnalisisConvalidaciones
                //    {
                //        lAnalisisConvalidacion_id = AnalisisID,
                //        sOrigenMateria_fl = "01",
                //        lMateria_id = materia.lProgramaAnalitico_id,
                //        lUnidad_id = Unidad.lDProgramaAnalitico_id,
                //        lTema_id = tema.lDProgramaAnaliticoTema_id,
                //        CalificacionUnidad = nota,

                //        iEstado_fl = "1",
                //        iEliminado_fl = "1",
                //        sCreated_by = DateTime.Now.ToString(),
                //        iConcurrencia_id = 1,
                //    };

                //    db.gatbl_DAnalisisConvalidaciones.Add(DAnalisis);
                //}

                

                var materiaorigendestino = from c in db.gatbl_PConvalidaciones
                                           join pc in db.gatbl_AnalisisPreConvalidaciones on c.lPConvalidacion_id equals pc.lPConvalidacion_id
                                           join ac in db.gatbl_AnalisisConvalidaciones on pc.lAnalisisPreConvalidacion_id equals ac.lAnalisisPreConvalidacion_id
                                           where ac.lAnalisisConvalidacion_id == AnalisisID
                                           select c;

                //Destino
                foreach (string TemaID in TemaListDestino.Split(','))
                {
                    int tID = Convert.ToInt32(TemaID.Split(':')[0]);

                    decimal nota = 0;
                    if (TemaID.Split(':').Count() > 1)
                    {
                        nota = Convert.ToDecimal(TemaID.Split(':')[1]);
                    }


                    var tema = db.gatbl_DProgramasAnaliticosTemas.Find(tID);
                    var Unidad = db.gatbl_DProgramasAnaliticos.Find(tema.lDProgramaAnalitico_id);
                    var materia = db.gatbl_ProgramasAnaliticos.Find(Unidad.lProgramaAnalitico_id);
                    string origen = "02";

                    int ori = materiaorigendestino.Where(m => m.lCarreraOrigen_id == materia.lCarrera_id).Count();

                    if(ori > 0)
                    {
                        origen = "01";
                    }


                    var DAnalisis = new gatbl_DAnalisisConvalidaciones
                    {
                        lAnalisisConvalidacion_id = AnalisisID,
                        sOrigenMateria_fl = origen,
                        lMateria_id = materia.lProgramaAnalitico_id,
                        lUnidad_id = Unidad.lDProgramaAnalitico_id,
                        lTema_id = tema.lDProgramaAnaliticoTema_id,
                        CalificacionUnidad = nota,

                        iEstado_fl = "1",
                        iEliminado_fl = "1",
                        sCreated_by = DateTime.Now.ToString(),
                        iConcurrencia_id = 1,
                    };

                    db.gatbl_DAnalisisConvalidaciones.Add(DAnalisis);
                }

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ViewBag.MateriasOrigen = GetEditDetailProgramData(gatbl_AnalisisConvalidaciones.gatbl_AnalisisPreConvalidaciones.lAnalisisPreConvalidacion_id, "01");
                ViewBag.MateriasDestino = GetEditDetailProgramData(gatbl_AnalisisConvalidaciones.gatbl_AnalisisPreConvalidaciones.lAnalisisPreConvalidacion_id, "02");

                return View(gatbl_AnalisisConvalidaciones);
            }                       
        }

        // GET: gatbl_AnalisisConvalidaciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_AnalisisConvalidaciones gatbl_AnalisisConvalidaciones = db.gatbl_AnalisisConvalidaciones.Find(id);
            if (gatbl_AnalisisConvalidaciones == null)
            {
                return HttpNotFound();
            }

            ViewBag.MateriasOrigen = GetDetailUnitData(id, "01");
            ViewBag.MateriasDestino = GetDetailUnitData(id, "02");

            return View(gatbl_AnalisisConvalidaciones);
        }

        // GET: gatbl_AnalisisConvalidaciones/Create
        public ActionResult Create()
        {
            ViewBag.lCarrera_destino = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm");
            ViewBag.lCarrera_Origen = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm");
            ViewBag.lUniversidad_destino = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc");
            ViewBag.lUniversidad_origen = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc");
            ViewBag.lResponsable_id = new SelectList(db.agendas, "agd_codigo", "NombreCompleto");            
            ViewBag.UniversidadList = db.gatbl_Universidades;

            Session["analisis"] = new gatbl_AnalisisConvalidaciones();

            return View();
        }

        public List<gatbl_ProgramasAnaliticos> ListaMaterias(string UnidadList)
        {
            var ListMateria = new List<gatbl_ProgramasAnaliticos>();
            foreach (var item in UnidadList.Split(','))
            {
                var unidad = db.gatbl_DProgramasAnaliticos.Find(int.Parse(item));

                if (unidad != null)
                {
                    var materia = db.gatbl_ProgramasAnaliticos.Find(unidad.lProgramaAnalitico_id);

                    if (materia != null)
                    {
                        if (!ListMateria.Contains(materia))
                        {
                            ListMateria.Add(materia);
                        }
                    }
                }
            }

            return ListMateria;
        }

        // POST: gatbl_AnalisisConvalidaciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(gatbl_AnalisisConvalidaciones gatbl_AnalisisConvalidaciones)
        {
            if (ModelState.IsValid)
            {
                //string UnidadListOrigen = gatbl_AnalisisConvalidaciones.sGestion_desc;
                //string UnidadListDestino = gatbl_AnalisisConvalidaciones.sCreated_by;

                //gatbl_AnalisisConvalidaciones.sGestion_desc = gatbl_AnalisisConvalidaciones.sPeriodo_desc.Split('-')[0];                               

                gatbl_AnalisisConvalidaciones.iEstado_fl = "1";
                gatbl_AnalisisConvalidaciones.iEliminado_fl = "1";
                gatbl_AnalisisConvalidaciones.sCreated_by = DateTime.Now.ToString();
                gatbl_AnalisisConvalidaciones.iConcurrencia_id = 1;

                
                db.gatbl_AnalisisConvalidaciones.Add(gatbl_AnalisisConvalidaciones);
                db.SaveChanges();

                var AnalisisID = db.gatbl_AnalisisConvalidaciones.Select(p => p.lAnalisisConvalidacion_id).Max();
                //Materias y Unidades Origen
                //    var MateriasOrigen = ListaMaterias(UnidadListOrigen);
                //    foreach (var itemMateria in MateriasOrigen)
                //    {
                //        var itMateria = new gatbl_DAnalisisConvalidaciones
                //        {
                //            lAnalisisConvalidacion_id = AnalisisID,
                //            sOrigenMateria_fl = "01",
                //            lMateria_id = itemMateria.lProgramaAnalitico_id,
                //            iEstado_fl = "1",
                //            iEliminado_fl = "1",
                //            sCreated_by = DateTime.Now.ToString(),
                //            iConcurrencia_id = 1
                //        };

                //        db.gatbl_DAnalisisConvalidaciones.Add(itMateria);
                //        db.SaveChanges();

                //        var MateriaId = db.gatbl_DAnalisisConvalidaciones.Select(p => p.lDAnalisisConvalidacion_id).Max();
                //        foreach (var itemUnidad in itemMateria.gatbl_DProgramasAnaliticos)
                //        {
                //            if(UnidadListOrigen.Split(',').Contains(itemUnidad.lDProgramaAnalitico_id.ToString()))
                //            {
                //                var itUnidad = new gatbl_UnidadesConvalidadas
                //                {
                //                    lDAnalisisConvalidacion_id = MateriaId,
                //                    lUnidad_nro = itemUnidad.lDProgramaAnalitico_id,
                //                    iEstado_fl = "1",
                //                    iEliminado_fl = "1",
                //                    sCreated_by = DateTime.Now.ToString(),
                //                    iConcurrencia_id = 1
                //                };

                //                db.gatbl_UnidadesConvalidadas.Add(itUnidad);
                //            }                        
                //        }
                //        db.SaveChanges();
                //    }

                //    //Materias y Unidades Destino
                //    var MateriasDestino = ListaMaterias(UnidadListDestino);
                //    foreach (var itemMateria in MateriasDestino)
                //    {
                //        var itMateria = new gatbl_DAnalisisConvalidaciones
                //        {
                //            lAnalisisConvalidacion_id = AnalisisID,
                //            sOrigenMateria_fl = "02",
                //            lMateria_id = itemMateria.lProgramaAnalitico_id,
                //            iEstado_fl = "1",
                //            iEliminado_fl = "1",
                //            sCreated_by = DateTime.Now.ToString(),
                //            iConcurrencia_id = 1
                //        };

                //        db.gatbl_DAnalisisConvalidaciones.Add(itMateria);
                //        db.SaveChanges();

                //        var MateriaId = db.gatbl_DAnalisisConvalidaciones.Select(p => p.lDAnalisisConvalidacion_id).Max();
                //        foreach (var itemUnidad in itemMateria.gatbl_DProgramasAnaliticos)
                //        {
                //            if (UnidadListDestino.Split(',').Contains(itemUnidad.lDProgramaAnalitico_id.ToString()))
                //            {
                //                var itUnidad = new gatbl_UnidadesConvalidadas
                //                {
                //                    lDAnalisisConvalidacion_id = MateriaId,
                //                    lUnidad_nro = itemUnidad.lDProgramaAnalitico_id,
                //                    iEstado_fl = "1",
                //                    iEliminado_fl = "1",
                //                    sCreated_by = DateTime.Now.ToString(),
                //                    iConcurrencia_id = 1
                //                };

                //                db.gatbl_UnidadesConvalidadas.Add(itUnidad);
                //            }
                //        }
                //        db.SaveChanges();
                //    }


                return RedirectToAction("Index");
            }

            //ViewBag.lCarrera_destino = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm", gatbl_AnalisisConvalidaciones.lCarrera_destino);
            //ViewBag.lCarrera_Origen = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm", gatbl_AnalisisConvalidaciones.lCarrera_Origen);
            //ViewBag.lUniversidad_destino = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", gatbl_AnalisisConvalidaciones.lUniversidad_destino);
            //ViewBag.lUniversidad_origen = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", gatbl_AnalisisConvalidaciones.lUniversidad_origen);
            //ViewBag.lResponsable_id = new SelectList(db.agendas, "agd_codigo", "NombreCompleto", gatbl_AnalisisConvalidaciones.lResponsable_id);
            //ViewBag.UniversidadList = db.gatbl_Universidades;

            return View(gatbl_AnalisisConvalidaciones);
        }

        // GET: gatbl_AnalisisConvalidaciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_AnalisisConvalidaciones gatbl_AnalisisConvalidaciones = db.gatbl_AnalisisConvalidaciones.Find(id);
            if (gatbl_AnalisisConvalidaciones == null)
            {
                return HttpNotFound();
            }
            
            //ViewBag.lCarrera_destino = new SelectList(db.gatbl_Carreras.Where(c => c.lCarrera_id == gatbl_AnalisisConvalidaciones.lCarrera_destino), "lCarrera_id", "sCarrera_nm", gatbl_AnalisisConvalidaciones.lCarrera_destino);
            //ViewBag.lCarrera_Origen = new SelectList(db.gatbl_Carreras.Where(c => c.lCarrera_id == gatbl_AnalisisConvalidaciones.lCarrera_Origen), "lCarrera_id", "sCarrera_nm", gatbl_AnalisisConvalidaciones.lCarrera_Origen);
            //ViewBag.lUniversidad_destino = new SelectList(db.gatbl_Universidades.Where(u => u.lUniversidad_id == gatbl_AnalisisConvalidaciones.lUniversidad_destino), "lUniversidad_id", "sNombre_desc", gatbl_AnalisisConvalidaciones.lUniversidad_destino);
            //ViewBag.lUniversidad_origen = new SelectList(db.gatbl_Universidades.Where(u => u.lUniversidad_id == gatbl_AnalisisConvalidaciones.lUniversidad_origen), "lUniversidad_id", "sNombre_desc", gatbl_AnalisisConvalidaciones.lUniversidad_origen);
            //ViewBag.lResponsable_id = new SelectList(db.agendas, "agd_codigo", "NombreCompleto", gatbl_AnalisisConvalidaciones.lResponsable_id);

            //ViewBag.MateriasOrigen = GetEditDetailUnitData(id, gatbl_AnalisisConvalidaciones.lCarrera_Origen);
            //ViewBag.MateriasDestino = GetEditDetailUnitData(id, gatbl_AnalisisConvalidaciones.lCarrera_destino);

            ViewBag.UniversidadList = db.gatbl_Universidades;

            Session["analisis"] = gatbl_AnalisisConvalidaciones;

            return View(gatbl_AnalisisConvalidaciones);
        }

        // POST: gatbl_AnalisisConvalidaciones/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "lAnalisisConvalidacion_id,lUniversidad_origen,lUniversidad_destino,lCarrera_Origen,lCarrera_destino,sGestion_desc,sPeriodo_desc,sPensumDestino_desc,lResponsable_id,sVersion_nro,dtAnalisisConvalidacion_dt,sEquivalencia_porc,sObs_desc,iEstado_fl,iEliminado_fl,sCreated_by,iConcurrencia_id")] gatbl_AnalisisConvalidaciones gatbl_AnalisisConvalidaciones)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gatbl_AnalisisConvalidaciones).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.lCarrera_destino = new SelectList(db.gatbl_Carreras.Where(c => c.lCarrera_id == gatbl_AnalisisConvalidaciones.lCarrera_destino), "lCarrera_id", "sCarrera_nm", gatbl_AnalisisConvalidaciones.lCarrera_destino);
            //ViewBag.lCarrera_Origen = new SelectList(db.gatbl_Carreras.Where(c => c.lCarrera_id == gatbl_AnalisisConvalidaciones.lCarrera_Origen), "lCarrera_id", "sCarrera_nm", gatbl_AnalisisConvalidaciones.lCarrera_Origen);

            //ViewBag.lUniversidad_destino = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", gatbl_AnalisisConvalidaciones.lUniversidad_destino);
            //ViewBag.lUniversidad_origen = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", gatbl_AnalisisConvalidaciones.lUniversidad_origen);
            //ViewBag.lResponsable_id = new SelectList(db.agendas, "agd_codigo", "NombreCompleto", gatbl_AnalisisConvalidaciones.lResponsable_id);
            //ViewBag.UniversidadList = db.gatbl_Universidades;

            //ViewBag.MateriasOrigen = GetEditDetailUnitData(gatbl_AnalisisConvalidaciones.lAnalisisConvalidacion_id, gatbl_AnalisisConvalidaciones.lCarrera_Origen);
            //ViewBag.MateriasDestino = GetEditDetailUnitData(gatbl_AnalisisConvalidaciones.lAnalisisConvalidacion_id, gatbl_AnalisisConvalidaciones.lCarrera_destino);

            return View(gatbl_AnalisisConvalidaciones);
        }

        // GET: gatbl_AnalisisConvalidaciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_AnalisisConvalidaciones gatbl_AnalisisConvalidaciones = db.gatbl_AnalisisConvalidaciones.Find(id);
            if (gatbl_AnalisisConvalidaciones == null)
            {
                return HttpNotFound();
            }
            ViewBag.MateriasOrigen = GetDetailUnitData(id, "01");
            ViewBag.MateriasDestino = GetDetailUnitData(id, "02");

            return View(gatbl_AnalisisConvalidaciones);
        }

        // POST: gatbl_AnalisisConvalidaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            gatbl_AnalisisConvalidaciones gatbl_AnalisisConvalidaciones = db.gatbl_AnalisisConvalidaciones.Find(id);

            ViewBag.MateriasOrigen = GetDetailUnitData(id, "01");
            ViewBag.MateriasDestino = GetDetailUnitData(id, "02");

            var unidades = from u in db.gatbl_UnidadesConvalidadas
                           join d in db.gatbl_DAnalisisConvalidaciones on u.lDAnalisisConvalidacion_id equals
                           d.lDAnalisisConvalidacion_id
                           where d.lAnalisisConvalidacion_id == id
                           select u;
            foreach (var uni in unidades)
            {
                db.gatbl_UnidadesConvalidadas.Remove(uni);
            }


            var detalle = from d in db.gatbl_DAnalisisConvalidaciones
                          where d.lAnalisisConvalidacion_id == id
                          select d;

            foreach (var item in detalle)
            {

                db.gatbl_DAnalisisConvalidaciones.Remove(item);
            }

            db.gatbl_AnalisisConvalidaciones.Remove(gatbl_AnalisisConvalidaciones);
            db.SaveChanges();


            return RedirectToAction("Index");
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
