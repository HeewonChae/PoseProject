namespace Repository.Mysql.FootballDBMigrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Initialize : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "country",
                c => new
                    {
                        code = c.String(nullable: false, maxLength: 4, fixedLength: true, unicode: false, storeType: "char"),
                        name = c.String(nullable: false, maxLength: 32, unicode: false),
                        flag = c.String(maxLength: 64, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "")
                                },
                            }),
                        upt_time = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => new { t.code, t.name })                
                .Index(t => t.name, name: "IDX_COUNTRY_NAME");
            
            CreateTable(
                "coverage",
                c => new
                    {
                        league_id = c.Int(nullable: false, storeType: "mediumint"),
                        standings = c.Boolean(nullable: false),
                        odds = c.Boolean(nullable: false),
                        fixture_statistics = c.Boolean(nullable: false),
                        players = c.Boolean(nullable: false),
                        lineups = c.Boolean(nullable: false),
                        predictions = c.Boolean(nullable: false),
                        upt_time = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.league_id)                ;
            
            CreateTable(
                "league",
                c => new
                    {
                        id = c.Int(nullable: false, storeType: "mediumint"),
                        name = c.String(maxLength: 64, unicode: false),
                        type = c.String(maxLength: 8, unicode: false),
                        country_name = c.String(nullable: false, maxLength: 32, unicode: false),
                        is_current = c.Boolean(nullable: false),
                        season_start = c.DateTime(nullable: false, precision: 0),
                        season_end = c.DateTime(nullable: false, precision: 0),
                        logo = c.String(maxLength: 64, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "")
                                },
                            }),
                        upt_time = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.id)                
                .Index(t => t.name, name: "IDX_LEAGUE_NAME")
                .Index(t => t.country_name, name: "IDX_COUNTRY_NAME")
                .Index(t => t.season_end, name: "IDX_SEASON_END");
            
            CreateTable(
                "standing",
                c => new
                    {
                        league_id = c.Int(nullable: false, storeType: "mediumint"),
                        team_id = c.Int(nullable: false, storeType: "mediumint"),
                        group = c.String(nullable: false, maxLength: 64, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "")
                                },
                            }),
                        rank = c.Int(nullable: false, storeType: "mediumint"),
                        forme = c.String(maxLength: 8, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "")
                                },
                            }),
                        points = c.Int(nullable: false, storeType: "mediumint"),
                        description = c.String(maxLength: 128, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "")
                                },
                            }),
                        played = c.Int(nullable: false, storeType: "mediumint"),
                        win = c.Int(nullable: false, storeType: "mediumint"),
                        draw = c.Int(nullable: false, storeType: "mediumint"),
                        lose = c.Int(nullable: false, storeType: "mediumint"),
                        goals_for = c.Int(nullable: false, storeType: "mediumint"),
                        goals_against = c.Int(nullable: false, storeType: "mediumint"),
                        upt_time = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => new { t.league_id, t.team_id, t.group })                
                .Index(t => t.league_id, name: "IDX_LEAGUE_ID")
                .Index(t => t.team_id, name: "IDX_TEAM_ID");
            
            CreateTable(
                "team",
                c => new
                    {
                        id = c.Int(nullable: false, storeType: "mediumint"),
                        name = c.String(maxLength: 64, unicode: false),
                        country_name = c.String(nullable: false, maxLength: 32, unicode: false),
                        logo = c.String(maxLength: 64, unicode: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "")
                                },
                            }),
                        upt_time = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.id)                
                .Index(t => t.name, name: "IDX_TEAM_NAME")
                .Index(t => t.country_name, name: "IDX_COUNTRY_NAME");
            
        }
        
        public override void Down()
        {
            DropIndex("team", "IDX_COUNTRY_NAME");
            DropIndex("team", "IDX_TEAM_NAME");
            DropIndex("standing", "IDX_TEAM_ID");
            DropIndex("standing", "IDX_LEAGUE_ID");
            DropIndex("league", "IDX_SEASON_END");
            DropIndex("league", "IDX_COUNTRY_NAME");
            DropIndex("league", "IDX_LEAGUE_NAME");
            DropIndex("country", "IDX_COUNTRY_NAME");
            DropTable("team");
            DropTable("standing");
            DropTable("league");
            DropTable("coverage");
            DropTable("country");
        }
    }
}
