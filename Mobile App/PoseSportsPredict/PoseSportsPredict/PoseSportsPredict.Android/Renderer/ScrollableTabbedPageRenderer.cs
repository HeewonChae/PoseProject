using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using PoseSportsPredict.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(ScrollableTabbedPageRenderer))]

namespace PoseSportsPredict.Droid.Renderer
{
    public class ScrollableTabbedPageRenderer : TabbedPageRenderer
    {
        private TabLayout TabsLayout { get; set; }
        private ViewPager PagerLayout { get; set; }

        public ScrollableTabbedPageRenderer(Context context) : base(context)
        {
        }

        public override void OnViewAdded(Android.Views.View child)
        {
            base.OnViewAdded(child);

            //Scrollable
            if (child is TabLayout tabLayout)
            {
                tabLayout.TabMode = TabLayout.ModeScrollable;
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);

            //// Disable tab swipe
            ////find the pager and tabs
            //for (int i = 0; i < ChildCount; ++i)
            //{
            //    Android.Views.View view = (Android.Views.View)GetChildAt(i);
            //    if (view is TabLayout) TabsLayout = (TabLayout)view;
            //    else if (view is ViewPager) PagerLayout = (ViewPager)view;
            //}

            //if (PagerLayout != null) //now disable the swiping
            //{
            //    var propertyInfo = PagerLayout.GetType().GetProperty("EnableGesture");
            //    propertyInfo.SetValue(PagerLayout, false, null);
            //}
        }
    }
}