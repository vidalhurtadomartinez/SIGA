using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.OYM.Models
{
    public class tblProcesoDetalleCategoria
    {
        [Key]
        public int lProcesoDetalleCategoria_id { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Proceso")]
        public int lProceso_id { get; set; }        

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Categoria")]
        public int lCategoria_id { get; set; }        

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Rev. (# dias)")]
        public int lRevision1 { get; set; }

        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Rev. 2")]
        public int? lRevision2 { get; set; }

        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Rev. 3")]
        public int? lRevision3 { get; set; }


        [ForeignKey(name: "lProceso_id")]
        public virtual tblProceso tblProceso { get; set; }
        
        [ForeignKey(name: "lCategoria_id")]
        public virtual tblCategoria tblCategoria { get; set; }        
    }
}