using GalaSoft.MvvmLight.Command;
using PosePacket.Service.Enum;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Services;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.League.Detail;
using PoseSportsPredict.ViewModels.Football.Team;
using Shiny;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels
{
    public class BookmarkMenuListViewModel : BaseViewModel, IExpandable, ITempletable
    {
        #region BaseViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            _expanded = true;

            // Football
            var message = BookmarkServiceHelper.BuildBookmarkMessage(null, SportsType.Football, PageDetailType.League);
            MessagingCenter.Subscribe<BookmarkService, FootballLeagueInfo>(this, message, (s, e) => BookmarkMessageHandler(e));
            message = BookmarkServiceHelper.BuildBookmarkMessage(null, SportsType.Football, PageDetailType.Team);
            MessagingCenter.Subscribe<BookmarkService, FootballTeamInfo>(this, message, (s, e) => BookmarkMessageHandler(e));

            return true;
        }

        #endregion BaseViewModel

        #region Fields

        private string _title;
        private bool _expanded;
        private ObservableCollection<IBookmarkMenuItem> _items;

        #endregion Fields

        #region Properties

        public MatchGroupType GroupType { get; set; }
        public AdsBannerType AdsBannerType { get; set; }
        public PageDetailType BookMarkType { get; set; }
        public ObservableCollection<IBookmarkMenuItem> Items { get => _items; set => SetValue(ref _items, value); }
        public string StateIcon => Expanded ? "ic_expanded.png" : "ic_collapsed.png";
        public string Title { get => _title; set => SetValue(ref _title, value); }

        public bool Expanded
        {
            get { return _expanded; }
            set
            {
                SetValue(ref _expanded, value);
                OnPropertyChanged("StateIcon");
            }
        }

        public string TitleLogo { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        #endregion Properties

        #region Commands

        public ICommand SelectGroupHeaderCommand { get => new RelayCommand(SelectGroupHeader); }

        private void SelectGroupHeader()
        {
            Expanded = !Expanded;
        }

        public ICommand SelectItemCommand { get => new RelayCommand<IBookmarkMenuItem>((e) => SelectItem(e)); }

        private async void SelectItem(IBookmarkMenuItem item)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            switch (item)
            {
                case FootballLeagueInfo footballLeague:
                    await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballLeagueDetailViewModel>(), footballLeague);
                    break;

                case FootballTeamInfo footballTeam:
                    await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballTeamDetailViewModel>(), footballTeam);
                    break;
            }

            SetIsBusy(false);
        }

        #endregion Commands

        public BookmarkMenuListViewModel()
        {
            OnInitializeView();
        }

        private async void BookmarkMessageHandler(IBookmarkMenuItem item)
        {
            if (_items == null
                || item.BookMarkType != this.BookMarkType)
                return;

            SetIsBusy(true);

            switch (BookMarkType)
            {
                case PageDetailType.League:
                    await ShinyHost.Resolve<AppMasterMenuViewModel>().RefrashBookmarkedLeague();
                    break;

                case PageDetailType.Team:
                    await ShinyHost.Resolve<AppMasterMenuViewModel>().RefrashBookmarkedTeam();
                    break;
            }

            SetIsBusy(false);
        }
    }
}