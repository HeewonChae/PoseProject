using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics.Common;
using PoseSportsPredict.Models;
using PoseSportsPredict.Views.Test;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels.Test
{
    public class NewItemViewModel : BaseViewModel
    {
        #region BaseViewModel

        public override Task<bool> PrepareView(params object[] data)
        {
            Item = new Item();

            return Task.FromResult(true);
        }

        public override Task ShowedView(params object[] datas)
        {
            //var entry = CoupledPage.FindByName<Entry>("_txtEntry");
            //entry.CursorPosition = 0;
            //entry.Focus();

            return base.ShowedView(datas);
        }

        #endregion BaseViewModel

        #region Fields

        private Item _item;

        #endregion Fields

        #region Properties

        public Item Item { get => _item; set => SetValue(ref _item, value); }

        #endregion Properties

        #region Constructors

        public NewItemViewModel(NewItemPage page) : base(page)
        {
        }

        #endregion Constructors

        #region Commands

        public ICommand CancelCommand { get => new RelayCommand(Cancel); }

        public async void Cancel()
        {
            await PageSwitcher.PopModalAsync();
        }

        public ICommand SaveItemCommand { get => new RelayCommand(SaveItem); }

        public async void SaveItem()
        {
            MessagingCenter.Send(this, "AddItem", Item);
            await PageSwitcher.PopModalAsync();
        }

        #endregion Commands
    }
}