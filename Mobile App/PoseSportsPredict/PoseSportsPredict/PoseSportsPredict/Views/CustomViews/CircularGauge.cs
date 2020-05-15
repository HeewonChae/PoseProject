using Syncfusion.SfGauge.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Views.CustomViews
{
    public class CircularGauge : ContentView
    {
        public static readonly BindableProperty MaxValueProperty = BindableProperty.Create(
            nameof(MaxValue),
            typeof(double),
            typeof(CircularGauge),
            -1.0,
            propertyChanged: (bindable, oldValue, newValue) => { (bindable as CircularGauge).ReloadView(); });

        public static readonly BindableProperty CurValueProperty = BindableProperty.Create(
            nameof(CurValue),
            typeof(double),
            typeof(CircularGauge),
            -1.0,
            propertyChanged: (bindable, oldValue, newValue) => { (bindable as CircularGauge).ReloadView(); });

        public static readonly BindableProperty GaugeColorProperty = BindableProperty.Create(
            nameof(GaugeColor),
            typeof(Color),
            typeof(CircularGauge),
            Color.Black);

        public static readonly BindableProperty UngaugeColorProperty = BindableProperty.Create(
            nameof(UngaugeColor),
            typeof(Color),
            typeof(CircularGauge),
            Color.Black);

        public static readonly BindableProperty GaugeHeightProperty = BindableProperty.Create(
            nameof(GaugeHeight),
            typeof(double),
            typeof(CircularGauge),
            -1.0);

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(CircularGauge),
            string.Empty);

        public static readonly BindableProperty HeaderSizeProperty = BindableProperty.Create(
           nameof(HeaderSize),
           typeof(int),
           typeof(CircularGauge),
           0);

        public static readonly BindableProperty TextSizeProperty = BindableProperty.Create(
           nameof(TextSize),
           typeof(int),
           typeof(CircularGauge),
           0);

        public static readonly BindableProperty GaugeThicknessProperty = BindableProperty.Create(
           nameof(GaugeThickness),
           typeof(double),
           typeof(CircularGauge),
           10.0);

        public double MaxValue { get { return (double)GetValue(MaxValueProperty); } set { SetValue(MaxValueProperty, value); } }
        public double CurValue { get { return (double)GetValue(CurValueProperty); } set { SetValue(CurValueProperty, value); } }
        public Color GaugeColor { get { return (Color)GetValue(GaugeColorProperty); } set { SetValue(GaugeColorProperty, value); } }
        public Color UngaugeColor { get { return (Color)GetValue(UngaugeColorProperty); } set { SetValue(UngaugeColorProperty, value); } }
        public double GaugeHeight { get { return (double)GetValue(GaugeHeightProperty); } set { SetValue(GaugeHeightProperty, value); } }
        public string Text { get { return (string)GetValue(TextProperty); } set { SetValue(TextProperty, value); } }
        public int HeaderSize { get { return (int)GetValue(HeaderSizeProperty); } set { SetValue(HeaderSizeProperty, value); } }
        public int TextSize { get { return (int)GetValue(TextSizeProperty); } set { SetValue(TextSizeProperty, value); } }
        public double GaugeThickness { get { return (double)GetValue(GaugeThicknessProperty); } set { SetValue(GaugeThicknessProperty, value); } }

        #region Fields

        private SfCircularGauge _circularGauge = new SfCircularGauge();
        private Header _header1 = new Header();
        private Header _header2 = new Header();
        private Range _gaugeRange = new Range();
        private Range _ungaugeRange = new Range();

        #endregion Fields

        public CircularGauge()
        {
            _header1.TextSize = 24;
            _header1.Position = new Point(0.5, 0.5);

            _header2.Position = new Point(0.5, 0.91);

            _gaugeRange.StartValue = 0;
            _gaugeRange.Offset = 1;

            _ungaugeRange.EndValue = 100;
            _ungaugeRange.Offset = 1;

            _circularGauge.Scales = new ObservableCollection<Scale>
            {
                new Scale
                {
                    StartValue = 0,
                    EndValue = 100,
                    ShowLabels = false,
                    ShowTicks = false,
                    ShowRim = false,
                    Ranges = new ObservableCollection<Range>
                    {
                        _gaugeRange, _ungaugeRange,
                    },
                }
            };

            _circularGauge.Headers.Add(_header1);
            _circularGauge.Headers.Add(_header2);

            this.Content = _circularGauge;
        }

        private void ReloadView()
        {
            if (CurValue == -1.0 || CurValue > MaxValue)
                return;

            _circularGauge.HeightRequest = this.GaugeHeight;

            int avg = (int)Math.Round((CurValue / MaxValue) * 100, 0);
            _header1.Text = $"{avg}%";
            _header1.TextSize = this.HeaderSize;
            _header1.ForegroundColor = this.GaugeColor;

            _header2.Text = this.Text;
            _header2.TextSize = this.TextSize;
            _header2.ForegroundColor = this.GaugeColor;
            _header2.FontAttributes = FontAttributes.Bold;

            _gaugeRange.EndValue = avg;
            _gaugeRange.Thickness = this.GaugeThickness;
            _gaugeRange.Color = this.GaugeColor;

            _ungaugeRange.StartValue = avg;
            _ungaugeRange.Thickness = this.GaugeThickness;
            _ungaugeRange.Color = this.UngaugeColor;
        }
    }
}