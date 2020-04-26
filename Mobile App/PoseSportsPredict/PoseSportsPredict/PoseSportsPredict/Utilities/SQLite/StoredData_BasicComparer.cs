using PoseSportsPredict.InfraStructure.SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.Utilities.SQLite
{
    public class StoredData_BasicComparer : IComparer<ISQLiteStorable>
    {
        public int Compare(ISQLiteStorable x, ISQLiteStorable y)
        {
            if (x.Order.CompareTo(y.Order) == 0)
            {
                return x.StoredTime.CompareTo(y.StoredTime);
            }

            return x.Order.CompareTo(y.Order) * -1;
        }
    }
}