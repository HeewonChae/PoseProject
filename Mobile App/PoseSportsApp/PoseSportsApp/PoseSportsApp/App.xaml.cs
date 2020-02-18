using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PoseSportsApp.Services;
using PoseSportsApp.Views;
using System.Threading.Tasks;
using Flurl.Http;
using WebServiceShare.WebServiceClient;
using WebServiceShare.ServiceContext;
using WebServiceShare;
using PosePacket.Proxy;

namespace PoseSportsApp
{
	public partial class App : Application
	{

		public App()
		{
			InitializeComponent();
			DependencyService.Register<MockDataStore>();
			MainPage = new MainPage();

		}

		protected override void OnStart()
		{
			//ClientContext.eCredentials = await WebFacade.RequestAsync<byte[]>(
			//									WebConfig.WebMethodType.POST
			//									, AuthProxy.ServiceUrl
			//									, AuthProxy.P_GetCredentials);
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
