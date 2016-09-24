namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sape : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PresentacionInformes", "FechaEvaluacionInforme", c => c.DateTime());
            AlterColumn("dbo.PresentacionPlans", "FechaEvaluacionPlan", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PresentacionPlans", "FechaEvaluacionPlan", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PresentacionInformes", "FechaEvaluacionInforme", c => c.DateTime(nullable: false));
        }
    }
}
