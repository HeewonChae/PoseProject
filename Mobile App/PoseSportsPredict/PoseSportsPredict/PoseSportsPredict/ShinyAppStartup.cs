using Microsoft.Extensions.DependencyInjection;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Logic.Utilities;
using PoseSportsPredict.Services;
using PoseSportsPredict.Utilities.LocalStorage;
using PoseSportsPredict.ViewModels;
using PoseSportsPredict.Views;
using Shiny;
using WebServiceShare.ExternAuthentication;

namespace PoseSportsPredict
{
	public class ShinyAppStartup : ShinyStartup
	{
		public override void ConfigureServices(IServiceCollection services)
		{
			// this is where you'll load things like BLE, GPS, etc - those are covered in other sections
			// things like the jobs, environment, power, are all installed automatically
			RegisterServices(services);
			MatchViewModels(services);
		}

		private void RegisterServices(IServiceCollection services)
		{
			services.AddSingleton<IWebApiService, WebApiService>();
			services.AddSingleton<ISQLiteService, SQLiteService>();
			services.AddSingleton<IOAuthService, ExternOAuthService>();
		}

		private void MatchViewModels(IServiceCollection services)
		{
			services.AddSingleton<LoadingPage>();
			services.AddSingleton<LoginPage>();
			services.AddSingleton<LoginViewModel>();
		}
	}
}