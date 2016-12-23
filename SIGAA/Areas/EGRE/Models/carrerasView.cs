using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public class carrerasView
    {
        [Key]
        public string crr_codigo { get; set; }
        public string crr_descripcion { get; set; }
        public string crr_sigla { get; set; }
        public short crr_nivel { get; set; }
        public string nva_codigo { get; set; }
        public string sca_codigo { get; set; }
        public string crr_pensumactual { get; set; }
        public bool crr_status { get; set; }
        public string ccs_codigo { get; set; }
        public bool crr_con_convenio { get; set; }
        public bool crr_imprimir_nota_convenio { get; set; }
        public DateTime crr_fechacreacion { get; set; }
        public string crr_resolucionrectoral { get; set; }
        public string crr_resoluciongubernamental { get; set; }
        public string crr_URL { get; set; }
        public string crr_abreviatura { get; set; }
    }
}