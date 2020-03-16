using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football;
using PoseSportsPredict.Views;
using Shiny;
using System.Collections.Generic;

namespace PoseSportsPredict.ViewModels
{
    public class AppMasterMenuViewModel : NavigableViewModel
    {
        #region BaseViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            if (datas == null)
                return true;

            var sportsCategories = new List<Models.AppMenuItem>();
            foreach (var data in datas)
            {
                if (data is NavigableViewModel viewModel)
                {
                    sportsCategories.Add(new Models.AppMenuItem
                    {
                        Title = viewModel.CoupledPage.Title,
                        IconSource = viewModel.CoupledPage.IconImageSource.ToString().Replace("File: ", ""),
                    });
                }
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
            if (OnInitializeView(ShinyHost.Resolve<FootballMainViewModel>()))
            {
                CoupledPage.Appearing += (s, e) => OnAppearing();
            }
        }

        #endregion Constructors
    }
}