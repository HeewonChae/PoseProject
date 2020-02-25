using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using PoseSportsPredict.Models;
using PoseSportsPredict.Views;
using PoseSportsPredict.ViewModels;

namespace PoseSportsPredict.Views
{
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible(false)]
	public partial class ItemsPage : ContentPage
	{
		public class MyPageSearchHandler : SearchHandler
		{
			public MyPageSearchHandler()
			{
				SearchBoxVisibility = SearchBoxVisibility.Collapsible;
				IsSearchEnabled = true;
			}

			protected override void OnQueryConfirmed()
			{
			}

			protected override void OnQueryChanged(string oldValue, string newValue)
			{
				// Do nothing, we will wait for confirmation
			}
		}

		private ItemsViewModel viewModel;

		public ItemsPage()
		{
			InitializeComponent();

			BindingContext = viewModel = new ItemsViewModel();
			Shell.SetSearchHandler(this, new MyPageSearchHandler());
		}

		private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
		{
			var item = args.SelectedItem as Item;
			if (item == null)
				return;

			await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

			// Manually deselect item.
			ItemsListView.SelectedItem = null;
		}

		private async void AddItem_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			if (viewModel.Items.Count == 0)
				viewModel.LoadItemsCommand.Execute(null);
		}
	}
}