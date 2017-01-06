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
    public class gatbl_DetProgramasAnaliticos
    {
        [Key]
        public int lDetProgramaAnalitico_id { get; set; }

        [Display(Name = "Programa Analitico", Description = "Programa Analitico")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int? lProgramaAnalitico_id { get; set; }

        [Display(Name = "Codigo", Description = "Codigo")]
        public string sCodigo { get; set; }

        [Display(Name = "Numero", Description = "Numero")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        public int? sNumero { get; set; }

        [Display(Name = "Nivel", Description = "Nivel")]
        public int? Nivel { get; set; }

        [UIHint("TextEditorTemplate")]
        [Display(Name = "Descripcion", Description = "Descripcion")]
        [Required(ErrorMessage = "Debe ingresar el campo {0}")]
        [DataType(DataType.MultilineText)]
        //[StringLength(250)]
        public string sDescripcion_desc { get; set; }

        [Display(Name = "Contenido General", Description = "Contenido General")]
        //[Required(ErrorMessage = "Debe ingresar el campo {0}")]
        //[StringLength(500)]
        [DataType(DataType.MultilineText)]
        public string sContenido_gral { get; set; }

        [Display(Name = "Programa Analitico Padre", Description = "Programa Analitico Padre")]
        public int? lDetProgramaAnaliticoPadre_id { get; set; }

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
        public int? iConcurrencia_id { get; set; }


        [ForeignKey(name: "lProgramaAnalitico_id")]
        public virtual gatbl_ProgramasAnaliticos gatbl_ProgramasAnaliticos { get; set; }

        [ForeignKey(name: "lDetProgramaAnaliticoPadre_id")]
        public virtual gatbl_DetProgramasAnaliticos gatbl_DetProgramasAnaliticosPadre { get; set; }



        public virtual ICollection<gatbl_DetProgramasAnaliticos> gatbl_DetProgramasAnaliticosPadres { get; set; }
        public virtual ICollection<gatbl_AnalisisConvalidacionesUnidad> gatbl_AnalisisConvalidacionesUnidad { get; set; }
    }
}