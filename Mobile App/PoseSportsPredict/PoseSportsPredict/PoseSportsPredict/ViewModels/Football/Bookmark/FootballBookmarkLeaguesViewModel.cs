using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football.Bookmark;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.ViewModels.Football.Bookmark
{
    public class FootballBookmarkLeaguesViewModel : NavigableViewModel
    {
        public FootballBookmarkLeaguesViewModel(FootballBookmarkLeaguesPage coupledPage) : base(coupledPage)
        {
            if (OnInitializeView())
            {
                coupledPage.Appearing += (s, e) => this.OnAppearing();
            }
        }
    }
}