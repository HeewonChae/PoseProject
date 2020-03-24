using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football;
using PoseSportsPredict.Views;
using Shiny;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels
{
    public class AppMasterMenuViewModel : NavigableViewModel
    {
        #region BaseViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            var sportsPageList = new List<Page>
            {
                ShinyHost.Resolve<FootballMainViewModel>().CoupledPage,
            };

            var sportsCategories = new List<Models.AppMenuItem>();
            foreach (var sportsPage in sportsPageList)
            {
                sportsCategories.Add(new Models.AppMenuItem
                {
                    Title = sportsPage.Title,
                    IconSource = sportsPage.IconImageSource.ToString().Replace("File: ", ""),
                });
            }

            SportsCategories = sportsCategories;

            return true;
        }

        #endregion BaseViewModel

        #region Fields

        private List<Models.AppMenuItem> _sportsCategories;

        #endregion Fields

        #region Properties

        public List<Models.AppMenuItem> SportsCategories { get => _sportsCategories; set => SetValue(ref _sportsCategories, value); }

        #endregion Properties

        #region Constructors

        public AppMasterMenuViewModel(AppMasterMenuPage page) : base(page)
        {
            if (OnInitializeView())
            {
                CoupledPage.Appearing += (s, e) => OnAppearing();
            }
        }

        #endregion Constructors
    }
}