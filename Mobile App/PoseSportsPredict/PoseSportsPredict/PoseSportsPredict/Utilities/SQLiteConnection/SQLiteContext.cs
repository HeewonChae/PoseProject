using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Models.Football;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PoseSportsPredict.Utilities.SQLiteConnection
{
    public class SQLiteContext<T> : IDisposable where T : ISQLiteStorable, new()
    {
        private static readonly SQLiteAsyncConnection Connection;

        static SQLiteContext()
        {
            if (string.IsNullOrEmpty(AppConfig.SQLiteServicePath) || string.IsNullOrEmpty(AppConfig.SQLiteScheme))
                throw new ArgumentException("Invalidate SQLiteConfig Path, FileName");

            Connection = new SQLiteAsyncConnection(Path.Combine(AppConfig.SQLiteServicePath, AppConfig.SQLiteScheme));

            Connection.CreateTableAsync<FootballMatchInfo>().Wait();
            Connection.CreateTableAsync<FootballLeagueInfo>().Wait();
            Connection.CreateTableAsync<FootballTeamInfo>().Wait();
        }

        public SQLiteContext()
        {
        }

        public async Task<int> InsertAsync(T model)
        {
            return await Connection.InsertAsync(model);
        }

        public async Task<int> InsertAllAsync(IEnumerable<T> models)
        {
            return await Connection.InsertAllAsync(models);
        }

        public async Task<int> UpdateAsync(T model)
        {
            return await Connection.UpdateAsync(model);
        }

        public async Task<int> UpdateAllAsync(IEnumerable<T> models)
        {
            return await Connection.UpdateAllAsync(models);
        }

        public async Task<int> DeleteAsync(T model)
        {
            return await Connection.DeleteAsync(model);
        }

        public async Task<int> DeleteAllAsync()
        {
            return await Connection.DeleteAllAsync<T>();
        }

        public async Task<T> FirstAsync()
        {
            return await Connection.Table<T>().FirstOrDefaultAsync();
        }

        public async Task<List<T>> SelectAllAsync()
        {
            return await Connection.Table<T>().ToListAsync();
        }

        public async Task<T> SelectAsync(string primaryKey)
        {
            return await Connection.Table<T>().Where(m => m.PrimaryKey == primaryKey).FirstOrDefaultAsync();
        }

        public void Dispose()
        {
            //_connection.CloseAsync();
        }
    }
}