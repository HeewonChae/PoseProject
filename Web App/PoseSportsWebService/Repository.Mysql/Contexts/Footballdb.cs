using Repository.Mysql.FootballDB.Tables;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.Contexts
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class FootballDB : DbContext
    {
        public FootballDB()
            : base("footballdb")
        {
        }

        public FootballDB(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
        }

        public static string GetConnectionName()
        {
            return "footballdb";
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(new AttributeToColumnAnnotationConvention<SqlDefaultValueAttribute, string>("SqlDefaultValue", (p, attributes) => attributes.Single().DefaultValue));
        }

        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
        public class SqlDefaultValueAttribute : Attribute
        {
            public string DefaultValue { get; set; }
        }

        // tables
        public virtual DbSet<Country> Country { get; set; }

        public virtual DbSet<League> League { get; set; }
        public virtual DbSet<LeagueCoverage> Coverage { get; set; }
        public virtual DbSet<Team> Team { get; set; }
        public virtual DbSet<Standings> Standing { get; set; }
        public virtual DbSet<Fixture> Fixture { get; set; }
        public virtual DbSet<FixtureStatistics> FixtureStatistic { get; set; }
        public virtual DbSet<Odds> Odds { get; set; }
        public virtual DbSet<Prediction> Predict { get; set; }
    }
}