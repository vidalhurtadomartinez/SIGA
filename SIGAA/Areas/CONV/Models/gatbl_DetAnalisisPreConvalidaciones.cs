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
    public class gatbl_DetAnalisisPreConvalidaciones
    {
        [Key]
        public int lDetAnalisisPreConvalidacion_id { get; set; }

        [Display(Name = "Analisis Pre Convalidacion", Description = "Analisis Pre Convalidacion")]
        [Required]
        public int lAnalisisPreConvalidacion_id { get; set; }        

        [Display(Name = "Materias Origen", Description = "Materias Origen")]
        public string sMateriaOrigen_id { get; set; }

        [Display(Name = "Materias Destino", Description = "Materias Destino")]
        public string sMateriaDestino_id { get; set; }

        [Display(Name = "Materias Origen", Description = "Materias Origen")]
        public string sMateriaOrigen { get; set; }

        [Display(Name = "Materias Destino", Description = "Materias Destino")]
        public string sMateriaDestino { get; set; }

        [Display(Name = "Observaciones", Description = "Observaciones")]
        [DataType(DataType.MultilineText)]
        public string sObservaciones { get; set; }

        [Display(Name = "Estado", Description = "Estado")]
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


        [ForeignKey(name: "lAnalisisPreConvalidacion_id")]
        public virtual gatbl_AnalisisPreConvalidaciones gatbl_AnalisisPreConvalidaciones { get; set; }

        //[ForeignKey(name: "lMateriaOrigen_id")]
        //public virtual gatbl_ProgramasAnaliticos gatbl_OrigenProgramasAnaliticos { get; set; }

        //[ForeignKey(name: "lMateriaDestino_id")]
        //public virtual gatbl_ProgramasAnaliticos gatbl_DestinoProgramasAnaliticos { get; set; }


        public virtual ICollection<gatbl_AnalisisConvalidacionesUnidad> gatbl_AnalisisConvalidacionesUnidad { get; set; }
        public virtual ICollection<gatbl_AnalisisPreConvalidacionesMateria> gatbl_AnalisisPreConvalidacionesMateria { get; set; }
    }
}