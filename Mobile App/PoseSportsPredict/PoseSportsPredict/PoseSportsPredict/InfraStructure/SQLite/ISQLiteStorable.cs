using System;

namespace PoseSportsPredict.InfraStructure.SQLite
{
    public interface ISQLiteStorable
    {
        string PrimaryKey { get; set; }
        int Order { get; set; }
        DateTime StoredTime { get; set; }
    }
}