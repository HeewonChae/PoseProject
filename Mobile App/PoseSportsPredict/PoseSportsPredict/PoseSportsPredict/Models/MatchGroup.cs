using PosePacket.Service.Football;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace PoseSportsPredict.Models
{
    public class MatchGroup : ObservableCollection<O_GET_FIXTURES_BY_DATE.FixtureInfo>, INotifyPropertyChanged
    {
        //#region INotifyPropertyChanged

        //public event PropertyChangedEventHandler PropertyChanged;

        //protected virtual void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        //#endregion INotifyPropertyChanged

        #region Fields

        private bool _expanded;
        private string _title;
        private string _countryLogo;

        #endregion Fields

        #region Properties

        public string Title => _title;
        public int MatchCount => this.Count;
        public string StateIcon => Expanded ? "ic_expanded.png" : "ic_collapsed.png";
        public string CountryLogo => _countryLogo;

        public bool Expanded
        {
            get { return _expanded; }
            set
            {
                if (_expanded != value)
                {
                    _expanded = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("StateIcon"));
                }
            }
        }

        #endregion Properties

        #region Constructors

        public MatchGroup(string title, string countryLogo, bool expanded = true)
        {
            _title = title;
            _expanded = expanded;
            _countryLogo = countryLogo;
        }

        #endregion Constructors
    }
}