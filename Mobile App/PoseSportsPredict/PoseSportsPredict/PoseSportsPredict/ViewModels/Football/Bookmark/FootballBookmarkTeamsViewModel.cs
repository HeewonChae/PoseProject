using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football.Bookmark;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels.Football.Bookmark
{
    public class FootballBookmarkTeamsViewModel : NavigableViewModel
    {
        public FootballBookmarkTeamsViewModel(FootballBookmarkTeamsPage coupledPage) : base(coupledPage)
        {
            if (OnInitializeView())
            {
                coupledPage.Appearing += (s, e) => this.OnAppearing();
            }
        }
    }
}