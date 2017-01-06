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
    public class gatbl_AnalisisPreConvalidaciones
    {
        [Key]
        public int lAnalisisPreConvalidacion_id { get; set; }

        [Display(Name = "Solicitud de Convalidacion", Description = "Solicitud de Convalidacion")]
        [Required]
        public int lPConvalidacion_id { get; set; }

        //[Display(Name = "Universidad Origen", Description = "Universidad Origen")]
        //[Required]
        //public int lUniversidad_origen { get; set; }

        //[Display(Name = "Universidad Destino", Description = "Universidad Destino")]
        //[Required]
        //public int lUniversidad_destino { get; set; }

        //[Display(Name = "Carrera Origen", Description = "Carrera Origen")]
        //[Required]
        //public int lCarrera_Origen { get; set; }

        //[Display(Name = "Carrera Destino", Description = "Carrera Destino")]
        //[Required]
        //public int lCarrera_destino { get; set; }
        
        //[Display(Name = "Pensum Destino", Description = "Pensum Destino")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        //[StringLength(50)]
        //public string sPensumDestino_desc { get; set; }

        [Display(Name = "Responsable", Description = "Responsable")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(10)]
        public string lResponsable_id { get; set; }

        [Display(Name = "Version", Description = "Version")]
        [Required]
        public int sVersion_nro { get; set; }

        [Display(Name = "Fecha de Pre-Analisis", Description = "Fecha de Pre-Analisis")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        //[DisplayFormat(DataFormatString = "{0:d}")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime dtAnalisisConvalidacion_dt { get; set; }
        
        [Display(Name = "Observaciones", Description = "Observaciones")]
        [StringLength(250)]
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


        [ForeignKey(name: "lPConvalidacion_id")]
        public virtual gatbl_PConvalidaciones gatbl_PConvalidaciones { get; set; }
        
        //[ForeignKey(name: "lUniversidad_origen")]
        //public virtual gatbl_Universidades gatbl_Universidades_Origen { get; set; }

        //[ForeignKey(name: "lUniversidad_destino")]
        //public virtual gatbl_Universidades gatbl_Universidades_Destino { get; set; }

        //[ForeignKey(name: "lCarrera_Origen")]
        //public virtual gatbl_Carreras gatbl_Carreras_Origen { get; set; }

        //[ForeignKey(name: "lCarrera_destino")]
        //public virtual gatbl_Carreras gatbl_Carreras_Destino { get; set; }

        [ForeignKey(name: "lResponsable_id")]
        public virtual agenda Responsables { get; set; }

        public virtual ICollection<gatbl_DAnalisisPreConvalidaciones> gatbl_DAnalisisPreConvalidaciones { get; set; }        
        public virtual ICollection<gatbl_DetAnalisisPreConvalidaciones> gatbl_DetAnalisisPreConvalidaciones { get; set; }
        public virtual ICollection<gatbl_AnalisisConvalidaciones> gatbl_AnalisisConvalidaciones { get; set; }
    }
}