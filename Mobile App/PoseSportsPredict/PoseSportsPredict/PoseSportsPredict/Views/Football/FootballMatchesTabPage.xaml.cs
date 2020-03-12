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
    public partial class FootballMatchesTabPage : TabbedPage
    {
        public FootballMatchesTabPage()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnAppearing();
            var bindingCtx = this.BindingContext as BaseViewModel;
            bindingCtx.OnPrepareView();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var bindingCtx = this.BindingContext as BaseViewModel;
            bindingCtx.OnApearing();
        }
    }
}