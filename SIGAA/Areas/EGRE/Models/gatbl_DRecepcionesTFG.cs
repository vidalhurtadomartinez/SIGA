using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public class gatbl_DRecepcionesTFG : BaseModelo
    {
        public gatbl_DRecepcionesTFG() {
            iObs_nro = 1;
            sTipoObs_fl = ParTipoObsDRecepcionesTFG.DE_FORMA;
            sObsCorta = string.Empty;
            sObsDetallada = string.Empty;
            sSugerencias = string.Empty;
        }

        [Key]
        public int iDRecepcionTFG_id { get; set; }

        [Required(ErrorMessage ="El campo {0} es requerido.")]
        [Display(Name = "Nro.")]
        public int iObs_nro { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Tipo Observcación")]
        public ParTipoObsDRecepcionesTFG sTipoObs_fl { get; set; }//01=DeForma; 02=DeFondo

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener una longitud maxima de {1} caracteres.")]
        [Display(Name ="Observación corta")]
        [DataType(DataType.MultilineText)]
        public string sObsCorta { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(500, ErrorMessage = "El campo {0} debe tener una longitud maxima de {1} caracteres.")]
        [Display(Name="Observación detallada")]
        [DataType(DataType.MultilineText)]
        public string sObsDetallada { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(500, ErrorMessage = "El campo {0} debe tener una longitud maxima de {1} caracteres.")]
        [Display(Name = "Sugerencias")]
        [DataType(DataType.MultilineText)]
        public string sSugerencias { get; set; }


        public int iRecepcionTFG_id { get; set; }
        public virtual gatbl_RecepcionesTFG RecepcionTFG { get; set; }


    }
}