using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.EGRE.Models
{
    public class EgresadosContext : DbContext
    {
        public EgresadosContext() : base("name=EgresadossConnection")
        {
        }
        public DbSet<gatbl_CProfesionales> Profesionales { get; set; }
        public DbSet<gatbl_Profesiones> Profesiones { get; set; }
        public DbSet<gatbl_Tutores> Tutorez { get; set; }
        public DbSet<gatbl_DRecepcionesTFG> DRecepcionesTFG { get; set; }
        public DbSet<gatbl_EntregaTFG> EntregasTFG { get; set; }
        public DbSet<gatbl_EntregaTribunales> EntregasTribunales { get; set; }
        public DbSet<gatbl_Perfiles> Perfiles { get; set; }
        public DbSet<gatbl_RecepcionesTFG> RecepcionesTFG { get; set; }
        public DbSet<gatbl_AreasAdministrativas> AreasAdministrativas { get; set; }
        public DbSet<gatbl_Salas> Salas { get; set; }
        public DbSet<agenda> Personas { get; set; }
        public DbSet<alumnos_agenda> Alumnos { get; set; }
        public DbSet<gatbl_EntregaAlEst> EntregasAlEstudiante { get; set; }
        public DbSet<gatbl_ComunicacionInt> ComunicacionesInternas { get; set; }
        public DbSet<gatbl_ComunicacionEst> ComunicacionesEstudiantes { get; set; }
        public DbSet<gatbl_ActaSorteoEG> ActasSorteosEG { get; set; }
        public DbSet<gatbl_ActaDefensaFinal> ActasDefensasFinales { get; set; }
        public DbSet<tipo_graduacion> tipoGraduacion { get; set; }
        public DbSet<semestres> Semestres { get; set; }
        public DbSet<disciplinas_academicas> AreasDefensas { get; set; }
        public DbSet<carreras> Carreras { get; set; }
        public DbSet<secciones_academicas> Facultades { get; set; }
        public DbSet<universidades> Universidades { get; set; }
        public DbSet<gatbl_RectorUniversidadPublica> RectoresUniversidadesPublicas { get; set; }
        public DbSet<gatbl_PresidenteColegioProfesional> PresidentesColegiosProfesionales { get; set; }
        public DbSet<Niveles_academicos> NivelesAcademicos { get; set; }
        public DbSet<gatbl_ComunicacionExtUnivPublica> ComExternasUniversidadesPublicas { get; set; }
        public DbSet<gatbl_ComunicacionExtColProf> ComExternasColegiosProfesionales { get; set; }
        public DbSet<Tipo_estado> TiposEstados { get; set; }

    }
}