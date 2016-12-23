using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.OYM.Models
{
    public class tblProceso
    {
        [Key]
        public int lProceso_id { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Nombre", Description = "Nombre", Prompt = "Nombre")]
        [StringLength(100)]
        public string sNombre { get; set; }

        [Display(Name = "Descripcion", Description = "Descripcion", Prompt = "Descripcion")]
        [StringLength(400)]
        [DataType(DataType.MultilineText)]
        public string sDescripcion { get; set; }

        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Tipo proceso")]
        public int? lTipoProceso_id { get; set; }

        [Display(Name = "Fecha registro")]
        //[DisplayFormat(DataFormatString = "{0:d}")]

        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime dtFechaRegistro_dt { get; set; }

        [Display(Name = "Activo")]
        [DefaultValue(true)]
        public bool bActivo { get; set; }

        [Display(Name = "Usuario")]
        public int? lUsuario_id { get; set; }


        [ForeignKey(name: "lTipoProceso_id")]
        public virtual TipoProceso TipoProceso { get; set; }

        public virtual ICollection<tblProcesoDetalle> tblProcesoDetalle { get; set; }
        public virtual ICollection<tblProcesoDetalleCategoria> tblProcesoDetalleCategoria { get; set; }
        public virtual ICollection<tblDocumentoProceso> tblDocumentoProceso { get; set; }
        public virtual ICollection<tblFormularioProceso> tblFormularioProceso { get; set; }
        //public virtual ICollection<tblDocumento> tblDocumento { get; set; }
    }
}