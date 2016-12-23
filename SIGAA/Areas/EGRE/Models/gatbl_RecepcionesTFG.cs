using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public class gatbl_RecepcionesTFG:BaseModelo
    {
        public gatbl_RecepcionesTFG() {
            sGestion_desc = "2016";
            sPeriodo_desc = "2016-1";
            sRecepcionTFG_fl = ParEstadoRecepcionesTFG.PRIM_CORRECCION;
            sEstadoEntrega_fl = ParEstadoEntregaRecepcionesTFG.CON_OBSERVACIONES;
            dtRecepcion_dt = DateTime.Today;
        }

        [Key]
        public int iRecepcionTFG_id { get; set; }

        [Required(ErrorMessage ="El campo {0} es requerido.")]
        [StringLength(50,ErrorMessage ="El campo {0} debe tener {1} caracteres com máximo.")]
        [Display(Name ="Gestión")]
        public string sGestion_desc { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener {1} caracteres com máximo.")]
        [Display(Name = "Periódo")]
        public string sPeriodo_desc { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Estado recepción")]
        public ParEstadoRecepcionesTFG sRecepcionTFG_fl { get; set; } //01 = PRIMERA ENTREGA; 02 = SEGUNDA ENTREGA; 03 = APROBADO

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Resultado de revisión")]
        public ParEstadoEntregaRecepcionesTFG sEstadoEntrega_fl { get; set; } //01 = APROBADA; 02 = RECHAZADA; 03 = CON OBSERVACIONES

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha recepción")]
        public DateTime dtRecepcion_dt { get; set; } //Fecha en que  se recepciona la entrega de revision hecha por el tribunal.

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public int iETribunal_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string lEstudiante_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string lRecepciona_id { get; set; }

        //propiedades de navegacion
        [ForeignKey("lEstudiante_id")]
        public virtual alumnos_agenda Alumno { get; set; }

        [ForeignKey("lRecepciona_id")]
        public virtual agenda Persona { get; set; }
        public virtual gatbl_EntregaTribunales EntregaTribunal { get; set; }
        public virtual IEnumerable<gatbl_DRecepcionesTFG> DRecepcionesTFG { get; set; }
        public virtual IEnumerable<gatbl_EntregaAlEst> EntregasAlEstudiante { get; set; }

    }
}