using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace PoseSportsPredict.Views.Test
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {
        public NewItemPage()
        {
            InitializeComponent();

            Xamarin.Forms.Application.Current.On<Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
        }
    }
}