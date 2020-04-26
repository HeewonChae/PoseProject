using PoseSportsPredict.InfraStructure.SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.Utilities.SQLite
{
    public class StoredData_InverseDateComparer : IComparer<ISQLiteStorable>
    {
        public int Compare(ISQLiteStorable x, ISQLiteStorable y)
        {
            return x.StoredTime.CompareTo(y.StoredTime) * -1;
        }
    }
}