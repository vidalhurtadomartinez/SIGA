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
    public class gatbl_MateriasConvalidadas
    {
        [Key]
        public int lMateriaConvalidada_id { get; set; }

        [Display(Name = "Convalidacion Externa", Description = "Convalidacion Externa")]
        [Required]
        public int lDConvalidacionExternaPA_id { get; set; }

        [Display(Name = "Origen", Description = "Origen")]
        [Required]
        public string sOrigenMateria_fl { get; set; }

        [Display(Name = "Materia", Description = "Materia")]
        [Required]
        public int lMateria_id { get; set; }

        [Display(Name = "Nota Origen", Description = "Nota Origen")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(50)]
        public string sNota_Origen { get; set; }

        [Display(Name = "Nota Destino", Description = "Nota Destino")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [StringLength(50)]
        public string sNota_destino { get; set; }

        [Display(Name = "Observaciones", Description = "Observaciones")]
        [StringLength(200)]
        [DataType(DataType.MultilineText)]
        public string sObs_desc { get; set; }

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
        [DefaultValue(1)]
        public int iConcurrencia_id { get; set; }


        [ForeignKey(name: "lDConvalidacionExternaPA_id")]
        public virtual gatbl_DConvalidacionesExternasPA gatbl_DConvalidacionesExternasPA { get; set; }

        [ForeignKey(name: "lMateria_id")]
        public virtual gatbl_ProgramasAnaliticos gatbl_ProgramasAnaliticos { get; set; }
    }
}