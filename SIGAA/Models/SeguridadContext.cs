using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SIGAA.Models
{
    public class SeguridadContext : DbContext
    {
        public SeguridadContext() : base("name=SIGAAConnection")
        {
        }
        public DbSet<PermisoDenegadoPorRol>  PermisoDenegadoPorRol { get; set; }
        public DbSet<Rol> Rol { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Permiso> Permiso { get; set; }
        public DbSet<agenda> Personas { get; set; }

    }
}