using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Utilities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.Utilities
{
    // https://bugs.mysql.com/bug.php?id=73278 버그 수정판
    public class FixedMySqlMigrationCodeGenerator : MySql.Data.Entity.MySqlMigrationCodeGenerator
    {
        private string TrimSchemaPrefix(string table)
        {
            if (table.StartsWith("dbo."))
                return table.Replace("dbo.", "");

            return table;
        }

        protected override void GenerateInline(CreateIndexOperation createIndexOperation, IndentedTextWriter writer)
        {
            writer.WriteLine();
            writer.Write(".Index(");

            Generate(createIndexOperation.Columns, writer);

            writer.Write(createIndexOperation.IsUnique ? ", unique: true" : "");
            writer.Write(!createIndexOperation.HasDefaultName ? string.Format(", name: \"{0}\"", TrimSchemaPrefix(createIndexOperation.Name)) : "");

            writer.Write(")");
        }

        protected override void Generate(AddColumnOperation addColumnOperation, IndentedTextWriter writer)
        {
            SetAnnotatedColumn(addColumnOperation.Column, writer);
            base.Generate(addColumnOperation, writer);
        }

        protected override void Generate(AlterColumnOperation alterColumnOperation, IndentedTextWriter writer)
        {
            SetAnnotatedColumn(alterColumnOperation.Column, writer);
            base.Generate(alterColumnOperation, writer);
        }

        protected override void Generate(CreateTableOperation createTableOperation, IndentedTextWriter writer)
        {
            SetAnnotatedColumns(createTableOperation.Columns, writer);
            base.Generate(createTableOperation, writer);
        }

        protected override void Generate(AlterTableOperation alterTableOperation, IndentedTextWriter writer)
        {
            SetAnnotatedColumns(alterTableOperation.Columns, writer);
            base.Generate(alterTableOperation, writer);
        }

        private void SetAnnotatedColumn(ColumnModel col, IndentedTextWriter writer)
        {
            AnnotationValues values;
            if (col.Annotations.TryGetValue("SqlDefaultValue", out values))
            {
                col.DefaultValueSql = (string)values.NewValue;
            }
        }

        private void SetAnnotatedColumns(IEnumerable<ColumnModel> columns, IndentedTextWriter writer)
        {
            foreach (var column in columns)
            {
                SetAnnotatedColumn(column, writer);
            }
        }
    }
}