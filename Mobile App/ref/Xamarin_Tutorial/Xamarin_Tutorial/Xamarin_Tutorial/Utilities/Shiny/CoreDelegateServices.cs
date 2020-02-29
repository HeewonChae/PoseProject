//using Shiny.Notifications;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace Xamarin_Tutorial.Utilities.Shiny
//{
//	public class CoreDelegateServices
//	{
//		public CoreDelegateServices(Shiny_SqliteConnection conn,
//									INotificationManager notifications)
//		{
//			this.Connection = conn;
//			this.Notifications = notifications;
//		}

//		public Shiny_SqliteConnection Connection { get; }
//		public INotificationManager Notifications { get; }

//		public async Task SendNotification(string title, string message)
//		{
//			await this.Notifications.Send(title, message);
//		}
//	}
//}