﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SIGAA.Areas.CRM.Models;

namespace MapiriSoftCRM.Views.tblEventos
{
    public class SchedulerController : Controller
    {
        private DataDb db = new DataDb();

        public ActionResult Index()
        {
            var data = db.tEvento.ToList()
                        .Select(tEvento => new tEventoViewModel(tEvento))
                        .AsQueryable();

            return View(data);
        }


        public ActionResult tEvento_Read([DataSourceRequest] DataSourceRequest request)
        {
            var devento = from ev in db.tEvento
                       where !db.tblEvento.Any(m => m.lRecordatorio_id == ev.lEvento_id)
                       select ev;

            var data = devento.ToList()
                        .Select(tEvento => new tEventoViewModel(tEvento))
                        .AsQueryable();

            //var data = db.tEvento.ToList()
            //            .Select(tEvento => new tEventoViewModel(tEvento))
            //            .AsQueryable();

            return Json(data.ToDataSourceResult(request));
        }

        public virtual JsonResult tEvento_Create([DataSourceRequest] DataSourceRequest request, tEventoViewModel tEvento)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty( tEvento.Title))
                {
                     tEvento.Title = "";
                }

                var entity = tEvento.ToEntity();
                db.tEvento.Add(entity);
                db.SaveChanges();
                tEvento.lEvento_id = entity.lEvento_id;
            }

            return Json(new[] { tEvento }.ToDataSourceResult(request, ModelState));
        }
        public virtual JsonResult tEvento_Update([DataSourceRequest] DataSourceRequest request, tEventoViewModel tEvento)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty( tEvento.Title))
                {
                     tEvento.Title = "";
                }

                var entity = tEvento.ToEntity();
                db.tEvento.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new[] { tEvento }.ToDataSourceResult(request, ModelState));
        }
        public virtual JsonResult tEvento_Destroy([DataSourceRequest] DataSourceRequest request, tEventoViewModel tEvento)
        {
            if (ModelState.IsValid)
            {
                var entity = tEvento.ToEntity();
                db.tEvento.Attach(entity);
                db.tEvento.Remove(entity);
                db.SaveChanges();
            }

            return Json(new[] { tEvento }.ToDataSourceResult(request, ModelState));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}