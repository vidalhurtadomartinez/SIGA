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
    public class gatbl_CertificadosMateria
    {
        [Key]
        public int lDCertificado_id { get; set; }

        [Required]
        public int lPConvalidacion_id { get; set; }
        
        [Display(Name = "Materia", Description = "Materia")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]        
        public int lMateria_id { get; set; }

        [Display(Name = "Tipo presentacion", Description = "Tipo presentacion")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(2)]
        public string sTipoPresentacion_fl { get; set; }

        [Display(Name = "Homologado", Description = "Homologado")]
        public bool bHomologado { get; set; }

        [Display(Name = "Calificacion", Description = "Calificacion")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [DataType("number")]
        public double dCalificacion { get; set; }

        [Display(Name = "Certificado Ajunto", Description = "Certificado Ajunto")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(200)]
        public string sDocumento_adjunto { get; set; }

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

        [ForeignKey("lMateria_id")]
        public virtual gatbl_ProgramasAnaliticos gatbl_ProgramasAnaliticos { get; set; }

        [ForeignKey("sTipoPresentacion_fl")]
        public virtual TipoPresentacionDocumento TipoPresentacionDocumento { get; set; }
    }
}