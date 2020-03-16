using PoseSportsPredict.ViewModels.Base;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoseSportsPredict.Views.Football.Match.Detail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FootballMatchDetailOddsView : ContentView
    {
        public FootballMatchDetailOddsView()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            var bindingCtx = this.BindingContext as BaseViewModel;
            bindingCtx.OnAppearing();
        }
    }
}