using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure;

namespace SIGAA.Areas.CONV.Models
{
    public class ConvalidacionesContext : DbContext
    {
    
        public ConvalidacionesContext() : base("name=ConvalidacionesContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {            
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public DbSet<Departamento> Departamentos { get; set; }

        public DbSet<OrigenOtraUniversidad> OrigenOtraUniversidades { get; set; }

        public DbSet<TipoPresentacionDocumento> TipoPresentacionDocumentos { get; set; }

        public DbSet<NivelProgramaAnalitico> NivelProgramaAnaliticos { get; set; }

        public DbSet<OrigenProgramaAnalitico> OrigenProgramaAnaliticos { get; set; }

        public DbSet<UnidadNegocio> UnidadNegocios { get; set; }

        public DbSet<gatbl_Universidades> gatbl_Universidades { get; set; }

        public DbSet<gatbl_Facultades> gatbl_Facultades { get; set; }

        public DbSet<gatbl_Carreras> gatbl_Carreras { get; set; }

        public DbSet<gatbl_PConvalidaciones> gatbl_PConvalidaciones { get; set; }        
        public DbSet<carreras> carreras { get; set; }
        public DbSet<secciones_academicas> secciones_academicas { get; set; }
        public DbSet<agenda> agendas { get; set; }
        public DbSet<alumnos_agenda> alumnos_agenda { get; set; }
        public DbSet<gatbl_DPConvalidaciones> gatbl_DPConvalidaciones { get; set; }

        public DbSet<TipoEscalaEvaluacion> TipoEscalaEvaluacions { get; set; }

        public DbSet<gatbl_EscalaCalificaciones> gatbl_EscalaCalificaciones { get; set; }
        public DbSet<tipo_documentos> tipo_documentos { get; set; }

        public DbSet<gatbl_ProgramasAnaliticos> gatbl_ProgramasAnaliticos { get; set; }

        public DbSet<gatbl_DProgramasAnaliticos> gatbl_DProgramasAnaliticos { get; set; }

        public DbSet<gatbl_AnalisisConvalidaciones> gatbl_AnalisisConvalidaciones { get; set; }
        public DbSet<gatbl_DAnalisisConvalidaciones> gatbl_DAnalisisConvalidaciones { get; set; }
        public DbSet<gatbl_UnidadesConvalidadas> gatbl_UnidadesConvalidadas { get; set; }
        public DbSet<gatbl_ConvalidacionesExternasPA> gatbl_ConvalidacionesExternasPA { get; set; }
        public DbSet<gatbl_DConvalidacionesExternasPA> gatbl_DConvalidacionesExternasPA { get; set; }
        public DbSet<gatbl_MateriasConvalidadas> gatbl_MateriasConvalidadas { get; set; }
        public DbSet<gatbl_Postulantes> gatbl_Postulantes { get; set; }
        public DbSet<gatbl_DProgramasAnaliticosTemas> gatbl_DProgramasAnaliticosTemas { get; set; }

        public DbSet<gatbl_AnalisisPreConvalidaciones> gatbl_AnalisisPreConvalidaciones { get; set; }
        public DbSet<gatbl_DAnalisisPreConvalidaciones> gatbl_DAnalisisPreConvalidaciones { get; set; }

        public DbSet<TipoCargaHoraria> TipoCargaHorarias { get; set; }

        public DbSet<Pensum> Pensums { get; set; }
        public DbSet<gatbl_CertificadosMateria> gatbl_CertificadosMateria { get; set; }

        public DbSet<TipoDocumentoSolicitud> TipoDocumentoSolicitudes { get; set; }

        public DbSet<TipoDocumentoPersonal> TipoDocumentoPersonales { get; set; }

        public DbSet<Nacionalidad> Nacionalidades { get; set; }
        public DbSet<gatbl_DetProgramasAnaliticos> gatbl_DetProgramasAnaliticos { get; set; }
        public DbSet<gatbl_DetAnalisisPreConvalidaciones> gatbl_DetAnalisisPreConvalidaciones { get; set; }
        public DbSet<gatbl_AnalisisConvalidacionesUnidad> gatbl_AnalisisConvalidacionesUnidad { get; set; }
        public DbSet<gatbl_DAnalisisConvalidacionesUnidad> gatbl_DAnalisisConvalidacionesUnidad { get; set; }
        public DbSet<gatbl_AnalisisPreConvalidacionesMateria> gatbl_AnalisisPreConvalidacionesMateria { get; set; }
    }
}
