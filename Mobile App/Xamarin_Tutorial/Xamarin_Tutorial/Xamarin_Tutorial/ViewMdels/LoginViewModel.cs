using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin_Tutorial.InfraStructure;
using Xamarin_Tutorial.Views;

namespace Xamarin_Tutorial.ViewMdels
{
	public class LoginViewModel : BaseViewModel
	{
		#region Attributes
		private string _email;
		private string _password;
		#endregion

		#region Properties
		public string Email { get => _email; set => SetValue(ref _email, value); }
		public string Password { get => _password; set => SetValue(ref _password, value); }
		public bool IsRemembered { get; set; }
		#endregion

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

			if(!Email.Equals("korman2444@gmail.com") || !Password.Equals("1234"))
			{
				await UserDialogs.Instance.AlertAsync(
					"Email or Password Incorrect",
					"Error",
					"Accept");

				Password = string.Empty;

				return;
			}
			
			// Put Lands Page
			Application.Current.MainPage = new NavigationPage(
				await Singleton.Get<MainViewModel>().Lands.ShowViewPage());
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
		#endregion

		#region Constructors
		public LoginViewModel(){ }
		#endregion

		#region Method
		public override async Task<Page> ShowViewPage()
		{
			this.IsRemembered = true;
			this.Email = "korman2444@gmail.com";
			this.Password = "1234";

			return await Task.FromResult(_coupledViewPage);
		}
		#endregion
	}
}
