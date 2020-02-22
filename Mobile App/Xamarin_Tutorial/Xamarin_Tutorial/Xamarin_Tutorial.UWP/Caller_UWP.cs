﻿using Xamarin.Forms;
using Xamarin_Tutorial.UWP;

[assembly: Dependency(typeof(Caller_UWP))]

namespace Xamarin_Tutorial.UWP
{
	public class Caller_UWP : IDialer
	{
		public bool Dial(string phoneNumber)
		{
			System.Diagnostics.Debug.WriteLine("Calling in UWP");
			return true;
		}
	}
}