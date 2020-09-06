using PoseSportsPredict.Droid.DependencyImpl;
using PoseSportsPredict.InfraStructure;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceInfoHelper))]

namespace PoseSportsPredict.Droid.DependencyImpl
{
    public class DeviceInfoHelper : IDeviceInfoHelper
    {
        public string DeviceId { get; set; }
        public string AppPackageName { get; set; }
        public bool? IsLicensed { get; set; }
        public string AppVersionName { get; set; }
    }
}