using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin_Tutorial
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage_UWP : ContentPage
	{
		private string _phone;
		public string Phone 
		{
			get{ return _phone; }
			set 
			{
				_phone = value;
				OnPropertyChanged();
			} 
		}
		public MainPage_UWP()
		{
			Phone = "010-8395-7750";

			InitializeComponent();

			this.BindingContext = this;
		}

		private void Button_Clicked(object sender, EventArgs e)
		{
			Phone = "010-2441-2444";
		}
	}
}