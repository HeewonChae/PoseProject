using PoseSportsPredict.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PoseSportsPredict.Views.CustomViews
{
    public class HideableToolbarItem : ToolbarItem
    {
        public HideableToolbarItem() : base()
        {
            OnIsVisibleChanged(this, false, IsVisible);
        }

        public static BindableProperty IsVisibleProperty = BindableProperty.Create(
            nameof(IsVisible),
            typeof(bool),
            typeof(HideableToolbarItem),
            true,
            propertyChanged: OnIsVisibleChanged);

        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        private static void OnIsVisibleChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var item = bindable as HideableToolbarItem;
            var parent = (item.BindingContext as NavigableViewModel)?.CoupledPage;

            if (parent == null)
                return;

            var items = parent.ToolbarItems;

            if ((bool)newvalue && !items.Contains(item))
            {
                Device.BeginInvokeOnMainThread(() => items.Add(item));
            }
            else if (!(bool)newvalue && items.Contains(item))
            {
                Device.BeginInvokeOnMainThread(() => items.Remove(item));
            }
        }
    }
}