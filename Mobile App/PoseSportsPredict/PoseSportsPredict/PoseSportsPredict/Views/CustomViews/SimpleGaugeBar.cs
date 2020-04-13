using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using XF.Material.Forms.UI;

namespace PoseSportsPredict.Views.CustomViews
{
    public class SimpleGaugeBar : ContentView
    {
        public static readonly BindableProperty MaxValueProperty = BindableProperty.Create(
            nameof(MaxValue),
            typeof(double),
            typeof(SimpleGaugeBar),
            -1.0);

        public static readonly BindableProperty CurValueProperty = BindableProperty.Create(
            nameof(CurValue),
            typeof(double),
            typeof(SimpleGaugeBar),
            -1.0,
            propertyChanged: OnPropertyChanged);

        public static readonly BindableProperty IsLeftStartProperty = BindableProperty.Create(
            nameof(IsLeftStart),
            typeof(bool),
            typeof(SimpleGaugeBar),
            true);

        public static readonly BindableProperty IsAnimationProperty = BindableProperty.Create(
            nameof(IsAnimation),
            typeof(bool),
            typeof(SimpleGaugeBar),
            false);

        public static readonly BindableProperty GaugeColor1Property = BindableProperty.Create(
            nameof(GaugeColor1),
            typeof(Color),
            typeof(SimpleGaugeBar),
            Color.Black);

        public static readonly BindableProperty GaugeColor2Property = BindableProperty.Create(
            nameof(GaugeColor2),
            typeof(Color),
            typeof(SimpleGaugeBar),
            Color.Black);

        public static readonly BindableProperty GaugeBackgroundColorProperty = BindableProperty.Create(
            nameof(GaugeBackgroundColor),
            typeof(Color),
            typeof(SimpleGaugeBar),
            Color.Gray);

        private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as SimpleGaugeBar;
            view.ReloadView();
        }

        public double MaxValue { get { return (double)GetValue(MaxValueProperty); } set { SetValue(MaxValueProperty, value); } }
        public double CurValue { get { return (double)GetValue(CurValueProperty); } set { SetValue(CurValueProperty, value); } }
        public bool IsLeftStart { get { return (bool)GetValue(IsLeftStartProperty); } set { SetValue(IsLeftStartProperty, value); } }
        public bool IsAnimation { get { return (bool)GetValue(IsAnimationProperty); } set { SetValue(IsAnimationProperty, value); } }
        public Color GaugeColor1 { get { return (Color)GetValue(GaugeColor1Property); } set { SetValue(GaugeColor1Property, value); } }
        public Color GaugeColor2 { get { return (Color)GetValue(GaugeColor2Property); } set { SetValue(GaugeColor2Property, value); } }
        public Color GaugeBackgroundColor { get { return (Color)GetValue(GaugeBackgroundColorProperty); } set { SetValue(GaugeBackgroundColorProperty, value); } }

        #region Fields

        private ColumnDefinition _column1 = new ColumnDefinition();
        private ColumnDefinition _column2 = new ColumnDefinition();
        private Frame _frame = new Frame();
        private Grid _grid = new Grid();
        private BoxView _boxView1 = new BoxView();
        private BoxView _boxView2 = new BoxView();
        private Label _textLable = new Label();

        #endregion Fields

        public SimpleGaugeBar()
        {
            _frame.Padding = 0;
            _frame.Margin = 0;
            _frame.HasShadow = false;
            //_frame.CornerRadius = 5;

            _grid = new Grid();
            _grid.ColumnSpacing = 0;
            _grid.ColumnDefinitions.Add(_column1);
            _grid.ColumnDefinitions.Add(_column2);

            //_boxView1.CornerRadius = 5;
            //_boxView2.CornerRadius = 5;

            _grid.Children.Add(_boxView1, 0, 0);
            _grid.Children.Add(_boxView2, 1, 0);

            _frame.Content = _grid;
            Content = _frame;
        }

        private void ReloadView()
        {
            if (CurValue > MaxValue)
                throw new Exception("CurValue is bigger than MaxValue");

            _frame.BackgroundColor = GaugeBackgroundColor;

            var gaugeRate = CurValue / MaxValue;
            GridLength gaugeLenth = new GridLength(gaugeRate, GridUnitType.Star);
            GridLength ungaugeLenth = new GridLength((1 - gaugeRate), GridUnitType.Star);

            ColumnDefinition gaugeColumn = IsLeftStart ? _column1 : _column2;
            ColumnDefinition ungaugeColumn = IsLeftStart ? _column2 : _column1;
            gaugeColumn.Width = gaugeLenth;
            ungaugeColumn.Width = ungaugeLenth;

            // Gauge
            BoxView gaugeBox = IsLeftStart ? _boxView1 : _boxView2;
            BoxView ungaugeBox = IsLeftStart ? _boxView2 : _boxView1;
            gaugeBox.Color = gaugeRate > 0.5 ? GaugeColor1 : GaugeColor2;
            ungaugeBox.Color = Color.Transparent;

            if (IsAnimation && gaugeLenth.Value > 0)
            {
                var animation = new Animation(v =>
                {
                    gaugeColumn.Width = new GridLength(v, GridUnitType.Star);
                },
                0,
                gaugeLenth.Value,
                Easing.Linear);

                animation.Commit(this, "default animation", 16, 700, Easing.Linear);
            }
        }
    }
}