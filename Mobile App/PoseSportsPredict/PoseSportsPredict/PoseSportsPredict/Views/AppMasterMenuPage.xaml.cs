using PoseSportsPredict.ViewModels;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football;
using Shiny;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoseSportsPredict.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppMasterMenuPage : ContentPage
    {
        public AppMasterMenuPage()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            var bindingCtx = this.BindingContext as BaseViewModel;
            bindingCtx.OnPrepareView(ShinyHost.Resolve<FootballMainViewModel>());
        }
    }
}