namespace MVCTutorial.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Feature : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Features",
                c => new
                {
                    FeatureId = c.Int(nullable: false, identity: true),
                    FeatureName = c.String(nullable: true, maxLength: 500),
                    FeatureIconName = c.String(nullable: true, maxLength: 50),
                    FeatureDescribtion = c.String( nullable:true,maxLength:Int32.MaxValue)
                })
                .PrimaryKey(t => new { t.FeatureId })
                .Index(t => t.FeatureId);
        }
        
        public override void Down()
        {
            DropTable("dbo.Features");
        }
    }
}
