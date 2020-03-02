using Acr.UserDialogs;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Utilities.SQLiteConnection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoseSportsPredict.Services
{
    public class SQLiteService : ISQLiteService
    {
        public async Task DeleteAllAsync<T>() where T : ISQLiteStorable, new()
        {
            try
            {
                using (var con = new SQLiteContext<T>())
                {
                    await con.DeleteAllAsync();
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message);
            }
        }

        public Task DeleteAsync<T>(int pk) where T : ISQLiteStorable, new()
        {
            throw new NotImplementedException();
        }

        public async Task InsertOrUpdateAsync<T>(T model) where T : ISQLiteStorable, new()
        {
            try
            {
                using (var con = new SQLiteContext<T>())
                {
                    var foundRecord = await con.SelectAsync(model.GetPrimaryKey());
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

        public Task<List<T>> SelectAllAsync<T>() where T : ISQLiteStorable, new()
        {
            throw new NotImplementedException();
        }

        public async Task<T> SelectAsync<T>(int pk) where T : ISQLiteStorable, new()
        {
            T result = default;

            try
            {
                using (var con = new SQLiteContext<T>())
                {
                    result = await con.SelectAsync(pk);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message);
            }

            return result;
        }

        public async Task<T> SelectFirstAsync<T>() where T : ISQLiteStorable, new()
        {
            T result = default;

            try
            {
                using (var con = new SQLiteContext<T>())
                {
                    result = await con.FirstAsync();
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message);
            }

            return result;
        }
    }
}