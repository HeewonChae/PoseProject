//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;
//using Shiny;

//namespace Xamarin_Tutorial.Droid
//{
//	[Application]
//	public class MainApplication : Shiny.ShinyAndroidApplication<Xamarin_Tutorial.XamarinShinyStartup>
//	{
//		public MainApplication(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
//		{
//		}

//		public override void OnCreate()
//		{
//			base.OnCreate();
//			Shiny.Notifications.AndroidOptions.DefaultSmallIconResourceName = "ic_stat";
//		}
//	}
//}