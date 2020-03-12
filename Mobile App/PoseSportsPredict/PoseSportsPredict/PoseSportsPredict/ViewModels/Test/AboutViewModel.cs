using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.ViewModels.Base;
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
    public class AboutViewModel : NavigableViewModel
    {
        #region Constructors

        public AboutViewModel(AboutPage page) : base(page)
        {
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