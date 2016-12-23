using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.OYM.Models
{
    public class tblCategoria
    {
        [Key]
        public int lCategoria_id { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Descripcion", Description = "Descripcion", Prompt = "Descripcion")]
        [StringLength(100)]
        public string sDescripcion { get; set; }

        [Display(Name = "Prioridad", Description = "Prioridad")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int lPrioridad { get; set; }

        [Display(Name = "Activo")]
        [DefaultValue(true)]
        public bool bActivo { get; set; }

        public virtual ICollection<tblProcesoDetalle> tblProcesoDetalle { get; set; }
        public virtual ICollection<tblSeguimientoDocumento> tblSeguimientoDocumento { get; set; }
        public virtual ICollection<tblDocumentoProceso> tblDocumentoProceso { get; set; }
        public virtual ICollection<tblProcesoDetalleCategoria> tblProcesoDetalleCategoria { get; set; }
    }
}