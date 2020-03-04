using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics.Common;
using PoseSportsPredict.Models;
using PoseSportsPredict.Views.Football;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels.Football
{
    public class NewItemViewModel : BaseViewModel
    {
        #region BaseViewModel

        public override Task<bool> PrepareView(params object[] data)
        {
            Item = new Item
            {
                Text = "Item name",
                Description = "This is an item description."
            };

            return Task.FromResult(true);
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

        public ICommand AddItemCommand { get => new RelayCommand(AddItem); }

        public async void AddItem()
        {
            MessagingCenter.Send(this, "AddItem", Item);
            await PageSwitcher.PopModalAsync();
        }

        #endregion Commands
    }
}