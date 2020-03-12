using PoseSportsPredict.ViewModels.Base;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace PoseSportsPredict.Views.Football
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FootballMainPage : Xamarin.Forms.TabbedPage
    {
        public FootballMainPage()
        {
            InitializeComponent();
            On<Android>().SetIsSwipePagingEnabled(false);
            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
        }
    }
}