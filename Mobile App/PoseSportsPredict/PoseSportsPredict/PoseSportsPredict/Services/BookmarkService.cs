using Acr.UserDialogs;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Resources;
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

        public async Task<bool> AddBookmark<T>(T item, SportsType sportsType, BookMarkType bookmarkType) where T : ISQLiteStorable, IBookmarkable, new()
        {
            item.Order = 0;
            item.StoredTime = DateTime.UtcNow;
            item.IsBookmarked = true;

            var ret = await _sqliteService.InsertOrUpdateAsync<T>(item);
            Debug.Assert(ret != 0);

            string message = this.BuildBookmarkMessage(sportsType, bookmarkType);
            MessagingCenter.Send(this, message, item);

            UserDialogs.Instance.Toast(LocalizeString.Set_Bookmark);

            return true;
        }

        public async Task<bool> UpdateBookmark<T>(T item) where T : ISQLiteStorable, new()
        {
            var ret = await _sqliteService.InsertOrUpdateAsync<T>(item);
            Debug.Assert(ret != 0);

            return true;
        }

        public async Task<bool> RemoveBookmark<T>(T item, SportsType sportsType, BookMarkType bookmarkType, bool showToastMsg = true) where T : ISQLiteStorable, IBookmarkable, new()
        {
            item.Order = 0;
            item.StoredTime = DateTime.UtcNow;
            item.IsBookmarked = false;

            var ret = await _sqliteService.DeleteAsync<T>(item.PrimaryKey);
            Debug.Assert(ret != 0);

            string message = this.BuildBookmarkMessage(sportsType, bookmarkType);
            MessagingCenter.Send(this, message, item);

            if (showToastMsg)
            {
                UserDialogs.Instance.Toast(LocalizeString.Delete_Bookmark);
            }

            return true;
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