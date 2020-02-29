using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin_Tutorial.InfraStructure;

namespace Xamarin_Tutorial.Models.Shiny
{
	public class NotificationEvent : ISQLiteData
	{
		[PrimaryKey]
		[AutoIncrement]
		public int Id { get; set; }

		public int NotificationId { get; set; }
		public string NotificationTitle { get; set; }
		public string Action { get; set; }
		public string ReplyText { get; set; }
		public string Payload { get; set; }
		public bool IsEntry { get; set; }
		public DateTime Timestamp { get; set; }
	}
}