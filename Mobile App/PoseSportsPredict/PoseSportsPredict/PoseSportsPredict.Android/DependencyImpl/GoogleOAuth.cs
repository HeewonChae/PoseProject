using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common.Apis;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Interop;
using Plugin.CurrentActivity;
using PoseSportsPredict.Droid.DependencyImpl;
using PoseSportsPredict.InfraStructure;
using Xamarin.Forms;
using static Android.Gms.Common.Apis.GoogleApiClient;

[assembly: Dependency(typeof(GoogleOAuth))]

namespace PoseSportsPredict.Droid.DependencyImpl
{
    public class GoogleOAuth : Java.Lang.Object, IGoogleOAuth, IConnectionCallbacks
    {
        public void Disposed()
        {
        }

        public void DisposeUnlessReferenced()
        {
        }

        public void Finalized()
        {
        }

        private GoogleApiClient googleApiClient;

        public void Login(string clientId)
        {
            GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestIdToken(clientId)
                .RequestEmail()
                .RequestId()
                .RequestProfile()
                .Build();

            googleApiClient = new GoogleApiClient.Builder(CrossCurrentActivity.Current.Activity)
                .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
                .Build();

            googleApiClient.RegisterConnectionCallbacks(this);
            googleApiClient.Connect();
        }

        public async void OnConnected(Bundle connectionHint)
        {
            await Auth.GoogleSignInApi.SignOut(googleApiClient);
            var intent = Auth.GoogleSignInApi.GetSignInIntent(googleApiClient);
            CrossCurrentActivity.Current.Activity.StartActivityForResult(intent, 99);
        }

        public void OnConnectionSuspended(int cause)
        {
        }

        public void SetJniIdentityHashCode(int value)
        {
        }

        public void SetJniManagedPeerState(JniManagedPeerStates value)
        {
        }

        public void SetPeerReference(JniObjectReference reference)
        {
        }
    }
}