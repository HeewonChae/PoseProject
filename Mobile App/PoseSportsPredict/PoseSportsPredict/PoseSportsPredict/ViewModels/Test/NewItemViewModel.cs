using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics.Common;
using PoseSportsPredict.Models;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Test;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels.Test
{
    public class NewItemViewModel : NavigableViewModel
    {
        #region BaseViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            Item = new Item();

            return base.OnInitializeView(datas);
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