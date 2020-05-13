using PoseSportsPredict.Logics;
using PoseSportsPredict.Models.Football;
using Syncfusion.SfChart.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PoseSportsPredict.Views.CustomViews
{
    public class GoalLineChart : ContentView
    {
        public static readonly BindableProperty ItemsSource1Property = BindableProperty.Create(
            nameof(ItemsSource1),
            typeof(IEnumerable<FootballGoalLineChartData>),
            typeof(GoalLineChart),
            null,
            propertyChanged: (bindable, oldValue, newValue) => { (bindable as GoalLineChart).ItemsSource1Changed(); });

        public static readonly BindableProperty ItemsSource2Property = BindableProperty.Create(
            nameof(ItemsSource2),
            typeof(IEnumerable<FootballGoalLineChartData>),
            typeof(GoalLineChart),
            null,
            propertyChanged: (bindable, oldValue, newValue) => { (bindable as GoalLineChart).ItemsSource2Changed(); });

        public static readonly BindableProperty Chart1ColorProperty = BindableProperty.Create(
            nameof(Chart1Color),
            typeof(Color),
            typeof(GoalLineChart),
            Color.Accent);

        public static readonly BindableProperty Chart2ColorProperty = BindableProperty.Create(
            nameof(Chart2Color),
            typeof(Color),
            typeof(GoalLineChart),
            Color.Accent);

        public static readonly BindableProperty ChartSelectedColorProperty = BindableProperty.Create(
            nameof(ChartSelectedColor),
            typeof(Color),
            typeof(GoalLineChart),
            Color.Red);

        public static readonly BindableProperty SelectedCommandProperty = BindableProperty.Create(
            nameof(SelectedCommand),
            typeof(ICommand),
            typeof(GoalLineChart),
            propertyChanged: (bindable, oldValue, newValue) => { (bindable as GoalLineChart).CommandChanged(); });

        public IEnumerable<FootballGoalLineChartData> ItemsSource1 { get { return (IEnumerable<FootballGoalLineChartData>)GetValue(ItemsSource1Property); } set { SetValue(ItemsSource1Property, value); } }
        public IEnumerable<FootballGoalLineChartData> ItemsSource2 { get { return (IEnumerable<FootballGoalLineChartData>)GetValue(ItemsSource2Property); } set { SetValue(ItemsSource2Property, value); } }
        public Color Chart1Color { get { return (Color)GetValue(Chart1ColorProperty); } set { SetValue(Chart1ColorProperty, value); } }
        public Color Chart2Color { get { return (Color)GetValue(Chart2ColorProperty); } set { SetValue(Chart2ColorProperty, value); } }
        public Color ChartSelectedColor { get { return (Color)GetValue(ChartSelectedColorProperty); } set { SetValue(ChartSelectedColorProperty, value); } }
        public ICommand SelectedCommand { get { return (ICommand)GetValue(SelectedCommandProperty); } set { SetValue(SelectedCommandProperty, value); } }

        #region Fields

        private SfChart _chart = new SfChart();
        private CategoryAxis _primaryAxis = new CategoryAxis();
        private NumericalAxis _secondaryAxis = new NumericalAxis();
        private ChartDataMarker _dataMarker = new ChartDataMarker();
        private SplineSeries _splineSeries1;
        private SplineSeries _splineSeries2;

        #endregion Fields

        public GoalLineChart()
        {
            _chart.BackgroundColor = Color.Transparent;

            _primaryAxis.AxisLineOffset = 10;
            _primaryAxis.PlotOffset = 20;
            _primaryAxis.ShowMajorGridLines = false;

            _secondaryAxis.Minimum = 0;
            _secondaryAxis.PlotOffset = 29;
            _secondaryAxis.ShowMajorGridLines = false;
            _secondaryAxis.IsVisible = false;

            _dataMarker.LabelStyle.LabelPosition = DataMarkerLabelPosition.Center;

            _chart.PrimaryAxis = _primaryAxis;
            _chart.SecondaryAxis = _secondaryAxis;

            _splineSeries1 = new SplineSeries
            {
                XBindingPath = "Category",
                YBindingPath = "Score",
                DataMarker = _dataMarker,
                SplineType = SplineType.Monotonic,
                StrokeWidth = 1,
                EnableAnimation = true,
                AnimationDuration = 1.0,
            };

            _splineSeries2 = new SplineSeries
            {
                XBindingPath = "Category",
                YBindingPath = "Score",
                DataMarker = _dataMarker,
                SplineType = SplineType.Monotonic,
                StrokeWidth = 1,
                EnableAnimation = true,
                AnimationDuration = 1.0,
            };

            _chart.Series.Add(_splineSeries1);
            _chart.Series.Add(_splineSeries2);

            this.Content = _chart;
        }

        #region Methods

        private void ItemsSource1Changed()
        {
            int maxValue = Math.Max(ItemsSource1?.Max(elem => elem.Score) ?? 0, ItemsSource2?.Max(elem => elem.Score) ?? 0);
            _secondaryAxis.Maximum = maxValue;

            _splineSeries1.ItemsSource = ItemsSource1;
            _splineSeries1.EnableDataPointSelection = true;
            _splineSeries1.Color = Chart1Color;
            _splineSeries1.SelectedDataPointColor = ChartSelectedColor;
        }

        private void ItemsSource2Changed()
        {
            int maxValue = Math.Max(ItemsSource1?.Max(elem => elem.Score) ?? 0, ItemsSource2?.Max(elem => elem.Score) ?? 0);
            _secondaryAxis.Maximum = maxValue;

            _splineSeries2.ItemsSource = ItemsSource2;
            _splineSeries2.EnableDataPointSelection = true;
            _splineSeries2.Color = Chart2Color;
            _splineSeries2.SelectedDataPointColor = ChartSelectedColor;
        }

        private void CommandChanged()
        {
            _chart.SelectionChanged += (s, e) =>
            {
                SelectedCommand.Execute(e);
            };
        }

        #endregion Methods
    }
}