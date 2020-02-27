using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin_Tutorial.InfraStructure;

namespace Xamarin_Tutorial.Models.Shiny
{
	public class JobLog : ISQLiteData
	{
		[PrimaryKey]
		[AutoIncrement]
		public int Id { get; set; }

		public string JobIdentifier { get; set; }
		public string JobType { get; set; }
		public string Error { get; set; }
		public string Parameters { get; set; }
		public bool Started { get; set; }
		public DateTime Timestamp { get; set; }
	}
}