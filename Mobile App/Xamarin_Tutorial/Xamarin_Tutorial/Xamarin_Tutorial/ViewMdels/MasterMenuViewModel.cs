using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin_Tutorial.InfraStructure;
using Xamarin_Tutorial.Models;
using Xamarin_Tutorial.Utilities;

namespace Xamarin_Tutorial.ViewMdels
{
	public class MasterMenuViewModel : BaseViewModel
	{
		#region BaseViewModel Impl

		public override Task<bool> PrepareView(params object[] data)
		{
			Menus = new ObservableCollection<MenuItem>(_menuList);

			return Task.FromResult(true);
		}

		#endregion BaseViewModel Impl

		#region Attributes

		private readonly List<MenuItem> _menuList;
		private ObservableCollection<MenuItem> _menus;

		#endregion Attributes

		#region Properties

		public ObservableCollection<MenuItem> Menus { get => _menus; set => SetValue(ref _menus, value); }

		#endregion Properties

		#region Commands

		public ICommand SelectMenuCommand
		{
			get
			{
				return new RelayCommand<MenuItem>(SelectMenu);
			}
		}

		private async void SelectMenu(MenuItem selectedMenu)
		{
			if (selectedMenu.Page == null)
				return;

			if (selectedMenu.Title.Equals("Logout"))
			{
				await PageSwitcher.SwitchMainPageAsync(selectedMenu.Page, isNavPage: true);
			}
			else
			{
				await PageSwitcher.PushNavPageAsync(selectedMenu.Page);
			}
		}

		#endregion Commands

		#region Constructor

		public MasterMenuViewModel()
		{
			_menuList = new List<MenuItem>()
			{
				new MenuItem(){ Icon= "ic_setting", Title= "Setting", Page= null,},
				new MenuItem(){ Icon= "ic_chart", Title= "Statistics", Page = null,},
				new MenuItem(){ Icon= "ic_exit", Title= "Logout", Page= Singleton.Get<ViewLocator>().Login,},
			};
		}

		#endregion Constructor
	}
}