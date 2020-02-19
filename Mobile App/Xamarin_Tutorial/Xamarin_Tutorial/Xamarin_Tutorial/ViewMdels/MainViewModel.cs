using System;
using System.Collections.Generic;
using System.Text;
using Xamarin_Tutorial.InfraStructure;
using Xamarin_Tutorial.Views;

namespace Xamarin_Tutorial.ViewMdels
{
	public class MainViewModel : Singleton.INode
	{
		#region ViewModels
		public LoginViewModel Login { get; set; }
		public LandsViewModel Lands { get; set; }
		#endregion

		public void Initialize()
		{
			Login = new LoginViewModel();
			Login.SetViewPage(new LoginPage());

			Lands = new LandsViewModel();
			Lands.SetViewPage(new LandsPage());
		}
	}
}
