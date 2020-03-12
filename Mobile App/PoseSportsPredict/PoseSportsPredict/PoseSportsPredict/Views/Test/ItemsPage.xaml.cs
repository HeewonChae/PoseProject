using PoseSportsPredict.Models;
using PoseSportsPredict.ViewModels.Test;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoseSportsPredict.Views.Test
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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_viewModel.Items == null)
                _viewModel.OnInitializeView();

            _viewModel.OnAppearing();
        }
    }
}