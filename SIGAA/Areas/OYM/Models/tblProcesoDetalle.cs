using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.OYM.Models
{
    public class tblProcesoDetalle
    {
        [Key]
        public int lProcesoDetalle_id { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Proceso")]
        public int lProceso_id { get; set; }

        //[Display(Name = "Agenda", Description = "Agenda")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        //[StringLength(10)]
        //public string agd_codigo { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Participante")]
        public int lParticipante_id { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Categoria")]
        public int lCategoria_id { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Rol")]
        public int lRolParticipante_id { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Rev. (# dias)")]
        public int lRevision1 { get; set; }

        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Rev. 2")]
        public int? lRevision2 { get; set; }

        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Rev. 3")]
        public int? lRevision3 { get; set; }


        [ForeignKey(name: "lProceso_id")]
        public virtual tblProceso tblProceso { get; set; }

        //[ForeignKey(name: "agd_codigo")]
        //public virtual agenda agenda { get; set; }

        [ForeignKey(name: "lCategoria_id")]
        public virtual tblCategoria tblCategoria { get; set; }

        [ForeignKey(name: "lParticipante_id")]
        public virtual vVistaPersonal vVistaPersonal { get; set; }

        [ForeignKey(name: "lRolParticipante_id")]
        public virtual RolParticipante RolParticipante { get; set; }        
    }
}