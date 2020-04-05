using PoseSportsPredict.InfraStructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Models
{
    public class ChangableIcon : IIconChange, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged

        #region Fields

        private string _selectedIcon;
        private string _unselectedIcon;
        private Color _selectedColor;
        private Color _unselectedColor;
        private bool _isSelected;

        #endregion Fields

        #region Properties

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;

                    OnPropertyChanged(nameof(CurrentIcon));
                    OnPropertyChanged(nameof(CurrentColor));
                }
            }
        }

        public string CurrentIcon => IsSelected ? _selectedIcon : _unselectedIcon;

        public Color CurrentColor => IsSelected ? _selectedColor : _unselectedColor;

        #endregion Properties

        public ChangableIcon(string selectedIcon, Color selectedColor, string unselectedIcon, Color unselectedColor)
        {
            _isSelected = false;
            _selectedIcon = selectedIcon;
            _selectedColor = selectedColor;
            _unselectedIcon = unselectedIcon;
            _unselectedColor = unselectedColor;
        }
    }
}