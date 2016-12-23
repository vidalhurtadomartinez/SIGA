using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.OYM.Models
{
    public class tblSeguimientoDocumento
    {
        [Key]
        public int lSeguimientoDocumento_id { get; set; }

        [Display(Name = "Fecha de respuesta")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public DateTime? dtFechaRespuesta_dt { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Comentarios", Description = "Comentarios", Prompt = "Comentarios")]
        //[StringLength(100)]
        [DataType(DataType.MultilineText)]
        public string sComentario { get; set; }

        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Adjunto", Description = "Adjunto", Prompt = "Adjunto")]
        [StringLength(300)]
        public string sPathDocumento { get; set; }

        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Formulario proceso")]
        public int lDocumentoProceso_id { get; set; }

        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Nivel Categoria")]
        public int lCategoria_id { get; set; }

        [Display(Name = "Aceptado/Rechazado")]
        public string OrigenRespuesta { get; set; }

        [Display(Name = "Usuario")]
        public int? lUsuario_id { get; set; }


        [ForeignKey(name: "lCategoria_id")]
        public virtual tblCategoria tblCategoria { get; set; }
    }
}