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

namespace SIGAA.Areas.CONV.Controllers
{
    public class gatbl_AnalisisPreConvalidacionesController : Controller
    {
        private ConvalidacionesContext db = new ConvalidacionesContext();

        // GET: gatbl_AnalisisPreConvalidaciones
        public ActionResult Index()
        {
            var gatbl_AnalisisPreConvalidaciones = db.gatbl_AnalisisPreConvalidaciones.Include(g => g.gatbl_PConvalidaciones).Include(g => g.Responsables);
            return View(gatbl_AnalisisPreConvalidaciones.ToList());
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
            var pos = from po in db.gatbl_AnalisisPreConvalidaciones.Include(g => g.gatbl_PConvalidaciones).Include(g => g.Responsables)
                      join post in db.gatbl_Postulantes on po.gatbl_PConvalidaciones.lPostulante_id equals post.lPostulante_id
                      join uo in db.gatbl_Universidades on po.gatbl_PConvalidaciones.lUniversidadOrigen_id equals uo.lUniversidad_id
                      join ud in db.gatbl_Universidades on po.gatbl_PConvalidaciones.lUniversidadDestino_id equals ud.lUniversidad_id
                      join co in db.gatbl_Carreras on po.gatbl_PConvalidaciones.lCarreraOrigen_id equals co.lCarrera_id
                      join cd in db.gatbl_Carreras on po.gatbl_PConvalidaciones.lCarreraDestino_id equals cd.lCarrera_id
                      join pe in db.Pensums on po.gatbl_PConvalidaciones.lPensum_id equals pe.lPensum_id
                      join pa in db.gatbl_AnalisisConvalidaciones on po.lAnalisisPreConvalidacion_id equals pa.lAnalisisPreConvalidacion_id
                      orderby po.gatbl_PConvalidaciones.lNro_solucitud descending

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

        
        public ActionResult Index_Read_Pendiente([DataSourceRequest] DataSourceRequest request)
        {
            var pos = from po in db.gatbl_AnalisisPreConvalidaciones.Include(g => g.gatbl_PConvalidaciones).Include(g => g.Responsables)
                      join post in db.gatbl_Postulantes on po.gatbl_PConvalidaciones.lPostulante_id equals post.lPostulante_id
                      join uo in db.gatbl_Universidades on po.gatbl_PConvalidaciones.lUniversidadOrigen_id equals uo.lUniversidad_id
                      join ud in db.gatbl_Universidades on po.gatbl_PConvalidaciones.lUniversidadDestino_id equals ud.lUniversidad_id
                      join co in db.gatbl_Carreras on po.gatbl_PConvalidaciones.lCarreraOrigen_id equals co.lCarrera_id
                      join cd in db.gatbl_Carreras on po.gatbl_PConvalidaciones.lCarreraDestino_id equals cd.lCarrera_id
                      join pe in db.Pensums on po.gatbl_PConvalidaciones.lPensum_id equals pe.lPensum_id
                      where !db.gatbl_AnalisisConvalidaciones.Any(m => m.lAnalisisPreConvalidacion_id == po.lAnalisisPreConvalidacion_id)
                      && po.iEstado_fl == "2"

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
                      join post in db.gatbl_Postulantes on po.gatbl_PConvalidaciones.lPostulante_id equals post.lPostulante_id
                      join uo in db.gatbl_Universidades on po.gatbl_PConvalidaciones.lUniversidadOrigen_id equals uo.lUniversidad_id
                      join ud in db.gatbl_Universidades on po.gatbl_PConvalidaciones.lUniversidadDestino_id equals ud.lUniversidad_id
                      join co in db.gatbl_Carreras on po.gatbl_PConvalidaciones.lCarreraOrigen_id equals co.lCarrera_id
                      join cd in db.gatbl_Carreras on po.gatbl_PConvalidaciones.lCarreraDestino_id equals cd.lCarrera_id
                      join pe in db.Pensums on po.gatbl_PConvalidaciones.lPensum_id equals pe.lPensum_id
                      where !db.gatbl_AnalisisConvalidaciones.Any(m => m.lAnalisisPreConvalidacion_id == po.lAnalisisPreConvalidacion_id)
                      && po.iEstado_fl != "2"

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
        public JsonResult CarreraList(int id)
        {
            var carreras = from s in db.gatbl_Carreras
                           where s.lUniversidad_id == id
                           select s;

            return Json(new SelectList(carreras.ToArray(), "lCarrera_id", "sCarrera_nm"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Materias(int? id)
        {            
            var materias = from s in db.gatbl_PConvalidaciones
                           join dm in db.gatbl_CertificadosMateria on s.lPConvalidacion_id equals dm.lPConvalidacion_id
                           join m in db.gatbl_ProgramasAnaliticos on dm.lMateria_id equals m.lProgramaAnalitico_id
                           where (id.HasValue ? s.lPConvalidacion_id == id : s.lPConvalidacion_id == 0)
                           && !db.gatbl_DAnalisisPreConvalidaciones.Any(t => t.gatbl_AnalisisPreConvalidaciones.lPConvalidacion_id == id
                           && t.lMateria_id == m.lProgramaAnalitico_id && t.sOrigenMateria_fl == "01")
                           select new
                           {
                               id = m.lProgramaAnalitico_id,
                               Name = m.sMateria_desc
                           };


            return Json(materias, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MateriasDestino(int? id)
        {            
            var materias = from m in db.gatbl_ProgramasAnaliticos
                           join pc in db.gatbl_PConvalidaciones on m.lCarrera_id equals pc.lCarreraDestino_id
                           where pc.lPConvalidacion_id == id && m.lPensum_id == pc.lPensum_id
                           && !db.gatbl_DAnalisisPreConvalidaciones.Any(t => t.gatbl_AnalisisPreConvalidaciones.lPConvalidacion_id == id
                           && t.lMateria_id == m.lProgramaAnalitico_id && t.sOrigenMateria_fl == "02")
                           select new
                           {
                               id = m.lProgramaAnalitico_id,
                               Name = m.sMateria_desc
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

        private IEnumerable<TreeViewItemModel> GetEditDetailProgramData(int? id, int lPConvalidacion)
        {
            List<TreeViewItemModel> inlineDefault = new List<TreeViewItemModel>();

            var materias = from c in db.gatbl_CertificadosMateria
                           join p in db.gatbl_ProgramasAnaliticos on c.lMateria_id equals p.lProgramaAnalitico_id
                           where c.lPConvalidacion_id == lPConvalidacion
                           select p;                           
            
            foreach (var item in materias.ToList())
            {
                var MateriasChecked = from m in db.gatbl_DAnalisisPreConvalidaciones
                                      join d in db.gatbl_ProgramasAnaliticos on m.lMateria_id equals d.lProgramaAnalitico_id
                                      where m.lAnalisisPreConvalidacion_id == id
                                      select d;

                TreeViewItemModel itmateria = new TreeViewItemModel();
                itmateria.Text = item.sMateria_desc;
                itmateria.Id = item.lProgramaAnalitico_id.ToString();
                itmateria.Checked = MateriasChecked.ToList().FindIndex(m => m.lProgramaAnalitico_id == item.lProgramaAnalitico_id) != -1;                

                inlineDefault.Add(itmateria);
            }

            return inlineDefault;
        }

        private IEnumerable<TreeViewItemModel> GetEditDetailProgramDataDestino(int? id, int carreraID)
        {
            List<TreeViewItemModel> inlineDefault = new List<TreeViewItemModel>();

            var materias = db.gatbl_ProgramasAnaliticos.Where(m => m.lCarrera_id == carreraID).ToList();

            foreach (var item in materias)
            {
                var MateriasChecked = from m in db.gatbl_DAnalisisPreConvalidaciones
                                      join d in db.gatbl_ProgramasAnaliticos on m.lMateria_id equals d.lProgramaAnalitico_id
                                      where m.lAnalisisPreConvalidacion_id == id
                                      select d;

                TreeViewItemModel itmateria = new TreeViewItemModel();
                itmateria.Text = item.sMateria_desc;
                itmateria.Id = item.lProgramaAnalitico_id.ToString();
                itmateria.Checked = MateriasChecked.ToList().FindIndex(m => m.lProgramaAnalitico_id == item.lProgramaAnalitico_id) != -1;

                inlineDefault.Add(itmateria);
            }

            return inlineDefault;
        }

        private IEnumerable<TreeViewItemModel> GetDetailProgramData(int? id, string source)
        {
            List<TreeViewItemModel> inlineDefault = new List<TreeViewItemModel>();

            var materias = from m in db.gatbl_ProgramasAnaliticos
                           join dm in db.gatbl_DAnalisisPreConvalidaciones on m.lProgramaAnalitico_id equals dm.lMateria_id
                           where dm.lAnalisisPreConvalidacion_id == id && dm.sOrigenMateria_fl.Equals(source)
                           select new
                           {
                               lProgramaAnalitico_id = m.lProgramaAnalitico_id,
                               sMateria_desc = m.sMateria_desc,
                               lDAnalisisConvalidacion_id = dm.lDAnalisisConvalidacion_id
                           };

            foreach (var item in materias.ToList())
            {
                TreeViewItemModel itmateria = new TreeViewItemModel();
                itmateria.Text = item.sMateria_desc;
                itmateria.Id = item.lProgramaAnalitico_id.ToString();
                itmateria.ImageUrl = "~/Content/Web/img/active_rooms_32.png";                

                inlineDefault.Add(itmateria);
            }

            return inlineDefault;
        }

        public JsonResult ObtenerSolicitudes(string text)
        {
            return Json((from s in db.gatbl_PConvalidaciones
                         select new
                         {
                             lPConvalidacion_id = s.lPConvalidacion_id,
                             lNro_solucitud = s.lNro_solucitud,
                             lCarreraOrigen_id = s.lCarreraOrigen_id,
                             lCarreraDestino_id = s.lCarreraDestino_id,
                             gatbl_Postulantes_sNombre_desc = "(" + s.lNro_solucitud + ") " + s.gatbl_Postulantes.sApPaterno_desc + "" + s.gatbl_Postulantes.sApMaterno_desc + ", " + s.gatbl_Postulantes.sNombre_desc,
                             gatbl_UniversidadesOrigen_sNombre_desc = s.gatbl_UniversidadesOrigen.sNombre_desc,
                             gatbl_CarrerasOrigen_sCarrera_nm = s.gatbl_CarrerasOrigen.sCarrera_nm,
                             gatbl_UniversidadesDestino_sNombre_desc = s.gatbl_UniversidadesDestino.sNombre_desc,
                             gatbl_CarrerasDestino_sCarrera_nm = s.gatbl_CarrerasDestino.sCarrera_nm                             
                         }).Where(g => g.gatbl_Postulantes_sNombre_desc.Contains(text)).OrderBy(g => g.gatbl_Postulantes_sNombre_desc), JsonRequestBehavior.AllowGet);
        }

        // GET: gatbl_AnalisisPreConvalidaciones/Details/5
        public ActionResult Details(int? id)
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

            ViewBag.MateriasOrigen = GetDetailProgramData(id, "01");
            ViewBag.MateriasDestino = GetDetailProgramData(id, "02");

            return View(gatbl_AnalisisPreConvalidaciones);
        }

        // GET: gatbl_AnalisisPreConvalidaciones/Details/5
        public ActionResult Details_View(int? id)
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

            ViewBag.MateriasOrigen = GetDetailProgramData(id, "01");
            ViewBag.MateriasDestino = GetDetailProgramData(id, "02");

            return View(gatbl_AnalisisPreConvalidaciones);
        }

        // GET: gatbl_AnalisisPreConvalidaciones/Create
        public ActionResult Create(int id)
        {
            ViewModels.PreAnalisis preanalisis = new ViewModels.PreAnalisis();

            preanalisis.gatbl_AnalisisPreConvalidaciones = new gatbl_AnalisisPreConvalidaciones();
            preanalisis.gatbl_DetAnalisisPreConvalidaciones = new List<gatbl_DetAnalisisPreConvalidaciones>();
            preanalisis.gatbl_DetAnalisisPreConvalidacionesEliminados = new List<gatbl_DetAnalisisPreConvalidaciones>();

            Session["vPreAnalisis"] = preanalisis;
            

            ViewBag.lCarrera_destino = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm");
            ViewBag.lCarrera_Origen = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm");
            ViewBag.lUniversidad_destino = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc");
            ViewBag.lUniversidad_origen = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc");
            ViewBag.lResponsable_id = new SelectList(db.agendas, "agd_codigo", "tagd_codigo");
            ViewBag.UniversidadList = db.gatbl_Universidades.OrderBy(u=> u.sNombre_desc);



            return View(new gatbl_AnalisisPreConvalidaciones());
        }

        // POST: gatbl_AnalisisPreConvalidaciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(gatbl_AnalisisPreConvalidaciones gatbl_AnalisisPreConvalidaciones)
        {            
            try
            {
                string MateriaListOrigen = gatbl_AnalisisPreConvalidaciones.iEstado_fl;
                string MateriaListDestino = gatbl_AnalisisPreConvalidaciones.sCreated_by;

                gatbl_PConvalidaciones solicitud = db.gatbl_PConvalidaciones.FirstOrDefault(s => s.lNro_solucitud == gatbl_AnalisisPreConvalidaciones.gatbl_PConvalidaciones.lNro_solucitud);
                gatbl_AnalisisPreConvalidaciones.lPConvalidacion_id = solicitud.lPConvalidacion_id;

                gatbl_AnalisisPreConvalidaciones.iEstado_fl = "1";
                gatbl_AnalisisPreConvalidaciones.iEliminado_fl = "1";
                gatbl_AnalisisPreConvalidaciones.sCreated_by = DateTime.Now.ToString();
                gatbl_AnalisisPreConvalidaciones.iConcurrencia_id = 1;
                gatbl_AnalisisPreConvalidaciones.lResponsable_id = "0000001138";
                gatbl_AnalisisPreConvalidaciones.dtAnalisisConvalidacion_dt = DateTime.Now;

                var preanalisis = new gatbl_AnalisisPreConvalidaciones
                {
                    lPConvalidacion_id = solicitud.lPConvalidacion_id,
                    lResponsable_id = gatbl_AnalisisPreConvalidaciones.lResponsable_id,
                    sVersion_nro = gatbl_AnalisisPreConvalidaciones.sVersion_nro,
                    dtAnalisisConvalidacion_dt = gatbl_AnalisisPreConvalidaciones.dtAnalisisConvalidacion_dt,
                    sObs_desc = gatbl_AnalisisPreConvalidaciones.sObs_desc,
                    iEstado_fl = gatbl_AnalisisPreConvalidaciones.iEstado_fl,
                    iEliminado_fl = gatbl_AnalisisPreConvalidaciones.iEliminado_fl,
                    sCreated_by = gatbl_AnalisisPreConvalidaciones.sCreated_by,
                    iConcurrencia_id = gatbl_AnalisisPreConvalidaciones.iConcurrencia_id
                };

                
                db.gatbl_AnalisisPreConvalidaciones.Add(preanalisis);
                db.SaveChanges();

                var AnalisisID = db.gatbl_AnalisisPreConvalidaciones.Select(p => p.lAnalisisPreConvalidacion_id).Max();
                //Materias Origen                
                foreach (var itemMateria in MateriaListOrigen.Split(','))
                {
                    var materiaOrigen = new gatbl_DAnalisisPreConvalidaciones
                    {
                        lMateria_id = int.Parse(itemMateria),
                        lAnalisisPreConvalidacion_id = AnalisisID,
                        sOrigenMateria_fl = "01",
                        iEstado_fl = "1",
                        iEliminado_fl = "1",
                        sCreated_by = DateTime.Now.ToString(),
                        iConcurrencia_id = 1
                    };

                    db.gatbl_DAnalisisPreConvalidaciones.Add(materiaOrigen);
                }

                db.SaveChanges();

                //Materias Destino  
                foreach (var itemMateria in MateriaListDestino.Split(','))
                {
                    var materiaDestino = new gatbl_DAnalisisPreConvalidaciones
                    {
                        lMateria_id = int.Parse(itemMateria),
                        lAnalisisPreConvalidacion_id = AnalisisID,
                        sOrigenMateria_fl = "02",
                        iEstado_fl = "1",
                        iEliminado_fl = "1",
                        sCreated_by = DateTime.Now.ToString(),
                        iConcurrencia_id = 1
                    };

                    db.gatbl_DAnalisisPreConvalidaciones.Add(materiaDestino);
                }

                db.SaveChanges();


                return RedirectToAction("Index_Pendiente");                
            }
            catch (Exception ex)
            {
                //ViewBag.lCarrera_destino = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm", gatbl_AnalisisPreConvalidaciones.lCarrera_destino);
                //ViewBag.lCarrera_Origen = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm", gatbl_AnalisisPreConvalidaciones.lCarrera_Origen);
                //ViewBag.lUniversidad_destino = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", gatbl_AnalisisPreConvalidaciones.lUniversidad_destino);
                //ViewBag.lUniversidad_origen = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", gatbl_AnalisisPreConvalidaciones.lUniversidad_origen);
                ViewBag.lResponsable_id = new SelectList(db.agendas, "agd_codigo", "tagd_codigo", gatbl_AnalisisPreConvalidaciones.lResponsable_id);
                ViewBag.UniversidadList = db.gatbl_Universidades.OrderBy(u => u.sNombre_desc);

                return View(gatbl_AnalisisPreConvalidaciones);
            }                       
        }

        // GET: gatbl_AnalisisPreConvalidaciones/Edit/5
        public ActionResult Edit(int? id)
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



            ViewModels.PreAnalisis preanalisis = new ViewModels.PreAnalisis();

            preanalisis.gatbl_AnalisisPreConvalidaciones = db.gatbl_AnalisisPreConvalidaciones.Find(id);
            preanalisis.gatbl_DetAnalisisPreConvalidaciones = db.gatbl_DetAnalisisPreConvalidaciones.Where(p=>p.lAnalisisPreConvalidacion_id == id).ToList();
            preanalisis.gatbl_DetAnalisisPreConvalidacionesEliminados = new List<gatbl_DetAnalisisPreConvalidaciones>();

            Session["vPreAnalisis"] = preanalisis;


            //ViewBag.lCarrera_destino = new SelectList(db.gatbl_Carreras.Where(c => c.lCarrera_id == gatbl_AnalisisPreConvalidaciones.lCarrera_destino), "lCarrera_id", "sCarrera_nm", gatbl_AnalisisPreConvalidaciones.lCarrera_destino);
            //ViewBag.lCarrera_Origen = new SelectList(db.gatbl_Carreras.Where(c => c.lCarrera_id == gatbl_AnalisisPreConvalidaciones.lCarrera_Origen), "lCarrera_id", "sCarrera_nm", gatbl_AnalisisPreConvalidaciones.lCarrera_Origen);
            //ViewBag.lUniversidad_destino = new SelectList(db.gatbl_Universidades.Where(u => u.lUniversidad_id == gatbl_AnalisisPreConvalidaciones.lUniversidad_destino), "lUniversidad_id", "sNombre_desc", gatbl_AnalisisPreConvalidaciones.lUniversidad_destino);
            //ViewBag.lUniversidad_origen = new SelectList(db.gatbl_Universidades.Where(u => u.lUniversidad_id == gatbl_AnalisisPreConvalidaciones.lUniversidad_origen), "lUniversidad_id", "sNombre_desc", gatbl_AnalisisPreConvalidaciones.lUniversidad_origen);
            ViewBag.lResponsable_id = new SelectList(db.agendas, "agd_codigo", "NombreCompleto", gatbl_AnalisisPreConvalidaciones.lResponsable_id);

            ViewBag.MateriasOrigen = GetEditDetailProgramData(id, gatbl_AnalisisPreConvalidaciones.gatbl_PConvalidaciones.lPConvalidacion_id);
            ViewBag.MateriasDestino = GetEditDetailProgramDataDestino(id, gatbl_AnalisisPreConvalidaciones.gatbl_PConvalidaciones.lCarreraDestino_id);

            ViewBag.UniversidadList = db.gatbl_Universidades;

            return View(gatbl_AnalisisPreConvalidaciones);
        }

        // POST: gatbl_AnalisisPreConvalidaciones/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(gatbl_AnalisisPreConvalidaciones gatbl_AnalisisPreConvalidaciones)
        {

            try
            {
                var preanalisis = db.gatbl_AnalisisPreConvalidaciones.Find(gatbl_AnalisisPreConvalidaciones.lAnalisisPreConvalidacion_id);

                preanalisis.sVersion_nro = gatbl_AnalisisPreConvalidaciones.sVersion_nro;
                preanalisis.sObs_desc = gatbl_AnalisisPreConvalidaciones.sObs_desc;
                preanalisis.iEstado_fl = "1";


                db.Entry(preanalisis).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index_Pendiente");
            }
            catch(Exception ex)
            {
                ViewBag.lResponsable_id = new SelectList(db.agendas, "agd_codigo", "tagd_codigo", gatbl_AnalisisPreConvalidaciones.lResponsable_id);
                return View(gatbl_AnalisisPreConvalidaciones);
            }



            //if (ModelState.IsValid)
            //{
            //    db.Entry(gatbl_AnalisisPreConvalidaciones).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index_Pendiente");
            //}



            //ViewBag.lCarrera_destino = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm", gatbl_AnalisisPreConvalidaciones.lCarrera_destino);
            //ViewBag.lCarrera_Origen = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm", gatbl_AnalisisPreConvalidaciones.lCarrera_Origen);
            //ViewBag.lUniversidad_destino = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", gatbl_AnalisisPreConvalidaciones.lUniversidad_destino);
            //ViewBag.lUniversidad_origen = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", gatbl_AnalisisPreConvalidaciones.lUniversidad_origen);           
        }

        // GET: gatbl_AnalisisPreConvalidaciones/Delete/5
        public ActionResult Delete(int? id)
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

            ViewBag.MateriasOrigen = GetDetailProgramData(id, "01");
            ViewBag.MateriasDestino = GetDetailProgramData(id, "02");

            return View(gatbl_AnalisisPreConvalidaciones);
        }

        // POST: gatbl_AnalisisPreConvalidaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            gatbl_AnalisisPreConvalidaciones gatbl_AnalisisPreConvalidaciones = db.gatbl_AnalisisPreConvalidaciones.Find(id);
            
            var detalle = from d in db.gatbl_DAnalisisPreConvalidaciones
                          where d.lAnalisisPreConvalidacion_id == id
                          select d;

            foreach (var item in detalle)
            {

                db.gatbl_DAnalisisPreConvalidaciones.Remove(item);
            }

            db.gatbl_AnalisisPreConvalidaciones.Remove(gatbl_AnalisisPreConvalidaciones);
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
