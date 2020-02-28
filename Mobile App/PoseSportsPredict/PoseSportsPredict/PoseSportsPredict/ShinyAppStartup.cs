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
using System.Text;
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
			services.AddSingleton<IOAuthService, ExternOAuthService>();
			services.AddSingleton<IPageSwitcher, PageSwitcher>();
		}

		private void MatchViewModels(IServiceCollection services)
		{
			services.AddSingleton<LoginPage>();
			services.AddSingleton<LoginViewModel>();
		}
	}
}