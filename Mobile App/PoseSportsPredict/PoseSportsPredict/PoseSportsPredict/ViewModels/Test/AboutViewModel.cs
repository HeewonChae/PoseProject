using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Views.Test;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels.Test
{
    public class AboutViewModel : BaseViewModel
    {
        #region BaseViewModel

        public override async Task<bool> PrepareView(params object[] data)
        {
            return await Task.FromResult(true);
        }

        #endregion BaseViewModel

        #region Constructors

        public AboutViewModel(AboutPage page) : base(page)
        {
            Title = "About";
        }

        public AboutViewModel() : base(null)
        {
        }

        #endregion Constructors

        #region Commands

        public ICommand OpenWebCommand { get => new RelayCommand(OpenWeb); }

        public async void OpenWeb()
        {
            await Browser.OpenAsync("https://xamarin.com");
        }

        #endregion Commands
    }
}