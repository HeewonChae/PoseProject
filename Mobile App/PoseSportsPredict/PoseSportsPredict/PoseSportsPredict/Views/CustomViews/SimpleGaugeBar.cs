using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;
using XF.Material.Forms.UI;

namespace PoseSportsPredict.Views.CustomViews
{
    public class SimpleGaugeBar : ContentView
    {
        public static readonly BindableProperty MaxValueProperty = BindableProperty.Create(
            nameof(MaxValue),
            typeof(double),
            typeof(SimpleGaugeBar),
            -1.0,
            propertyChanged: (bindable, oldValue, newValue) => (bindable as SimpleGaugeBar).ReloadView());

        public static readonly BindableProperty CurValueProperty = BindableProperty.Create(
            nameof(CurValue),
            typeof(double),
            typeof(SimpleGaugeBar),
            -1.0,
            propertyChanged: (bindable, oldValue, newValue) => (bindable as SimpleGaugeBar).ReloadView());

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

        public static readonly BindableProperty IsReverseProperty = BindableProperty.Create(
            nameof(IsReverse),
            typeof(bool),
            typeof(SimpleGaugeBar),
            false);

        public double MaxValue { get { return (double)GetValue(MaxValueProperty); } set { SetValue(MaxValueProperty, value); } }
        public double CurValue { get { return (double)GetValue(CurValueProperty); } set { SetValue(CurValueProperty, value); } }
        public bool IsAnimation { get { return (bool)GetValue(IsAnimationProperty); } set { SetValue(IsAnimationProperty, value); } }
        public Color GaugeColor1 { get { return (Color)GetValue(GaugeColor1Property); } set { SetValue(GaugeColor1Property, value); } }
        public Color GaugeColor2 { get { return (Color)GetValue(GaugeColor2Property); } set { SetValue(GaugeColor2Property, value); } }
        public Color GaugeBackgroundColor { get { return (Color)GetValue(GaugeBackgroundColorProperty); } set { SetValue(GaugeBackgroundColorProperty, value); } }
        public bool IsReverse { get { return (bool)GetValue(IsReverseProperty); } set { SetValue(IsReverseProperty, value); } }

        #region Fields

        private ColumnDefinition _column1 = new ColumnDefinition();
        private ColumnDefinition _column2 = new ColumnDefinition();
        private PancakeView _frame = new PancakeView();
        private Grid _grid = new Grid();
        private PancakeView _gaugeBar = new PancakeView();
        private BoxView _boxView = new BoxView();

        #endregion Fields

        public SimpleGaugeBar()
        {
            _frame.Padding = 0;
            _frame.Margin = 0;

            _grid = new Grid();
            _grid.ColumnSpacing = 0;
            _grid.ColumnDefinitions.Add(_column1);
            _grid.ColumnDefinitions.Add(_column2);

            _boxView.Color = Color.Transparent;

            //_boxView1.HorizontalOptions = new LayoutOptions
            //{
            //    Alignment = LayoutAlignment.Start,
            //};
            //_boxView2.HorizontalOptions = new LayoutOptions
            //{
            //    Alignment = LayoutAlignment.End,
            //};

            _grid.Children.Add(_gaugeBar, 0, 0);
            _grid.Children.Add(_boxView, 1, 0);

            _frame.Content = _grid;
            Content = _frame;

            ReloadView();
        }

        private void ReloadView()
        {
            if (CurValue == -1 || CurValue > MaxValue)
                return;

            _frame.BackgroundColor = GaugeBackgroundColor;

            var gaugeRate = MaxValue == 0 ? 0 : CurValue / MaxValue;
            _column1.Width = new GridLength(gaugeRate, GridUnitType.Star);
            _column2.Width = new GridLength((1 - gaugeRate), GridUnitType.Star);

            // Gauge
            if (gaugeRate == 0.5)
                _gaugeBar.BackgroundColor = GaugeColor2;
            else
                _gaugeBar.BackgroundColor =
                    gaugeRate > 0.5 ?
                    IsReverse ? GaugeColor2 : GaugeColor1 :
                    IsReverse ? GaugeColor1 : GaugeColor2;

            _frame.CornerRadius = FlowDirection == FlowDirection.LeftToRight ? new CornerRadius(0, 5, 0, 5) : new CornerRadius(5, 0, 5, 0);
            _gaugeBar.CornerRadius = FlowDirection == FlowDirection.LeftToRight ? new CornerRadius(0, 5, 0, 5) : new CornerRadius(5, 0, 5, 0);

            //if (IsAnimation && gaugeLenth.Value > 0)
            //{
            //    gaugeBox.WidthRequest = 0;
            //    var screenWidth = DependencyService.Resolve<IScreenHelper>().ScreenSize.Width;

            //    await Task.Delay(500);
            //    var animate = new Animation(d => gaugeBox.WidthRequest = d, 0, screenWidth / 2.0d, Easing.SinInOut);
            //    animate.Commit(gaugeBox, "BarGraph", 16, 1000);
            //}
        }
    }
}