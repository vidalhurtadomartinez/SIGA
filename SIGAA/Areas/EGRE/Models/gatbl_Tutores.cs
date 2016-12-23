using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public class gatbl_Tutores: BaseModelo
    {
        public gatbl_Tutores() {
            TipoTutor = ParTipoTutor.INTERNO;
            sNombre_desc = string.Empty;
            sDireccion_desc = string.Empty;
            sTelefonos_desc = string.Empty;
            sObs_fl = ParEstadoObservacionTutor.RECOMENDABLE;
            sObservacion_desc = string.Empty;
            EspecialidadTutor = ParEspecialidadTutor.MAESTRIA;
        }

        [Key]
        public int iTutuor_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Tipo Tutor")]
        public ParTipoTutor TipoTutor { get; set; } // interno; externo

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(100, ErrorMessage = "El campo debe tener entre {2} y {1} caracteres.", MinimumLength = 3)]
        [Display(Name = "Nombre Tutor")]
        public string sNombre_desc { get; set; }

        [StringLength(100, ErrorMessage = "el campo {0} de tener entre {2} y {1} caracteres.", MinimumLength = 3)]
        [Display(Name ="Dirección")]
        [DataType(DataType.MultilineText)]
        public string sDireccion_desc { get; set; }

        [StringLength(30,ErrorMessage ="El campo {0} debe tener entre {2} y {1} caracteres.",MinimumLength =3)]
        [Display(Name ="Teléfonos")]
        public string sTelefonos_desc { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Estado observación")]
        public ParEstadoObservacionTutor sObs_fl { get; set; }

        [StringLength(250, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 2)]
        [Display(Name = "Descripción de observación")]
        [DataType(DataType.MultilineText)]
        public string sObservacion_desc { get; set; }//describe el estado del campo sObs_fl

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Especialidad tutor")]
        public ParEspecialidadTutor EspecialidadTutor { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public int iProfesion_id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public int iCProfesional_id { get; set; }
        
        
        //propiedades de navegacion
        public virtual gatbl_Profesiones Profesion { get; set; }
        public virtual gatbl_CProfesionales ColegioProfesional { get; set; }
        public virtual IEnumerable<gatbl_Perfiles> Perfiles { get; set; }
        public virtual IEnumerable<gatbl_EntregaTribunales> EntregasTribunales { get; set; }


    }
}