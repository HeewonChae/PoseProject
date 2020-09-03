using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.InfraStructure
{
    public interface IDeviceInfoHelper
    {
        string DeviceId { get; set; }
        string AppPackageName { get; set; }
        bool? IsLicensed { get; set; }
    }
}