using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using PlatformConfig = Xamarin.Forms.PlatformConfiguration;

namespace PoseSportsPredict.Views.Test
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedPage1 : TabbedPage
    {
        public TabbedPage1()
        {
            InitializeComponent();
            PlatformConfig.AndroidSpecific.TabbedPage.SetIsSwipePagingEnabled(this, true);
            PlatformConfig.AndroidSpecific.TabbedPage.SetToolbarPlacement(this, PlatformConfig.AndroidSpecific.ToolbarPlacement.Top);
            this.CurrentPage = todayItems;
        }
    }
}