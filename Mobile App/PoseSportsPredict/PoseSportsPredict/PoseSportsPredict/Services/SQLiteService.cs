using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Utilities.SQLite.SQLiteConnection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XF.Material.Forms.UI.Dialogs;

namespace PoseSportsPredict.Services
{
    public class SQLiteService : ISQLiteService
    {
        private readonly SQLiteContext _sqliteContext;

        public SQLiteService()
        {
            _sqliteContext = new SQLiteContext();
        }

        public async Task DeleteAllAsync<T>() where T : ISQLiteStorable, new()
        {
            try
            {
                await _sqliteContext.DeleteAllAsync<T>();
            }
            catch (Exception ex)
            {
                await MaterialDialog.Instance.AlertAsync(ex.Message,
                    LocalizeString.App_Title,
                    LocalizeString.Ok,
                    DialogConfiguration.AppTitleAlterDialogConfiguration);
                throw;
            }
        }

        public async Task<int> DeleteAsync<T>(string pk) where T : ISQLiteStorable, new()
        {
            try
            {
                return await _sqliteContext.DeleteAsync<T>(pk);
            }
            catch (Exception ex)
            {
                await MaterialDialog.Instance.AlertAsync(ex.Message,
                    LocalizeString.App_Title,
                    LocalizeString.Ok,
                    DialogConfiguration.AppTitleAlterDialogConfiguration);
                throw;
            }
        }

        public async Task<int> InsertOrUpdateAsync<T>(T model) where T : ISQLiteStorable, new()
        {
            try
            {
                var found = await _sqliteContext.SelectAsync<T>(model.PrimaryKey);
                if (found != null)
                {
                    return await _sqliteContext.UpdateAsync(model);
                }
                else
                {
                    return await _sqliteContext.InsertAsync(model);
                }
            }
            catch (Exception ex)
            {
                await MaterialDialog.Instance.AlertAsync(ex.Message,
                    LocalizeString.App_Title,
                    LocalizeString.Ok,
                    DialogConfiguration.AppTitleAlterDialogConfiguration);
                throw;
            }
        }

        public async Task<List<T>> SelectAllAsync<T>() where T : ISQLiteStorable, new()
        {
            try
            {
                return await _sqliteContext.SelectAllAsync<T>();
            }
            catch (Exception ex)
            {
                await MaterialDialog.Instance.AlertAsync(ex.Message,
                    LocalizeString.App_Title,
                    LocalizeString.Ok,
                    DialogConfiguration.AppTitleAlterDialogConfiguration);
                throw;
            }
        }

        public async Task<T> SelectAsync<T>(string pk) where T : ISQLiteStorable, new()
        {
            try
            {
                return await _sqliteContext.SelectAsync<T>(pk);
            }
            catch (Exception ex)
            {
                await MaterialDialog.Instance.AlertAsync(ex.Message,
                    LocalizeString.App_Title,
                    LocalizeString.Ok,
                    DialogConfiguration.AppTitleAlterDialogConfiguration);
                throw;
            }
        }
    }
}