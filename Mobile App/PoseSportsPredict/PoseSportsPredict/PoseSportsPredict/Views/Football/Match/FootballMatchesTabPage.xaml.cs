using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace PoseSportsPredict.Views.Football.Match
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FootballMatchesTabPage : Xamarin.Forms.TabbedPage
    {
        public static FootballMatchesTabPage Current { get; private set; }

        public FootballMatchesTabPage()
        {
            InitializeComponent();
            Current = this;
        }

        public static void DisableSwipe()
        {
            Current.On<Android>().DisableSwipePaging();
        }

        public static void EnableSwipe()
        {
            Current.On<Android>().EnableSwipePaging();
        }
    }
}