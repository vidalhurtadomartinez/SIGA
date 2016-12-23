using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.OYM.Models
{
    public class tblDirectorio
    {
        [Key]
        public int lDirectorio_id { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Nombre", Description = "Nombre", Prompt = "Nombre")]
        [StringLength(100)]
        [DataType(DataType.MultilineText)]
        public string sNombre { get; set; }

        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Descripcion", Description = "Descripcion", Prompt = "Descripcion")]
        [StringLength(400)]
        [DataType(DataType.MultilineText)]
        public string sDescripcion { get; set; }

        [Display(Name = "Activo")]
        [DefaultValue(true)]
        public bool bActivo { get; set; }

        [Display(Name = "Directorio Padre")]
        public int? lDirectorioPadre_id { get; set; }

        [Display(Name = "Nivel")]
        public int? Nivel { get; set; }

        [Display(Name = "Codigo")]
        public string sCodigo { get; set; }

        [Display(Name = "Usuario")]
        public int? lUsuario_id { get; set; }

        public virtual string NombreDirectorio { get { return sNombre.PadLeft(Convert.ToInt32(Nivel*2 + sNombre.Length), '-'); } }


        [ForeignKey(name: "lDirectorioPadre_id")]
        public virtual tblDirectorio DirectorioPadre { get; set; }
        public virtual ICollection<tblDirectorio> tblDirectorios { get; set; }
        public virtual ICollection<tblDocumento> tblDocumento { get; set; }
        public virtual ICollection<tblDocumentoPublicado> tblDocumentoPublicado { get; set; }
        public virtual ICollection<tblFormulario> tblFormulario { get; set; }
    }
}