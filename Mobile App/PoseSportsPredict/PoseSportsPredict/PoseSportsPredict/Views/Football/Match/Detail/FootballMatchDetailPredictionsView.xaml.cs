using PoseSportsPredict.ViewModels.Football;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoseSportsPredict.Views.Football
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