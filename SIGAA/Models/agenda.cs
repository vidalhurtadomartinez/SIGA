using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGAA.Models
{
    public class agenda
    {
        [Key]
        public string agd_codigo { get; set; }
        public string tagd_codigo { get; set; }
        public string agd_ruc { get; set; }
        public string trps_codigo { get; set; }
        public string agd_appaterno { get; set; }
        public string agd_apmaterno { get; set; }
        public string agd_nombres { get; set; }
        public string agd_apCasada { get; set; }
        public string agd_razonsocial { get; set; }
        public string agd_sexo { get; set; }
        Nullable<System.DateTime> agd_fechanac { get; set; }
        public string agd_lugarnac { get; set; }
        public string agd_nacionalidad { get; set; }
        public string agd_docid { get; set; }
        public string agd_docnro { get; set; }
        public string agd_doclugar { get; set; }
        public string agd_estcivil { get; set; }
        public string agd_direccion { get; set; }
        public string agd_direccion1 { get; set; }
        public string agd_direccion2 { get; set; }
        public string agd_zona { get; set; }
        public string agd_telfcod1 { get; set; }
        public string agd_telf1 { get; set; }
        public string agd_telfcod2 { get; set; }
        public string agd_telf2 { get; set; }
        public string agd_telfcod3 { get; set; }
        public string agd_telf3 { get; set; }
        public string agd_telfcod4 { get; set; }
        public string agd_telf4 { get; set; }
        public string agd_telfcod5 { get; set; }
        public string agd_telf5 { get; set; }
        public string agd_email { get; set; }
        public string agd_email_utepsa { get; set; }
        public string agd_web { get; set; }
        public string cdd_codigo { get; set; }
        public string rub_codigo { get; set; }
        public string ctr_codigo { get; set; }
        public string agd_segmed_codigo { get; set; }
        public string agd_gruposanguineo { get; set; }
        public decimal agd_altura { get; set; }
        public decimal agd_peso { get; set; }
        public string agd_nroReferencia { get; set; }

        Nullable<System.DateTime> agd_trnfecha { get; set; }
        public string usr_codigo { get; set; }
        public string usr_codigo_cambio { get; set; }
        Nullable<System.DateTime> trn_fecha { get; set; }
        public string agd_prefijo { get; set; }

        [NotMapped]
        [Display(Name="Nombre Completo")]
        public string NombreCompleto { get { return string.Format("{0} {1} {2}", agd_nombres.Trim(), agd_appaterno.Trim(), agd_apmaterno.Trim()); } }

    }
}
