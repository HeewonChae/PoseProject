using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using PoseSportsApp.Models;
using PoseSportsApp.Views;
using PoseSportsApp.ViewModels;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using PosePacket.Proxy;
using System.Threading;
using WebServiceShare;
using PosePacket.Service.HelloWorld;
using PoseSportsApp.Services;

namespace PoseSportsApp.Views
{
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible(false)]
	public partial class ItemsPage : ContentPage
	{
		ItemsViewModel viewModel;

		public ItemsPage()
		{
			InitializeComponent();

			BindingContext = viewModel = new ItemsViewModel();
		}

		async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
		{
			if (!(args.SelectedItem is Item item))
				return;

			var hello = await WebFacade.RequestAsync<O_Hello>(
								WebConfig.WebMethodType.POST
								, HelloWorldProxy.ServiceUrl
								, HelloWorldProxy.P_Hello
								, new I_Hello() { Name = "HeeWon"});

			if (hello != null)
			{
				item.Text = hello.Name;
				item.Description = hello.Address;

				await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));
			}

			// Manually deselect item.
			ItemsListView.SelectedItem = null;
		}

		async void AddItem_Clicked(object sender, EventArgs e)
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