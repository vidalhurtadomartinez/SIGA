using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.OYM.Models
{
    public class tblFormularioRelacionado
    {
        [Key]
        public int Id { get; set; }

        //[Required]
        [StringLength(255, MinimumLength = 3)]
        [Display(Name = "Nombre")]
        public string NombreArchivo { get; set; }

        [Display(Name = "Ubicacion")]
        public string Ubicacion { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Relacion")]
        public string Relacion { get; set; }

        public int FormularioID { get; set; }

        [NotMapped]
        public virtual HttpPostedFileBase DocumentFile { get; set; }
        public virtual tblFormulario tblFormulario { get; set; }
    }

    //[NotMapped]
    //public class SliderImage : tblDocumentoRelacionado
    //{
    //    public HttpPostedFileBase ImageFile { get; set; }
    //}
}