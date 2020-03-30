using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.ViewModels.Football.Match;
using Shiny;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PoseSportsPredict.Models
{
    public class FootballMatchGroup : IGroupBase, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
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
        public int MatchCount => FootballMatchListViewModel.MatchCount;
        public string StateIcon => Expanded ? "ic_expanded.png" : "ic_collapsed.png";
        public string CountryLogo => _countryLogo;
        public int GroupHeaderHeight => _groupHeaderHeight;
        public FootballMatchListViewModel FootballMatchListViewModel { get; set; }

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

        public FootballMatchGroup(string title, string countryLogo, bool isAlarmEditMode, FootballMatchInfo[] matches, bool expanded = true)
        {
            _title = title;
            _expanded = expanded;
            _countryLogo = countryLogo;
            _expanded = expanded;

            FootballMatchListViewModel = ShinyHost.Resolve<FootballMatchListViewModel>();
            FootballMatchListViewModel.AlarmEditMode = isAlarmEditMode;
            FootballMatchListViewModel.Matches = new ObservableCollection<FootballMatchInfo>(matches);
        }

        #endregion Constructors
    }
}