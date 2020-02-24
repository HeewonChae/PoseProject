using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin_Tutorial.InfraStructure;
using Xamarin_Tutorial.Models;
using Xamarin_Tutorial.Services;
using Xamarin_Tutorial.Utilities;

namespace Xamarin_Tutorial.ViewMdels
{
	public class LoginViewModel : BaseViewModel
	{
		#region ICarryView Impl

		public override async Task<bool> PrepareView(params object[] data)
		{
			//string isRemember = LocalStorage.Storage[StorageKey.IsAccountRemember];
			//if (!string.IsNullOrEmpty(isRemember))
			//	IsRemembered = bool.Parse(isRemember);

			//Email = LocalStorage.Storage[StorageKey.UserEmail];
			//Password = LocalStorage.Storage[StorageKey.UserPassword];

			var user = await SQLiteService.FirstAsync<User>();

			if (user != null)
			{
				Email = user.Email;
				Password = user.Password;
				IsRemembered = user.IsRemember;
			}
			else
			{
				Email = string.Empty;
				Password = string.Empty;
				IsRemembered = false;
			}

			return await Task.FromResult(true);
		}

		#endregion ICarryView Impl

		#region Attributes

		private string _email;
		private string _password;
		private bool _isRemember;

		#endregion Attributes

		#region Properties

		public string Email { get => _email; set => SetValue(ref _email, value); }
		public string Password { get => _password; set => SetValue(ref _password, value); }
		public bool IsRemembered { get => _isRemember; set => SetValue(ref _isRemember, value); }

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

			//if (this.IsRemembered)
			//{
			//	LocalStorage.Storage[StorageKey.IsAccountRemember] = IsRemembered.ToString();
			//	LocalStorage.Storage[StorageKey.UserEmail] = Email;
			//	LocalStorage.Storage[StorageKey.UserPassword] = Password;
			//}
			//else
			//{
			//	LocalStorage.Storage[StorageKey.IsAccountRemember] = false.ToString();
			//	LocalStorage.Storage[StorageKey.UserEmail] = string.Empty;
			//	LocalStorage.Storage[StorageKey.UserPassword] = string.Empty;
			//}

			if (IsRemembered)
			{
				await SQLiteService.InsertOrUpdateAsync<User>(new User()
				{
					Email = Email,
					Password = Password,
					IsRemember = IsRemembered,
				});
			}

			// Put Lands Page
			PageSwitcher.SwitchMainPageAsync(Singleton.Get<ViewLocator>().Master);
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