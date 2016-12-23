using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public class gatbl_Perfiles : BaseModelo
    {
        public gatbl_Perfiles() {
            sGestion_desc = "2016";
            sPeriodo_desc = "2016-1";
            dtSolicitud_dt = DateTime.Today;
            sTitulo_tfg = string.Empty;
            sPerfil_fl = ParEstadoPerfil.ACEPTADO;
            dtRevision_dt = DateTime.Today;
            dtEntregado_dt = DateTime.Today;
        }

        [Key]
        public int iPerfil_id { get; set; }

        [Required(ErrorMessage ="El campo {0} es requerido.")]
        [StringLength(50,ErrorMessage ="El campo {0} debe tener {1} caracteres como máximo.")]
        [Display(Name ="Gestión")]
        public string sGestion_desc { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener {1} caracteres como máximo.")]
        [Display(Name = "Periódo")]
        public string sPeriodo_desc { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha de Solicitud Form")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime dtSolicitud_dt { get; set; } 

        [StringLength(500, ErrorMessage = "El campo {0} debe tener {1} caracteres como máximo.")]
        [Display(Name = "Título de Perfíl")]
        [DataType(DataType.MultilineText)]
        public string sTitulo_tfg { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Estado Perfil")]
        public ParEstadoPerfil sPerfil_fl { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name ="Fecha revisión")]
        public DateTime dtRevision_dt { get; set; }


        [StringLength(200, ErrorMessage = "El campo {0} debe tener {1} caracteres como máximo.")]
        [Display(Name = "Observación revisión")]
        [DataType(DataType.MultilineText)]
        public string sObsRevision_desc { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha recepción")]
        public DateTime dtEntregado_dt { get; set; }

        public string sModalidad_fl { get; set; }
        public int? iTutuor_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string lEstudiante_id { get; set; }
        
        public string lEntregaForm_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string lRecepciona_id { get; set; }



        ////propiedades de navegacion

        [ForeignKey("lEntregaForm_id")]
        public virtual agenda EntregadoPor { get; set; }

        [ForeignKey("lRecepciona_id")]
        public virtual agenda RecepcionadoPor { get; set; }

        [ForeignKey("lEstudiante_id")]
        public virtual alumnos_agenda Alumno { get; set; }
        public virtual gatbl_Tutores Tutor { get; set; }

        [ForeignKey("sModalidad_fl")]
        public virtual tipo_graduacion TipoGraduacion { get; set; }
        public virtual IEnumerable<gatbl_EntregaTFG> EntregasTFG { get; set; }
        public virtual IEnumerable<gatbl_ComunicacionInt> ComunicacionesInternas { get; set; }
        public virtual IEnumerable<gatbl_ComunicacionEst> ComunicacionesEstudiante { get; set; }
        public virtual IEnumerable<gatbl_ActaSorteoEG> ActasSorteosEG { get; set; }
        public virtual IEnumerable<gatbl_ActaDefensaFinal> ActasDefensasFinales { get; set; }
        // public virtual IEnumerable<gatbl_ComunicacionExt> ComunicacionesExternas { get; set; }
        public virtual IEnumerable<gatbl_ComunicacionExtColProf> ComExternasColegiosProfesionales { get; set; }
        public virtual IEnumerable<gatbl_ComunicacionExtUnivPublica> ComExternasUniversidadesPublicas { get; set; }




    }
}