using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.iOS.DependencyImpl;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceInfoHelper))]

namespace PoseSportsPredict.iOS.DependencyImpl
{
    public class DeviceInfoHelper : IDeviceInfoHelper
    {
        public string DeviceId { get; set; }
        public string AppPackageName { get; set; }
        public bool? IsLicensed { get; set; }
    }
}