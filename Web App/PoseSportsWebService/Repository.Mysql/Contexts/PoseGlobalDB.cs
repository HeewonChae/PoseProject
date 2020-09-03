using Repository.Mysql.PoseGlobalDB.Tables;
using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace Repository.Mysql.Contexts
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class PoseGlobalDB : DbContext
    {
        public PoseGlobalDB()
            : base("pose_globaldb")
        {
        }

        public PoseGlobalDB(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
        }

        public static string GetConnectionName()
        {
            return "pose_globaldb";
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
        public virtual DbSet<UserBase> userBase { get; set; }

        public virtual DbSet<UserRole> userRole { get; set; }
        public virtual DbSet<InAppBilling> inAppBilling { get; set; }
    }
}