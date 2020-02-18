namespace Repository.Mysql.FootballDBMigrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class CreateFixtureTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "fixture",
                c => new
                    {
                        id = c.Int(nullable: false),
                        league_id = c.Int(nullable: false, storeType: "mediumint"),
                        home_team_id = c.Int(nullable: false, storeType: "mediumint"),
                        away_team_id = c.Int(nullable: false, storeType: "mediumint"),
                        event_date = c.DateTime(nullable: false, precision: 0),
                        round = c.String(maxLength: 64, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "")
                                },
                            }),
                        status = c.String(nullable: false, maxLength: 8, fixedLength: true, unicode: false, storeType: "char"),
                        home_score = c.Int(nullable: false, defaultValueSql: "0", storeType: "mediumint",
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        away_score = c.Int(nullable: false, defaultValueSql: "0", storeType: "mediumint",
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        is_predicted = c.Boolean(nullable: false),
                        is_completed = c.Boolean(nullable: false),
                        upt_time = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.id)                
                .Index(t => new { t.league_id, t.event_date }, name: "IDX_LEAGUE_DATE")
                .Index(t => new { t.home_team_id, t.event_date }, name: "IDX_HOME_DATE")
                .Index(t => new { t.away_team_id, t.event_date }, name: "IDX_AWAY_DATE")
                .Index(t => t.is_predicted, name: "IDX_PREDICTED")
                .Index(t => t.is_completed, name: "IDX_COMPLETED");
            
            CreateTable(
                "fixture_statistic",
                c => new
                    {
                        fixture_id = c.Int(nullable: false),
                        team_id = c.Int(nullable: false, storeType: "mediumint"),
                        shots_total = c.Int(nullable: false, storeType: "mediumint"),
                        shots_on_goal = c.Int(nullable: false, storeType: "mediumint"),
                        shots_off_goal = c.Int(nullable: false, storeType: "mediumint"),
                        shots_blocked = c.Int(nullable: false, storeType: "mediumint"),
                        shots_inside_box = c.Int(nullable: false, storeType: "mediumint"),
                        shots_outside_box = c.Int(nullable: false, storeType: "mediumint"),
                        goalkeeper_saves = c.Int(nullable: false, storeType: "mediumint"),
                        fouls = c.Int(nullable: false, storeType: "mediumint"),
                        corner_kicks = c.Int(nullable: false, storeType: "mediumint"),
                        offsides = c.Int(nullable: false, storeType: "mediumint"),
                        yellow_cards = c.Int(nullable: false, storeType: "mediumint"),
                        red_cards = c.Int(nullable: false, storeType: "mediumint"),
                        ball_possessions = c.Int(nullable: false, storeType: "mediumint"),
                        passes_total = c.Int(nullable: false, storeType: "mediumint"),
                        passes_accurate = c.Int(nullable: false, storeType: "mediumint"),
                        upt_time = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => new { t.fixture_id, t.team_id })                ;
            
        }
        
        public override void Down()
        {
            DropIndex("fixture", "IDX_COMPLETED");
            DropIndex("fixture", "IDX_PREDICTED");
            DropIndex("fixture", "IDX_AWAY_DATE");
            DropIndex("fixture", "IDX_HOME_DATE");
            DropIndex("fixture", "IDX_LEAGUE_DATE");
            DropTable("fixture_statistic");
            DropTable("fixture");
        }
    }
}
