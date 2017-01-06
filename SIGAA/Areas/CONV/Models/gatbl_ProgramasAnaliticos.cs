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
    public class gatbl_ProgramasAnaliticos
    {
        [Key]
        public int lProgramaAnalitico_id { get; set; }
        
        [Display(Name = "Fecha de Registro", Description = "Fecha de Registro")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        //[DisplayFormat(DataFormatString = "{0:d}")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime dtRegistro_dt { get; set; }
        public int lMateria_id { get; set; }

        [Display(Name = "Materia", Description = "Materia")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(100)]
        public string sMateria_desc { get; set; }

        [Display(Name = "Unidad de Negocio", Description = "Unidad de Negocio")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(2)]
        public string lUNegocio_id { get; set; }

        [Display(Name = "Universidad", Description = "Universidad")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int lUniversidad_id { get; set; }

        [Display(Name = "Facultad", Description = "Facultad")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int lFacultad_id { get; set; }

        [Display(Name = "Carrera", Description = "Carrera")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int lCarrera_id { get; set; }
        
        [Display(Name = "Institucion", Description = "Institucion")]       
        public int? lInstitucion_id { get; set; }

        [Display(Name = "Institucion", Description = "Institucion")]
        public string sInstitucion_desc { get; set; }

        [Display(Name = "Codigo", Description = "Codigo")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(50)]
        public string sCodigo_nro { get; set; }

        [Display(Name = "Sigla", Description = "Sigla")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(50)]
        public string sSigla_desc { get; set; }

        [Display(Name = "Hr. Practicas", Description = "Hr. Practicas")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int sHorasPracticas_nro { get; set; }

        [Display(Name = "Hr. Teoricas", Description = "Hr. Teoricas")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int sHorasTeoricas_nro { get; set; }

        [Display(Name = "Hr. Servicio Social", Description = "Hr. Servicio Social")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int sHorasSociales_nro { get; set; }

        [Display(Name = "Hr. Ayudantia", Description = "Hr. Ayudantia")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int sHorasAyudantia_nro { get; set; }

        [Display(Name = "Creditos", Description = "Creditos")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int sCreditos_nro { get; set; }

        [Display(Name = "Carga Horaria", Description = "Carga Horaria")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int sCarga_Horaria { get; set; }

        [Display(Name = "Responsable", Description = "Responsable")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(10)]
        public string lResponsable_id { get; set; }

        [Display(Name = "Version", Description = "Version")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int sVersion_nro { get; set; }

        [Display(Name = "Observaciones", Description = "Observaciones")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(300)]
        public string sObs_desc { get; set; }

        [Display(Name = "Copia De", Description = "Copia De")]
        public int? CopiaDe { get; set; }

        [Display(Name = "Pensum", Description = "Pensum")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int lPensum_id { get; set; }

        [Display(Name = "Tipo Carga Horaria", Description = "Tipo Carga Horaria")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int lTipoCargaHoraria_fl { get; set; }

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
        [StringLength(50, ErrorMessage = "El campo {0} debe tener {1} caracteres.", MinimumLength = 8)]
        public string sCreated_by { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Display(Name = "Concurrencia")]
        public int iConcurrencia_id { get; set; }

        public virtual UnidadNegocio UnidadNegocios { get; set; }

        [ForeignKey(name: "lPensum_id")]
        public virtual Pensum Pensum { get; set; }

        [ForeignKey(name: "lTipoCargaHoraria_fl")]
        public virtual TipoCargaHoraria TipoCargaHoraria { get; set; }
        public virtual gatbl_Universidades gatbl_Universidades { get; set; }
        public virtual gatbl_Facultades gatbl_Facultades { get; set; }
        public virtual gatbl_Carreras gatbl_Carreras { get; set; }

        [ForeignKey(name: "CopiaDe")]
        public virtual gatbl_ProgramasAnaliticos gatbl_ProgramasAnaliticos_Copia{ get; set; }

        [ForeignKey(name: "lResponsable_id")]
        public virtual agenda Responsables { get; set; }

        
        public virtual ICollection<gatbl_ProgramasAnaliticos> gatbl_ProgramasAnaliticos_Copias { get; set; }
        public virtual ICollection<gatbl_DProgramasAnaliticos> gatbl_DProgramasAnaliticos { get; set; }
        public virtual ICollection<gatbl_DetProgramasAnaliticos> gatbl_DetProgramasAnaliticos { get; set; }
        public virtual ICollection<gatbl_DAnalisisPreConvalidaciones> gatbl_DAnalisisPreConvalidaciones { get; set; }
        public virtual ICollection<gatbl_DAnalisisConvalidaciones> gatbl_DAnalisisConvalidaciones { get; set; }
        //public virtual ICollection<gatbl_DetAnalisisPreConvalidaciones> gatbl_DetAnalisisPreConvalidaciones { get; set; }
        public virtual ICollection<gatbl_AnalisisPreConvalidacionesMateria> gatbl_AnalisisPreConvalidacionesMateria { get; set; }
    }
}