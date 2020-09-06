using System;
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
using PoseSportsPredict.Logics;

namespace PoseSportsPredict.Droid.DeepLinkIntercept
{
    [Activity(Label = "Poseidon Picks", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataSchemes = new[] { "https" },
        DataHost = "poseidonpicks.page.link",
        DataPathPrefix = "/pagelinker",
        AutoVerify = true)]
    public class URLInterceptor : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (Intent.Data != null)
            {
                // Convert Android.Net.Url to Uri
                var uri = new Uri(Intent.Data.ToString());
                PageUriLinker.FromUriQuery(uri.Query);
            }

            var intent = new Intent(this, typeof(MainActivity));
            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            StartActivity(intent);

            this.Finish();

            return;
        }
    }
}