using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin_Tutorial.InfraStructure;

namespace Xamarin_Tutorial.Models.Shiny
{
	public class PushEvent : ISQLiteData
	{
		[PrimaryKey]
		[AutoIncrement]
		public int Id { get; set; }

		public string Token { get; set; }
		public string Payload { get; set; }
		public DateTime Timestamp { get; set; }
	}
}