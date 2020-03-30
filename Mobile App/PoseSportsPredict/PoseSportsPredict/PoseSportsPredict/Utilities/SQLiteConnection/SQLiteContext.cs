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
    public class SQLiteContext : IDisposable
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

        public async Task<int> InsertAsync<T>(T model) where T : ISQLiteStorable, new()
        {
            return await Connection.InsertAsync(model);
        }

        public async Task<int> InsertAllAsync<T>(IEnumerable<T> models) where T : ISQLiteStorable, new()
        {
            return await Connection.InsertAllAsync(models);
        }

        public async Task<int> UpdateAsync<T>(T model) where T : ISQLiteStorable, new()
        {
            return await Connection.UpdateAsync(model);
        }

        public async Task<int> UpdateAllAsync<T>(IEnumerable<T> models) where T : ISQLiteStorable, new()
        {
            return await Connection.UpdateAllAsync(models);
        }

        public async Task<int> DeleteAsync<T>(string pk) where T : ISQLiteStorable, new()
        {
            return await Connection.DeleteAsync<T>(pk);
        }

        public async Task<int> DeleteAllAsync<T>() where T : ISQLiteStorable, new()
        {
            return await Connection.DeleteAllAsync<T>();
        }

        public async Task<T> FirstAsync<T>() where T : ISQLiteStorable, new()
        {
            return await Connection.Table<T>().FirstOrDefaultAsync();
        }

        public async Task<List<T>> SelectAllAsync<T>() where T : ISQLiteStorable, new()
        {
            return await Connection.Table<T>().ToListAsync();
        }

        public async Task<T> SelectAsync<T>(string primaryKey) where T : ISQLiteStorable, new()
        {
            return await Connection.Table<T>().FirstOrDefaultAsync(m => m.PrimaryKey == primaryKey);
        }

        public void Dispose()
        {
            //_connection.CloseAsync();
        }
    }
}