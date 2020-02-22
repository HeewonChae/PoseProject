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

		#endregion ViewModels

		public void Initialize(IProgressDialog progress)
		{
			Login = new LoginViewModel();
			Login.SetView(new LoginPage());
			progress.PercentComplete += 30;

			Lands = new LandsViewModel();
			Lands.SetView(new LandsPage());
			progress.PercentComplete += 30;
		}
	}
}