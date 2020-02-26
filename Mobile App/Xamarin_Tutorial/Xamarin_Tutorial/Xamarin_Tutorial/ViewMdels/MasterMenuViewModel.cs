using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin_Tutorial.InfraStructure;
using Xamarin_Tutorial.Models;
using Xamarin_Tutorial.Services.Authentication;
using Xamarin_Tutorial.Utilities;

namespace Xamarin_Tutorial.ViewMdels
{
	public class MasterMenuViewModel : BaseViewModel
	{
		#region BaseViewModel Impl

		public override Task<bool> PrepareView(params object[] data)
		{
			if (Menus == null)
			{
				_menuList = new List<MenuItem>()
				{
					new MenuItem(){ Icon= "ic_setting", Title= "Setting", Action= null,},
					new MenuItem(){ Icon= "ic_chart", Title= "Statistics", Action = null,},
					new MenuItem(){ Icon= "ic_exit", Title= "Logout", Action = this.LogoutAction, },
				};

				Menus = new ObservableCollection<MenuItem>(_menuList);
			}

			return Task.FromResult(true);
		}

		#endregion BaseViewModel Impl

		#region Attributes

		private List<MenuItem> _menuList;
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

		private void SelectMenu(MenuItem selectedMenu)
		{
			selectedMenu.Action?.Invoke();
		}

		#endregion Commands

		#region Actions

		public Action LogoutAction = () =>
		{
			Singleton.Get<ExternOAuthService>().Logout();

			PageSwitcher.SwitchMainPageAsync(Singleton.Get<ViewLocator>().Login, true);
		};

		#endregion Actions
	}
}