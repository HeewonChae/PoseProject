using Xamarin.Forms.Xaml;

namespace PoseSportsPredict.Views.Test
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShellPage : Xamarin.Forms.Shell
    {
        public AppShellPage()
        {
            InitializeComponent();

            this.CurrentItem = this.fi_football;
        }
    }
}