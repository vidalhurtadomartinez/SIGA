namespace SIGAA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inicio : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Permiso",
                c => new
                    {
                        iPermiso_id = c.Int(nullable: false),
                        Modulo = c.String(nullable: false, maxLength: 50),
                        Nemonico = c.String(nullable: false, maxLength: 6),
                        Descripcion = c.String(maxLength: 250),
                        iEstado_fl = c.Boolean(nullable: false),
                        iEliminado_fl = c.Int(nullable: false),
                        sCreado_by = c.String(maxLength: 100),
                        iConcurrencia_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.iPermiso_id);
            
            CreateTable(
                "dbo.PermisoDenegadoPorRol",
                c => new
                    {
                        iRol_id = c.Int(nullable: false),
                        iPermiso_id = c.Int(nullable: false),
                        iEstado_fl = c.Boolean(nullable: false),
                        iEliminado_fl = c.Int(nullable: false),
                        sCreado_by = c.String(maxLength: 100),
                        iConcurrencia_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.iRol_id, t.iPermiso_id })
                .ForeignKey("dbo.Permiso", t => t.iPermiso_id, cascadeDelete: false)
                .ForeignKey("dbo.Rol", t => t.iRol_id, cascadeDelete: false)
                .Index(t => t.iRol_id)
                .Index(t => t.iPermiso_id);
            
            CreateTable(
                "dbo.Rol",
                c => new
                    {
                        iRol_id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 250),
                        Observacion = c.String(maxLength: 500),
                        iEstado_fl = c.Boolean(nullable: false),
                        iEliminado_fl = c.Int(nullable: false),
                        sCreado_by = c.String(maxLength: 100),
                        iConcurrencia_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.iRol_id);
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        iUsuario_id = c.Int(nullable: false, identity: true),
                        NombreCompleto = c.String(nullable: false),
                        Sexo = c.Int(nullable: false),
                        FechaDeNacimiento = c.DateTime(nullable: false),
                        EmailUtepsa = c.String(nullable: false),
                        Contrasena = c.String(nullable: false, maxLength: 30),
                        Dierccion = c.String(maxLength: 500),
                        iRol_id = c.Int(nullable: false),
                        iEstado_fl = c.Boolean(nullable: false),
                        iEliminado_fl = c.Int(nullable: false),
                        sCreado_by = c.String(maxLength: 100),
                        iConcurrencia_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.iUsuario_id)
                .ForeignKey("dbo.Rol", t => t.iRol_id, cascadeDelete: false)
                .Index(t => t.iRol_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Usuario", "iRol_id", "dbo.Rol");
            DropForeignKey("dbo.PermisoDenegadoPorRol", "iRol_id", "dbo.Rol");
            DropForeignKey("dbo.PermisoDenegadoPorRol", "iPermiso_id", "dbo.Permiso");
            DropIndex("dbo.Usuario", new[] { "iRol_id" });
            DropIndex("dbo.PermisoDenegadoPorRol", new[] { "iPermiso_id" });
            DropIndex("dbo.PermisoDenegadoPorRol", new[] { "iRol_id" });
            DropTable("dbo.Usuario");
            DropTable("dbo.Rol");
            DropTable("dbo.PermisoDenegadoPorRol");
            DropTable("dbo.Permiso");
        }
    }
}
