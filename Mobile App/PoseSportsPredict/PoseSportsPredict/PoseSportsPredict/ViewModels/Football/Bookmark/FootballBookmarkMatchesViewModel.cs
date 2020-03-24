using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football.Bookmark;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.ViewModels.Football.Bookmark
{
    public class FootballBookmarkMatchesViewModel : NavigableViewModel
    {
        public FootballBookmarkMatchesViewModel(FootballBookmarkMatchesPage coupledPage) : base(coupledPage)
        {
            if (OnInitializeView())
            {
                coupledPage.Appearing += (s, e) => this.OnAppearing();
            }
        }
    }
}