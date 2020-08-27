namespace Repository.Mysql.FootballDBMigrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;

    public partial class AlterFixtureTable : DbMigration
    {
        public override void Up()
        {
            DropIndex("fixture", "IDX_PREDICTED_DATE");
            AddColumn("fixture", "has_odds", c => c.Boolean(nullable: false, defaultValueSql: "0",
                annotations: new Dictionary<string, AnnotationValues>
                {
                    {
                        "SqlDefaultValue",
                        new AnnotationValues(oldValue: null, newValue: "0")
                    },
                }));
            AddColumn("fixture", "max_grade", c => c.Short(nullable: false, defaultValueSql: "0",
                annotations: new Dictionary<string, AnnotationValues>
                {
                    {
                        "SqlDefaultValue",
                        new AnnotationValues(oldValue: null, newValue: "0")
                    },
                }));
            AlterColumn("fixture", "home_score", c => c.Int(nullable: false, storeType: "mediumint",
                annotations: new Dictionary<string, AnnotationValues>
                {
                    {
                        "SqlDefaultValue",
                        new AnnotationValues(oldValue: "0", newValue: null)
                    },
                }));
            AlterColumn("fixture", "away_score", c => c.Int(nullable: false, storeType: "mediumint",
                annotations: new Dictionary<string, AnnotationValues>
                {
                    {
                        "SqlDefaultValue",
                        new AnnotationValues(oldValue: "0", newValue: null)
                    },
                }));
            AlterColumn("fixture", "is_predicted", c => c.Boolean(nullable: false, defaultValueSql: "0",
                annotations: new Dictionary<string, AnnotationValues>
                {
                    {
                        "SqlDefaultValue",
                        new AnnotationValues(oldValue: null, newValue: "0")
                    },
                }));
            CreateIndex("fixture", new[] { "is_predicted", "match_time" }, name: "IDX_PREDICTED_DATE");
        }

        public override void Down()
        {
            DropIndex("fixture", "IDX_PREDICTED_DATE");
            AlterColumn("fixture", "is_predicted", c => c.Boolean(nullable: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    {
                        "SqlDefaultValue",
                        new AnnotationValues(oldValue: "0", newValue: null)
                    },
                }));
            AlterColumn("fixture", "away_score", c => c.Int(nullable: false, defaultValueSql: "0", storeType: "mediumint",
                annotations: new Dictionary<string, AnnotationValues>
                {
                    {
                        "SqlDefaultValue",
                        new AnnotationValues(oldValue: null, newValue: "0")
                    },
                }));
            AlterColumn("fixture", "home_score", c => c.Int(nullable: false, defaultValueSql: "0", storeType: "mediumint",
                annotations: new Dictionary<string, AnnotationValues>
                {
                    {
                        "SqlDefaultValue",
                        new AnnotationValues(oldValue: null, newValue: "0")
                    },
                }));
            DropColumn("fixture", "max_grade");
            DropColumn("fixture", "has_odds");
            CreateIndex("fixture", new[] { "is_predicted", "match_time" }, name: "IDX_PREDICTED_DATE");
        }
    }
}