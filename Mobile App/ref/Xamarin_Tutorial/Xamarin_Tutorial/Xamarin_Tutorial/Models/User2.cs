using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin_Tutorial.InfraStructure;

namespace Xamarin_Tutorial.Models
{
	public class User2 : ISQLiteData
	{
		[PrimaryKey]
		public int Id { get; set; } = 1;

		public string Email { get; set; }

		public string Password { get; set; }

		public bool IsRemember { get; set; }

		public DateTime UpdateTime { get; set; }
	}
}