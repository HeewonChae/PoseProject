using Microsoft.Extensions.DependencyInjection;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logic.Utilities;
using PoseSportsPredict.Services;
using PoseSportsPredict.Services.ExternOAuth;
using PoseSportsPredict.ViewModels;
using PoseSportsPredict.Views;
using Shiny;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceShare.ExternAuthentication;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoseSportsPredict
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoadingPage : ContentPage
	{
		#region Members

		private bool _isLoading = true;

		#endregion Members

		#region Properties

		public bool IsLoading
		{
			get => _isLoading;
			set
			{
				_isLoading = value;
				OnPropertyChanged();
			}
		}

		#endregion Properties

		#region Constructors

		public LoadingPage()
		{
			InitializeComponent();
			BindingContext = this;
		}

		#endregion Constructors

		#region Methods

		public async Task LoadingAsync()
		{
			if (!IsLoading)
				return;

			await Task.FromResult(true);

			IsLoading = false;
		}

		#endregion Methods
	}
}