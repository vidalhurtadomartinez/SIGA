using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public class gatbl_ComunicacionExtColProf:BaseModelo
    {

        public gatbl_ComunicacionExtColProf()
        {
            sGestion_desc = "2016";
            sPeriodo_desc = "2016-1";
            iNumeracion_num = 0;
           // sTipoCom_fl = ParTipoComunicacionExt.TRIBUNAL_EXAMEN_DE_GRADO;
            dtComunicacionExt_dt = DateTime.Today;
            dtSorteo_dt = DateTime.Now;
            dtHora_dt = DateTime.Now;
            dtDefensa_dt = DateTime.Today;
            dtHoraDefensa_dt = DateTime.Now;
        }
        [Key]
        public int iComExtColProfesional_id { get; set; }

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
        [Display(Name = "Tipo de comunicación")]
        public ParTipoComunicacionExt sTipoCom_fl { get; set; } //01 = TRIBUNALES PARA EXAMEND DE GRAD; 02 = TRIBUNALES PARA TFG; 03 = TRIBUNALES PARA EXCELENCIA

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha comunicación")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dtComunicacionExt_dt { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha Sorteo")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dtSorteo_dt { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Hora Sorteo")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime dtHora_dt { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha defensa")]
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

        public int iPerfil_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string lEstudiante_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string lEntregadoPor_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public int iPresidenteColProfesional_id { get; set; }


        //propiedades de navegacion
        [ForeignKey("lEstudiante_id")]
        public virtual alumnos_agenda Alumno { get; set; }

        [ForeignKey("lEntregadoPor_id")]
        public virtual agenda Persona { get; set; }

        public virtual gatbl_Perfiles Perfil { get; set; }

        public virtual gatbl_PresidenteColegioProfesional PresidenteColegioProfesional { get; set; }



    }
}