using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.Gms.Ads;
using Android.Gms.Ads.Formats;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using PoseSportsPredict.Droid.Renderer;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Views.Common.Ads;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(PoseSportsPredict.Views.Common.Ads.AdmobMediumNativeAdsView2), typeof(AdmobNativeAdsView2Renderer))]

namespace PoseSportsPredict.Droid.Renderer
{
    public class AdmobNativeAdsView2Renderer : ViewRenderer
    {
        private UnifiedNativeAd nativeAd;
        private TextView primaryView;
        private TextView secondaryView;
        private RatingBar ratingBar;
        private Android.Graphics.Color ButtonColor;

        //private TextView tertiaryView;
        private ImageView iconView;

        private MediaView mediaView;
        private Android.Widget.Button callToActionView;

        public AdmobNativeAdsView2Renderer(Context context) : base(context)
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
                        ButtonColor = (e.NewElement as AdmobMediumNativeAdsView2).ButtonColor.ToAndroid();
                        nativeAd = ad;
                        var root = new UnifiedNativeAdView(Context);
                        var inflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);

                        var nativeAdView = (UnifiedNativeAdView)inflater.Inflate(Resource.Layout.gnt_medium_template_view, root);

                        populateUnifiedNativeAdView(ad, nativeAdView);
                        SetNativeControl(nativeAdView);

                        e.NewElement.HeightRequest = DependencyService.Resolve<IScreenHelper>().DpToPixels(85);
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
                e.NewElement.HeightRequest = DependencyService.Resolve<IScreenHelper>().DpToPixels(85);
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
            mediaView = adView.FindViewById<MediaView>(Resource.Id.ad_media);

            // Ad data
            string store = nativeAd.Store;
            string advertiser = nativeAd.Advertiser;
            string headline = nativeAd.Headline;
            string body = nativeAd.Body;
            string cta = nativeAd.CallToAction;
            float starRating = nativeAd.StarRating?.FloatValue() ?? 0.0f;
            Drawable icon = nativeAd.Icon?.Drawable ?? null;

            // dynamic
            string secondaryText;

            // Set value in view
            adView.CallToActionView = callToActionView;
            adView.HeadlineView = primaryView;
            adView.MediaView = mediaView;

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
                adView.StarRatingView = ratingBar;
            }
            else
            {
                secondaryView.Text = secondaryText;
                secondaryView.Visibility = ViewStates.Visible;
                ratingBar.Visibility = ViewStates.Gone;
            }

            if (icon != null)
            {
                iconView.Visibility = ViewStates.Visible;
                iconView.SetImageDrawable(icon);
            }
            else
            {
                iconView.Visibility = ViewStates.Gone;
            }

            //if (tertiaryView != null)
            //{
            //    tertiaryView.Text = body;
            //    adView.BodyView = tertiaryView;
            //}

            // This method tells the Google Mobile Ads SDK that you have finished populating your
            // native ad view with this native ad.
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