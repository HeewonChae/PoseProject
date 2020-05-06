using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Models;
using PoseSportsPredict.Models.Enums;
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

        Task<bool> AddBookmark<T>(T item, SportsType sportsType, BookMarkType bookmarkType) where T : ISQLiteStorable, IBookmarkable, new();

        Task<bool> RemoveBookmark<T>(T item, SportsType sportsType, BookMarkType bookmarkType) where T : ISQLiteStorable, IBookmarkable, new();

        Task<bool> UpdateBookmark<T>(T item) where T : ISQLiteStorable, new();
    }
}