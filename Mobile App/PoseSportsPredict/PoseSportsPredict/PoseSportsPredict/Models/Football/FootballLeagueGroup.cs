using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.ViewModels.Football.League;
using Shiny;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace PoseSportsPredict.Models
{
    public class FootballLeagueGroup : IGroupBase, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged

        #region Fields

        private bool _expanded;
        private string _title;
        private string _countryLogo;
        private int _groupHeaderHeight = 33;

        #endregion Fields

        #region Properties

        public string Title => _title;
        public string StateIcon => Expanded ? "ic_expanded.png" : "ic_collapsed.png";
        public string CountryLogo => _countryLogo;
        public int GroupHeaderHeight => _groupHeaderHeight;
        public FootballLeagueListViewModel FootballLeagueListViewModel { get; set; }

        public bool Expanded
        {
            get { return _expanded; }
            set
            {
                if (_expanded != value)
                {
                    _expanded = value;
                    OnPropertyChanged("StateIcon");
                }
            }
        }

        #endregion Properties

        #region Constructors

        public FootballLeagueGroup(string title, string countryLogo, FootballLeagueInfo[] leagues, bool expanded = false)
        {
            _title = title;
            _countryLogo = countryLogo;
            _expanded = expanded;

            FootballLeagueListViewModel = ShinyHost.Resolve<FootballLeagueListViewModel>();
            FootballLeagueListViewModel.Leagues = new ObservableCollection<FootballLeagueInfo>(leagues);
        }

        #endregion Constructors
    }
}