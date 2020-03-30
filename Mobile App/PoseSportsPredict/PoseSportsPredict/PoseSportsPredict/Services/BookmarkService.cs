using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PoseSportsPredict.Services
{
    public class BookmarkService : IBookmarkService
    {
        private ISQLiteService _sqliteService;

        public BookmarkService(ISQLiteService sqliteService)
        {
            _sqliteService = sqliteService;
        }

        public async Task<int> AddBookmark<T>(T item, SportsType sportsType, BookMarkType bookmarkType) where T : ISQLiteStorable, new()
        {
            var ret = await _sqliteService.InsertOrUpdateAsync<T>(item);
            Debug.Assert(ret != 0);

            string message = this.BuildBookmarkMessage(sportsType, bookmarkType);
            MessagingCenter.Send(this, message, item);

            return ret;
        }

        public async Task<int> UpdateBookmark<T>(T item) where T : ISQLiteStorable, new()
        {
            var ret = await _sqliteService.InsertOrUpdateAsync<T>(item);
            Debug.Assert(ret != 0);

            return ret;
        }

        public async Task<int> RemoveBookmark<T>(T item, SportsType sportsType, BookMarkType bookmarkType) where T : ISQLiteStorable, new()
        {
            var ret = await _sqliteService.DeleteAsync<T>(item.PrimaryKey);
            Debug.Assert(ret != 0);

            string message = this.BuildBookmarkMessage(sportsType, bookmarkType);
            MessagingCenter.Send(this, message, item);

            return ret;
        }

        public async Task<List<T>> GetAllBookmark<T>() where T : ISQLiteStorable, new()
        {
            return await _sqliteService.SelectAllAsync<T>();
        }

        public async Task<T> GetBookmark<T>(string pk) where T : ISQLiteStorable, new()
        {
            return await _sqliteService.SelectAsync<T>(pk);
        }
    }
}