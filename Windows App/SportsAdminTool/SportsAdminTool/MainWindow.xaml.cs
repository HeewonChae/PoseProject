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
using System.Collections.Generic;

using FootballDB = Repository.Mysql.FootballDB;
using System.Linq;
using System.Drawing;

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

        #region Automated Processing Methods

        private void Fv_football_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var flipview = ((FlipView)sender);
            switch (flipview.SelectedIndex)
            {
                case 0:
                    flipview.BannerText = "Update League, Team";
                    break;

                case 1:
                    flipview.BannerText = "Update Fixtures, Predict";
                    break;

                case 2:
                    flipview.BannerText = "Check Completed Fixtures";
                    break;
            }
        }

        /// <summary>
        /// Initialize footballdb
        /// </summary>
        public async void Fv_item_UpdateLeagueNTeam_Click(object sender, MouseButtonEventArgs e)
        {
            if (this._progRing_initialize_footballdb.IsActive)
                return;

            // 해당 로직에 알람이 이미 설정되 있는 상황이면 삭제
            Singleton.Get<FootballAlarm.InitializeDatabase>().CancelReservation();

            string org_bannerText = this._lbl_initialize_footballdb.Content.ToString();
            this._progRing_initialize_footballdb.IsActive = true;

            await FootballCommands.UpdateLeagueAndTeam.Execute();

            // Error처리
            await FootballLogic.LogicFacade.SolveErrors(_lbl_initialize_footballdb);

            // 텍스트 원래대로 변경
            this._lbl_initialize_footballdb.Content = org_bannerText;
            this._progRing_initialize_footballdb.IsActive = false;

            //await AsyncHelper.Async(Singleton.Get<FootballLogic.CheckValidation>().OutputErrorToJsonFile, "UpdateLeagueAndTeam_Errors.json");

            // 다음 업데이트 알람
            TimeSpan ts = DateTime.Now.AddHours(24) - DateTime.Now; // 24시간 후
            Singleton.Get<FootballAlarm.InitializeDatabase>().SetAlarm((long)ts.TotalMilliseconds);
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

            await AsyncHelper.Async(Singleton.Get<FootballLogic.CheckValidation>().OutputErrorToJsonFile, "UpdateScheduledFixtures_Errors.json");

            // 텍스트 원래대로 변경
            this._lbl_collectDatasAndPredict.Content = org_bannerText;
            this._progRing_collectDatasAndPredict.IsActive = false;

            // 다음 업데이트 알람
            TimeSpan ts = DateTime.Now.AddHours(3) - DateTime.Now; // 2시간 후
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

            // Error처리
            await FootballLogic.LogicFacade.SolveErrors(_lbl_check_completed_fixtures);
            // await AsyncHelper.Async(Singleton.Get<FootballLogic.CheckValidation>().OutputErrorToJsonFile, "CheckCompletedFixtures.json");

            // 텍스트 원래대로 변경
            this._lbl_check_completed_fixtures.Content = org_bannerText;
            this._progRing_check_completed_fixtures.IsActive = false;

            TimeSpan ts = DateTime.Now.AddMinutes(5) - DateTime.Now; // 5분 후
            Singleton.Get<FootballAlarm.CheckCompletedFixtures>().SetAlarm((long)ts.TotalMilliseconds);
        }

        #endregion Automated Processing Methods

        #region Export

        /// <summary>
        /// Export CoverageLeague Json File
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_export_coverageLeague_Click(object sender, RoutedEventArgs e)
        {
            FootballCommands.ExportCoverageLeagues.Execute();
        }

        #endregion Export

        #region Manage LeagueCoverageTable

        private void LeagueCoverage_CountryName_Initialized(object sender, EventArgs e)
        {
            var allCountries = Logic.Database.FootballDBFacade.SelectCountries(orderBy: "name");

            _cb_countryName.ItemsSource = allCountries.Select(elem => elem.name);
        }

        private void LeagueCoverage_CountryName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedCountry = e.AddedItems[0] as string;

            var queryLeagues = Logic.Database.FootballDBFacade.SelectLeagues(
                where: $"country_name = \"{selectedCountry}\" AND is_current = 1");

            _cb_leagueName.ItemsSource = queryLeagues.Select(elem => elem.name);

            if (queryLeagues.Count() > 0)
                _cb_leagueName.SelectedIndex = 0;
        }

        private void Button_LeagueCoverage_Select(object sender, RoutedEventArgs e)
        {
            string selectedCountry = _cb_countryName.SelectedItem as string;
            string selectedLeague = _cb_leagueName.SelectedItem as string;

            if (string.IsNullOrEmpty(selectedCountry) || string.IsNullOrEmpty(selectedLeague))
            {
                _lbl_manage_leagueCoverage_result.Content = "Please select country or league.";
                return;
            }

            var selectedLeauge = Logic.Database.FootballDBFacade.SelectLeagues(
                where: $"name = \"{selectedLeague}\"  AND country_name = \"{selectedCountry}\" AND is_current = 1").FirstOrDefault();

            if (selectedLeauge == null)
            {
                _lbl_manage_leagueCoverage_result.Content = "Not found league.";
                return;
            }

            var selectedLeagueCoverage = Logic.Database.FootballDBFacade.SelectCoverages(where: $"league_id = {selectedLeauge.id}");
            if (selectedLeagueCoverage.Count() == 0)
            {
                _lbl_manage_leagueCoverage_result.Content = "Not found leagueCoverage.";
                return;
            }

            _sfdg_leagueCoverage.ItemsSource = selectedLeagueCoverage;

            _lbl_manage_leagueCoverage_result.Content = "Select complete!";
        }

        private void leagueCoverage_CurrentCellValueChanged(object sender, Syncfusion.UI.Xaml.Grid.CurrentCellValueChangedEventArgs e)
        {
            var changedCoverage = e.Record as FootballDB.Tables.LeagueCoverage;

            var result = Logic.Database.FootballDBFacade.UpdateCoverage(changedCoverage);
            if (!result)
                _lbl_manage_leagueCoverage_result.Content = "Update leagueCoverage failed.";

            Button_LeagueCoverage_Select(null, null);

            _lbl_manage_leagueCoverage_result.Content = "Update complete !";
        }

        #endregion Manage LeagueCoverageTable
    }
}