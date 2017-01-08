using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public class gatbl_ComunicacionEst : BaseModelo
    {
        public gatbl_ComunicacionEst()
        {
            sGestion_desc = "2016";
            sPeriodo_desc = "2016-1";
            iNumeracion_num = 0;
            sComunicacionEst_fl = ParEstadoComunicacionEst.DEFENSA_FINAL;
            dtComunicacionEst_dt = DateTime.Today;
            sDescripcion_desc = string.Empty;
            sObs_desc = string.Empty;

            dtDProgramada_dt = DateTime.Today;
            dtHProgramada_dt = DateTime.Now;
            sLugarDefensa_desc = "";
            dtSorteoArea_dt = DateTime.Today;
            dtHoraSorteo_dt = DateTime.Now;
            sLugarSorteo_desc = "";
            dtHoraEntrega_dt = DateTime.Now;
            sLugarEntrega_desc = "";
        }
        [Key]        
        public int iComunicacionEst_id { get; set; }

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
        [Display(Name = "Tipo de comunicación")]
        public ParEstadoComunicacionEst sComunicacionEst_fl { get; set; } //01 = AUTORIZACION PARA HABILITACION; 02 =  PREDEFENSA;  03 = DEFENSA FINAL

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha defensa programada")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dtDProgramada_dt { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Hora defensa programada")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime dtHProgramada_dt { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener {1} caracteres como máximo.")]
        [Display(Name = "Lugar defensa progr")]
        public string sLugarDefensa_desc { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha sorteo de Área")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dtSorteoArea_dt { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Hora sorteo de Área")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime dtHoraSorteo_dt { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener {1} caracteres como máximo.")]
        [Display(Name = "Lugar sorteo de Área")]
        public string sLugarSorteo_desc { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Hora entrega Caso")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime dtHoraEntrega_dt { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener {1} caracteres como máximo.")]
        [Display(Name = "Lugar entrega caso")]
        public string sLugarEntrega_desc { get; set; }     

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha Comunicación")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dtComunicacionEst_dt { get; set; }

        [StringLength(500, ErrorMessage = "El campo {0} debe tener {1} caracteres como máximo.")]
        [Display(Name = "Descripción")]
        public string sDescripcion_desc { get; set; }

        [StringLength(250, ErrorMessage = "El campo {0} debe tener {1} caracteres como máximo.")]
        [Display(Name = "Observación")]
        public string sObs_desc { get; set; }


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
    }
}