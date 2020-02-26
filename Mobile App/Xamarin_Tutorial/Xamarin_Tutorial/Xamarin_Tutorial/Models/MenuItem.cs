using System;
using System.Collections.Generic;
using System.Text;
using Xamarin_Tutorial.InfraStructure;

namespace Xamarin_Tutorial.Models
{
	public class MenuItem
	{
		public string Icon { get; set; }
		public string Title { get; set; }
		public Action Action { get; set; }
	}
}