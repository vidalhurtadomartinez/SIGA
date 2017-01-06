using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGAA.Areas.CONV.Models
{
    public class gatbl_ConvalidacionesExternasPA
    {
        [Key]
        public int lConvalidacionExternaPA_id { get; set; }

        [Display(Name = "Estudiante", Description = "Estudiante")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(10)]
        public string lEstudiante_id { get; set; }

        [Display(Name = "Facultad", Description = "Facultad")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(2)]
        public string sca_codigo { get; set; }

        [Display(Name = "Carrera", Description = "Carrera")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(3)]
        public string crr_codigo { get; set; }

        [Display(Name = "Fecha de Convalidacion", Description = "Fecha de Convalidacion")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        //[DisplayFormat(DataFormatString = "{0:d}")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime dtConvalidacionExterna_dt { get; set; }

        [Display(Name = "Formulario", Description = "Formulario")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(50)]
        public string sFormulario_nro { get; set; }

        [Display(Name = "Nro. de Informe", Description = "Nro. de Informe")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(50)]
        public string sInforme_nro { get; set; }

        [Display(Name = "Responsable", Description = "Responsable")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(10)]
        public string lResponsable_id { get; set; }

        [Display(Name = "Observaciones", Description = "Observaciones")]
        [StringLength(200)]
        [DataType(DataType.MultilineText)]
        public string sObs_desc { get; set; }

        [HiddenInput(DisplayValue = false)]
        [DefaultValue("1")]
        [StringLength(2)]
        public string iEstado_fl { get; set; }

        [HiddenInput(DisplayValue = false)]
        [DefaultValue("1")]
        [StringLength(2)]
        public string iEliminado_fl { get; set; }

        [HiddenInput(DisplayValue = false)]
        [DefaultValue("")]
        public string sCreated_by { get; set; }

        [HiddenInput(DisplayValue = false)]
        [DefaultValue(1)]
        public int iConcurrencia_id { get; set; }


        [ForeignKey(name: "lEstudiante_id")]
        public virtual agenda agenda { get; set; }

        [ForeignKey(name: "sca_codigo")]
        public virtual secciones_academicas secciones_academicas { get; set; }

        [ForeignKey(name: "crr_codigo")]
        public virtual carreras carreras { get; set; }

        [ForeignKey(name: "lResponsable_id")]
        public virtual agenda Responsable { get; set; }

        public virtual alumnos_agenda alumnos_agenda { get; set; }

        public virtual ICollection<gatbl_DConvalidacionesExternasPA> gatbl_DConvalidacionesExternasPA { get; set; }
    }
}