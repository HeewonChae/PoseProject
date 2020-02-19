using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin_Tutorial.ViewMdels
{
	public class LoginViewModel : BaseViewModel
	{
		#region Attributes
		private string _password;
		private bool _isRunning;
		#endregion

		#region Properties
		public string Email { get; set; }
		public string Password { get => _password; set => SetValue(ref _password, value); }
		public bool IsRunning { get => _isRunning; set => SetValue(ref _isRunning, value); }
		public bool IsRemembered { get; set; }
		public bool IsEnable { get; set; }
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
			if (!IsEnable)
				return;

			if (string.IsNullOrEmpty(Email))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"Please enter your email",
					"Accept");
			}
			else if (string.IsNullOrEmpty(Password))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"Please enter your password",
					"Accept");
			}

			IsRunning = true;
			IsEnable = false;

			if(!Email.Equals("korman7750@gmail.com") || !Password.Equals("1234"))
			{
				await Application.Current.MainPage.DisplayAlert(
					"Error",
					"Email or Password Incorrect",
					"Accept");

				Password = string.Empty;
				IsRunning = false;
				IsEnable = true;

				return;
			}

			IsRunning = false;
			IsEnable = true;

			await Application.Current.MainPage.DisplayAlert(
				"Ok",
				"Login Success",
				"Accept");
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
		public LoginViewModel()
		{
			this.IsRemembered = true;

		}
		#endregion
	}
}
