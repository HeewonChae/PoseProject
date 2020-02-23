using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using Xamarin_Tutorial.InfraStructure;
using Xamarin_Tutorial.Models;

namespace Xamarin_Tutorial.ViewMdels
{
	public class LandItemViewModel : CountryItem
	{
		#region Commands

		public ICommand SelectCountryCommand
		{
			get
			{
				return new RelayCommand(SelectCountry);
			}
		}

		private async void SelectCountry()
		{
			await PageSwitcher.PushNavPageAsync(
				Singleton.Get<ViewLocator>().LandTabbed,
				null,
				this,
				Singleton.Get<ViewLocator>().Land,
				Singleton.Get<ViewLocator>().Currency);
		}

		#endregion Commands
	}
}