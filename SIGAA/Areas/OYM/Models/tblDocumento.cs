using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.OYM.Models
{
    public class tblDocumento
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Origen")]
        public int lOrigenDocumento_id { get; set; }

        [Display(Name = "Fecha Creacion")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public DateTime dtFechaCreacion_dt { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Tipo Documento")]
        public int lTipoDocumento_id { get; set; }

        [Display(Name = "Titulo")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(255, MinimumLength = 3)]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Tipo Proceso")]
        public int lTipoProceso_id { get; set; }

        [Display(Name = "Correlativo")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(50, MinimumLength = 3)]
        public string sCorrelativo { get; set; }

        [Display(Name = "Codigo")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(50, MinimumLength = 3)]
        public string sCodigo { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Version")]
        public int lVersion { get; set; }

        [Display(Name = "Valido desde")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        //[DisplayFormat(DataFormatString = "{0:d}")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? dtValidoDesde_dt { get; set; }

        [Display(Name = "Ultima Actualizacion")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        //[DisplayFormat(DataFormatString = "{0:d}")]
        
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime dtUltimaActualizacion_dt { get; set; }

        [Display(Name = "Nombre archivo")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(255)]
        public string NombreArchivo { get; set; }

        [Display(Name = "Ubicacion del archivo")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(255)]
        public string UbicacionArchivo { get; set; }
        
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Formato plantilla")]
        public int lPlantilla_id { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Directorio")]
        public int lDirectorio_id { get; set; }

        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        //[Display(Name = "Flujo o Proceso")]
        //public int lProceso_id { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Control de calidad")]
        public int lControlCalidad_id { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Elaborado Por")]
        public int lElaboradoPor_id { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Revisado Por")]
        public int lRevisadoPor_id { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Aprobado Por")]
        public int lAprobadoPor_id { get; set; }

        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Comentarios", Description = "Comentarios", Prompt = "Comentarios")]
        [StringLength(400)]
        [DataType(DataType.MultilineText)]
        public string sComentarios { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Estado documento")]
        public int lEstado_id { get; set; }

        [Display(Name = "ISO")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(50, MinimumLength = 3)]
        public string sISO { get; set; }

        [Display(Name = "Usuario")]
        public int? lUsuario_id { get; set; }



        [ForeignKey(name: "lOrigenDocumento_id")]
        public virtual OrigenDocumento OrigenDocumento { get; set; }

        [ForeignKey(name: "lTipoDocumento_id")]
        public virtual tblTipoDocumento tblTipoDocumento { get; set; }

        [ForeignKey(name: "lTipoProceso_id")]
        public virtual tblTipoProceso tblTipoProceso { get; set; }

        [ForeignKey(name: "lPlantilla_id")]
        public virtual tblPlantilla tblPlantilla { get; set; }

        [ForeignKey(name: "lDirectorio_id")]
        public virtual tblDirectorio tblDirectorio { get; set; }

        [ForeignKey(name: "lEstado_id")]
        public virtual EstadoDocumento EstadoDocumento { get; set; }

        //[ForeignKey(name: "lProceso_id")]
        //public virtual tblProceso tblProceso { get; set; }

        [ForeignKey(name: "lControlCalidad_id")]
        public virtual vVistaPersonal ControlCalidad { get; set; }

        [ForeignKey(name: "lElaboradoPor_id")]
        public virtual vVistaPersonal ElaboradoPor { get; set; }

        [ForeignKey(name: "lRevisadoPor_id")]
        public virtual vVistaPersonal RevisadoPor { get; set; }

        [ForeignKey(name: "lAprobadoPor_id")]
        public virtual vVistaPersonal AprobadoPor { get; set; }


        public virtual ICollection<tblFormulario> tblFormularios { get; set; }
        public virtual ICollection<tblDocumentoRelacionado> tblDocumentoRelacionados { get; set; }
        public virtual ICollection<tblDocumentoAprobacion> tblDocumentoAprobacion { get; set; }
        public virtual ICollection<tblDocumentoProceso> tblDocumentoProceso { get; set; }
    }
}