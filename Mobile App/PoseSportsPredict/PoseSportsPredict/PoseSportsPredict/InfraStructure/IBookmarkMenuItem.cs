using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Models;
using PoseSportsPredict.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.InfraStructure
{
    public interface IBookmarkMenuItem : ISQLiteStorable, IBookmarkable
    {
        string MenuName { get; }
        string Logo { get; }
        PageDetailType BookMarkType { get; }
    }
}