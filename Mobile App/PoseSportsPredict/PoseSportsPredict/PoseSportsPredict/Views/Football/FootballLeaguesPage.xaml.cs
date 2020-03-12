using PoseSportsPredict.ViewModels.Base;
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
    public partial class FootballLeaguesPage : ContentPage
    {
        public FootballLeaguesPage()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnAppearing();
            var bindingCtx = this.BindingContext as BaseViewModel;
            bindingCtx.OnPrepareView();
        }
    }
}