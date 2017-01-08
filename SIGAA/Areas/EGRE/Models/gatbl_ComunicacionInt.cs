using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public class gatbl_ComunicacionInt : BaseModelo
    {
        public gatbl_ComunicacionInt()
        {
            sGestion_desc = "2016";
            sPeriodo_desc = "2016-1";
            iNumeracion_num = 0;
            sTipoCom_fl = ParTipoComunicacionInt.DESIG_TRIB_INT_REVISION;
            dtComunicacionInt_dt = DateTime.Today;
            sCopia5_nm = string.Empty;
            dtSorteo_dt = DateTime.Now;
            dtHoraEntregaCaso_dt = DateTime.Now;
            dtHora_dt = DateTime.Now;
            dtDefensa_dt = DateTime.Today;
            dtHoraDefensa_dt = DateTime.Now;
            sDescripcion_desc = string.Empty;
            sObs_desc = string.Empty;
        }
        [Key]
        public int iComunicacionInt_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener {1} caracteres como máximo.")]
        [Display(Name = "Gestión")]
        public string sGestion_desc { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener {1} caracteres como máximo.")]
        [Display(Name = "Periodo")]
        public string sPeriodo_desc { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Numeración")]
        public int iNumeracion_num { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Tipo de Comunicación")]
        public ParTipoComunicacionInt sTipoCom_fl { get; set; } //01 = SOLICITUD DE TRIBUNALES EXTERNOS; 02 = OTROS

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha Comunicación")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dtComunicacionInt_dt { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(100)]
        [Display(Name = "Copia persona 1")]
        public string sCopia1_nm { get; set; } //guarda el nombre a quien va dirigido

        [StringLength(100)]
        [Display(Name = "Copia persona 2")]
        public string sCopia2_nm { get; set; }//guarda el nombre a quien va dirigido

        [StringLength(100)]
        [Display(Name = "Copia persona 3")]
        public string sCopia3_nm { get; set; }//guarda el nombre a quien va dirigido

        [StringLength(100)]
        [Display(Name = "Copia persona 4")]
        public string sCopia4_nm { get; set; }//guarda el nombre a quien va dirigido

        [StringLength(100)]
        [Display(Name = "Copia persona 5")]
        public string sCopia5_nm { get; set; }//guarda el nombre a quien va dirigido

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha sorteo")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dtSorteo_dt { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Hora sorteo")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime dtHora_dt { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Hora entrega caso")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime dtHoraEntregaCaso_dt { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "fecha defensa")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dtDefensa_dt { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Hora defensa")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime dtHoraDefensa_dt { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener {1} caracteres como máximo.")]
        [Display(Name = "Lugar defensa")]
        public string sLugar_desc { get; set; } //// se refiere a la sala, se de guardar el nombre de la sala, no relacionar

        [StringLength(500, ErrorMessage = "El campo {0} debe tener {1} caracterres como máximo.")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripción")]
        public string sDescripcion_desc { get; set; }

        [StringLength(250, ErrorMessage = "El campo {0} debe tener {1} caracterres como máximo.")]
        [Display(Name = "Observación")]
        public string sObs_desc { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public int iAreaAdministrativa_id { get; set; }
        public int iPerfil_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string lEstudiante_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string lEntregadoPor_id { get; set; }

        //propiedades de navegacion
        [ForeignKey("lEstudiante_id")]
        public virtual alumnos_agenda Alumno { get; set; }

        [ForeignKey("lEntregadoPor_id")]
        public virtual agenda Persona { get; set; }

        public virtual gatbl_Perfiles Perfil { get; set; }
        public virtual gatbl_AreasAdministrativas AreaAdministrativa { get; set; }
    }
}