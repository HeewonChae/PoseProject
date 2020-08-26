using PoseSportsPredict.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.InfraStructure
{
    public interface IExpandable
    {
        string TitleLogo { get; set; }
        string Title { get; set; }
        bool Expanded { get; set; }
        string StateIcon { get; }
    }
}