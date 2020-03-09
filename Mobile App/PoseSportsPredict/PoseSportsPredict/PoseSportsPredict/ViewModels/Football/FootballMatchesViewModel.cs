using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Views.Football;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;

namespace PoseSportsPredict.ViewModels.Football
{
    public class FootballMatchesViewModel : BaseViewModel
    {
        #region BaseViewModel

        public override Task<bool> PrepareView(params object[] data)
        {
            return Task.FromResult(true);
        }

        #endregion BaseViewModel

        #region Constructors

        public FootballMatchesViewModel(FootballMatchesPage page) : base(page)
        {
        }

        #endregion Constructors

        #region Commands

        public ICommand AddItemCommand { get => new RelayCommand(AddItem); }

        private async void AddItem()
        {
            string selected = await UserDialogs.Instance.ActionSheetAsync("", "", "", null, "1111", "2222", "333");
        }

        #endregion Commands
    }
}