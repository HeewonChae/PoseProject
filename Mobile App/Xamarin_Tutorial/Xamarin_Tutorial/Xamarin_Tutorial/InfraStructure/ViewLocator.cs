using Acr.UserDialogs;
using Xamarin_Tutorial.ViewMdels;
using Xamarin_Tutorial.Views;

namespace Xamarin_Tutorial.InfraStructure
{
	public class ViewLocator : Singleton.INode
	{
		#region ViewModels

		public LoginViewModel Login { get; set; }
		public LandsViewModel Lands { get; set; }
		public LandViewModel Land { get; set; }
		public LandTabbedViewModel LandTabbed { get; set; }
		public CurrencyViewModel Currency { get; set; }

		#endregion ViewModels

		public void Initialize(IProgressDialog progress)
		{
			Login = new LoginViewModel();
			Login.SetCoupledView(new LoginPage());

			Lands = new LandsViewModel();
			Lands.SetCoupledView(new LandsPage());

			Land = new LandViewModel();
			Land.SetCoupledView(new LandPage());

			LandTabbed = new LandTabbedViewModel();
			LandTabbed.SetCoupledView(new LandTabbedPage());

			Currency = new CurrencyViewModel();
			Currency.SetCoupledView(new CurrencyPage());
		}
	}
}