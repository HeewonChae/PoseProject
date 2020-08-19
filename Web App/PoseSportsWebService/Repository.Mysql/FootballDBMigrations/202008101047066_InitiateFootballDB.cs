﻿namespace Repository.Mysql.FootballDBMigrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class InitiateFootballDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "country",
                c => new
                    {
                        code = c.String(nullable: false, maxLength: 4, fixedLength: true, unicode: false, storeType: "char"),
                        name = c.String(nullable: false, maxLength: 32, unicode: false),
                        logo = c.String(maxLength: 100, unicode: false,
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
                "league_coverage",
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
                .PrimaryKey(t => t.league_id)                
                .Index(t => t.predictions, name: "IDX_PREDICT");
            
            CreateTable(
                "fixture",
                c => new
                    {
                        id = c.Int(nullable: false),
                        league_id = c.Int(nullable: false, storeType: "mediumint"),
                        home_team_id = c.Int(nullable: false, storeType: "mediumint"),
                        away_team_id = c.Int(nullable: false, storeType: "mediumint"),
                        match_time = c.DateTime(nullable: false, precision: 0),
                        round = c.String(nullable: false, maxLength: 64, unicode: false),
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
                        is_recommended = c.Boolean(nullable: false, defaultValueSql: "0",
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        upt_time = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.id)                
                .Index(t => new { t.league_id, t.away_team_id }, name: "IDX_LEAGUE_AWAY")
                .Index(t => new { t.league_id, t.match_time }, name: "IDX_LEAGUE_DATE")
                .Index(t => new { t.league_id, t.home_team_id }, name: "IDX_LEAGUE_HOME")
                .Index(t => new { t.home_team_id, t.away_team_id, t.match_time }, name: "IDX_HOME_AWAY_DATE")
                .Index(t => new { t.home_team_id, t.match_time }, name: "IDX_HOME_DATE")
                .Index(t => new { t.away_team_id, t.match_time }, name: "IDX_AWAY_DATE")
                .Index(t => new { t.is_completed, t.match_time }, name: "IDX_COMPLETED_DATE")
                .Index(t => t.match_time, name: "IDX_MATCH_TIME")
                .Index(t => new { t.is_predicted, t.match_time }, name: "IDX_PREDICTED_DATE");
            
            CreateTable(
                "fixture_statistics",
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
            
            CreateTable(
                "league",
                c => new
                    {
                        id = c.Int(nullable: false, storeType: "mediumint"),
                        name = c.String(nullable: false, maxLength: 64, unicode: false),
                        type = c.String(nullable: false, maxLength: 8, unicode: false),
                        country_name = c.String(nullable: false, maxLength: 32, unicode: false),
                        is_current = c.Boolean(nullable: false),
                        season_start = c.DateTime(nullable: false, precision: 0),
                        season_end = c.DateTime(nullable: false, precision: 0),
                        logo = c.String(maxLength: 100, unicode: false,
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
                .Index(t => new { t.country_name, t.name }, name: "IDX_COUNTRY_LEAGUE");
            
            CreateTable(
                "odds",
                c => new
                    {
                        fixture_id = c.Int(nullable: false),
                        bookmaker_type = c.String(nullable: false, maxLength: 32, unicode: false),
                        label_type = c.String(nullable: false, maxLength: 16, unicode: false),
                        subtitle_1 = c.String(nullable: false, maxLength: 8, unicode: false,
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
                        subtitle_2 = c.String(nullable: false, maxLength: 8, unicode: false,
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
                        subtitle_3 = c.String(nullable: false, maxLength: 8, unicode: false,
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
                .PrimaryKey(t => new { t.fixture_id, t.bookmaker_type, t.label_type })                ;
            
            CreateTable(
                "prediction",
                c => new
                    {
                        fixture_id = c.Int(nullable: false),
                        pred_seq = c.Int(nullable: false, storeType: "mediumint"),
                        main_label = c.Int(nullable: false, storeType: "mediumint"),
                        sub_label = c.Int(nullable: false, storeType: "mediumint"),
                        value1 = c.Int(nullable: false, defaultValueSql: "0",
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        value2 = c.Int(nullable: false, defaultValueSql: "0",
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        value3 = c.Int(nullable: false, defaultValueSql: "0",
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        value4 = c.Int(nullable: false, defaultValueSql: "0",
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "0")
                                },
                            }),
                        grade = c.Int(nullable: false, storeType: "mediumint"),
                        is_recommended = c.Boolean(nullable: false),
                        is_hit = c.Boolean(nullable: false),
                        upt_time = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => new { t.fixture_id, t.pred_seq })                
                .Index(t => new { t.fixture_id, t.main_label }, name: "IDX_ID_MAIN_LABEL");
            
            CreateTable(
                "standings",
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
                .PrimaryKey(t => new { t.league_id, t.team_id, t.group })                ;
            
            CreateTable(
                "team",
                c => new
                    {
                        id = c.Int(nullable: false, storeType: "mediumint"),
                        name = c.String(nullable: false, maxLength: 64, unicode: false),
                        country_name = c.String(nullable: false, maxLength: 32, unicode: false),
                        logo = c.String(nullable: false, maxLength: 100, unicode: false,
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
                .Index(t => new { t.name, t.country_name }, name: "IDX_NAME_COUNTRY")
                .Index(t => t.country_name, name: "IDX_COUNTRY_NAME");
            
        }
        
        public override void Down()
        {
            DropIndex("team", "IDX_COUNTRY_NAME");
            DropIndex("team", "IDX_NAME_COUNTRY");
            DropIndex("prediction", "IDX_ID_MAIN_LABEL");
            DropIndex("league", "IDX_COUNTRY_LEAGUE");
            DropIndex("fixture", "IDX_PREDICTED_DATE");
            DropIndex("fixture", "IDX_MATCH_TIME");
            DropIndex("fixture", "IDX_COMPLETED_DATE");
            DropIndex("fixture", "IDX_AWAY_DATE");
            DropIndex("fixture", "IDX_HOME_DATE");
            DropIndex("fixture", "IDX_HOME_AWAY_DATE");
            DropIndex("fixture", "IDX_LEAGUE_HOME");
            DropIndex("fixture", "IDX_LEAGUE_DATE");
            DropIndex("fixture", "IDX_LEAGUE_AWAY");
            DropIndex("league_coverage", "IDX_PREDICT");
            DropIndex("country", "IDX_COUNTRY_NAME");
            DropTable("team");
            DropTable("standings");
            DropTable("prediction");
            DropTable("odds");
            DropTable("league");
            DropTable("fixture_statistics");
            DropTable("fixture");
            DropTable("league_coverage");
            DropTable("country");
        }
    }
}