using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.OYM.Models
{
    public class vVistaPersonal
    {
        [Key]
        public int idcont { get; set; }

        [Display(Name = "Codigo Nivel1", Description = "Codigo Nivel1", Prompt = "Codigo Nivel1")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(30)]
        public string CodCargoNivel1 { get; set; }

        [Display(Name = "Descripcion Nivel1", Description = "Descripcion Nivel1", Prompt = "Descripcion Nivel1")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(200)]
        public string DescCargoNivel1 { get; set; }

        [Display(Name = "Codigo Nivel2", Description = "Codigo Nivel2", Prompt = "Codigo Nivel2")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(30)]
        public string CodCargoNivel2 { get; set; }

        [Display(Name = "Descripcion Nivel2", Description = "Descripcion Nivel2", Prompt = "Descripcion Nivel2")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(200)]
        public string DescCargoNivel2 { get; set; }
        

        [Display(Name = "Nombre", Description = "Nombre", Prompt = "Nombre")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(300)]
        public string Nombre { get; set; }

        [Display(Name = "E-mail", Description = "E-mail", Prompt = "E-mail")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(300)]
        public string Email { get; set; }

        [Display(Name = "Tipo de Contrato", Description = "Tipo de Contrato")]
        public string TipoContrato { get; set; }

        [Display(Name = "Codigo", Description = "Codigo")]
        [StringLength(10)]
        public string agd_codigo { get; set; }


        public virtual ICollection<tblProcesoDetalle> tblProcesoDetalle { get; set; }
        public virtual ICollection<tblDocumento> tblDocumento { get; set; }
        public virtual ICollection<tblDocumentoPublicado> tblDocumentoPublicado { get; set; }
        public virtual ICollection<tblFormulario> tblFormulario { get; set; }
    }
}