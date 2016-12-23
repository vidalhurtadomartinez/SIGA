using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.OYM.Models
{
    public class tblCargoAgenda
    {
        [Key]
        public int lCargoAgenda_id { get; set; }

        [Display(Name = "Cargo", Description = "Tipo")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int lCargo_id { get; set; }

        [Display(Name = "Agenda", Description = "Agenda")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(10)]
        public string agd_codigo { get; set; }

        [Display(Name = "Fecha Inicio")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        //[DisplayFormat(DataFormatString = "{0:d}")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dtFechaInicio_dt { get; set; }

        [Display(Name = "Fecha Final")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        //[DisplayFormat(DataFormatString = "{0:d}")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? dtFechaFinal_dt { get; set; }

        [Display(Name = "Estado", Description = "Estado")]
        public int iEstado_fl { get; set; }

        [Display(Name = "Eliminado", Description = "Eliminado")]
        public int iEliminado_fl { get; set; }

        [Display(Name = "Creado Por", Description = "Creado Por")]
        public int sCreado_by { get; set; }

        [Display(Name = "Concurrencia", Description = "Concurrencia")]
        public int iConcurrencia_id { get; set; }


        [ForeignKey(name: "lCargo_id")]
        public virtual tblCargo tblCargo { get; set; }

        [ForeignKey(name: "agd_codigo")]
        public virtual agenda agenda { get; set; }


        //public virtual ICollection<tblDocumento> tblDocumentos { get; set; }
        //public virtual ICollection<tblProcesoDetalle> tblProcesoDetalle { get; set; }
    }
}