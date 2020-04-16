using LogicCore.Utility;
using LogicCore.Debug;
using LogicCore.Thread;
using MahApps.Metro.Controls;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading;

using FootballLogic = SportsAdminTool.Logic.Football;
using FootballAlarm = SportsAdminTool.Logic.Football.Alarm;
using System;
using FootballCommands = SportsAdminTool.Commands.Football;

namespace SportsAdminTool
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow, Singleton.INode
    {
        public MainWindow()
        {
            InitializeComponent();

            // 다른 컨텍스트에서 해당 윈도우에 접근하기 위해 싱글톤으로 등록
            Singleton.Register(this);

            Logic.LogicStatic.StartWindow();
        }

        private void TitleBar_ListIcon_Click(object sender, RoutedEventArgs e)
        {
            this._globalFlyout.IsOpen = true;
        }

        private void Fv_football_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var flipview = ((FlipView)sender);
            switch (flipview.SelectedIndex)
            {
                case 0:
                    flipview.BannerText = "Initialize FootballDB";
                    break;

                case 1:
                    flipview.BannerText = "Collect Datas & Predict";
                    break;

                case 2:
                    flipview.BannerText = "Check Completed Fixtures";
                    break;
            }
        }

        #region Thread Safe Change UI Element

        /// <summary>
        /// Set Label Content
        /// </summary>
        /// <param name="str"></param>
        public void Set_Lable(Label e, string str)
        {
            Dev.DebugString($"Set_Lable: {nameof(e)} - {str}");

            this.Dispatcher.Invoke(() =>
            {
                e.Content = str;
            });
        }

        #endregion Thread Safe Change UI Element

        /// <summary>
        /// Initialize footballdb
        /// </summary>
        public async void Fv_item_initialize_footballdb_Click(object sender, MouseButtonEventArgs e)
        {
            if (this._progRing_initialize_footballdb.IsActive)
                return;

            // 해당 로직에 알람이 이미 설정되 있는 상황이면 삭제
            //Singleton.Get<FootballAlarm.InitializeDatabase>().CancelReservation();

            string org_bannerText = this._lbl_initialize_footballdb.Content.ToString();
            this._progRing_initialize_footballdb.IsActive = true;

            await FootballCommands.InitializeFootballDB.Execute();

            // 텍스트 원래대로 변경
            this._lbl_initialize_footballdb.Content = org_bannerText;
            this._progRing_initialize_footballdb.IsActive = false;

            // 다음 업데이트 알람
            //TimeSpan ts = DateTime.Now.AddHours(24) - DateTime.Now; // 24시간 후
            //Singleton.Get<FootballAlarm.InitializeDatabase>().SetAlarm((long)ts.TotalMilliseconds);
        }

        /// <summary>
        /// Update scheduled fixtures
        /// </summary>
        public async void Fv_item_collectDatas_and_predict_Click(object sender, MouseButtonEventArgs e)
        {
            if (this._progRing_collectDatasAndPredict.IsActive)
                return;

            // 해당 로직에 알람이 이미 설정되 있는 상황이면 삭제
            var alarm = Singleton.Get<FootballAlarm.CollectDatasAndPredict>();
            alarm.CancelReservation();

            string org_bannerText = this._lbl_collectDatasAndPredict.Content.ToString();
            this._progRing_collectDatasAndPredict.IsActive = true;

            if (await FootballCommands.UpdateScheduledFixtures.Execute())
            {
                await FootballCommands.PredictFixtures.Execute();
            }
            else
            {
                // Error처리
                await FootballLogic.LogicFacade.SolveErrors(_lbl_collectDatasAndPredict);
                alarm.SetAlarm(30000); // 30초 후 다시 시작
            }

            await AsyncHelper.Async(Singleton.Get<FootballLogic.CheckValidation>().OutputErrorToJsonFile);

            // 텍스트 원래대로 변경
            this._lbl_collectDatasAndPredict.Content = org_bannerText;
            this._progRing_collectDatasAndPredict.IsActive = false;

            // 다음 업데이트 알람
            TimeSpan ts = DateTime.Now.AddHours(3) - DateTime.Now; // 3시간 후
            alarm.SetAlarm((long)ts.TotalMilliseconds);
        }

        /// <summary>
        /// Check completed fixture
        /// </summary>
        public async void Fv_item_check_completed_fixtures_Click(object sender, MouseButtonEventArgs e)
        {
            if (this._progRing_check_completed_fixtures.IsActive)
                return;

            // 해당 로직에 알람이 이미 설정되 있는 상황이면 삭제
            Singleton.Get<FootballAlarm.CheckCompletedFixtures>().CancelReservation();

            string org_bannerText = this._lbl_check_completed_fixtures.Content.ToString();
            this._progRing_check_completed_fixtures.IsActive = true;

            await FootballCommands.CheckCompletedFixtures.Execute();

            // 텍스트 원래대로 변경
            this._lbl_check_completed_fixtures.Content = org_bannerText;
            this._progRing_check_completed_fixtures.IsActive = false;

            TimeSpan ts = DateTime.Now.AddMinutes(5) - DateTime.Now; // 5분 후
            Singleton.Get<FootballAlarm.CheckCompletedFixtures>().SetAlarm((long)ts.TotalMilliseconds);
        }

        /// <summary>
        /// Export CoverageLeague Json File
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_export_coverageLeague_Click(object sender, RoutedEventArgs e)
        {
            FootballCommands.ExportCoverageLeagues.Execute();
        }
    }
}