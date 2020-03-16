using PoseSportsPredict.ViewModels.Test;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoseSportsPredict.Views.Test
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();

            new AboutViewModel(this);
        }
    }
}