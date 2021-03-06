﻿using Acr.UserDialogs;
using PosePacket.Service.Enum;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Resources.Common;
using PoseSportsPredict.Resources;
using Shiny;
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

        public async Task<bool> AddBookmark<T>(T item, SportsType sportsType, PageDetailType bookmarkType) where T : ISQLiteStorable, IBookmarkable, new()
        {
            var membershipType = ShinyHost.Resolve<MembershipService>().MemberRoleType;
            if (MembershipAdvantage.TryGetValue(membershipType, out MembershipAdvantage advantage))
            {
                int limitSize = 0;
                switch (bookmarkType)
                {
                    case PageDetailType.Match:
                        limitSize = advantage.MatchBookmarkLimit;
                        break;

                    case PageDetailType.League:
                        limitSize = advantage.LeagueBookmarkLimit;
                        break;

                    case PageDetailType.Team:
                        limitSize = advantage.TeamBookmarkLimit;
                        break;
                }

                int curSavedCnt = (await _sqliteService.SelectAllAsync<T>()).Count;
                if (curSavedCnt < limitSize) // 저장 가능
                {
                    item.Order = 0;
                    item.StoredTime = DateTime.UtcNow;
                    item.IsBookmarked = true;

                    var ret = await _sqliteService.InsertOrUpdateAsync<T>(item);
                    Debug.Assert(ret != 0);

                    string message = this.BuildBookmarkMessage(sportsType, bookmarkType);
                    MessagingCenter.Send(this, message, item);

                    UserDialogs.Instance.Toast(LocalizeString.Set_Bookmark);
                }
                else
                {
                    UserDialogs.Instance.Toast(LocalizeString.Bookmark_Inventory_Full);
                }
            }
            else
                return false;

            return true;
        }

        public async Task<bool> UpdateBookmark<T>(T item) where T : ISQLiteStorable, new()
        {
            var ret = await _sqliteService.InsertOrUpdateAsync<T>(item);
            Debug.Assert(ret != 0);

            return true;
        }

        public async Task<bool> RemoveBookmark<T>(T item, SportsType sportsType, PageDetailType bookmarkType, bool showToastMsg = true) where T : ISQLiteStorable, IBookmarkable, new()
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