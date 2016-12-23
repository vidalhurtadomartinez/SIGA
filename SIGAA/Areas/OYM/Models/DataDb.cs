using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.OYM.Models
{
    public class DataDb : DbContext
    {    
        public DataDb() : base("name=SeguimientoContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
            modelbuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelbuilder.Conventions.Remove<IncludeMetadataConvention>();
            modelbuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public DbSet<tblDocumento> tblDocumento { get; set; }
        public DbSet<tblDocumentoPublicado> tblDocumentoPublicado { get; set; }
        public DbSet<tblDocumentoRelacionado> tblDocumentoRelacionado { get; set; }
        public DbSet<tblDocumentoCaracteristica> tblDocumentoCaracteristica { get; set; }
        public DbSet<tblFormulario> tblFormulario { get; set; }
        public DbSet<tblFormularioRelacionado> tblFormularioRelacionado { get; set; }
        public DbSet<tblFormaDocumento> tblFormaDocumentos { get; set; }
        public DbSet<tblFormaDocumentoTipo> tblFormaDocumentoTipos { get; set; }
        public DbSet<tblTipoDocumento> tblTipoDocumentos { get; set; }
        public DbSet<tblTipoProceso> tblTipoProcesos { get; set; }    
        public DbSet<tblPlantilla> tblPlantilla { get; set; }
        public DbSet<tblPlantillaMarcador> tblPlantillaMarcador { get; set; }
        public DbSet<UbicacionPlantilla> UbicacionPlantilla { get; set; }
        public DbSet<FormatoNumeracionPlantilla> FormatoNumeracionPlantilla { get; set; }
        public DbSet<LugarNumeracionPlantilla> LugarNumeracionPlantilla { get; set; }
        public DbSet<OrigenDocumento> OrigenDocumento { get; set; }
        public DbSet<TipoProceso> TipoProceso { get; set; }
        public DbSet<EstadoDocumento> EstadoDocumento { get; set; }
        public DbSet<agenda> agenda { get; set; }
        public DbSet<tblCargo> tblCargo { get; set; }
        public DbSet<tblCargoAgenda> tblCargoAgenda { get; set; }
        public DbSet<TipoParticipante> TipoParticipante { get; set; }        
        public DbSet<tblCategoria> tblCategoria { get; set; }
        public DbSet<RolParticipante> RolParticipante { get; set; }
        public DbSet<tblProceso> tblProceso { get; set; }
        public DbSet<tblProcesoDetalle> tblProcesoDetalle { get; set; }
        public DbSet<tblProcesoDetalleCategoria> tblProcesoDetalleCategoria { get; set; }
        public DbSet<tblDocumentoProceso> tblDocumentoProceso { get; set; }
        public DbSet<tblFormularioProceso> tblFormularioProceso { get; set; }    
        public DbSet<tblSeguimientoDocumento> tblSeguimientoDocumento { get; set; }

        public DbSet<tblSeguimientoFormulario> tblSeguimientoFormularios { get; set; }
        public DbSet<tblDirectorio> tblDirectorio { get; set; }
        public DbSet<EstadoProceso> EstadoProceso { get; set; }
        public DbSet<vVistaPersonal> vVistaPersonal { get; set; }
        public DbSet<tblDocumentoHistorico> tblDocumentoHistorico { get; set; }
        public DbSet<tblFormularioHistorico> tblFormularioHistorico { get; set; }
        public DbSet<tblFormularioPublicado> tblFormularioPublicado { get; set; }
    }
}
