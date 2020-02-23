using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin_Tutorial.InfraStructure;

namespace Xamarin_Tutorial.ViewMdels
{
	public class LoginViewModel : BaseViewModel
	{
		#region ICarryView Impl

		public override async Task<bool> PrepareView(params object[] data)
		{
			this.IsRemembered = true;
			this.Email = "korman2444@gmail.com";
			this.Password = "1234";

			return await Task.FromResult(true);
		}

		#endregion ICarryView Impl

		#region Attributes

		private string _email;
		private string _password;

		#endregion Attributes

		#region Properties

		public string Email { get => _email; set => SetValue(ref _email, value); }
		public string Password { get => _password; set => SetValue(ref _password, value); }
		public bool IsRemembered { get; set; }

		#endregion Properties

		#region Commands

		public ICommand LoginCommand
		{
			get
			{
				return new RelayCommand(Login);
			}
		}

		private async void Login()
		{
			if (string.IsNullOrEmpty(Email))
			{
				await UserDialogs.Instance.AlertAsync(
					"Please enter your email",
					"Error",
					"Accept");
			}
			else if (string.IsNullOrEmpty(Password))
			{
				await UserDialogs.Instance.AlertAsync(
					"Please enter your password",
					"Error",
					"Accept");
			}

			if (!Email.Equals("korman2444@gmail.com") || !Password.Equals("1234"))
			{
				await UserDialogs.Instance.AlertAsync(
					"Email or Password Incorrect",
					"Error",
					"Accept");

				Password = string.Empty;

				return;
			}

			// Put Lands Page
			await PageSwitcher.SwitchMainPageAsync(Singleton.Get<ViewLocator>().Lands, this.CoupledView, true);
		}

		public ICommand RegisterCommand
		{
			get
			{
				return new RelayCommand(Register);
			}
		}

		private void Register()
		{
			throw new NotImplementedException();
		}

		#endregion Commands

		#region Constructors

		public LoginViewModel()
		{
		}

		#endregion Constructors
	}
}