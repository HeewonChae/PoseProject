using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics.Common;
using PoseSportsPredict.Models;
using PoseSportsPredict.Views;
using PoseSportsPredict.Views.Test;
using Shiny;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels.Test
{
    public class AppShellViewModel : BaseViewModel
    {
        #region BaseViewModel

        public override async Task<bool> PrepareView(params object[] data)
        {
            if (IsBusy)
                return false;

            IsBusy = true;

            try
            {
                Shell shellPage = CoupledPage as Shell;

                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    shellPage.Items.Add(CreateMenuItem(item));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }

            return true;
        }

        #endregion BaseViewModel

        #region Properties

        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();

        #endregion Properties

        #region Constructors

        public AppShellViewModel(AppShellPage page) : base(page)
        {
            MessagingCenter.Subscribe<NewItemViewModel, Item>(this, "AddItem", (obj, item) =>
            {
                Shell shellPage = CoupledPage as Shell;
                shellPage.Items.Add(CreateMenuItem(item));
            });
        }

        #endregion Constructors

        #region Commands

        public ICommand MenuSelectedCommands { get => new RelayCommand<object>(param => MenuSelected(param)); }

        private async void MenuSelected(object param)
        {
            Shell shellPage = CoupledPage as Shell;
            shellPage.FlyoutIsPresented = false;

            Item item = param as Item;
            await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<ItemDetailViewModel>(), null, item);
        }

        #endregion Commands

        #region Methods

        private MenuItem CreateMenuItem(Item item)
        {
            var menuItem = new MenuItem
            {
                Text = item.Text,
                Command = MenuSelectedCommands,
                CommandParameter = item
            };
            return menuItem;
        }

        #endregion Methods
    }
}