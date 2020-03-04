using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using PlatformConfig = Xamarin.Forms.PlatformConfiguration;

namespace PoseSportsPredict.Views.Football
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedPage1 : TabbedPage
    {
        public TabbedPage1()
        {
            InitializeComponent();
            //PlatformConfig.AndroidSpecific.TabbedPage.SetIsSwipePagingEnabled(this, false);
            this.CurrentPage = todayItems;
        }
    }
}