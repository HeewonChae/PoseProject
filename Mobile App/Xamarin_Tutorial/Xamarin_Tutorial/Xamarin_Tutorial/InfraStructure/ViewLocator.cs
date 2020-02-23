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
		public LandDetailViewModel LandDetail { get; set; }

		#endregion ViewModels

		public void Initialize(IProgressDialog progress)
		{
			Login = new LoginViewModel();
			Login.SetCoupledView(new LoginPage());

			Lands = new LandsViewModel();
			Lands.SetCoupledView(new LandsPage());

			LandDetail = new LandDetailViewModel();
			LandDetail.SetCoupledView(new LandDetailPage());
		}
	}
}