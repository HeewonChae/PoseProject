using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Ads;
using Android.Gms.Ads.Formats;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PoseSportsPredict.Droid.Renderer;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Views.Common.Ads;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(PoseSportsPredict.Views.Common.Ads.AdmobSmallNativeAdsView), typeof(AdmobSmallNativeAdsViewRenderer))]

namespace PoseSportsPredict.Droid.Renderer
{
    public class AdmobSmallNativeAdsViewRenderer : ViewRenderer
    {
        private UnifiedNativeAd nativeAd;
        private TextView primaryView;
        private TextView secondaryView;
        private RatingBar ratingBar;
        private ImageView iconView;
        private Android.Widget.Button callToActionView;
        private Android.Graphics.Color ButtonColor;

        public AdmobSmallNativeAdsViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                var adLoader = new AdLoader.Builder(Context, AppConfig.ADMOB_NATIVE_ADS_ID);

                var listener = new UnifiedNativeAdLoadedListener();
                listener.OnNativeAdLoaded += (s, ad) =>
                {
                    // Load ad Completed
                    try
                    {
                        ButtonColor = (e.NewElement as AdmobSmallNativeAdsView).ButtonColor.ToAndroid();
                        nativeAd = ad;
                        var root = new UnifiedNativeAdView(Context);
                        var inflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);

                        var nativeAdView = (UnifiedNativeAdView)inflater.Inflate(Resource.Layout.gnt_small_template_view, root);

                        populateUnifiedNativeAdView(ad, nativeAdView);
                        SetNativeControl(nativeAdView);

                        e.NewElement.HeightRequest = 91; // DependencyService.Resolve<IScreenHelper>().DpToPixels(35);
                        e.NewElement.IsVisible = true;
                    }
                    catch
                    {
                    }
                };

                adLoader.ForUnifiedNativeAd(listener);
                var requestBuilder = new AdRequest.Builder();
                adLoader.Build().LoadAd(requestBuilder.Build());
            }
            else
            {
                e.NewElement.HeightRequest = 91; //DependencyService.Resolve<IScreenHelper>().DpToPixels(35);
                e.NewElement.IsVisible = true;
            }
        }

        private void populateUnifiedNativeAdView(UnifiedNativeAd nativeAd, UnifiedNativeAdView adView)
        {
            // Find View Ctrl
            primaryView = adView.FindViewById<TextView>(Resource.Id.ad_primary);
            secondaryView = adView.FindViewById<TextView>(Resource.Id.ad_secondary);
            //tertiaryView = adView.FindViewById<TextView>(Resource.Id.ad_body);

            ratingBar = adView.FindViewById<RatingBar>(Resource.Id.ad_rating);
            ratingBar.Enabled = false;

            callToActionView = adView.FindViewById<Android.Widget.Button>(Resource.Id.ad_call_to_action);
            iconView = adView.FindViewById<ImageView>(Resource.Id.ad_icon);

            // Ad data
            string store = nativeAd.Store;
            string advertiser = nativeAd.Advertiser;
            string headline = nativeAd.Headline;
            string cta = nativeAd.CallToAction;
            float starRating = nativeAd.StarRating?.FloatValue() ?? 0.0f;
            Drawable icon = nativeAd.Icon?.Drawable ?? null;
            Drawable image = nativeAd.Images.Count > 0 ? nativeAd.Images[0].Drawable : null;

            // dynamic
            string secondaryText;

            // Set value in view
            adView.CallToActionView = callToActionView;
            adView.HeadlineView = primaryView;

            secondaryView.Visibility = ViewStates.Visible;
            if (AdHasOnlyStore(nativeAd))
            {
                adView.StoreView = secondaryView;
                secondaryText = store;
            }
            else if (!string.IsNullOrEmpty(advertiser))
            {
                adView.AdvertiserView = secondaryView;
                secondaryText = advertiser;
            }
            else
            {
                secondaryText = "";
            }

            primaryView.Text = headline;

            // CallToActionView
            callToActionView.Text = cta;
            callToActionView.SetBackgroundColor(ButtonColor);

            //  Set the secondary view to be the star rating if available.
            if (starRating > 0)
            {
                secondaryView.Visibility = ViewStates.Gone;
                ratingBar.Visibility = ViewStates.Visible;
                ratingBar.Max = 5;
                ratingBar.Rating = starRating;
                ratingBar.StepSize = 0.1f;
                adView.StarRatingView = ratingBar;
            }
            else
            {
                secondaryView.Text = secondaryText;
                secondaryView.Visibility = ViewStates.Visible;
                ratingBar.Visibility = ViewStates.Gone;
            }

            if (image != null)
            {
                iconView.Visibility = ViewStates.Visible;
                iconView.SetImageDrawable(image);
            }
            else if (icon != null)
            {
                iconView.Visibility = ViewStates.Visible;
                iconView.SetImageDrawable(icon);
            }
            else
            {
                iconView.Visibility = ViewStates.Gone;
            }

            adView.SetNativeAd(nativeAd);
        }

        private bool AdHasOnlyStore(UnifiedNativeAd nativeAd)
        {
            string store = nativeAd.Store;
            string advertiser = nativeAd.Advertiser;
            return !string.IsNullOrEmpty(store) && string.IsNullOrEmpty(advertiser);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            nativeAd?.Dispose();
        }
    }
}