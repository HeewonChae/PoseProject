//using Shiny.IO;
//using SQLite;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Text;
//using Xamarin_Tutorial.Models.Shiny;

//namespace Xamarin_Tutorial.Utilities.Shiny
//{
//	public class Shiny_SqliteConnection : SQLiteAsyncConnection
//	{
//		public Shiny_SqliteConnection(IFileSystem fileSystem) : base(Path.Combine(fileSystem.AppData.FullName, "app_system.db3"))
//		{
//			var conn = this.GetConnection();
//			conn.CreateTable<JobLog>();
//			conn.CreateTable<NotificationEvent>();
//			conn.CreateTable<PushEvent>();
//		}

//		public AsyncTableQuery<JobLog> JobLogs => this.Table<JobLog>();
//		public AsyncTableQuery<NotificationEvent> NotificationEvents => this.Table<NotificationEvent>();
//		public AsyncTableQuery<PushEvent> PushEvents => this.Table<PushEvent>();
//	}
//}