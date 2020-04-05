﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.InfraStructure
{
    public interface IGroupBase
    {
        string Title { get; set; }
        bool Expanded { get; set; }
        string StateIcon { get; }
    }
}