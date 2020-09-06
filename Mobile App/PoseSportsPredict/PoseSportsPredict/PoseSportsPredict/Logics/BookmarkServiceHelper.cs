using PosePacket.Service.Enum;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Models;
using PoseSportsPredict.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.Logics
{
    public static class BookmarkServiceHelper
    {
        public static string BuildBookmarkMessage(this IBookmarkService bookmarkService, SportsType sportsType, PageDetailType bookmarkType)
        {
            return $"{sportsType}-{bookmarkType}";
        }
    }
}