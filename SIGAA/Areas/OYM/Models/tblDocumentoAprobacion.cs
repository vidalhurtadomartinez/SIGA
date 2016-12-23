using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.OYM.Models
{
    public class tblDocumentoAprobacion
    {
        [Key]
        public int lDocumentoAprobacion_id { get; set; }

        [Display(Name = "Participante", Description = "Participante")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(10)]
        public string agd_codigo { get; set; }

        [Display(Name = "Tipo Participante", Description = "Tipo Participante")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int lTipoParticipante_id { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Prioridad")]
        public int Prioridad { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Revision 1")]
        public int Revision1 { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Revision 2")]
        public int Revision2 { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Revision 3")]
        public int Revision3 { get; set; }

        [Display(Name = "Mail")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        public string CorreoElectronico { get; set; }

        public int DocumentoID { get; set; }


        [ForeignKey(name: "agd_codigo")]
        public virtual agenda agenda { get; set; }

        [ForeignKey(name: "lTipoParticipante_id")]
        public virtual TipoParticipante TipoParticipante { get; set; }

        [ForeignKey(name: "DocumentoID")]
        public virtual tblDocumento tblDocumento { get; set; }
    }
}