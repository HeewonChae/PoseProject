using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql
{
    public static class Extensions
    {
        public static IEnumerable<T> QuerySQL<T>(this DbContext dbcontext, string sql, object param = null)
        {
            return dbcontext.Database.Connection.Query<T>(sql, param: param, transaction: dbcontext.Database.CurrentTransaction?.UnderlyingTransaction);
        }

        public static IEnumerable<T> QuerySP<T>(this DbContext dbcontext, string sql, DynamicParameters param = null)
        {
            return dbcontext.Database.Connection.Query<T>(sql, param: param, transaction: dbcontext.Database.CurrentTransaction?.UnderlyingTransaction,
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(this DbContext dbcontext, string sql, object param = null)
        {
            return await dbcontext.Database.Connection.QueryAsync<T>(sql, param: param, transaction: dbcontext.Database.CurrentTransaction?.UnderlyingTransaction);
        }

        public static IEnumerable<dynamic> Query(this DbContext dbcontext, string sql, object param = null)
        {
            return dbcontext.Database.Connection.Query(sql, param: param, transaction: dbcontext.Database.CurrentTransaction?.UnderlyingTransaction);
        }

        public static async Task<IEnumerable<dynamic>> QueryAsync(this DbContext dbcontext, string sql, object param = null)
        {
            return await dbcontext.Database.Connection.QueryAsync(sql, param: param, transaction: dbcontext.Database.CurrentTransaction?.UnderlyingTransaction);
        }

        public static int ExecuteSQL(this DbContext dbcontext, string sql, object param = null)
        {
            return dbcontext.Database.Connection.Execute(sql, param: param, transaction: dbcontext.Database.CurrentTransaction?.UnderlyingTransaction);
        }

        public static int ExecuteSP(this DbContext dbcontext, string sql, DynamicParameters param = null)
        {
            return dbcontext.Database.Connection.Execute(sql, param: param, transaction: dbcontext.Database.CurrentTransaction?.UnderlyingTransaction,
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public static async Task<int> ExecuteAsync(this DbContext dbcontext, string sql, object param = null)
        {
            return await dbcontext.Database.Connection.ExecuteAsync(sql, param: param, transaction: dbcontext.Database.CurrentTransaction?.UnderlyingTransaction);
        }

        public static T ExecuteScalar<T>(this DbContext dbcontext, string sql, object param = null)
        {
            return dbcontext.Database.Connection.ExecuteScalar<T>(sql, param: param, transaction: dbcontext.Database.CurrentTransaction?.UnderlyingTransaction);
        }

        public static T ExecuteScalarSP<T>(this DbContext dbcontext, string sql, DynamicParameters param = null)
        {
            return dbcontext.Database.Connection.ExecuteScalar<T>(sql, param: param, transaction: dbcontext.Database.CurrentTransaction?.UnderlyingTransaction,
                commandType: System.Data.CommandType.StoredProcedure);
        }

        public static async Task<T> ExecuteScalarAsync<T>(this DbContext dbcontext, string sql, object param = null)
        {
            return await dbcontext.Database.Connection.ExecuteScalarAsync<T>(sql, param: param, transaction: dbcontext.Database.CurrentTransaction?.UnderlyingTransaction);
        }
    }
}