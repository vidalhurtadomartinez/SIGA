using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public class gatbl_EntregaTribunales : BaseModelo
    {
        public gatbl_EntregaTribunales() {
            sGestion_desc = "2016";
            sPeriodo_desc = "2016-1";
            sEntregaTribunal_fl = ParEstadoEntregaTribunales.PRIM_ENTREGA;
            dtEntregaTribunal_dt = DateTime.Today;
        }
        [Key]
        public int iETribunal_id { get; set; }

        [Required(ErrorMessage ="El campo {0} es requerido.")]
        [StringLength(50,ErrorMessage ="El campo {0} debe tener {1} caracteres como máximo")]
        [Display(Name ="Gestión")]
        public string sGestion_desc { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener {1} caracteres como máximo")]
        [Display(Name ="Periódo")]
        public string sPeriodo_desc { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name ="Estado de Entrega")]
        public ParEstadoEntregaTribunales sEntregaTribunal_fl { get; set; }//01 = PRIMERA ENTREGA; 02 = RESPUESTA A OBSERVACIONES No 1; 03 =  RESPUESTA A OBSERVACIONES No 2

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name="Fecha entrega")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime dtEntregaTribunal_dt { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public int iTutuor_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public int iEntrega_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string lEstudiante_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string lRecepciona_id { get; set; }

        //propiedades de navegacion
        [ForeignKey("lEstudiante_id")]
        public virtual alumnos_agenda Alumno { get; set; }

        [ForeignKey("lRecepciona_id")]
        public virtual agenda Persona { get; set; }
        public virtual gatbl_Tutores Tutor { get; set; }
        public virtual gatbl_EntregaTFG EntregaTFG { get; set; }
        public virtual IEnumerable<gatbl_RecepcionesTFG> RecepcionesTFG { get; set; }


    }
}