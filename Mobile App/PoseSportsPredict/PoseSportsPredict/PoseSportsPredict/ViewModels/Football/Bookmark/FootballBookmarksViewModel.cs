using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football.Bookmark;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.ViewModels.Football.Bookmark
{
    public class FootballBookmarksViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override void OnAppearing(params object[] datas)
        {
            base.OnAppearing(datas);
        }

        #endregion NavigableViewModel

        #region Constructors

        public FootballBookmarksViewModel(FootballBookmarksPage page) : base(page)
        {
            OnInitializeView();
        }

        #endregion Constructors
    }
}