using PoseSportsPredict.ViewModels.Football.Match.Detail;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoseSportsPredict.Views.Football.Match.Detail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FootballMatchDetailPredictionsView : ContentView
    {
        public FootballMatchDetailPredictionsView()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            var bindingCtx = this.BindingContext as FootballMatchDetailPredictionsViewModel;
            bindingCtx.OnAppearing();
        }
    }
}