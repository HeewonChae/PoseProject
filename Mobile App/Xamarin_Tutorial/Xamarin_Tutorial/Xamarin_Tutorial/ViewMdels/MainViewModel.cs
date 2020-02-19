using System;
using System.Collections.Generic;
using System.Text;
using Xamarin_Tutorial.InfraStructure;

namespace Xamarin_Tutorial.ViewMdels
{
	public class MainViewModel : Singleton.INode
	{
		#region ViewModels
		public LoginViewModel Login { get; set; }
		public LandsViewModel Lands { get; set; }
		#endregion

		public MainViewModel()
		{
			Login = new LoginViewModel();
			Lands = new LandsViewModel();
		}
	}
}
