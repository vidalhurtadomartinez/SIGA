using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public class gatbl_ActaSorteoEG : BaseModelo
    {
        public gatbl_ActaSorteoEG()
        {
            sGestion_desc = "2016";
            sPeriodo_desc = "2016-1";
            iNumeracion_num = 0;
            dtSorteo_dt = DateTime.Today;
            dtHora_dt = DateTime.Now;
            sBolos_desc = string.Empty;
            sArea_desc = string.Empty;
            dtHoraFinalizacion_dt = DateTime.Now;
            sLugar_desc = string.Empty;
            sObs_desc = string.Empty;
        }


        [Key]
        public int iActaSorteoEG_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener {1} caracteres como máximo.")]
        [Display(Name = "Gestión")]
        public string sGestion_desc { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener {1} caracteres como máximo.")]
        [Display(Name = "Periódo")]
        public string sPeriodo_desc { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Numeración")]
        public int iNumeracion_num { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha Sorteo")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dtSorteo_dt { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Hora sorteo")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime dtHora_dt { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(250, ErrorMessage = "El campo {0} debe tener {1} caracteres como máximo.")]
        [Display(Name = "Area a defender")]
        public string sBolos_desc { get; set; } // Es el área que el estudiante eligió defender en caso que sea examen de grado. Debe Guardar elnombre de la disciplina

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(250, ErrorMessage = "El campo {0} debe tener {1} caracteres como máximo.")]
        [Display(Name = "Áreas de Defensa")]
        public string sArea_desc { get; set; } 

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Hora Finalización")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime dtHoraFinalizacion_dt { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener {1} caracteres como máximo.")]
        [Display(Name = "Lugar de sorteo")]
        public string sLugar_desc { get; set; } // se refiere a la sala, se de guardar el nombre de la sala, no relacionar

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(500, ErrorMessage = "El campo {0} debe tener {1} caracteres como máximo.")]
        [Display(Name = "Caso de Estudio")]
        public string sObs_desc { get; set; }


        public int iPerfil_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string alm_registro { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string lPresidente_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string lSecretario_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string lEvaluador1I_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string lEvaluador2I_id { get; set; }

        public string lEvaluador1E_id { get; set; }

        public string lEvaluador2E_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string lRealizadoPor_id { get; set; }
        
        //propiedades de navegacion
        [ForeignKey("lPresidente_id")]
        public virtual agenda Presidente { get; set; }

        [ForeignKey("lSecretario_id")]
        public virtual agenda Secretario { get; set; }

        [ForeignKey("lEvaluador1I_id")]
        public virtual agenda Evaluador1I { get; set; }

        [ForeignKey("lEvaluador2I_id")]
        public virtual agenda Evaluador2I { get; set; }

        [ForeignKey("lEvaluador1E_id")]
        public virtual agenda Evaluador1E { get; set; }

        [ForeignKey("lEvaluador2E_id")]
        public virtual agenda Evaluador2E { get; set; }

        [ForeignKey("lRealizadoPor_id")]
        public virtual agenda RealizadoPor { get; set; }

        public virtual alumnos_agenda Alumno { get; set; }
        public virtual gatbl_Perfiles Perfil { get; set; }
    }
}