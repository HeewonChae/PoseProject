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

        //private TextView tertiaryView;
        private ImageView iconView;

        private MediaView mediaView;
        private Android.Widget.Button callToActionView;

        public AdmobSmallNativeAdsViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                var adLoader = new AdLoader.Builder(Context, "ca-app-pub-3381862928005780/5854172385");

                var listener = new UnifiedNativeAdLoadedListener();
                listener.OnNativeAdLoaded += (s, ad) =>
                {
                    // Load ad Completed
                    try
                    {
                        nativeAd = ad;
                        var root = new UnifiedNativeAdView(Context);
                        var inflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);

                        UnifiedNativeAdView nativeAdView = null;
                        if (e.NewElement.BindingContext is ITempletable templetable)
                        {
                            if (templetable.NativeAdsType == Models.Enums.NativeAdsSizeType.Medium)
                            {
                                nativeAdView = (UnifiedNativeAdView)inflater.Inflate(Resource.Layout.gnt_medium_template_view, root);
                                e.NewElement.HeightRequest = DependencyService.Resolve<IScreenHelper>().DpToPixels(84);
                            }
                            else if (templetable.NativeAdsType == Models.Enums.NativeAdsSizeType.Small)
                            {
                                nativeAdView = (UnifiedNativeAdView)inflater.Inflate(Resource.Layout.gnt_small_template_view, root);
                                e.NewElement.HeightRequest = DependencyService.Resolve<IScreenHelper>().DpToPixels(39);
                            }
                        }

                        populateUnifiedNativeAdView(ad, nativeAdView);

                        SetNativeControl(nativeAdView);

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
                if (e.NewElement.BindingContext is ITempletable templetable)
                {
                    e.NewElement.IsVisible = true;

                    if (templetable.NativeAdsType == Models.Enums.NativeAdsSizeType.Medium)
                        e.NewElement.HeightRequest = DependencyService.Resolve<IScreenHelper>().DpToPixels(84);
                    else if (templetable.NativeAdsType == Models.Enums.NativeAdsSizeType.Small)
                        e.NewElement.HeightRequest = DependencyService.Resolve<IScreenHelper>().DpToPixels(39);
                }
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