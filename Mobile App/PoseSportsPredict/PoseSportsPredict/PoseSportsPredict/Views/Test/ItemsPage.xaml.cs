using PoseSportsPredict.Models;
using PoseSportsPredict.ViewModels.Football;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoseSportsPredict.Views.Football
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        private ItemsViewModel _viewModel;

        public ItemsPage()
        {
            InitializeComponent();
            _viewModel = new ItemsViewModel(this);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (_viewModel.Items == null)
                await _viewModel.PrepareView();
        }
    }
}