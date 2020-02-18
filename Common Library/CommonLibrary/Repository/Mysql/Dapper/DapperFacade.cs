using LogicCore.Debug;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.Dapper
{
	public static class DapperFacade
	{
		delegate object ConstructorDelegate(params object[] args);

		static ConstructorDelegate CreateDBContextConstructor(Type type, params Type[] parameters)
		{
			var constructorInfo = type.GetConstructor(parameters);
			var paramExpr = Expression.Parameter(typeof(Object[]));

			var constructorParameters = parameters.Select((paramType, index) =>
				Expression.Convert(
					Expression.ArrayAccess(
						paramExpr,
						Expression.Constant(index)),
					paramType)).ToArray();

			var body = Expression.New(constructorInfo, constructorParameters);

			var constructor = Expression.Lambda<ConstructorDelegate>(body, paramExpr);
			return constructor.Compile();
		}

		private static readonly ConcurrentDictionary<Type, ConstructorDelegate> _contextConstructors
			= new ConcurrentDictionary<Type, ConstructorDelegate>();

		public static void DoWithDBContext<TDBContext>(string shard,
										Action<TDBContext> businessWorker,
										Action<EntityStatus> errorEvent = null) where TDBContext : DbContext, new()
		{
			var connectionString = shard;
			if (string.IsNullOrEmpty(shard))
			{
				var connectionName = typeof(TDBContext).GetMethod("GetConnectionName").Invoke(null, null) as string;
				connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
			}

			MySqlConnectionStringBuilder connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString);
			var oldDatabase = connectionStringBuilder.Database;
			connectionStringBuilder.Remove("database");

			using (MySqlConnection dbConnection = new MySqlConnection(connectionStringBuilder.ToString()))
			{
				try
				{
					dbConnection.Open();
					dbConnection.ChangeDatabase(oldDatabase);

					if (false == _contextConstructors.TryGetValue(typeof(TDBContext), out var contextConstructor))
					{
						contextConstructor = CreateDBContextConstructor(typeof(TDBContext), new Type[] { typeof(DbConnection), typeof(bool) });
						_contextConstructors.TryAdd(typeof(TDBContext), contextConstructor);
					}

					var dbContext = contextConstructor(dbConnection, false) as TDBContext;
					businessWorker(dbContext);
				}
				catch (DbEntityValidationException ex)
				{
					errorEvent?.Invoke(new EntityStatus(ex));

				}
				catch (DbUpdateException ex)
				{
					errorEvent?.Invoke(new EntityStatus(ex));
				}
				catch (MySqlException ex)
				{
					errorEvent?.Invoke(new EntityStatus(ex));
				}
				catch (Exception ex)
				{
					errorEvent?.Invoke(new EntityStatus(ex));
					Dev.Assert(false, ex.StackTrace);
				}
				finally
				{
					dbConnection.Close();
				}
			}
		}

		public static void DoWithDBContext<TDBContext1, TDBContext2>(
			string shard1,
			string shard2,
			Action<TDBContext1, TDBContext2> businessWorker,
			Action<EntityStatus> errorEvent = null) where TDBContext1 : DbContext where TDBContext2 : DbContext
		{
			var connectionString1 = shard1;
			if (string.IsNullOrEmpty(shard1))
			{
				var connectionName = typeof(TDBContext1).GetMethod("GetConnectionName").Invoke(null, null) as string;
				connectionString1 = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
			}

			var connectionString2 = shard2;
			if (string.IsNullOrEmpty(shard2))
			{
				var connectionName = typeof(TDBContext2).GetMethod("GetConnectionName").Invoke(null, null) as string;
				connectionString2 = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
			}

			MySqlConnectionStringBuilder connectionStringBuilder1 = new MySqlConnectionStringBuilder(connectionString1);
			var oldDatabase1 = connectionStringBuilder1.Database;
			connectionStringBuilder1.Remove("database");
			MySqlConnectionStringBuilder connectionStringBuilder2 = new MySqlConnectionStringBuilder(connectionString2);
			var oldDatabase2 = connectionStringBuilder2.Database;
			connectionStringBuilder2.Remove("database");

			using (MySqlConnection dbConnection1 = new MySqlConnection(connectionStringBuilder1.ToString()))
			using (MySqlConnection dbConnection2 = new MySqlConnection(connectionStringBuilder2.ToString()))
			{
				try
				{
					dbConnection1.Open();
					dbConnection2.Open();
					dbConnection1.ChangeDatabase(oldDatabase1);
					dbConnection2.ChangeDatabase(oldDatabase2);

					if (false == _contextConstructors.TryGetValue(typeof(TDBContext1), out var contextConstructor1))
					{
						contextConstructor1 = CreateDBContextConstructor(typeof(TDBContext1), new Type[] { typeof(DbConnection), typeof(bool) });
						_contextConstructors.TryAdd(typeof(TDBContext1), contextConstructor1);
					}

					var dbContext1 = contextConstructor1(dbConnection1, false) as TDBContext1;

					if (false == _contextConstructors.TryGetValue(typeof(TDBContext2), out var contextConstructor2))
					{
						contextConstructor2 = CreateDBContextConstructor(typeof(TDBContext2), new Type[] { typeof(DbConnection), typeof(bool) });
						_contextConstructors.TryAdd(typeof(TDBContext2), contextConstructor2);
					}
					var dbContext2 = contextConstructor2(dbConnection2, false) as TDBContext2;

					businessWorker(dbContext1, dbContext2);
				}
				catch (DbEntityValidationException ex)
				{
					errorEvent?.Invoke(new EntityStatus(ex));

				}
				catch (DbUpdateException ex)
				{
					errorEvent?.Invoke(new EntityStatus(ex));
				}
				catch (MySqlException ex)
				{
					errorEvent?.Invoke(new EntityStatus(ex));
				}
				catch (Exception ex)
				{
					errorEvent?.Invoke(new EntityStatus(ex));
					Dev.Assert(false, ex.StackTrace);
				}
				finally
				{
					dbConnection2.Close();
					dbConnection1.Close();
				}
			}
		}
	}
}
