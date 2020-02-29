using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.InfraStructure.SQLite;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PoseSportsPredict.Logic.Utilities
{
	public class SQLiteContext<T> : IDisposable where T : ISQLiteStorable, new()
	{
		private readonly SQLiteAsyncConnection _connection;

		public SQLiteContext()
		{
			if (string.IsNullOrEmpty(AppConfig.SQLiteServicePath) || string.IsNullOrEmpty(AppConfig.SQLiteScheme))
				throw new ArgumentException("Invalidate SQLiteConfig Path, FileName");

			_connection = new SQLiteAsyncConnection(Path.Combine(AppConfig.SQLiteServicePath, AppConfig.SQLiteScheme));
			_connection.CreateTableAsync<T>().Wait();
		}

		public async Task<int> InsertAsync(T model)
		{
			return await _connection.InsertAsync(model);
		}

		public async Task<int> InsertAllAsync(IEnumerable<T> models)
		{
			return await _connection.InsertAllAsync(models);
		}

		public async Task<int> UpdateAsync(T model)
		{
			return await _connection.UpdateAsync(model);
		}

		public async Task<int> UpdateAllAsync(IEnumerable<T> models)
		{
			return await _connection.UpdateAllAsync(models);
		}

		public async Task<int> DeleteAsync(T model)
		{
			return await _connection.DeleteAsync(model);
		}

		public async Task<int> DeleteAllAsync()
		{
			return await _connection.DeleteAllAsync<T>();
		}

		public async Task<T> FirstAsync()
		{
			return await _connection.Table<T>().FirstOrDefaultAsync();
		}

		public async Task<List<T>> SelectAllAsync()
		{
			return await _connection.Table<T>().ToListAsync();
		}

		public async Task<T> SelectAsync(int primaryKey)
		{
			return await _connection.Table<T>().Where(m => m.GetPrimaryKey() == primaryKey).FirstOrDefaultAsync();
		}

		public void Dispose()
		{
			_connection.CloseAsync();
		}
	}
}