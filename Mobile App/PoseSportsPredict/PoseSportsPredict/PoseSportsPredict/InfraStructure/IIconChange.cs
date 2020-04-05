using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.InfraStructure
{
    public interface IIconChange
    {
        bool IsSelected { get; set; }
        string CurrentIcon { get; }
    }
}