namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Alumnoes",
                c => new
                    {
                        IdAlumno = c.Int(nullable: false, identity: true),
                        Legajo = c.Int(nullable: false),
                        NombreAlu = c.String(),
                        ApellidoAlu = c.String(),
                        CorreoAlu = c.String(),
                    })
                .PrimaryKey(t => t.IdAlumno);
            
            CreateTable(
                "dbo.PS",
                c => new
                    {
                        IdPS = c.Int(nullable: false, identity: true),
                        NroDisposicion = c.Int(nullable: false),
                        Tutor = c.String(),
                        TituloProyecto = c.String(),
                        CicloLectivo = c.Int(nullable: false),
                        Cuatrimestre = c.Int(nullable: false),
                        IdOrganizacion = c.Int(nullable: false),
                        IdArea = c.Int(nullable: false),
                        IdTipoPS = c.Int(nullable: false),
                        IdAlumno = c.Int(nullable: false),
                        Estado = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdPS)
                .ForeignKey("dbo.Alumnoes", t => t.IdAlumno, cascadeDelete: true)
                .ForeignKey("dbo.Areas", t => t.IdArea, cascadeDelete: true)
                .ForeignKey("dbo.Organizacions", t => t.IdOrganizacion, cascadeDelete: true)
                .ForeignKey("dbo.TipoPS", t => t.IdTipoPS, cascadeDelete: true)
                .Index(t => t.IdOrganizacion)
                .Index(t => t.IdArea)
                .Index(t => t.IdTipoPS)
                .Index(t => t.IdAlumno);
            
            CreateTable(
                "dbo.Areas",
                c => new
                    {
                        IdArea = c.Int(nullable: false, identity: true),
                        NombreArea = c.String(),
                        DescripcionArea = c.String(),
                    })
                .PrimaryKey(t => t.IdArea);
            
            CreateTable(
                "dbo.Organizacions",
                c => new
                    {
                        IdOrganizacion = c.Int(nullable: false, identity: true),
                        DenominacionOrg = c.String(),
                        DireccionOrg = c.String(),
                        TelefonoOrg = c.Int(nullable: false),
                        DescripcionOrg = c.String(),
                    })
                .PrimaryKey(t => t.IdOrganizacion);
            
            CreateTable(
                "dbo.PresentacionInformes",
                c => new
                    {
                        IdPresentacionInforme = c.Int(nullable: false, identity: true),
                        FechaPresentacionInforme = c.DateTime(nullable: false),
                        FechaEvaluacionInforme = c.DateTime(nullable: false),
                        EstadoEvaluacionInforme = c.Int(nullable: false),
                        ObservacionesInforme = c.String(),
                        IdPS = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdPresentacionInforme)
                .ForeignKey("dbo.PS", t => t.IdPS, cascadeDelete: true)
                .Index(t => t.IdPS);
            
            CreateTable(
                "dbo.PresentacionPlans",
                c => new
                    {
                        IdPresentacionPlan = c.Int(nullable: false, identity: true),
                        FechaPresentacionPlan = c.DateTime(nullable: false),
                        FechaEvaluacionPlan = c.DateTime(nullable: false),
                        EstadoEvaluacionPlan = c.Int(nullable: false),
                        ObservacionesPlan = c.String(),
                        IdPS = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdPresentacionPlan)
                .ForeignKey("dbo.PS", t => t.IdPS, cascadeDelete: true)
                .Index(t => t.IdPS);
            
            CreateTable(
                "dbo.TipoPS",
                c => new
                    {
                        IdTipoPS = c.Int(nullable: false, identity: true),
                        NombreTipoPS = c.String(),
                        DescripcionTipoPS = c.String(),
                    })
                .PrimaryKey(t => t.IdTipoPS);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PS", "IdTipoPS", "dbo.TipoPS");
            DropForeignKey("dbo.PresentacionPlans", "IdPS", "dbo.PS");
            DropForeignKey("dbo.PresentacionInformes", "IdPS", "dbo.PS");
            DropForeignKey("dbo.PS", "IdOrganizacion", "dbo.Organizacions");
            DropForeignKey("dbo.PS", "IdArea", "dbo.Areas");
            DropForeignKey("dbo.PS", "IdAlumno", "dbo.Alumnoes");
            DropIndex("dbo.PresentacionPlans", new[] { "IdPS" });
            DropIndex("dbo.PresentacionInformes", new[] { "IdPS" });
            DropIndex("dbo.PS", new[] { "IdAlumno" });
            DropIndex("dbo.PS", new[] { "IdTipoPS" });
            DropIndex("dbo.PS", new[] { "IdArea" });
            DropIndex("dbo.PS", new[] { "IdOrganizacion" });
            DropTable("dbo.TipoPS");
            DropTable("dbo.PresentacionPlans");
            DropTable("dbo.PresentacionInformes");
            DropTable("dbo.Organizacions");
            DropTable("dbo.Areas");
            DropTable("dbo.PS");
            DropTable("dbo.Alumnoes");
        }
    }
}
