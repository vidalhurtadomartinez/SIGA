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


namespace SIGAA.Areas.CONV.Controllers
{
    public class SolicitudConvalidacionesController : Controller
    {
        private ConvalidacionesContext db = new ConvalidacionesContext();
        // GET: SolicitudConvalidaciones
        public ActionResult Index()
        {
            ViewBag.lTipoDocumentoSolicitud_id = new SelectList(db.TipoDocumentoSolicitudes, "lTipoDocumentoSolicitud_id", "sDescripcion");
            ViewBag.sTipoPresentacion_fl = new SelectList(db.TipoPresentacionDocumentos, "sTipoPresentacion_fl", "sDescripcion");

            ViewData["lTipoDocumentoSolicitud_id"] = db.TipoDocumentoSolicitudes;

            var convalidaciones = from c in db.gatbl_PConvalidaciones.Include(u => u.gatbl_Postulantes).Include(u => u.gatbl_UniversidadesOrigen).Include(u => u.gatbl_UniversidadesDestino).Include(u => u.gatbl_CarrerasOrigen).Include(u => u.gatbl_CarrerasDestino).Include(u => u.Responsables)
                                  select c;


            return View(convalidaciones.ToList());

        }

        public ActionResult Index_Read([DataSourceRequest] DataSourceRequest request)
        {
            var pos = from po in db.gatbl_PConvalidaciones.Include(u => u.gatbl_Postulantes).Include(u => u.gatbl_UniversidadesOrigen).Include(u => u.gatbl_UniversidadesDestino).Include(u => u.gatbl_CarrerasOrigen).Include(u => u.gatbl_CarrerasDestino).Include(u => u.Pensum).Include(u => u.Responsables)
                      select po;
            return Json(pos.ToDataSourceResult(request, p => new
            {
                p.lPConvalidacion_id,
                p.lNro_solucitud,
                p.lPostulante_id,
                p.lUniversidadOrigen_id,
                p.lCarreraOrigen_id,
                p.lUniversidadDestino_id,
                p.lCarreraDestino_id,
                p.dtPostulacion_dt,
                p.lResponsable_id,
                p.sObs_desc,                
                gatbl_Postulantes = new
                {
                    p.lPostulante_id,
                    p.gatbl_Postulantes.NombreCompleto,
                    p.gatbl_Postulantes.sNombre_desc,
                    p.gatbl_Postulantes.sDocumento_nro,
                    p.gatbl_Postulantes.sDireccion_desc
                },
                gatbl_UniversidadesOrigen = new
                {
                    p.lUniversidadOrigen_id,
                    p.gatbl_UniversidadesOrigen.sNombre_desc,
                    p.gatbl_UniversidadesOrigen.sDireccion_desc
                },
                gatbl_UniversidadesDestino = new
                {
                    p.lUniversidadDestino_id,
                    p.gatbl_UniversidadesDestino.sNombre_desc,
                    p.gatbl_UniversidadesDestino.sDireccion_desc
                },
                gatbl_CarrerasOrigen = new
                {
                    p.lCarreraOrigen_id,
                    p.gatbl_CarrerasOrigen.sCarrera_nm,
                    p.gatbl_CarrerasOrigen.sTelefono_desc
                },
                gatbl_CarrerasDestino = new
                {
                    p.lCarreraDestino_id,
                    p.gatbl_CarrerasDestino.sCarrera_nm,
                    p.gatbl_CarrerasDestino.sTelefono_desc
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

        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        {
            SolicitudConvalidacion solicitud = Session["solicitudConvalidacion"] as SolicitudConvalidacion;
            var det = solicitud.gatbl_DPConvalidaciones;

            //var det = from po in db.gatbl_DPConvalidaciones.Include(u => u.TipoDocumentoSolicitud).Include(u => u.TipoPresentacionDocumento)
            //          select po;

            return Json(det.ToDataSourceResult(request, p => new
            {
                p.lDPConvalicacion_id,
                p.lPConvalidacion_id,
                p.lTipoDocumentoSolicitud_id,
                p.sTipoPresentacion_fl,
                p.lCantidad_nro,
                p.iEstado_fl,
                p.iEliminado_fl,
                p.iConcurrencia_id,
                p.sCreated_by
                //,                
                //TipoDocumentoSolicitud = new
                //{
                //    p.lTipoDocumentoSolicitud_id,
                //    p.TipoDocumentoSolicitud.sDescripcion
                //},
                //TipoPresentacionDocumento = new
                //{
                //    p.sTipoPresentacion_fl,
                //    p.TipoPresentacionDocumento.sDescripcion
                //}
            }));
        }

        public ActionResult MEditing_Read([DataSourceRequest] DataSourceRequest request)
        {
            SolicitudConvalidacion solicitud = Session["solicitudConvalidacion"] as SolicitudConvalidacion;
            var det = solicitud.gatbl_CertificadosMaterias;
            
            return Json(det.ToDataSourceResult(request, p => new
            {
                p.lDCertificado_id,
                p.lPConvalidacion_id,
                p.lMateria_id,
                p.sTipoPresentacion_fl,
                p.dCalificacion,
                p.bHomologado,
                p.sDocumento_adjunto,
                p.iEstado_fl,
                p.iEliminado_fl,
                p.iConcurrencia_id,
                p.sCreated_by                
            }));
        }

        public ActionResult Editing()
        {
            return View();
        }

        public void DocumentCreate(gatbl_DPConvalidaciones documento)
        {
            SolicitudConvalidacion solicitud = Session["solicitudConvalidacion"] as SolicitudConvalidacion;

            var entity = new gatbl_DPConvalidaciones();

            entity.lDPConvalicacion_id = -(solicitud.gatbl_DPConvalidaciones.Count()+1);
            entity.lTipoDocumentoSolicitud_id = documento.lTipoDocumentoSolicitud_id;
            entity.sTipoPresentacion_fl = documento.sTipoPresentacion_fl;
            entity.lCantidad_nro = documento.lCantidad_nro;
            entity.iEstado_fl = "1";
            entity.iEliminado_fl = "1";
            entity.iConcurrencia_id = 1;
            entity.sCreated_by = DateTime.Now.ToString();

            solicitud.gatbl_DPConvalidaciones.Add(entity);
            
            //if (entity.lTipoDocumentoSolicitud_id == null)
            //{
            //    entity.lTipoDocumentoSolicitud_id = 1;
            //}

            //if (product.Category != null)
            //{
            //    entity.CategoryID = product.Category.CategoryID;
            //}

            //entities.Products.Add(entity);
            //entities.SaveChanges();

            //product.ProductID = entity.ProductID;


        }

        public void DocumentUpdate(gatbl_DPConvalidaciones documento)
        {
            SolicitudConvalidacion solicitud = Session["solicitudConvalidacion"] as SolicitudConvalidacion;

            //var entity = new gatbl_DPConvalidaciones();

            //entity.lTipoDocumentoSolicitud_id = documento.lTipoDocumentoSolicitud_id;
            //entity.sTipoPresentacion_fl = documento.sTipoPresentacion_fl;
            //entity.lCantidad_nro = documento.lCantidad_nro;
            //entity.iEstado_fl = documento.iEstado_fl;
            //entity.iEliminado_fl = documento.iEliminado_fl;
            //entity.iConcurrencia_id = documento.iConcurrencia_id;
            //entity.sCreated_by = DateTime.Now.ToString();

            var docactual = solicitud.gatbl_DPConvalidaciones.First(d=>d.lDPConvalicacion_id == documento.lDPConvalicacion_id);
            if(docactual != null)
            {
                docactual.lDPConvalicacion_id = documento.lDPConvalicacion_id;
                docactual.lPConvalidacion_id = documento.lPConvalidacion_id;
                docactual.lTipoDocumentoSolicitud_id = documento.lTipoDocumentoSolicitud_id;
                docactual.sTipoPresentacion_fl = documento.sTipoPresentacion_fl;
                docactual.lCantidad_nro = documento.lCantidad_nro;
                docactual.iEstado_fl = documento.iEstado_fl;
                docactual.iEliminado_fl = documento.iEliminado_fl;
                docactual.iConcurrencia_id = documento.iConcurrencia_id;
                docactual.sCreated_by = DateTime.Now.ToString();
            }

            db.Entry(docactual).State = EntityState.Modified;
            db.SaveChanges();

            //if (documento.TipoDocumentoSolicitud != null)
            //{
            //    entity.lTipoDocumentoSolicitud_id = documento.TipoDocumentoSolicitud.lTipoDocumentoSolicitud_id;                
            //}

            //if (documento.TipoPresentacionDocumento != null)
            //{
            //    entity.sTipoPresentacion_fl = documento.TipoPresentacionDocumento.sTipoPresentacion_fl;
            //}

            //entities.Products.Attach(entity);
            //entities.Entry(entity).State = EntityState.Modified;
            //entities.SaveChanges();
        }

        public void DocumentDestroy(gatbl_DPConvalidaciones documento)
        {
            SolicitudConvalidacion solicitud = Session["solicitudConvalidacion"] as SolicitudConvalidacion;
          
            var docactual = solicitud.gatbl_DPConvalidaciones.First(d => d.lDPConvalicacion_id == documento.lDPConvalicacion_id);
            if (docactual != null)
            {
                docactual.iEstado_fl = "2";                             
            }

            db.Entry(docactual).State = EntityState.Modified;
            db.SaveChanges();


            //var entity = new gatbl_DPConvalidaciones();

            //entity.ProductID = product.ProductID;

            //entities.Products.Attach(entity);

            //entities.Products.Remove(entity);

            //var orderDetails = entities.Order_Details.Where(pd => pd.ProductID == entity.ProductID);

            //foreach (var orderDetail in orderDetails)
            //{
            //    entities.Order_Details.Remove(orderDetail);
            //}

            //entities.SaveChanges();
        }

        public void MDocumentCreate(gatbl_CertificadosMateria materia)
        {
            SolicitudConvalidacion solicitud = Session["solicitudConvalidacion"] as SolicitudConvalidacion;

            var entity = new gatbl_CertificadosMateria();

            entity.lDCertificado_id = -(solicitud.gatbl_CertificadosMaterias.Count() + 1);
            entity.lMateria_id = materia.lMateria_id;
            entity.sTipoPresentacion_fl = materia.sTipoPresentacion_fl;
            entity.dCalificacion = materia.dCalificacion;
            entity.sDocumento_adjunto = materia.sDocumento_adjunto;
            entity.bHomologado = materia.bHomologado;
            entity.iEstado_fl = "1";
            entity.iEliminado_fl = "1";
            entity.iConcurrencia_id = 1;
            entity.sCreated_by = DateTime.Now.ToString();

            solicitud.gatbl_CertificadosMaterias.Add(entity);

        }

        public void MDocumentUpdate(gatbl_CertificadosMateria materia)
        {
            SolicitudConvalidacion solicitud = Session["solicitudConvalidacion"] as SolicitudConvalidacion;
            
            var matactual = solicitud.gatbl_CertificadosMaterias.First(d => d.lDCertificado_id == materia.lDCertificado_id);
            if (matactual != null)
            {
                matactual.lDCertificado_id = materia.lDCertificado_id;
                matactual.lPConvalidacion_id = materia.lPConvalidacion_id;
                matactual.lMateria_id = materia.lMateria_id;
                matactual.sTipoPresentacion_fl = materia.sTipoPresentacion_fl;
                matactual.dCalificacion = materia.dCalificacion;
                matactual.sDocumento_adjunto = materia.sDocumento_adjunto;
                matactual.bHomologado = materia.bHomologado;
                matactual.iEstado_fl = materia.iEstado_fl;
                matactual.iEliminado_fl = materia.iEliminado_fl;
                matactual.iConcurrencia_id = materia.iConcurrencia_id;
                matactual.sCreated_by = DateTime.Now.ToString();
            }

            db.Entry(matactual).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void MDocumentDestroy(gatbl_CertificadosMateria materia)
        {
            SolicitudConvalidacion solicitud = Session["solicitudConvalidacion"] as SolicitudConvalidacion;

            var matactual = solicitud.gatbl_CertificadosMaterias.First(d => d.lDCertificado_id == materia.lDCertificado_id);
            if (matactual != null)
            {
                matactual.iEstado_fl = "2";
            }

            db.Entry(matactual).State = EntityState.Modified;
            db.SaveChanges();          
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<gatbl_DPConvalidaciones> documentos)
        {
            var results = new List<gatbl_DPConvalidaciones>();

            if (documentos != null && ModelState.IsValid)
            {
                foreach (var documento in documentos)
                {                    
                    DocumentCreate(documento);
                    results.Add(documento);
                }
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<gatbl_DPConvalidaciones> documentos)
        {
            if (documentos != null && ModelState.IsValid)
            {
                foreach (var documento in documentos)
                {
                    DocumentUpdate(documento);
                }
            }

            return Json(documentos.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<gatbl_DPConvalidaciones> documentos)
        {
            if (documentos.Any())
            {
                foreach (var documento in documentos)
                {
                    DocumentDestroy(documento);
                }
            }

            return Json(documentos.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MEditing_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<gatbl_CertificadosMateria> materias)
        {
            var results = new List<gatbl_CertificadosMateria>();

            if (materias != null && ModelState.IsValid)
            {
                foreach (var materia in materias)
                {
                    MDocumentCreate(materia);
                    results.Add(materia);
                }
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MEditing_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<gatbl_CertificadosMateria> materias)
        {
            if (materias != null && ModelState.IsValid)
            {
                foreach (var materia in materias)
                {
                    MDocumentUpdate(materia);
                }
            }

            return Json(materias.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MEditing_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<gatbl_CertificadosMateria> materias)
        {
            if (materias.Any())
            {
                foreach (var materia in materias)
                {
                    MDocumentDestroy(materia);
                }
            }

            return Json(materias.ToDataSourceResult(request, ModelState));
        }


        public JsonResult PensumList(int id)
        {
            var pensum = from s in db.Pensums
                         where s.lCarrera_id == id
                         select s;

            return Json(new SelectList(pensum.ToArray(), "lPensum_id", "sDescripcion"), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ObtenerPostulantes(string text)
        {
            SolicitudConvalidacion solicitud = Session["solicitudConvalidacion"] as SolicitudConvalidacion;

            if (string.IsNullOrEmpty(text) && solicitud.gatbl_PConvalidaciones.lPostulante_id != 0)
            {
                return Json((from p in db.gatbl_Postulantes
                             //join ca in db.carreras on p.crr_codigo equals ca.crr_codigo
                             //join sa in db.secciones_academicas on ca.sca_codigo equals sa.sca_codigo
                             where p.lPostulante_id == solicitud.gatbl_PConvalidaciones.lPostulante_id
                             //orderby p.sNombre_desc ascending
                             select new
                             {
                                 lPostulante_id = p.lPostulante_id,
                                 NombreCompleto = p.sApPaterno_desc + " " + p.sApMaterno_desc + ", " + p.sNombre_desc,
                                 sNombre_desc = p.sNombre_desc,
                                 agd_docnro = p.sDocumento_nro
                                 //,
                                 //crr_descripcion = ca.crr_descripcion,
                                 //sca_descripcion = sa.sca_descripcion
                             }).OrderBy(p=> p.NombreCompleto), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json((from p in db.gatbl_Postulantes
                             //join ca in db.carreras on p.crr_codigo equals ca.crr_codigo
                             //join sa in db.secciones_academicas on ca.sca_codigo equals sa.sca_codigo
                             //where p.NombreCompleto.Contains(text)
                             orderby p.sNombre_desc ascending
                             select new
                             {
                                 lPostulante_id = p.lPostulante_id,
                                 NombreCompleto = p.sApPaterno_desc + " " + p.sApMaterno_desc + ", " + p.sNombre_desc,
                                 sNombre_desc = p.sNombre_desc,
                                 agd_docnro = p.sDocumento_nro                                 
                             }).Where(p=>p.NombreCompleto.Contains(text)).OrderBy(p => p.NombreCompleto), JsonRequestBehavior.AllowGet);
            }

            //return Json((from ag in db.agendas
            //             join alg in db.alumnos_agenda on ag.agd_codigo equals alg.agd_codigo
            //             //orderby ag.agd_nombres ascending
            //             select new { agd_nombres = ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + ", " + ag.agd_nombres.Trim(), agd_codigo = ag.agd_codigo.Trim(), agd_docnro = ag.agd_docnro.Trim() }).Where(g => g.agd_nombres.Contains(text)).OrderBy(g => g.agd_nombres), JsonRequestBehavior.AllowGet);
        }        

        public JsonResult ObtenerResponsables(string text)
        {
            SolicitudConvalidacion solicitud = Session["solicitudConvalidacion"] as SolicitudConvalidacion;

            if (string.IsNullOrEmpty(text) && solicitud.gatbl_PConvalidaciones.lResponsable_id != null)
            {
                return Json((from ag in db.agendas
                             select new { agd_nombres = ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + ", " + ag.agd_nombres.Trim(), agd_codigo = ag.agd_codigo.Trim(), agd_docnro = ag.agd_docnro.Trim() }).Where(g => g.agd_codigo.Contains(solicitud.gatbl_PConvalidaciones.lResponsable_id)).OrderBy(g => g.agd_nombres), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json((from ag in db.agendas
                             select new { agd_nombres = ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + ", " + ag.agd_nombres.Trim(), agd_codigo = ag.agd_codigo.Trim(), agd_docnro = ag.agd_docnro.Trim() }).Where(g => g.agd_nombres.Contains(text)).OrderBy(g => g.agd_nombres), JsonRequestBehavior.AllowGet);
            }

            //return Json((from ag in db.agendas
            //             select new { agd_nombres = ag.agd_appaterno.Trim() + " " + ag.agd_apmaterno.Trim() + ", " + ag.agd_nombres.Trim(), agd_codigo = ag.agd_codigo.Trim(), agd_docnro = ag.agd_docnro.Trim() }).Where(g => g.agd_nombres.Contains(text)).OrderBy(g => g.agd_nombres), JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult FacultadList(int id)
        {
            var facultades = from s in db.gatbl_Carreras
                             where s.lUniversidad_id == id
                             select s;

            return Json(new SelectList(facultades.ToArray(), "lCarrera_id", "sCarrera_nm"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerMaterias(string text)
        {
            SolicitudConvalidacion solicitud = Session["solicitudConvalidacion"] as SolicitudConvalidacion;

            if (string.IsNullOrEmpty(text))
            {
                return Json((from p in db.gatbl_ProgramasAnaliticos
                             where p.lCarrera_id == solicitud.gatbl_PConvalidaciones.lCarreraOrigen_id
                             orderby p.sMateria_desc
                             select new { lProgramaAnalitico_id = p.lProgramaAnalitico_id, sMateria_desc = p.sMateria_desc }), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json((from p in db.gatbl_ProgramasAnaliticos
                             where p.lCarrera_id == solicitud.gatbl_PConvalidaciones.lCarreraOrigen_id
                             && p.sMateria_desc.Contains(text)
                             orderby p.sMateria_desc
                             select new { lProgramaAnalitico_id = p.lProgramaAnalitico_id, sMateria_desc = p.sMateria_desc }), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CarreraList(int id)
        {
            var carreras = from s in db.gatbl_Carreras
                             where s.lUniversidad_id == id
                             select s;

            return Json(new SelectList(carreras.ToArray(), "lCarrera_id", "sCarrera_nm"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerCarreras()
        {
            var listacarrera = from c in db.carreras
                               join f in db.secciones_academicas on c.sca_codigo equals f.sca_codigo   
                               orderby c.crr_descripcion                         
                               select new { crr_descripcion = c.crr_descripcion.Trim(), crr_codigo = c.crr_codigo.Trim(), sca_codigo = c.sca_codigo, sca_descripcion = f.sca_descripcion };
                                
            return Json(listacarrera.ToList(), JsonRequestBehavior.AllowGet);
            //return Json(db.carreras.Select(g => new { crr_descripcion = g.crr_descripcion.Trim(), crr_codigo = g.crr_codigo.Trim(), sca_codigo = g.sca_codigo }).OrderBy(g => g.crr_descripcion), JsonRequestBehavior.AllowGet);
        }

        // GET: SolicitudConvalidaciones/Details/5
        public ActionResult Details(int id)
        {
            var solicitudConvalidacion = new SolicitudConvalidacion();
            solicitudConvalidacion.gatbl_PConvalidaciones = db.gatbl_PConvalidaciones.Find(id);
            solicitudConvalidacion.gatbl_DPConvalidaciones = solicitudConvalidacion.gatbl_PConvalidaciones.gatbl_DPConvalidaciones.Where(d=>d.iEstado_fl != "2").ToList();
            solicitudConvalidacion.gatbl_CertificadosMateria = new gatbl_CertificadosMateria();
            solicitudConvalidacion.gatbl_CertificadosMaterias = db.gatbl_CertificadosMateria.Where(c => c.lPConvalidacion_id == id && c.iEstado_fl != "2").ToList();

            return View(solicitudConvalidacion);
        }

        // GET: SolicitudConvalidaciones/Create
        public ActionResult Create()
        {
            ViewBag.lResponsable_id = db.agendas.Select(g => new { agd_nombres = g.agd_appaterno.Trim() + " " + g.agd_apmaterno.Trim() + ", " + g.agd_nombres.Trim(), agd_codigo = g.agd_codigo.Trim() }).OrderBy(g => g.agd_nombres);

            ViewBag.sca_codigo = new SelectList(db.secciones_academicas, "sca_codigo", "sca_descripcion");
            ViewBag.UniversidadList = db.gatbl_Universidades;
            ViewBag.UniversidadDestinoList = db.gatbl_Universidades.Where(u=>u.bInterno == true);

            ViewBag.lCarrera_id = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm");
            ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc");
            ViewBag.lTipoDocumentoSolicitud_id = new SelectList(db.TipoDocumentoSolicitudes, "lTipoDocumentoSolicitud_id", "sDescripcion");
            ViewBag.sTipoPresentacion_fl = new SelectList(db.TipoPresentacionDocumentos, "sTipoPresentacion_fl", "sDescripcion");
            ViewBag.lPensum_id = new SelectList(db.Pensums, "lPensum_id", "sDescripcion");

            ViewData["lTipoDocumentoSolicitud_id"] = db.TipoDocumentoSolicitudes;
            ViewData["sTipoPresentacion_fl"] = db.TipoPresentacionDocumentos;
            

            //SolicitudConvalidacion solicitud = Session["SolicitudConvalidacion"] as SolicitudConvalidacion;

            //if (solicitud != null && solicitud.gatbl_DPConvalidaciones.Count > 0)
            //{
            //    return View(solicitud);
            //}


            var solicitudConvalidacion = new SolicitudConvalidacion();
            solicitudConvalidacion.gatbl_PConvalidaciones = new gatbl_PConvalidaciones();
            solicitudConvalidacion.gatbl_DPConvalidaciones = new List<gatbl_DPConvalidaciones>();

            Session["solicitudConvalidacion"] = solicitudConvalidacion;

            return View(solicitudConvalidacion);
        }

        
        // POST: SolicitudConvalidaciones/Create
        [HttpPost]
        public ActionResult Create(SolicitudConvalidacion solicitudConvalidacion)
        {
            try
            {
                // TODO: Add insert logic here
                solicitudConvalidacion.gatbl_DPConvalidaciones = (Session["SolicitudConvalidacion"] as SolicitudConvalidacion).gatbl_DPConvalidaciones;

                ViewBag.lResponsable_id = db.agendas.Select(g => new { agd_nombres = g.agd_appaterno.Trim() + " " + g.agd_apmaterno.Trim() + ", " + g.agd_nombres.Trim(), agd_codigo = g.agd_codigo.Trim() }).OrderBy(g => g.agd_nombres);

                ViewBag.sca_codigo = new SelectList(db.secciones_academicas, "sca_codigo", "sca_descripcion");
                ViewBag.UniversidadList = db.gatbl_Universidades;
                ViewBag.UniversidadDestinoList = db.gatbl_Universidades.Where(u => u.bInterno == true);
                ViewBag.lPensum_id = new SelectList(db.Pensums, "lPensum_id", "sDescripcion");

                solicitudConvalidacion.gatbl_PConvalidaciones.gatbl_Postulantes = db.gatbl_Postulantes.Find(solicitudConvalidacion.gatbl_PConvalidaciones.lPostulante_id);

                int NumeroSolicitud = db.gatbl_PConvalidaciones.Select(c => c.lPConvalidacion_id).Count()+1;

                solicitudConvalidacion.gatbl_PConvalidaciones.dtPostulacion_dt = DateTime.Now;
                solicitudConvalidacion.gatbl_PConvalidaciones.lNro_solucitud = NumeroSolicitud;
                solicitudConvalidacion.gatbl_PConvalidaciones.lResponsable_id = "0000001138";
                
                solicitudConvalidacion.gatbl_PConvalidaciones.iEstado_fl = "1";
                solicitudConvalidacion.gatbl_PConvalidaciones.iEliminado_fl = "1";
                solicitudConvalidacion.gatbl_PConvalidaciones.sCreated_by = DateTime.Now.ToString();
                solicitudConvalidacion.gatbl_PConvalidaciones.iConcurrencia_id = 1;
                
                db.gatbl_PConvalidaciones.Add(solicitudConvalidacion.gatbl_PConvalidaciones);
                db.SaveChanges();

                var lPConvalidacion_id = db.gatbl_PConvalidaciones.Select(c => c.lPConvalidacion_id).Max();
                foreach (var item in solicitudConvalidacion.gatbl_DPConvalidaciones)
                {
                    var detalle = new gatbl_DPConvalidaciones
                    {
                        lPConvalidacion_id = lPConvalidacion_id,
                        lTipoDocumentoSolicitud_id = item.lTipoDocumentoSolicitud_id,
                        sTipoPresentacion_fl = item.sTipoPresentacion_fl,
                        lCantidad_nro = item.lCantidad_nro,
                        iEstado_fl = item.iEstado_fl,
                        iEliminado_fl = item.iEliminado_fl,
                        sCreated_by = item.sCreated_by,
                        iConcurrencia_id = item.iConcurrencia_id
                    };
                    db.gatbl_DPConvalidaciones.Add(detalle);
                }


                var preconvalidacion = new gatbl_AnalisisPreConvalidaciones()
                {
                    lPConvalidacion_id = solicitudConvalidacion.gatbl_PConvalidaciones.lPConvalidacion_id,
                    lResponsable_id = "0000001138",
                    sVersion_nro = 1,
                    dtAnalisisConvalidacion_dt = DateTime.Now,
                    sObs_desc = string.Empty,
                    iEstado_fl = "2",
                    iEliminado_fl = "1",
                    sCreated_by = DateTime.Now.ToString(),
                    iConcurrencia_id = 1
                };

                db.gatbl_AnalisisPreConvalidaciones.Add(preconvalidacion);
                
                db.SaveChanges();

                return RedirectToAction("Index");

            }
            catch
            {
                return View(solicitudConvalidacion);
            }
        }

        public ActionResult AdicionarDocumento()
        {
            ViewBag.lCarrera_id = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm");
            ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc");
            ViewBag.lTipoDocumentoSolicitud_id = new SelectList(db.TipoDocumentoSolicitudes, "lTipoDocumentoSolicitud_id", "sDescripcion");
            ViewBag.sTipoPresentacion_fl = new SelectList(db.TipoPresentacionDocumentos, "sTipoPresentacion_fl", "sDescripcion");
            ViewBag.UniversidadList = db.gatbl_Universidades;
            ViewBag.UniversidadDestinoList = db.gatbl_Universidades.Where(u => u.bInterno == true);

            var solicitudConvalidacion = Session["solicitudConvalidacion"] as SolicitudConvalidacion;

            solicitudConvalidacion.gatbl_PConvalidaciones.lUniversidadOrigen_id = Convert.ToInt32(Request.Form["gatbl_PConvalidaciones_lUniversidadOrigen_id"]);


            return View();
        }

        public ActionResult AddDocument()
        {
            ViewBag.lCarrera_id = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm");
            ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc");
            ViewBag.lTipoDocumentoSolicitud_id = new SelectList(db.TipoDocumentoSolicitudes, "lTipoDocumentoSolicitud_id", "sDescripcion");
            ViewBag.sTipoPresentacion_fl = new SelectList(db.TipoPresentacionDocumentos, "sTipoPresentacion_fl", "sDescripcion");
            ViewBag.UniversidadList = db.gatbl_Universidades;

            return PartialView();
        }

        //public PartialViewResult AddItemDetail()
        //{
        //    ViewBag.lTipoDocumentoSolicitud_id = new SelectList(db.TipoDocumentoSolicitudes, "lTipoDocumentoSolicitud_id", "sDescripcion");
        //    ViewBag.sTipoPresentacion_fl = new SelectList(db.TipoPresentacionDocumentos, "sTipoPresentacion_fl", "sDescripcion");

        //    var model = new gatbl_DPConvalidaciones();
        //    return PartialView("_AddItemDetail");
        //}

        public ActionResult AddItemDetail()
        {
            var solicitudConvalidacion = Session["solicitudConvalidacion"] as SolicitudConvalidacion;

            ViewBag.lResponsable_id = db.agendas.Select(g => new { agd_nombres = g.agd_appaterno.Trim() + " " + g.agd_apmaterno.Trim() + ", " + g.agd_nombres.Trim(), agd_codigo = g.agd_codigo.Trim() }).OrderBy(g => g.agd_nombres);

            ViewBag.sca_codigo = new SelectList(db.secciones_academicas, "sca_codigo", "sca_descripcion");
            ViewBag.UniversidadList = db.gatbl_Universidades;
            ViewBag.UniversidadDestinoList = db.gatbl_Universidades.Where(u => u.bInterno == true);

            ViewBag.lCarrera_id = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm");
            ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc");
            ViewBag.lTipoDocumentoSolicitud_id = new SelectList(db.TipoDocumentoSolicitudes, "lTipoDocumentoSolicitud_id", "sDescripcion");
            ViewBag.sTipoPresentacion_fl = new SelectList(db.TipoPresentacionDocumentos, "sTipoPresentacion_fl", "sDescripcion");

            solicitudConvalidacion.gatbl_DPConvalidaciones.Add(new gatbl_DPConvalidaciones());

            return View("Create", solicitudConvalidacion);
        }

        [HttpPost]
        public ActionResult AdicionarDocumento(gatbl_DPConvalidaciones documento)
        {
            var solicitudConvalidacion = Session["solicitudConvalidacion"] as SolicitudConvalidacion;
            
            ViewBag.lCarrera_id = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm");
            ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc");
            ViewBag.lTipoDocumentoSolicitud_id = new SelectList(db.TipoDocumentoSolicitudes, "lTipoDocumentoSolicitud_id", "sDescripcion");
            ViewBag.sTipoPresentacion_fl = new SelectList(db.TipoPresentacionDocumentos, "sTipoPresentacion_fl", "sDescripcion");
            ViewBag.sca_codigo = new SelectList(db.secciones_academicas, "sca_codigo", "sca_descripcion");
            ViewBag.UniversidadList = db.gatbl_Universidades;
            ViewBag.UniversidadDestinoList = db.gatbl_Universidades.Where(u => u.bInterno == true);

            ViewBag.lUniversidadOrigen_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", solicitudConvalidacion.gatbl_PConvalidaciones.lUniversidadOrigen_id);
            ViewBag.lUniversidadDestino_id = new SelectList(db.gatbl_Universidades.Where(u => u.bInterno == true), "lUniversidad_id", "sNombre_desc", solicitudConvalidacion.gatbl_PConvalidaciones.lUniversidadDestino_id);
            ViewBag.lCarreraOrigen_id = new SelectList(db.gatbl_Carreras.Where(c => c.lUniversidad_id == solicitudConvalidacion.gatbl_PConvalidaciones.lUniversidadOrigen_id), "lCarrera_id", "sCarrera_nm", solicitudConvalidacion.gatbl_PConvalidaciones.lCarreraOrigen_id);
            ViewBag.lCarreraDestino_id = new SelectList(db.gatbl_Carreras.Where(c => c.lUniversidad_id == solicitudConvalidacion.gatbl_PConvalidaciones.lUniversidadDestino_id), "lCarrera_id", "sCarrera_nm", solicitudConvalidacion.gatbl_PConvalidaciones.lCarreraDestino_id);         

            int TipoDocId = documento.lTipoDocumentoSolicitud_id;
            var TipoDocumento = db.TipoDocumentoSolicitudes.Find(TipoDocId);

            string TipoPresId = documento.sTipoPresentacion_fl;
            var TipoPresentacion = db.TipoPresentacionDocumentos.Find(TipoPresId);

            documento.iEstado_fl = "1";
            documento.iEliminado_fl = "1";
            documento.sCreated_by = DateTime.Now.ToString();
            documento.iConcurrencia_id = 1;

            documento.lTipoDocumentoSolicitud_id = TipoDocId;
            documento.sTipoPresentacion_fl = TipoPresId;
                        
            documento.TipoDocumentoSolicitud = TipoDocumento;
            documento.TipoPresentacionDocumento = TipoPresentacion;


            solicitudConvalidacion.gatbl_DPConvalidacion = new gatbl_DPConvalidaciones
            {
                lTipoDocumentoSolicitud_id = documento.lTipoDocumentoSolicitud_id,
                sTipoPresentacion_fl = documento.sTipoPresentacion_fl,
                lCantidad_nro = documento.lCantidad_nro,
                iEstado_fl = documento.iEstado_fl,
                iEliminado_fl = documento.iEliminado_fl,
                sCreated_by = documento.sCreated_by,                
                TipoDocumentoSolicitud = documento.TipoDocumentoSolicitud,
                TipoPresentacionDocumento = documento.TipoPresentacionDocumento
            };

            solicitudConvalidacion.gatbl_DPConvalidaciones.Add(solicitudConvalidacion.gatbl_DPConvalidacion);

            string sController = "Create";
            if(solicitudConvalidacion.gatbl_PConvalidaciones.lPConvalidacion_id != 0)
            {
                sController = "Edit";
            }

            return View(sController, solicitudConvalidacion);
        }

        public ActionResult EditarDocumento(int? id)
        {
            var solicitudConvalidacion = Session["solicitudConvalidacion"] as SolicitudConvalidacion;

            //ViewBag.lCarrera_id = new SelectList(db.gatbl_Carreras, "lCarrera_id", "sCarrera_nm");

            //ViewBag.lUniversidad_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc");
            ViewBag.lTipoDocumentoSolicitud_id = new SelectList(db.TipoDocumentoSolicitudes, "lTipoDocumentoSolicitud_id", "sDescripcion"); ;
            ViewBag.sTipoPresentacion_fl = new SelectList(db.TipoPresentacionDocumentos, "sTipoPresentacion_fl", "sDescripcion");
            ViewBag.UniversidadList = db.gatbl_Universidades;
            

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            gatbl_DPConvalidaciones documento = new gatbl_DPConvalidaciones();

            documento = db.gatbl_DPConvalidaciones.Find(id);
            if (documento == null)
            {
                return HttpNotFound();
            }

            solicitudConvalidacion.gatbl_DPConvalidacion = documento;

            
            return View(documento);
        }

        [HttpPost]
        public ActionResult EditarDocumento(gatbl_DPConvalidaciones documento)
        {
            var solicitudConvalidacion = Session["solicitudConvalidacion"] as SolicitudConvalidacion;

            ViewBag.lTipoDocumentoSolicitud_id = new SelectList(db.TipoDocumentoSolicitudes, "lTipoDocumentoSolicitud_id", "sDescripcion");
            ViewBag.sTipoPresentacion_fl = new SelectList(db.TipoPresentacionDocumentos, "sTipoPresentacion_fl", "sDescripcion");
            ViewBag.UniversidadList = db.gatbl_Universidades;
            ViewBag.UniversidadDestinoList = db.gatbl_Universidades.Where(u => u.bInterno == true);
            

            ViewBag.lUniversidadOrigen_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", solicitudConvalidacion.gatbl_PConvalidaciones.lUniversidadOrigen_id);
            ViewBag.lUniversidadDestino_id = new SelectList(db.gatbl_Universidades.Where(u => u.bInterno == true), "lUniversidad_id", "sNombre_desc", solicitudConvalidacion.gatbl_PConvalidaciones.lUniversidadDestino_id);
            ViewBag.lCarreraOrigen_id = new SelectList(db.gatbl_Carreras.Where(c => c.lUniversidad_id == solicitudConvalidacion.gatbl_PConvalidaciones.lUniversidadOrigen_id), "lCarrera_id", "sCarrera_nm", solicitudConvalidacion.gatbl_PConvalidaciones.lCarreraOrigen_id);
            ViewBag.lCarreraDestino_id = new SelectList(db.gatbl_Carreras.Where(c => c.lUniversidad_id == solicitudConvalidacion.gatbl_PConvalidaciones.lUniversidadDestino_id), "lCarrera_id", "sCarrera_nm", solicitudConvalidacion.gatbl_PConvalidaciones.lCarreraDestino_id);
            
            int TipoDocId = documento.lTipoDocumentoSolicitud_id;
            var TipoDocumento = db.TipoDocumentoSolicitudes.Find(TipoDocId);

            string TipoPresId = documento.sTipoPresentacion_fl;
            var TipoPresentacion = db.TipoPresentacionDocumentos.Find(TipoPresId);

            documento.iEstado_fl = "1";
            documento.iEliminado_fl = "1";
            documento.sCreated_by = DateTime.Now.ToString();
            documento.iConcurrencia_id = 1;

            //documento.lCarrera_id = CarId;
            //documento.sTipoDocumento_fl = TipoDocId;
            //documento.sTipoPresentacion_fl = TipoPresId;
            
            documento.TipoDocumentoSolicitud = TipoDocumento;
            documento.TipoPresentacionDocumento = TipoPresentacion;

            var documentoActual = solicitudConvalidacion.gatbl_DPConvalidaciones.First(d => d.lDPConvalicacion_id == solicitudConvalidacion.gatbl_DPConvalidacion.lDPConvalicacion_id);
            if(documentoActual != null)
            {
                documentoActual.lDPConvalicacion_id = solicitudConvalidacion.gatbl_DPConvalidacion.lDPConvalicacion_id;                
                documentoActual.lTipoDocumentoSolicitud_id = documento.lTipoDocumentoSolicitud_id;
                documentoActual.sTipoPresentacion_fl = documento.sTipoPresentacion_fl;
                documentoActual.lCantidad_nro = documento.lCantidad_nro;
                documentoActual.iEstado_fl = documento.iEstado_fl;
                documentoActual.iEliminado_fl = documento.iEliminado_fl;
                documentoActual.sCreated_by = documento.sCreated_by;
                
                documentoActual.TipoDocumentoSolicitud = documento.TipoDocumentoSolicitud;
                documentoActual.TipoPresentacionDocumento = documento.TipoPresentacionDocumento;
            }
            

            string sController = "Create";
            if (solicitudConvalidacion.gatbl_PConvalidaciones.lPConvalidacion_id != 0)
            {
                sController = "Edit";
            }

            return View(sController, solicitudConvalidacion);                       
        }

        // GET: SolicitudConvalidaciones/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.lResponsable_id = db.agendas.Select(g => new { agd_nombres = g.agd_appaterno.Trim() + " " + g.agd_apmaterno.Trim() + ", " + g.agd_nombres.Trim(), agd_codigo = g.agd_codigo.Trim() }).OrderBy(g => g.agd_nombres);
            ViewData["lTipoDocumentoSolicitud_id"] = db.TipoDocumentoSolicitudes;
            ViewData["sTipoPresentacion_fl"] = db.TipoPresentacionDocumentos;

            ViewBag.UniversidadList = db.gatbl_Universidades;
            ViewBag.UniversidadDestinoList = db.gatbl_Universidades.Where(u => u.bInterno == true);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SolicitudConvalidacion solicitud = new SolicitudConvalidacion();
            
            gatbl_PConvalidaciones gatbl_PConvalidaciones = db.gatbl_PConvalidaciones.Find(id);
            if (gatbl_PConvalidaciones == null)
            {
                return HttpNotFound();
            }

            solicitud.gatbl_PConvalidaciones = gatbl_PConvalidaciones;
            
            ViewBag.lUniversidadOrigen_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", solicitud.gatbl_PConvalidaciones.lUniversidadOrigen_id);
            ViewBag.lUniversidadDestino_id = new SelectList(db.gatbl_Universidades.Where(u => u.bInterno == true), "lUniversidad_id", "sNombre_desc", solicitud.gatbl_PConvalidaciones.lUniversidadDestino_id);
            ViewBag.lCarreraOrigen_id = new SelectList(db.gatbl_Carreras.Where(c => c.lUniversidad_id == gatbl_PConvalidaciones.lUniversidadOrigen_id), "lCarrera_id", "sCarrera_nm", solicitud.gatbl_PConvalidaciones.lCarreraOrigen_id);
            ViewBag.lCarreraDestino_id = new SelectList(db.gatbl_Carreras.Where(c => c.lUniversidad_id == gatbl_PConvalidaciones.lUniversidadDestino_id), "lCarrera_id", "sCarrera_nm", solicitud.gatbl_PConvalidaciones.lCarreraDestino_id);
            ViewBag.lPensum_id = new SelectList(db.Pensums.Where(c => c.lCarrera_id == gatbl_PConvalidaciones.lCarreraDestino_id), "lPensum_id", "sDescripcion", solicitud.gatbl_PConvalidaciones.lPensum_id);

            //var queryDetalle = db.gatbl_DPConvalidaciones.Where(d => d.lPConvalidacion_id == id);

            //if (queryDetalle.ToList().Count > 0)
            //{
            //    List<gatbl_DPConvalidaciones> detalle = queryDetalle.ToList<gatbl_DPConvalidaciones>();

            //    solicitud.gatbl_DPConvalidaciones = detalle;
            //}
            //else
            //{
            //    solicitud.gatbl_DPConvalidaciones = new List<gatbl_DPConvalidaciones>();
            //}

            solicitud.gatbl_DPConvalidaciones = gatbl_PConvalidaciones.gatbl_DPConvalidaciones.Where(d=>d.iEstado_fl != "2").ToList();

            Session["solicitudConvalidacion"] = solicitud;

            return View(solicitud);
        }

        // POST: SolicitudConvalidaciones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SolicitudConvalidacion solicitudConvalidacion)
        {
            try
            {
                // TODO: Add update logic here
                SolicitudConvalidacion solicitud = Session["SolicitudConvalidacion"] as SolicitudConvalidacion;
                solicitudConvalidacion.gatbl_DPConvalidaciones = solicitud.gatbl_DPConvalidaciones;

                ViewBag.lResponsable_id = db.agendas.Select(g => new { agd_nombres = g.agd_appaterno.Trim() + " " + g.agd_apmaterno.Trim() + ", " + g.agd_nombres.Trim(), agd_codigo = g.agd_codigo.Trim() }).OrderBy(g => g.agd_nombres);
                ViewBag.lPensum_id = new SelectList(db.Pensums.Where(c => c.lCarrera_id == solicitud.gatbl_PConvalidaciones.lCarreraDestino_id), "lPensum_id", "sDescripcion", solicitud.gatbl_PConvalidaciones.lPensum_id);
                ViewData["lTipoDocumentoSolicitud_id"] = db.TipoDocumentoSolicitudes;
                ViewData["sTipoPresentacion_fl"] = db.TipoPresentacionDocumentos;
                ViewBag.UniversidadList = db.gatbl_Universidades;
                ViewBag.UniversidadDestinoList = db.gatbl_Universidades.Where(u => u.bInterno == true);


                solicitudConvalidacion.gatbl_PConvalidaciones.lPConvalidacion_id = solicitud.gatbl_PConvalidaciones.lPConvalidacion_id;

                solicitudConvalidacion.gatbl_PConvalidaciones.gatbl_Postulantes = db.gatbl_Postulantes.Find(solicitudConvalidacion.gatbl_PConvalidaciones.lPostulante_id);

                solicitudConvalidacion.gatbl_PConvalidaciones.dtPostulacion_dt = solicitud.gatbl_PConvalidaciones.dtPostulacion_dt;
                solicitudConvalidacion.gatbl_PConvalidaciones.lNro_solucitud = solicitud.gatbl_PConvalidaciones.lNro_solucitud;
                solicitudConvalidacion.gatbl_PConvalidaciones.lResponsable_id = solicitud.gatbl_PConvalidaciones.lResponsable_id;

                solicitudConvalidacion.gatbl_PConvalidaciones.lUniversidadOrigen_id = Convert.ToInt32(Request["lUniversidadOrigen_id"]);
                solicitudConvalidacion.gatbl_PConvalidaciones.lCarreraOrigen_id = Convert.ToInt32(Request["lCarreraOrigen_id"]);
                solicitudConvalidacion.gatbl_PConvalidaciones.lUniversidadDestino_id = Convert.ToInt32(Request["lUniversidadDestino_id"]);
                solicitudConvalidacion.gatbl_PConvalidaciones.lCarreraDestino_id = Convert.ToInt32(Request["lCarreraDestino_id"]);
                solicitudConvalidacion.gatbl_PConvalidaciones.lPensum_id = Convert.ToInt32(Request["lPensum_id"]);

                solicitudConvalidacion.gatbl_PConvalidaciones.iEstado_fl = "1";
                solicitudConvalidacion.gatbl_PConvalidaciones.iEliminado_fl = "1";
                solicitudConvalidacion.gatbl_PConvalidaciones.sCreated_by = DateTime.Now.ToString();
                solicitudConvalidacion.gatbl_PConvalidaciones.iConcurrencia_id = 1;

                db.Entry(solicitudConvalidacion.gatbl_PConvalidaciones).State = EntityState.Modified;
                db.SaveChanges();

                var lPConvalidacion_id = solicitudConvalidacion.gatbl_PConvalidaciones.lPConvalidacion_id;
                foreach (var item in solicitudConvalidacion.gatbl_DPConvalidaciones)
                {
                    var detalle = new gatbl_DPConvalidaciones
                    {
                        lDPConvalicacion_id = item.lDPConvalicacion_id,
                        lPConvalidacion_id = lPConvalidacion_id,
                        lTipoDocumentoSolicitud_id = item.lTipoDocumentoSolicitud_id,
                        sTipoPresentacion_fl = item.sTipoPresentacion_fl,
                        lCantidad_nro = item.lCantidad_nro,
                        iEstado_fl = item.iEstado_fl,
                        iEliminado_fl = item.iEliminado_fl,
                        sCreated_by = item.sCreated_by,
                        iConcurrencia_id = item.iConcurrencia_id
                    };

                    if (detalle.lDPConvalicacion_id > 0)
                        db.Entry(detalle).State = EntityState.Modified;
                    else
                        db.gatbl_DPConvalidaciones.Add(detalle);
                }

                db.SaveChanges();

                ViewBag.lUniversidadOrigen_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", solicitudConvalidacion.gatbl_PConvalidaciones.lUniversidadOrigen_id);
                ViewBag.lUniversidadDestino_id = new SelectList(db.gatbl_Universidades.Where(u => u.bInterno == true), "lUniversidad_id", "sNombre_desc", solicitudConvalidacion.gatbl_PConvalidaciones.lUniversidadDestino_id);
                ViewBag.lCarreraOrigen_id = new SelectList(db.gatbl_Carreras.Where(c => c.lUniversidad_id == solicitudConvalidacion.gatbl_PConvalidaciones.lUniversidadOrigen_id), "lCarrera_id", "sCarrera_nm", solicitudConvalidacion.gatbl_PConvalidaciones.lCarreraOrigen_id);
                ViewBag.lCarreraDestino_id = new SelectList(db.gatbl_Carreras.Where(c => c.lUniversidad_id == solicitudConvalidacion.gatbl_PConvalidaciones.lUniversidadDestino_id), "lCarrera_id", "sCarrera_nm", solicitudConvalidacion.gatbl_PConvalidaciones.lCarreraDestino_id);


                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ViewBag.lUniversidadOrigen_id = new SelectList(db.gatbl_Universidades, "lUniversidad_id", "sNombre_desc", solicitudConvalidacion.gatbl_PConvalidaciones.lUniversidadOrigen_id);
                ViewBag.lUniversidadDestino_id = new SelectList(db.gatbl_Universidades.Where(u => u.bInterno == true), "lUniversidad_id", "sNombre_desc", solicitudConvalidacion.gatbl_PConvalidaciones.lUniversidadDestino_id);
                ViewBag.lCarreraOrigen_id = new SelectList(db.gatbl_Carreras.Where(c => c.lUniversidad_id == solicitudConvalidacion.gatbl_PConvalidaciones.lUniversidadOrigen_id), "lCarrera_id", "sCarrera_nm", solicitudConvalidacion.gatbl_PConvalidaciones.lCarreraOrigen_id);
                ViewBag.lCarreraDestino_id = new SelectList(db.gatbl_Carreras.Where(c => c.lUniversidad_id == solicitudConvalidacion.gatbl_PConvalidaciones.lUniversidadDestino_id), "lCarrera_id", "sCarrera_nm", solicitudConvalidacion.gatbl_PConvalidaciones.lCarreraDestino_id);


                return View();
            }
        }

        public ActionResult AddCertificate()
        {
            SolicitudConvalidacion solicitud = Session["SolicitudConvalidacion"] as SolicitudConvalidacion;

            ViewBag.lMateria_id = new SelectList(db.gatbl_ProgramasAnaliticos.Where(m=>m.lCarrera_id == solicitud.gatbl_PConvalidaciones.lCarreraOrigen_id), "lProgramaAnalitico_id", "sMateria_desc");
            ViewBag.sTipoPresentacion_fl = new SelectList(db.TipoPresentacionDocumentos, "sTipoPresentacion_fl", "sDescripcion");

            return View();
        }

        [HttpPost]
        public ActionResult AddCertificate(gatbl_CertificadosMateria certificado)
        {
            SolicitudConvalidacion solicitud = Session["SolicitudConvalidacion"] as SolicitudConvalidacion;

            ViewBag.lTipoDocumentoSolicitud_id = new SelectList(db.TipoDocumentoSolicitudes, "lTipoDocumentoSolicitud_id", "sDescripcion");
            ViewBag.sTipoPresentacion_fl = new SelectList(db.TipoPresentacionDocumentos, "sTipoPresentacion_fl", "sDescripcion");
            ViewBag.lMateria_id = new SelectList(db.gatbl_ProgramasAnaliticos.Where(m => m.lCarrera_id == solicitud.gatbl_PConvalidaciones.lCarreraOrigen_id), "lProgramaAnalitico_id", "sMateria_desc");

            certificado.lPConvalidacion_id = solicitud.gatbl_PConvalidaciones.lPConvalidacion_id;
            certificado.gatbl_PConvalidaciones = solicitud.gatbl_PConvalidaciones;
            certificado.gatbl_ProgramasAnaliticos = db.gatbl_ProgramasAnaliticos.Find(certificado.lMateria_id);
            certificado.TipoPresentacionDocumento = db.TipoPresentacionDocumentos.Find(certificado.sTipoPresentacion_fl);

            certificado.iEstado_fl = "1";
            certificado.iEliminado_fl = "1";
            certificado.sCreated_by = DateTime.Now.ToString();
            certificado.iConcurrencia_id = 1;

            solicitud.gatbl_CertificadosMaterias.Add(certificado);

            string sController = "Certificate";
            //if (solicitudConvalidacion.gatbl_PConvalidaciones.lPConvalidacion_id != 0)
            //{
            //    sController = "Edit";
            //}

            return View(sController, solicitud);
        }

        // GET: SolicitudConvalidaciones/Certificate/5
        public ActionResult Certificate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_PConvalidaciones gatbl_PConvalidaciones = db.gatbl_PConvalidaciones.Find(id);
            if (gatbl_PConvalidaciones == null)
            {
                return HttpNotFound();
            }

            var solicitudConvalidacion = new SolicitudConvalidacion();
            solicitudConvalidacion.gatbl_PConvalidaciones = gatbl_PConvalidaciones;
            solicitudConvalidacion.gatbl_DPConvalidaciones = gatbl_PConvalidaciones.gatbl_DPConvalidaciones.ToList();
            solicitudConvalidacion.gatbl_CertificadosMateria = new gatbl_CertificadosMateria();
            solicitudConvalidacion.gatbl_CertificadosMaterias = db.gatbl_CertificadosMateria.Where(c=>c.lPConvalidacion_id == id && c.iEstado_fl != "2").ToList();

            Session["SolicitudConvalidacion"] = solicitudConvalidacion;

            ViewBag.lMateria_id = new SelectList(db.gatbl_ProgramasAnaliticos.Where(m => m.lCarrera_id == solicitudConvalidacion.gatbl_PConvalidaciones.lCarreraOrigen_id), "lProgramaAnalitico_id", "sMateria_desc");
            ViewBag.sTipoPresentacion_fl = new SelectList(db.TipoPresentacionDocumentos, "sTipoPresentacion_fl", "sDescripcion");

            ViewData["sTipoPresentacion_fl"] = db.TipoPresentacionDocumentos;
            ViewData["lMateria_id"] = db.gatbl_ProgramasAnaliticos.Where(m=> m.lCarrera_id == solicitudConvalidacion.gatbl_PConvalidaciones.lCarreraOrigen_id);

            return View(solicitudConvalidacion);
        }

        // POST: SolicitudConvalidaciones/Certificate/5
        [HttpPost, ActionName("Certificate")]
        [ValidateAntiForgeryToken]
        public ActionResult CertificateConfirmed(SolicitudConvalidacion solicitudConvalidacion)
        {
            try
            {
                // TODO: Add delete logic here

                SolicitudConvalidacion solicitud = Session["SolicitudConvalidacion"] as SolicitudConvalidacion;

                foreach (var certificado in solicitud.gatbl_CertificadosMaterias)
                {
                    var itemcertificado = new gatbl_CertificadosMateria
                    {
                        lDCertificado_id = certificado.lDCertificado_id,
                        lPConvalidacion_id = solicitud.gatbl_PConvalidaciones.lPConvalidacion_id,
                        lMateria_id = certificado.lMateria_id,
                        sTipoPresentacion_fl = certificado.sTipoPresentacion_fl,
                        bHomologado = certificado.bHomologado,
                        dCalificacion = certificado.dCalificacion,
                        sDocumento_adjunto = certificado.sDocumento_adjunto,
                        iEstado_fl = certificado.iEstado_fl,
                        iEliminado_fl = certificado.iEliminado_fl,
                        sCreated_by = certificado.sCreated_by,
                        iConcurrencia_id = certificado.iConcurrencia_id
                    };

                    if (itemcertificado.lDCertificado_id > 0)
                        db.Entry(itemcertificado).State = EntityState.Modified;
                    else
                        db.gatbl_CertificadosMateria.Add(itemcertificado);
                    
                }

                db.SaveChanges();

                ViewBag.lMateria_id = new SelectList(db.gatbl_ProgramasAnaliticos.Where(m => m.lCarrera_id == solicitud.gatbl_PConvalidaciones.lCarreraOrigen_id), "lProgramaAnalitico_id", "sMateria_desc");
                ViewBag.sTipoPresentacion_fl = new SelectList(db.TipoPresentacionDocumentos, "sTipoPresentacion_fl", "sDescripcion");
                ViewData["lMateria_id"] = db.gatbl_ProgramasAnaliticos.Where(m => m.lCarrera_id == solicitud.gatbl_PConvalidaciones.lCarreraDestino_id);

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: SolicitudConvalidaciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gatbl_PConvalidaciones gatbl_PConvalidaciones = db.gatbl_PConvalidaciones.Find(id);
            if (gatbl_PConvalidaciones == null)
            {
                return HttpNotFound();
            }

            var solicitudConvalidacion = new SolicitudConvalidacion();
            solicitudConvalidacion.gatbl_PConvalidaciones = gatbl_PConvalidaciones;
            solicitudConvalidacion.gatbl_DPConvalidaciones = gatbl_PConvalidaciones.gatbl_DPConvalidaciones.Where(d => d.iEstado_fl != "2").ToList();
            solicitudConvalidacion.gatbl_CertificadosMateria = new gatbl_CertificadosMateria();
            solicitudConvalidacion.gatbl_CertificadosMaterias = db.gatbl_CertificadosMateria.Where(c => c.lPConvalidacion_id == id && c.iEstado_fl != "2").ToList();

            return View(solicitudConvalidacion);
        }

        // POST: SolicitudConvalidaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                // TODO: Add delete logic here

                gatbl_PConvalidaciones gatbl_PConvalidaciones = db.gatbl_PConvalidaciones.Find(id);

                var detalle = from d in db.gatbl_DPConvalidaciones
                              where d.lPConvalidacion_id == id
                              select d;

                foreach (var item in detalle)
                {

                    db.gatbl_DPConvalidaciones.Remove(item);
                }

                db.gatbl_PConvalidaciones.Remove(gatbl_PConvalidaciones);
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
