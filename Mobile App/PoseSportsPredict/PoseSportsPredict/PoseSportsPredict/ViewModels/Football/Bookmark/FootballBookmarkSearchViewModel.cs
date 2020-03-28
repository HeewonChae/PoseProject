using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football.Bookmark;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.ViewModels.Football.Bookmark
{
    public class FootballBookmarkSearchViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            return base.OnInitializeView(datas);
        }

        #endregion NavigableViewModel

        #region Constructors

        public FootballBookmarkSearchViewModel(FootballBookmarkSearchPage page) : base(page)
        {
            if (OnInitializeView())
            {
                CoupledPage.Appearing += (s, e) => this.OnAppearing();
            }
        }

        #endregion Constructors
    }
}