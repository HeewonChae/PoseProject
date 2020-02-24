using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin_Tutorial.InfraStructure;

namespace Xamarin_Tutorial.Utilities
{
	public class SQLiteStorage<T> : IDisposable where T : ISQLiteData, new()
	{
		private readonly SQLiteAsyncConnection _connection;

		public SQLiteStorage()
		{
			_connection = new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "xamarin_tutorial3.db3"));
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
			return await _connection.Table<T>().Where(m => m.ID == pk).FirstOrDefaultAsync();
		}

		public void Dispose()
		{
			_connection.CloseAsync();
		}
	}
}