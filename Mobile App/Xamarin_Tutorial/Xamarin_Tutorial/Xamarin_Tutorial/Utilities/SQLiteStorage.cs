using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin_Tutorial.InfraStructure;

namespace Xamarin_Tutorial.Utilities
{
	public static class SQLiteConfig
	{
		public static string Path { get; set; }
		public static string FileName { get; set; }
	}

	public class SQLiteStorage<T> : IDisposable where T : ISQLiteData, new()
	{
		private readonly SQLiteAsyncConnection _connection;

		public SQLiteStorage()
		{
			if (string.IsNullOrEmpty(SQLiteConfig.Path) || string.IsNullOrEmpty(SQLiteConfig.FileName))
				throw new ArgumentException("Invalidate SQLiteConfig Path, FileName");

			_connection = new SQLiteAsyncConnection(Path.Combine(SQLiteConfig.Path, SQLiteConfig.FileName));
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

		public async Task<T> SelectAsync(int pk)
		{
			return await _connection.Table<T>().Where(m => m.Id == pk).FirstOrDefaultAsync();
		}

		public void Dispose()
		{
			_connection.CloseAsync();
		}
	}
}