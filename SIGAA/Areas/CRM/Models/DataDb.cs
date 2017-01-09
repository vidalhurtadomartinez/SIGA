using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace SIGAA.Areas.CRM.Models
{
    public class DataDb : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public DataDb() : base("name=CRMContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public System.Data.Entity.DbSet<SIGAA.Areas.CRM.Models.tblRelacionPostulante> tblRelacionPostulante { get; set; }
        public System.Data.Entity.DbSet<SIGAA.Areas.CRM.Models.tblActividad> tblActividad { get; set; }
        public System.Data.Entity.DbSet<SIGAA.Areas.CRM.Models.tblCarrera> tblCarrera { get; set; }
        public System.Data.Entity.DbSet<SIGAA.Areas.CRM.Models.tblCategoriaColegio> tblCategoriaColegio { get; set; }
        public System.Data.Entity.DbSet<SIGAA.Areas.CRM.Models.tblCiudad> tblCiudad { get; set; }
        public System.Data.Entity.DbSet<SIGAA.Areas.CRM.Models.tblColegio> tblColegio { get; set; }
        public System.Data.Entity.DbSet<SIGAA.Areas.CRM.Models.tblFormaContacto> tblFormaContacto { get; set; }
        public System.Data.Entity.DbSet<SIGAA.Areas.CRM.Models.tblinstitucion> tblinstitucion { get; set; }
        public System.Data.Entity.DbSet<SIGAA.Areas.CRM.Models.tblMedioInformacion> tblMedioInformacion { get; set; }
        public System.Data.Entity.DbSet<SIGAA.Areas.CRM.Models.tblOcupacion> tblOcupacion { get; set; }
        public System.Data.Entity.DbSet<SIGAA.Areas.CRM.Models.tblPais> tblPais { get; set; }
        public System.Data.Entity.DbSet<SIGAA.Areas.CRM.Models.tblProvincia> tblProvincia { get; set; }
        public System.Data.Entity.DbSet<SIGAA.Areas.CRM.Models.tblRubroActividad> tblRubroActividad { get; set; }
        public System.Data.Entity.DbSet<SIGAA.Areas.CRM.Models.tblTipoColegio> tblTipoColegio { get; set; }
        public System.Data.Entity.DbSet<SIGAA.Areas.CRM.Models.tblTipoDocumento> tblTipoDocumento { get; set; }
        public System.Data.Entity.DbSet<SIGAA.Areas.CRM.Models.tblTipoRespuesta> tblTipoRespuesta { get; set; }
        public System.Data.Entity.DbSet<SIGAA.Areas.CRM.Models.tblTipoUniversidad> tblTipoUniversidad { get; set; }
        public System.Data.Entity.DbSet<SIGAA.Areas.CRM.Models.tblUniversidad> tblUniversidad { get; set; }
        public System.Data.Entity.DbSet<SIGAA.Areas.CRM.Models.tblCargo> tblCargos { get; set; }
        public System.Data.Entity.DbSet<SIGAA.Areas.CRM.Models.tblCliente> tblCliente { get; set; }
        public System.Data.Entity.DbSet<SIGAA.Areas.CRM.Models.tblAsesor> tblAsesor { get; set; }
        public System.Data.Entity.DbSet<SIGAA.Areas.CRM.Models.tblTipoEvento> tblTipoEvento { get; set; }
        public System.Data.Entity.DbSet<SIGAA.Areas.CRM.Models.tblNotificacion> tblNotificacion { get; set; }
        public System.Data.Entity.DbSet<SIGAA.Areas.CRM.Models.tblEvento> tblEvento { get; set; }
        public System.Data.Entity.DbSet<SIGAA.Areas.CRM.Models.tEvento> tEvento { get; set; }
    }
}
