using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin_Tutorial.ViewMdels
{
	public class MainViewModel
	{
		#region ViewModels
		public LoginViewModel Login { get; set; }
		#endregion

		public MainViewModel()
		{
			Login = new LoginViewModel();
		}
	}
}
