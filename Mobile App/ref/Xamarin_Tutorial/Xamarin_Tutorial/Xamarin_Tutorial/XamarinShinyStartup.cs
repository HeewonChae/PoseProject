//using Microsoft.Extensions.DependencyInjection;
//using Shiny;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Xamarin_Tutorial.Utilities.Shiny;

//namespace Xamarin_Tutorial
//{
//	public class XamarinShinyStartup : ShinyStartup
//	{
//		public override void ConfigureServices(IServiceCollection services)
//		{
//			// this is where you'll load things like BLE, GPS, etc - those are covered in other sections
//			// things like the jobs, environment, power, are all installed automatically
//			services.UseMemoryCache();

//			InitInfrastructure(services);

//			RegisterShinyStuff(services);
//		}

//		private void InitInfrastructure(IServiceCollection services)
//		{
//			services.AddSingleton<Shiny_SqliteConnection>();
//			services.AddSingleton<CoreDelegateServices>();
//			services.AddSingleton<JobLoggerTask>();
//		}

//		private void RegisterShinyStuff(IServiceCollection services)
//		{
//			//services.UseFirebaseMessaging<PushDelegate>();

//			//Local notification
//			services.UseNotifications<NotificationDelegate>(
//				true,
//				notificationCategories: new NotificationCategory(
//					"default"
//				));
//		}
//	}
//}