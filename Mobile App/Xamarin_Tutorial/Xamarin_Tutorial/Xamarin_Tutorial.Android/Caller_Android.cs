using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin_Tutorial.Droid;

[assembly: Dependency(typeof(Caller_Android))]
namespace Xamarin_Tutorial.Droid
{
	public class Caller_Android : IDialer
	{
		public bool Dial(string phoneNumber)
		{
			System.Diagnostics.Debug.WriteLine("Calling in Android");
			return true;
		}
	}
}