using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public class gatbl_EntregaTFG:BaseModelo
    {
        public gatbl_EntregaTFG()
        {
            sGestion_desc = "2016";
            sPeriodo_desc = "2016-1";
            sEntrega_fl = ParEstadoEntregaTFG.PRIM_RECEPCION;
            dtEntrega_dt = DateTime.Today;
            sNumEjemplar = ParNumeroEjemplar.PRIMERO;
        }

        [Key]
        public int iEntrega_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(50,ErrorMessage ="La longitud maximas del campo {0} es {1} caracteres.")]
        [Display(Name ="Gestión")]
        public string sGestion_desc { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(50, ErrorMessage = "La longitud maximas del campo {0} es {1} caracteres.")]
        [Display(Name ="Periódo")]
        public string sPeriodo_desc { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name ="Estado recepcion")]
        public ParEstadoEntregaTFG sEntrega_fl { get; set; }//01 = PRIMERA ENTREGA; 02 = PRIMERA CORRECCIÓN; 03 = SEGUNDA CORRECCIÓN; 04 = ENTREGA FINAL

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name ="Fecha recepción")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dtEntrega_dt { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Numero de Ejemplar")]
        public ParNumeroEjemplar sNumEjemplar { get; set; }//01 = PRIMER EJEMPLAR; 02 = SEGUNDO EJEMPLAR;


        public int iPerfil_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string lEstudiante_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string lRecepciona_id { get; set; }

        //propiedades de navegacion
        [ForeignKey("lEstudiante_id")]
        public virtual alumnos_agenda Alumno { get; set; }

        [ForeignKey("lRecepciona_id")]
        public virtual agenda Persona { get; set; }
        public virtual gatbl_Perfiles Perfil { get; set; }
        public virtual IEnumerable<gatbl_EntregaTribunales> EntregasTribunales { get; set; }

    }
}