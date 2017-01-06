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
    public class gatbl_DPConvalidaciones
    {
        [Key]
        public int lDPConvalicacion_id { get; set; }

        [Required]
        public int lPConvalidacion_id { get; set; }        

        [Display(Name = "Tipo de documento", Description = "Tipo de documento")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]        
        public int lTipoDocumentoSolicitud_id { get; set; }

        [Display(Name = "Tipo presentacion", Description = "Tipo presentacion")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(2)]
        public string sTipoPresentacion_fl { get; set; }

        [Display(Name = "Cant. Dejada", Description = "Cant. Dejada")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [DataType("integer")]
        public int lCantidad_nro { get; set; }
        
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

        public virtual gatbl_PConvalidaciones gatbl_PConvalidaciones { get; set; }        
        
        [ForeignKey("lTipoDocumentoSolicitud_id")]
        public virtual TipoDocumentoSolicitud TipoDocumentoSolicitud { get; set; }

        [ForeignKey("sTipoPresentacion_fl")]
        public virtual TipoPresentacionDocumento TipoPresentacionDocumento { get; set; }

    }
}