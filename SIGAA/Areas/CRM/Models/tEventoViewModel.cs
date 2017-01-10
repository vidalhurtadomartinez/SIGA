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

namespace SIGAA.Areas.CRM.Models
{
    public class tEventoViewModel: ISchedulerEvent
    {
        public tEventoViewModel()
        {
        }

        public tEventoViewModel(tEvento tEvento)
        {
                lEvento_id = tEvento.lEvento_id;
                Title = tEvento.Titulo;
                Start = DateTime.SpecifyKind(tEvento.dtDesde_dt, DateTimeKind.Utc);
                End = DateTime.SpecifyKind(tEvento.dtHasta_dt, DateTimeKind.Utc);
                StartTimezone = tEvento.ZonaDesde;
                EndTimezone = tEvento.ZonaHasta;
                Description = tEvento.Descripcion;
                IsAllDay = tEvento.TodoElDia;
                RecurrenceRule = tEvento.RecurrenceRule;
                RecurrenceException = tEvento.RecurrenceException;
                RecurrenceID = tEvento.EventRecurrenceID;
        }

        public int lEvento_id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        private DateTime start;
        public DateTime Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value.ToUniversalTime();
            }
        }

        private DateTime end;
        public DateTime End
        {
            get
            {
                return end;
            }
            set
            {
                end = value.ToUniversalTime();
            }
        }

        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }

        public string RecurrenceRule { get; set; }
        public int RecurrenceID { get; set; }
        public string RecurrenceException { get; set; }
        public bool IsAllDay { get; set; }


        public tEvento ToEntity()
        {
            return new tEvento
            {
                lEvento_id = lEvento_id,
                Titulo = Title,
                dtDesde_dt = Start,
                dtHasta_dt = End,
                ZonaDesde = StartTimezone,
                ZonaHasta = EndTimezone,
                Descripcion = Description,
                TodoElDia = IsAllDay,
                RecurrenceRule = RecurrenceRule,
                RecurrenceException = RecurrenceException,
                EventRecurrenceID = RecurrenceID
            };
        }
    }
}
