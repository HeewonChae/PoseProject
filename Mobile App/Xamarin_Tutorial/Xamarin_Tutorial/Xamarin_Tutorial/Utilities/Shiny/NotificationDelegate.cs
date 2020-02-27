//using Shiny;
//using Shiny.Notifications;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using Xamarin_Tutorial.Models.Shiny;

//namespace Xamarin_Tutorial.Utilities.Shiny
//{
//	public class NotificationDelegate : INotificationDelegate
//	{
//		private readonly Shiny_SqliteConnection conn;
//		private readonly IMessageBus messageBus;

//		public NotificationDelegate(Shiny_SqliteConnection conn, IMessageBus messageBus)
//		{
//			this.conn = conn;
//			this.messageBus = messageBus;
//		}

//		public Task OnEntry(NotificationResponse response) => this.Store(new NotificationEvent
//		{
//			NotificationId = response.Notification.Id,
//			NotificationTitle = response.Notification.Title ?? response.Notification.Message,
//			Action = response.ActionIdentifier,
//			ReplyText = response.Text,
//			IsEntry = true,
//			Timestamp = DateTime.Now
//		});

//		public Task OnReceived(Notification notification) => this.Store(new NotificationEvent
//		{
//			NotificationId = notification.Id,
//			NotificationTitle = notification.Title ?? notification.Message,
//			IsEntry = false,
//			Timestamp = DateTime.Now
//		});

//		private async Task Store(NotificationEvent @event)
//		{
//			await this.conn.InsertAsync(@event);
//			this.messageBus.Publish(@event);
//		}
//	}
//}