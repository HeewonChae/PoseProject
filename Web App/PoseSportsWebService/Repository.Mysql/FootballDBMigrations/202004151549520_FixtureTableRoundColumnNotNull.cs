namespace Repository.Mysql.FootballDBMigrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class FixtureTableRoundColumnNotNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("fixture", "round", c => c.String(maxLength: 64, unicode: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "SqlDefaultValue",
                        new AnnotationValues(oldValue: "", newValue: null)
                    },
                }));
        }
        
        public override void Down()
        {
            AlterColumn("fixture", "round", c => c.String(maxLength: 64, unicode: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "SqlDefaultValue",
                        new AnnotationValues(oldValue: null, newValue: "")
                    },
                }));
        }
    }
}
