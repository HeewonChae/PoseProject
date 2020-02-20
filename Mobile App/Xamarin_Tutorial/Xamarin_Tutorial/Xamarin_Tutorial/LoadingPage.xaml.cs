using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin_Tutorial.InfraStructure;

namespace Xamarin_Tutorial
{
	public partial class LoadingPage : ContentPage
	{
		public LoadingPage()
		{
			InitializeComponent();
		}

		public async Task AppLoadAsync()
		{
			await Task.Delay(1).ConfigureAwait(false);

			using (var progress = UserDialogs.Instance.Progress("Please Wait..."))
			{
				var viewLocator = new ViewLocator();
				progress.PercentComplete += 10;
				Singleton.Register(viewLocator);
				progress.PercentComplete += 10;
				viewLocator.Initialize(progress);

				progress.PercentComplete = 100;
			}
		}
	}
}