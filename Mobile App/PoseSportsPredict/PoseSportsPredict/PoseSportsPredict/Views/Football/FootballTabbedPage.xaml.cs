using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PlatformConfig = Xamarin.Forms.PlatformConfiguration;

namespace PoseSportsPredict.Views.Football
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FootballTabbedPage : TabbedPage
    {
        public FootballTabbedPage()
        {
            InitializeComponent();
            PlatformConfig.AndroidSpecific.TabbedPage.SetIsSwipePagingEnabled(this, false);
            PlatformConfig.AndroidSpecific.TabbedPage.SetToolbarPlacement(this, PlatformConfig.AndroidSpecific.ToolbarPlacement.Bottom);
        }
    }
}