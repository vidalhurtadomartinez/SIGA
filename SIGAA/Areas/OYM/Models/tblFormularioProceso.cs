using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.OYM.Models
{
    public class tblFormularioProceso
    {
        [Key]
        public int lFormularioProceso_id { get; set; }

        [Display(Name = "Fecha de registro")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public DateTime? dtFechaRegistro_dt { get; set; }

        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Comentarios", Description = "Comentarios", Prompt = "Comentarios")]
        [StringLength(400)]
        [DataType(DataType.MultilineText)]
        public string sComentarios { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Formulario")]
        public int lFormulario_id { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Version formulario")]
        public int lFormulario_version { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Proceso")]
        public int lProceso_id { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Nivel Categoria")]
        public int lCategoria_id { get; set; }

        [Display(Name = "Iniciar proceso")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public DateTime? dtFechaInicio_dt { get; set; }

        [Display(Name = "Usuario")]
        public int? lUsuario_id { get; set; }



        [ForeignKey(name: "lFormulario_id")]
        public virtual tblFormulario tblFormulario { get; set; }

        [ForeignKey(name: "lProceso_id")]
        public virtual tblProceso tblProceso { get; set; }

        [ForeignKey(name: "lCategoria_id")]
        public virtual tblCategoria tblCategoria { get; set; }

        //public virtual ICollection<tblCargoAgenda> tblCargoAgenda { get; set; }
    }
}