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
    public class gatbl_PConvalidaciones
    {
        [Key]
        public int lPConvalidacion_id { get; set; }

        [Display(Name = "Postulante", Description = "Postulante")]
        [Required]
        public int lPostulante_id { get; set; }

        [Display(Name = "Nro. Solicitud", Description = "Postulante")]
        [Required]
        public int lNro_solucitud { get; set; }

        [Display(Name = "Universidad Origen", Description = "Universidad Origen")]
        [Required]
        public int lUniversidadOrigen_id { get; set; }

        [Display(Name = "Carrera Origen", Description = "Carrera Origen")]
        [Required]
        public int lCarreraOrigen_id { get; set; }

        [Display(Name = "Universidad Destino", Description = "Universidad Destino")]
        [Required]
        public int lUniversidadDestino_id { get; set; }

        [Display(Name = "Carrera Destino", Description = "Carrera Destino")]
        [Required]
        public int lCarreraDestino_id { get; set; }

        [Display(Name = "Pensum destino", Description = "Pensum destino")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int lPensum_id { get; set; }

        [Display(Name = "Fecha de Postulacion", Description = "Fecha de Postulacion")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        //[DisplayFormat(DataFormatString = "{0:d}")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime dtPostulacion_dt { get; set; }

        [Display(Name = "Responsable", Description = "Responsable")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(100)]
        public string lResponsable_id { get; set; }

        [Display(Name = "Observaciones", Description = "Observaciones")]
        [StringLength(500)]
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
        [StringLength(50, ErrorMessage = "El campo {0} debe tener {1} caracteres.", MinimumLength = 8)]
        public string sCreated_by { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Display(Name = "Concurrencia")]
        public int iConcurrencia_id { get; set; }


        [ForeignKey(name: "lUniversidadOrigen_id")]
        public virtual gatbl_Universidades gatbl_UniversidadesOrigen { get; set; }

        [ForeignKey(name: "lUniversidadDestino_id")]
        public virtual gatbl_Universidades gatbl_UniversidadesDestino { get; set; }

        [ForeignKey(name: "lCarreraOrigen_id")]
        public virtual gatbl_Carreras gatbl_CarrerasOrigen { get; set; }

        [ForeignKey(name: "lCarreraDestino_id")]
        public virtual gatbl_Carreras gatbl_CarrerasDestino { get; set; }

        [ForeignKey(name: "lPensum_id")]
        public virtual Pensum Pensum { get; set; }

        [ForeignKey(name: "lPostulante_id")]
        public virtual gatbl_Postulantes gatbl_Postulantes { get; set; }        

        [ForeignKey(name: "lResponsable_id")]
        public virtual agenda Responsables { get; set; }


        public virtual ICollection<gatbl_DPConvalidaciones> gatbl_DPConvalidaciones { get; set; }
        public virtual ICollection<gatbl_CertificadosMateria> gatbl_CertificadosMateria { get; set; }
        public virtual ICollection<gatbl_AnalisisPreConvalidaciones> gatbl_AnalisisPreConvalidaciones { get; set; }        
    }
}