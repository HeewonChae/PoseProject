using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PoseSportsPredict.InfraStructure
{
	public interface IPageSwitcher
	{
		Task SwitchMainPageAsync(BaseViewModel viewModel, bool isNavPage = false, params object[] prepareData);

		Task PushNavPageAsync(BaseViewModel viewModel, Page errorPage = null, params object[] prepareData);

		Task PushNavModalPageAsync(BaseViewModel viewModel, Page errorPage = null, bool isNavPage = false, params object[] prepareData);
	}
}