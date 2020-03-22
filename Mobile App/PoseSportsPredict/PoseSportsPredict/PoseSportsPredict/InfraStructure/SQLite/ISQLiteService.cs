using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoseSportsPredict.InfraStructure.SQLite
{
    public interface ISQLiteService
    {
        Task<T> SelectFirstAsync<T>() where T : ISQLiteStorable, new();

        Task<T> SelectAsync<T>(string pk) where T : ISQLiteStorable, new();

        Task<List<T>> SelectAllAsync<T>() where T : ISQLiteStorable, new();

        Task InsertOrUpdateAsync<T>(T model) where T : ISQLiteStorable, new();

        Task DeleteAsync<T>(string pk) where T : ISQLiteStorable, new();

        Task DeleteAllAsync<T>() where T : ISQLiteStorable, new();
    }
}