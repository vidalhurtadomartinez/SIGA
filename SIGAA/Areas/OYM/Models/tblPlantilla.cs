using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGAA.Areas.OYM.Models
{
    public class tblPlantilla
    {
        [Key]
        public int lPlantilla_id { get; set; }

        [Display(Name = "Nombre", Description = "Nombre", Prompt = "Nombre")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Descripcion", Description = "Descripcion", Prompt = "Descripcion")]
        [StringLength(400)]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }
        
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Formato plantilla", Description = "Formato plantilla", Prompt = "Formato plantilla")]
        [StringLength(300)]
        public string sPathFormato { get; set; }

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
        [Display(Name = "Concurrencia")]
        public int iConcurrencia_id { get; set; }

        [Display(Name = "Usuario")]
        public int? lUsuario_id { get; set; }

        public virtual string PathCompleto { get { return string.Format("{0}{1}", System.Configuration.ConfigurationManager.ConnectionStrings["FilePath"].ConnectionString, sPathFormato); } }

        public virtual ICollection<tblPlantillaMarcador> tblPlantillaMarcador { get; set; }
        public virtual ICollection<tblDocumento> tblDocumentos { get; set; }
        public virtual ICollection<tblDocumentoPublicado> tblDocumentoPublicado { get; set; }
    }
}