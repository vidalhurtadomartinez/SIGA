using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SIGAA.Areas.CONV.Models;
using SIGAA.Areas.CONV.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIGAA.Etiquetas;
using SIGAA.Commons;

namespace SIGAA.Areas.CONV.Controllers
{
    public class gatbl_DetProgramasAnaliticosController : Controller
    {
        private ConvalidacionesContext db = new ConvalidacionesContext();

        // GET: gatbl_DetProgramasAnaliticos
        public ActionResult Index()
        {
            ProgramaAnaliticoViewModels programa = Session["programa"] as ProgramaAnaliticoViewModels;

            var gatbl_DetProgramasAnaliticos = programa.gatbl_DetProgramasAnaliticos;
            return View(gatbl_DetProgramasAnaliticos.ToList());
        }     
        
        private int NivelActual(gatbl_DetProgramasAnaliticos detalle)
        {
            ProgramaAnaliticoViewModels programa = Session["programa"] as ProgramaAnaliticoViewModels;
            int rnivel = 0;
            
            while(detalle.lDetProgramaAnaliticoPadre_id != null)
            {
                rnivel++;

                detalle = programa.gatbl_DetProgramasAnaliticos.FirstOrDefault(d=>d.lDetProgramaAnalitico_id == detalle.lDetProgramaAnaliticoPadre_id);
            }

            return rnivel;
        }

        private string CodigoItem(gatbl_DetProgramasAnaliticos detalle)
        {
            ProgramaAnaliticoViewModels programa = Session["programa"] as ProgramaAnaliticoViewModels;
            string rcodigo = detalle.sNumero.ToString();

            while (detalle.lDetProgramaAnaliticoPadre_id != null)
            {                
                detalle = programa.gatbl_DetProgramasAnaliticos.FirstOrDefault(d => d.lDetProgramaAnalitico_id == detalle.lDetProgramaAnaliticoPadre_id);
                rcodigo = detalle.sNumero.ToString() + "." + rcodigo;
            }

            return rcodigo;
        }

        public JsonResult All([DataSourceRequest] DataSourceRequest request)
        {
            ProgramaAnaliticoViewModels programa = Session["programa"] as ProgramaAnaliticoViewModels;
            
            var post = from p in programa.gatbl_DetProgramasAnaliticos
                       orderby p.sNumero ascending                       
                       select p;
            return Json(post.ToDataSourceResult(request, p => new
            {
                p.lDetProgramaAnalitico_id,
                p.sNumero,
                p.sDescripcion_desc,
                p.sContenido_gral,
                p.lDetProgramaAnaliticoPadre_id                
            }));
        }

        public JsonResult Destroy([DataSourceRequest] DataSourceRequest request, gatbl_DetProgramasAnaliticos gatbl_DetProgramasAnaliticos)
        {
            ProgramaAnaliticoViewModels programa = Session["programa"] as ProgramaAnaliticoViewModels;

            if (gatbl_DetProgramasAnaliticos.lDetProgramaAnalitico_id > 0)
                programa.gatbl_DetProgramasAnaliticosEliminados.Add(gatbl_DetProgramasAnaliticos);

            return Json(new[] { gatbl_DetProgramasAnaliticos }.ToTreeDataSourceResult(request, ModelState));
        }

        public JsonResult CreateNew([DataSourceRequest] DataSourceRequest request, gatbl_DetProgramasAnaliticos gatbl_DetProgramasAnaliticos)
        {
            ProgramaAnaliticoViewModels programa = Session["programa"] as ProgramaAnaliticoViewModels;

            if (ModelState.IsValid)
            {
                gatbl_DetProgramasAnaliticos.lDetProgramaAnalitico_id = -(programa.gatbl_DetProgramasAnaliticos.Count() + 1);
                gatbl_DetProgramasAnaliticos.sNumero = programa.gatbl_DetProgramasAnaliticos.Count(p => p.lDetProgramaAnaliticoPadre_id == gatbl_DetProgramasAnaliticos.lDetProgramaAnaliticoPadre_id)+1;
                gatbl_DetProgramasAnaliticos.Nivel = NivelActual(gatbl_DetProgramasAnaliticos);
                gatbl_DetProgramasAnaliticos.sCodigo = CodigoItem(gatbl_DetProgramasAnaliticos);
                gatbl_DetProgramasAnaliticos.iEstado_fl = "1";
                gatbl_DetProgramasAnaliticos.iEliminado_fl = "1";
                gatbl_DetProgramasAnaliticos.sCreated_by = DateTime.Now.ToString();
                gatbl_DetProgramasAnaliticos.iConcurrencia_id = 1;

                programa.gatbl_DetProgramasAnaliticos.Add(gatbl_DetProgramasAnaliticos);                
            }

            return Json(new[] { gatbl_DetProgramasAnaliticos }.ToTreeDataSourceResult(request, ModelState));
        }
        
        public JsonResult Update([DataSourceRequest] DataSourceRequest request, gatbl_DetProgramasAnaliticos gatbl_DetProgramasAnaliticos)
        {
            ProgramaAnaliticoViewModels programa = Session["programa"] as ProgramaAnaliticoViewModels;

            gatbl_DetProgramasAnaliticos.lProgramaAnalitico_id = programa.gatbl_ProgramasAnaliticos.lProgramaAnalitico_id;

            if (ModelState.IsValid)
            {
                var detalle = programa.gatbl_DetProgramasAnaliticos.FirstOrDefault(p => p.lDetProgramaAnalitico_id == gatbl_DetProgramasAnaliticos.lDetProgramaAnalitico_id);

                if (detalle != null)
                {
                    detalle.sNumero = gatbl_DetProgramasAnaliticos.sNumero;
                    detalle.sDescripcion_desc = gatbl_DetProgramasAnaliticos.sDescripcion_desc;
                    detalle.sContenido_gral = gatbl_DetProgramasAnaliticos.sContenido_gral;
                }
            }

            return Json(new[] { gatbl_DetProgramasAnaliticos }.ToTreeDataSourceResult(request, ModelState));
        }
    }
}