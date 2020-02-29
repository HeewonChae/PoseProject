using Acr.UserDialogs;
using System;
using System.Threading.Tasks;
using Xamarin_Tutorial.InfraStructure;
using Xamarin_Tutorial.Utilities;

namespace Xamarin_Tutorial.Services
{
	public static class SQLiteService
	{
		public static async Task DeleteAllAsync<T>() where T : ISQLiteData, new()
		{
			try
			{
				using (var con = new SQLiteStorage<T>())
				{
					await con.DeleteAllAsync();
				}
			}
			catch (Exception ex)
			{
				await UserDialogs.Instance.AlertAsync(ex.Message);
			}
		}

		public static async Task InsertOrUpdateAsync<T>(T model) where T : ISQLiteData, new()
		{
			try
			{
				using (var con = new SQLiteStorage<T>())
				{
					var foundRecord = await con.SelectAsync(model.Id);
					if (foundRecord != null)
					{
						await con.UpdateAsync(model);
					}
					else
					{
						await con.InsertAsync(model);
					}
				}
			}
			catch (Exception ex)
			{
				await UserDialogs.Instance.AlertAsync(ex.Message);
			}
		}

		public static async Task<T> FindAsync<T>(int pk) where T : ISQLiteData, new()
		{
			try
			{
				using (var con = new SQLiteStorage<T>())
				{
					return await con.SelectAsync(pk);
				}
			}
			catch (Exception ex)
			{
				await UserDialogs.Instance.AlertAsync(ex.Message);
			}

			return default;
		}

		public static async Task<T> FirstAsync<T>() where T : ISQLiteData, new()
		{
			try
			{
				using (var con = new SQLiteStorage<T>())
				{
					return await con.FirstAsync();
				}
			}
			catch (Exception ex)
			{
				await UserDialogs.Instance.AlertAsync(ex.Message);
			}

			return default;
		}
	}
}