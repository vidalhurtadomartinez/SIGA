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
    public class gatbl_UnidadesConvalidadas
    {
        [Key]
        public int lUnidadConvalidada_id { get; set; }

        [Display(Name = "Detalle Analisis", Description = "Detalle Analisis")]
        [Required]
        public int lDAnalisisConvalidacion_id { get; set; }

        [Display(Name = "Unidad", Description = "Unidad")]
        [Required]
        public int lUnidad_nro { get; set; }

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

        public virtual gatbl_DAnalisisConvalidaciones gatbl_DAnalisisConvalidaciones { get; set; }

        [ForeignKey(name: "lUnidad_nro")]
        public virtual gatbl_DProgramasAnaliticos gatbl_DProgramasAnaliticos { get; set; }
    }
}