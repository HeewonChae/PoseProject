using PanCardView.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoseSportsPredict.Utilities
{
    public static class EnumerableExtensions
    {
        public static List<T[]> SeperateByCount<T>(this IEnumerable<T> origin, int count)
        {
            var result = new List<T[]>();
            var TotalCnt = origin.Count();

            if (origin == null || TotalCnt < count)
            {
                result.Add(origin?.ToArray());
                return result;
            }

            int nextIndex = 0;
            var originList = origin.ToList();
            while (nextIndex < TotalCnt)
            {
                var selectedCnt = Math.Min(count, TotalCnt - nextIndex);
                result.Add(originList.GetRange(nextIndex, selectedCnt).ToArray());
                nextIndex += count;
            }

            return result;
        }
    }
}