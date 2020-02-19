using System;
using System.Collections.Generic;
using System.Text;
using Xamarin_Tutorial.ViewMdels;

namespace Xamarin_Tutorial.InfraStructure
{
	public class App_Start
	{
		public App_Start()
		{
			RegisterSinglton();
		}

		private void RegisterSinglton()
		{
			Singleton.Register<MainViewModel>();
		}
	}
}
