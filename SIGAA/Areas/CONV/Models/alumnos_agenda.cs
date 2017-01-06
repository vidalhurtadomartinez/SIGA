using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.CONV.Models
{
    public class alumnos_agenda
    {
        [Key]
        public string alm_registro { get; set; }
        public string agd_codigo { get; set; }
        public DateTime alm_fechainsc { get; set; }

        [Display(Name = "Carrera", Description = "Carrera", Prompt = "Carrera")]
        public string crr_codigo { get; set; }
        public string pns_codigo { get; set; }
        public char mdl_codigo { get; set; }
        public char mdu_codigo { get; set; }
        public string tur_codigo { get; set; }
        public bool tur_exclusivo { get; set; }
        public string sem_codigo { get; set; }
        public string ctr_codigo { get; set; }
        public string agente_codigo { get; set; }
        public string tstd_codigo { get; set; }
        public string usr_codigo { get; set; }
        public DateTime trn_fecha { get; set; }
        public bool doc_confirmados { get; set; }
        public DateTime doc_fechaconfirmado { get; set; }
        public string doc_agdcodigo { get; set; }
        public bool alm_cambiocarrera { get; set; }

        public virtual carreras carreras { get; set; }


        public virtual ICollection<agenda> agenda { get; set; }
        public virtual ICollection<gatbl_PConvalidaciones> gatbl_PConvalidaciones { get; set; }
        public virtual ICollection<gatbl_ConvalidacionesExternasPA> gatbl_ConvalidacionesExternasPA { get; set; }
    }
}