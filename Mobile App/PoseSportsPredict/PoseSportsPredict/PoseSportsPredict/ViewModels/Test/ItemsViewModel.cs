using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics.Common;
using PoseSportsPredict.Models;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Test;
using Shiny;
using SlideOverKit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.KeyboardHelper;

namespace PoseSportsPredict.ViewModels.Test
{
    public class ItemsViewModel : NavigableViewModel
    {
        #region BaseViewModel

        public override bool OnInitializeView(params object[] data)
        {
            if (IsBusy)
                return false;

            if (_itemList != null)
                return true;

            IsBusy = true;

            try
            {
                _itemList = new List<Item>();

                var items = DataStore.GetItemsAsync(true).Result;
                foreach (var item in items)
                {
                    _itemList.Add(item);
                }

                Items = new ObservableCollection<Item>(_itemList);
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

        #region Fileds

        private List<Item> _itemList;
        private ObservableCollection<Item> _items;

        #endregion Fileds

        #region Properties

        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();
        public ObservableCollection<Item> Items { get => _items; set => SetValue(ref _items, value); }

        #endregion Properties

        #region Constructors

        public ItemsViewModel(ItemsPage page) : base(page)
        {
            MessagingCenter.Subscribe<NewItemViewModel, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                await DataStore.AddItemAsync(newItem);
                _itemList.Add(newItem);
                Items = new ObservableCollection<Item>(_itemList);
            });
        }

        #endregion Constructors

        #region Commands

        public ICommand RefreshItemsCommand { get => new RelayCommand(RefreshItems); }

        private void RefreshItems()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            Items = new ObservableCollection<Item>(_itemList);

            IsBusy = false;
        }

        public ICommand ItemSelectedCommand { get => new RelayCommand<Item>(item => ItemSelected(item)); }

        //private async void ItemSelected(Item selectedItem)
        //{
        //    if (selectedItem == null)
        //        return;

        //    await PageSwitcher.PushModalPageAsync(ShinyHost.Resolve<ItemDetailViewModel>(), null, selectedItem);
        //}

        private async void ItemSelected(Item selectedItem)
        {
            if (selectedItem == null)
                return;

            await PageSwitcher.PushModalPageAsync(ShinyHost.Resolve<ItemDetailViewModel>(), null, selectedItem);
        }

        public ICommand AddItemCommand { get => new RelayCommand(AddItem); }

        private async void AddItem()
        {
            await PageSwitcher.PushModalPageAsync(ShinyHost.Resolve<NewItemViewModel>());
        }

        #endregion Commands
    }
}