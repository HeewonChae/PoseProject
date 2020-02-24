using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin_Tutorial.InfraStructure;
using Xamarin_Tutorial.Views;

namespace Xamarin_Tutorial.ViewMdels
{
	public class MasterViewModel : BaseViewModel
	{
		#region BaseViewModel Impl

		public override async Task<bool> PrepareView(params object[] data)
		{
			MasterPage masterPage = CoupledView as MasterPage;

			if (!await Singleton.Get<ViewLocator>().MasterMenu.PrepareView())
				return await Task.FromResult(false);

			masterPage.Master = Singleton.Get<ViewLocator>().MasterMenu.CoupledView;
			masterPage.IsPresented = false;

			if (!await Singleton.Get<ViewLocator>().Lands.PrepareView())
				return await Task.FromResult(false);

			masterPage.Detail = new NavigationPage(Singleton.Get<ViewLocator>().Lands.CoupledView);

			return await Task.FromResult(true);
		}

		#endregion BaseViewModel Impl
	}
}