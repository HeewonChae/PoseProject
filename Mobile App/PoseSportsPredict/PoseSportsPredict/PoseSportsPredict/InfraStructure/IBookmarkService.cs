using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoseSportsPredict.InfraStructure
{
    public interface IBookmarkService
    {
        Task<List<T>> GetAllBookmark<T>() where T : ISQLiteStorable, new();

        Task<T> GetBookmark<T>(string pk) where T : ISQLiteStorable, new();

        Task<int> AddBookmark<T>(T item, SportsType sportsType, BookMarkType bookmarkType) where T : ISQLiteStorable, new();

        Task<int> RemoveBookmark<T>(T item, SportsType sportsType, BookMarkType bookmarkType) where T : ISQLiteStorable, new();

        Task<int> UpdateBookmark<T>(T item) where T : ISQLiteStorable, new();
    }
}