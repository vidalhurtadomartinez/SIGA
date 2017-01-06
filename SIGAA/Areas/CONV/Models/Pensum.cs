using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGAA.Areas.CONV.Models
{
    public class Pensum
    {
        [Key]        
        public int lPensum_id { get; set; }
        
        [Display(Name = "Universidad", Description = "Universidad")]
        public int lUniversidad_id { get; set; }

        [Display(Name = "Carrera", Description = "Carrera")]
        public int lCarrera_id { get; set; }

        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [Display(Name = "Descripcion", Description = "Descripcion", Prompt = "Pensum")]
        [StringLength(30, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.", MinimumLength = 4)]
        public string sDescripcion { get; set; }

        [Display(Name = "Activo")]
        [DefaultValue(true)]
        public bool bActivo { get; set; }

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

        public virtual gatbl_Universidades gatbl_Universidades { get; set; }
        public virtual gatbl_Carreras gatbl_Carreras { get; set; }

        public virtual ICollection<gatbl_ProgramasAnaliticos> gatbl_ProgramasAnaliticos { get; set; }
    }
}