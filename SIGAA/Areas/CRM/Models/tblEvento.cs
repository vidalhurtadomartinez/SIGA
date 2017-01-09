using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.CRM.Models
{
    public class tblEvento
    {
        [Key]
        public int lEvento_id { get; set; }

        [Display(Name = "Fecha Registro")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public DateTime dtFechaRegistro_dt { get; set; }

        [Display(Name = "Comentarios", Description = "Comentarios", Prompt = "Comentarios")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }

        [Display(Name = "Repetir")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public DateTime? dtFechaProgramada_dt { get; set; }

        [Display(Name = "Tipo de Evento", Description = "Tipo de Evento", Prompt = "Tipo de Evento")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int lTipoEvento_id { get; set; }

        [Display(Name = "Notificacion", Description = "Notificacion", Prompt = "Notificacion")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int? lNotificacion_id { get; set; }

        [Display(Name = "Tipo de Respuesta", Description = "Tipo de Respuesta", Prompt = "Tipo de Respuesta")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int? lTipoRespuesta_id { get; set; }

        [Display(Name = "Recordatorio", Description = "Recordatorio", Prompt = "Recordatorio")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int? lRecordatorio_id { get; set; }

        [Display(Name = "Cliente", Description = "Cliente", Prompt = "Cliente")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int lCliente_id { get; set; }


        [ForeignKey(name: "lTipoEvento_id")]
        public virtual tblTipoEvento tblTipoEvento { get; set; }

        [ForeignKey(name: "lTipoRespuesta_id")]
        public virtual tblTipoRespuesta tblTipoRespuesta { get; set; }

        [ForeignKey(name: "lNotificacion_id")]
        public virtual tblNotificacion tblNotificacion { get; set; }

        [ForeignKey(name: "lRecordatorio_id")]
        public virtual tEvento tEvento { get; set; }

        [ForeignKey(name: "lCliente_id")]
        public virtual tblCliente tblCliente { get; set; }
    }
}