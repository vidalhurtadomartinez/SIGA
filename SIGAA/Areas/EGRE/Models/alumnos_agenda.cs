using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIGAA.Areas.EGRE.Models
{    
    public class alumnos_agenda
    {
        [Key]
        public string alm_registro { get; set; }
        public string agd_codigo { get; set; }
        public DateTime alm_fechainsc { get; set; }
        public string crr_codigo { get; set; }
        public string pns_codigo { get; set; }
        public string mdl_codigo { get; set; }
        public string mdu_codigo { get; set; }
        public string tur_codigo { get; set; }
        public bool tur_exclusivo { get; set; }
        public string sem_codigo { get; set; }
        public string ctr_codigo { get; set; }
        public string agente_codigo { get; set; }
        public string tstd_codigo { get; set; }
        public string usr_codigo { get; set; }
        public DateTime trn_fecha { get; set; }
        public bool doc_confirmados { get; set; }
        public DateTime doc_fechaconfirmado { get; set; }
        public string doc_agdcodigo { get; set; }
        public bool alm_cambiocarrera { get; set; }


        //propiedades de Navegacion
        public virtual agenda Persona { get; set; }//23.05.2016  agregado par referencias los datos de los alumnos
        public virtual carreras Carrera { get; set; }//23.05.2016  agregado par referencias los datos de los alumnos

        public virtual IEnumerable<gatbl_Perfiles> Perfiles { get; set; }
        public virtual IEnumerable<gatbl_EntregaTFG> EntregasTFG { get; set; }
        public virtual IEnumerable<gatbl_EntregaTribunales> EntregasTribunales { get; set; }
        public virtual IEnumerable<gatbl_EntregaAlEst> EntregasAlEstudiante { get; set; }
        public virtual IEnumerable<gatbl_ComunicacionInt> ComunicacionesInternas { get; set; }
        public virtual IEnumerable<gatbl_ComunicacionEst> ComunicacionesEstudiante { get; set; }
        public virtual IEnumerable<gatbl_ActaSorteoEG> ActasSorteosEG { get; set; }
        public virtual IEnumerable<gatbl_ActaDefensaFinal> ActasDefensasFinales { get; set; }
        //public virtual IEnumerable<gatbl_ComunicacionExt> ComunicacionesExternas { get; set; }//se elimina
        public virtual IEnumerable<gatbl_ComunicacionExtColProf> ComExternasColegiosProfesionales { get; set; }
        public virtual IEnumerable<gatbl_ComunicacionExtUnivPublica> ComExternasUniversidadesPublicas { get; set; }
        public virtual Tipo_estado TipoEstado { get; set; }
    }
}
