using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace PoseSportsPredict.Views.Football.Bookmark
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FootballBookmarkSearchPage : PopupPage
    {
        private double _lastScorllY;

        public FootballBookmarkSearchPage()
        {
            InitializeComponent();
        }

        private void lv_Scrolled(object sender, ScrolledEventArgs e)
        {
            if (_lastScorllY != e.ScrollY)
            {
                _lastScorllY = e.ScrollY;

                if (_searchBar.IsFocused)
                    _searchBar.Unfocus();
            }
        }
    }
}