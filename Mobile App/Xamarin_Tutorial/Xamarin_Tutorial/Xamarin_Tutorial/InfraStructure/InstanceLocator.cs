using System;
using System.Collections.Generic;
using System.Text;
using Xamarin_Tutorial.ViewMdels;

namespace Xamarin_Tutorial.InfraStructure
{
	public class InstanceLocator
	{
		public MainViewModel Main { get; set; }

		public InstanceLocator()
		{
			this.Main = new MainViewModel();
		}
	}
}
