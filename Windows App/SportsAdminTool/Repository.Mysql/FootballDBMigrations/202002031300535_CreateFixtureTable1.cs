namespace Repository.Mysql.FootballDBMigrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class CreateFixtureTable1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "odds",
                c => new
                    {
                        seq = c.Long(nullable: false, identity: true),
                        fixture_id = c.Int(nullable: false),
                        bookmaker_id = c.Int(nullable: false, storeType: "mediumint"),
                        label_id = c.Int(nullable: false, storeType: "mediumint"),
                        subtitle_1 = c.String(nullable: false, maxLength: 16, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "")
                                },
                            }),
                        odds_1 = c.Single(nullable: false, defaultValueSql: "0",
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        subtitle_2 = c.String(nullable: false, maxLength: 16, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "")
                                },
                            }),
                        odds_2 = c.Single(nullable: false, defaultValueSql: "0",
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        subtitle_3 = c.String(nullable: false, maxLength: 16, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "")
                                },
                            }),
                        odds_3 = c.Single(nullable: false, defaultValueSql: "0",
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        upt_time = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.seq)                
                .Index(t => new { t.fixture_id, t.label_id }, name: "IDX_FIXTURE_LABEL");
            
        }
        
        public override void Down()
        {
            DropIndex("odds", "IDX_FIXTURE_LABEL");
            DropTable("odds");
        }
    }
}
