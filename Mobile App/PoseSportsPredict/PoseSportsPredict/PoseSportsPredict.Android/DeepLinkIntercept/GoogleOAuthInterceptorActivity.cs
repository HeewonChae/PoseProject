﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PoseSportsPredict.Droid.Renderer;
using PoseSportsPredict.Services;
using Xamarin.Auth;

namespace PoseSportsPredict.Droid
{
    [Activity(Label = "GoogleOAuthInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataSchemes = new[] { "com.pose.poseidon.picks" },
        DataPath = "/oauth2redirect")]
    public class GoogleOAuthInterceptorActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Android.Net.Uri uri_android = Intent.Data;

            // Convert Android.Net.Url to Uri
            var uri = new Uri(uri_android.ToString());

            // Load redirectUrl page
            ExternOAuthService.Cur_Authenticator.OnPageLoading(uri);

            var intent = new Intent(this, typeof(MainActivity));
            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            StartActivity(intent);

            this.Finish();

            return;
        }
    }
}