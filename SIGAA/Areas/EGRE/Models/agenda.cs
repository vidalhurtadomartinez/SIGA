using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIGAA.Areas.EGRE.Models
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


        ///propiedades de Navegacion
        public virtual IEnumerable<gatbl_Perfiles> Perfiles { get; set; }
        public virtual IEnumerable<gatbl_EntregaTFG> EntregasTFG { get; set; }
        public virtual IEnumerable<gatbl_EntregaTribunales> EntregasTribunales { get; set; }
        public virtual IEnumerable<gatbl_RecepcionesTFG> RecepcionesTFG { get; set; }
        public virtual IEnumerable<gatbl_EntregaAlEst> EntregasAlEstudiante { get; set; }
        public virtual IEnumerable<gatbl_ComunicacionInt> ComunicacionesInternas { get; set; }
        public virtual IEnumerable<gatbl_ComunicacionEst> ComunicacionesEstudiante { get; set; }


        [InverseProperty("EntregadoPor")]
        public virtual IEnumerable<gatbl_Perfiles> PerfilesEntregadoPor { get; set; }
        [InverseProperty("RecepcionadoPor")]
        public virtual IEnumerable<gatbl_Perfiles> PerfilesRecepcionadoPor { get; set; }


        [InverseProperty("Presidente")]
        public virtual IEnumerable<gatbl_ActaSorteoEG> ActasSorteosEGPresidente { get; set; }
        [InverseProperty("Secretario")]
        public virtual IEnumerable<gatbl_ActaSorteoEG> ActasSorteosEGSecretario { get; set; }
        [InverseProperty("Evaluador1I")]
        public virtual IEnumerable<gatbl_ActaSorteoEG> ActasSorteosEGEvaluador1I { get; set; }
        [InverseProperty("Evaluador2I")]
        public virtual IEnumerable<gatbl_ActaSorteoEG> ActasSorteosEGEvaluador2I { get; set; }
        [InverseProperty("Evaluador1E")]
        public virtual IEnumerable<gatbl_ActaSorteoEG> ActasSorteosEGEvaluador1E { get; set; }
        [InverseProperty("Evaluador2E")]
        public virtual IEnumerable<gatbl_ActaSorteoEG> ActasSorteosEGEvaluador2E { get; set; }
        [InverseProperty("RealizadoPor")]
        public virtual IEnumerable<gatbl_ActaSorteoEG> ActasSorteosEGRealizadoPor { get; set; }

        [InverseProperty("Presidente")]
        public virtual IEnumerable<gatbl_ActaDefensaFinal> ActasDefensasFinalesPresidente { get; set; }
        [InverseProperty("Secretario")]
        public virtual IEnumerable<gatbl_ActaDefensaFinal> ActasDefensasFinalesSecretario { get; set; }
        [InverseProperty("Evaluador1I")]
        public virtual IEnumerable<gatbl_ActaDefensaFinal> ActasDefensasFinalesEvaluador1I { get; set; }
        [InverseProperty("Evaluador2I")]
        public virtual IEnumerable<gatbl_ActaDefensaFinal> ActasDefensasFinalesEvaluador2I { get; set; }
        [InverseProperty("Evaluador1E")]
        public virtual IEnumerable<gatbl_ActaDefensaFinal> ActasDefensasFinalesEvaluador1E { get; set; }
        [InverseProperty("Evaluador2E")]
        public virtual IEnumerable<gatbl_ActaDefensaFinal> ActasDefensasFinalesEvaluador2E { get; set; }

        [InverseProperty("Representante")]
        public virtual IEnumerable<gatbl_ActaDefensaFinal> ActasDefensasFinalesRepresentante { get; set; }

       // public virtual IEnumerable<gatbl_ComunicacionExt> ComunicacionesExternas { get; set; }//para eliminar
        public virtual IEnumerable<gatbl_ComunicacionExtColProf> ComExternasColegiosProfesionales { get; set; }
        public virtual IEnumerable<gatbl_ComunicacionExtUnivPublica> ComExternasUniversidadesPublicas { get; set; }

    }
}
