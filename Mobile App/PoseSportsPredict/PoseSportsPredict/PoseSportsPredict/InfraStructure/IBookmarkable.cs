using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.InfraStructure
{
    public interface IBookmarkable
    {
        bool IsBookmarked { get; set; }
    }
}