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
//using PosePacket.Service.Auth.Models.Enums;
//using PoseSportsPredict.Droid.Renderer;
//using PoseSportsPredict.Services;
//using PoseSportsPredict.Views;
//using Shiny;
//using WebServiceShare.ExternAuthentication;
//using Xamarin.Auth;
//using Xamarin.Forms;
//using Xamarin.Forms.Platform.Android;

//[assembly: ExportRenderer(typeof(OAuthLoginPage), typeof(OAuthPageRenderer))]

//namespace PoseSportsPredict.Droid.Renderer
//{
//    public class OAuthPageRenderer : PageRenderer
//    {
//        private bool done = false;

//        public OAuthPageRenderer(Context context) : base(context)
//        {
//        }

//        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
//        {
//            base.OnElementChanged(e);

//            if (!done)
//            {
//                // this is a ViewGroup - so should be able to load an AXML file and FindView<>
//                var activity = this.Context as Activity;

//                var OAuthService = ShinyHost.Resolve<IOAuthService>();
//                var oAuth = ExternOAuthService.Cur_Authenticator;

//                oAuth.Completed += OAuthService.OnOAuthComplete;
//                oAuth.Error += OAuthService.OnOAuthError;

//                activity.StartActivity(oAuth.GetUI(activity));
//                done = true;
//            }
//        }
//    }
//}