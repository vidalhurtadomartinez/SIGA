using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public class gatbl_EntregaAlEst:BaseModelo
    {
        public gatbl_EntregaAlEst() {
            sGestion_desc = "2016";
            sPeriodo_desc = "2016-1";
            sEntregaAlEst_fl = ParEstadoEntregaAlEst.PRIM_ENTR_CON_OBS;
            dtEntregaAlEst_dt = DateTime.Today;
        }

        [Key]
        public int iEntregaAlEst_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener {1} caracteres como máximo.")]
        [Display(Name = "Gestión")]
        public string sGestion_desc { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener {1} caracteres como máximo.")]
        [Display(Name = "Periodo")]
        public string sPeriodo_desc { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Estado entrega")]
        public ParEstadoEntregaAlEst sEntregaAlEst_fl { get; set; }  //01 = PRIMERA ENTREGA; 02 = SEGUNDA ENTREGA

        [Required(ErrorMessage ="El campo {0} es requerido.")]
        [Display(Name ="Fecha entrega")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime dtEntregaAlEst_dt { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public int iRecepcionTFG_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string lEstudiante_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string lEntregadoPor_id { get; set; }

        //propiedades de navegacion
        [ForeignKey("lEstudiante_id")]
        public virtual alumnos_agenda Alumno { get; set; }

        [ForeignKey("lEntregadoPor_id")]
        public virtual agenda Persona { get; set; }
        public virtual gatbl_RecepcionesTFG RecepcionTFG { get; set; }


    }
}