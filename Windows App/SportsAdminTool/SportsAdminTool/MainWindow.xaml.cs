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

		/// <summary>
		/// Initialize footballdb
		/// </summary>
		private CancellationTokenSource _initFootballDb_cts = null;
		public async void Fv_item_initialize_footballdb_Click(object sender, MouseButtonEventArgs e)
		{
			if (this._progRing_initialize_footballdb.IsActive)
			{
				Dev.DebugString("Stoping initialize_footballdb");

				_initFootballDb_cts?.Cancel();
				_initFootballDb_cts = null;

				this._lbl_initialize_footballdb.Content = "Stoping...";
				return;
			}

			// 해당 로직에 알람이 이미 설정되 있는 상황이면 삭제
			var alarm = Singleton.Get<FootballAlarm.InitializeDatabase>();
			if (alarm.Reservation != null)
			{
				alarm.CancelReservation();
			}

			string org_bannerText = this._lbl_initialize_footballdb.Content.ToString();
			this._progRing_initialize_footballdb.IsActive = true;

			_initFootballDb_cts = new CancellationTokenSource();
			var token = _initFootballDb_cts.Token;

			var ret = await FootballLogic.LogicFacade.InitializeFootballDB(token);
			if(!ret)
			{
				// Error처리
				await FootballLogic.LogicFacade.SolveErrors(token, _lbl_initialize_footballdb);
				alarm.SetAlarm(30000); // 30초 후 다시 시작
			}

			await AsyncHelper.Async(Singleton.Get<FootballLogic.CheckValidation>().OutputErrorToJsonFile);

			this._lbl_initialize_footballdb.Content = "Complete!";
			await Task.Delay(500);

			// 텍스트 원래대로 변경
			this._lbl_initialize_footballdb.Content =  org_bannerText;
			this._progRing_initialize_footballdb.IsActive = false;

			// 다음 업데이트 알람
			if (alarm.Reservation == null)
			{
				TimeSpan ts = DateTime.Now.AddHours(24) - DateTime.Now; // 24시간 후
				alarm.SetAlarm((long)ts.TotalMilliseconds);
			}
		}

		/// <summary>
		/// Update scheduled fixtures
		/// </summary>
		private CancellationTokenSource _collectDatasAndPredict_Cts = null;
		public async void Fv_item_collectDatas_and_predict_Click(object sender, MouseButtonEventArgs e)
		{
			if (this._progRing_collectDatasAndPredict.IsActive)
			{
				Dev.DebugString("Stoping update_scheduled_fixtures");

				_collectDatasAndPredict_Cts?.Cancel();
				_collectDatasAndPredict_Cts = null;

				this._lbl_collectDatasAndPredict.Content = "Stoping...";
				return;
			}

			// 해당 로직에 알람이 이미 설정되 있는 상황이면 삭제
			var alarm = Singleton.Get<FootballAlarm.CollectDatasAndPredict>();
			if (alarm.Reservation != null)
			{
				alarm.CancelReservation();
			}

			string org_bannerText = this._lbl_collectDatasAndPredict.Content.ToString();
			this._progRing_collectDatasAndPredict.IsActive = true;

			_collectDatasAndPredict_Cts = new CancellationTokenSource();
			var token = _collectDatasAndPredict_Cts.Token;

			var ret = await FootballLogic.LogicFacade.UpdateScheduledFixtures(token);
			if (!ret)
			{
				// Error처리
				await FootballLogic.LogicFacade.SolveErrors(token, _lbl_collectDatasAndPredict);
				alarm.SetAlarm(30000); // 30초 후 다시 시작
			}

			await FootballLogic.LogicFacade.PredictFixtures(token);

			await AsyncHelper.Async(Singleton.Get<FootballLogic.CheckValidation>().OutputErrorToJsonFile);

			this._lbl_collectDatasAndPredict.Content = "Complete!";
			await Task.Delay(500);

			// 텍스트 원래대로 변경
			this._lbl_collectDatasAndPredict.Content = org_bannerText;
			this._progRing_collectDatasAndPredict.IsActive = false;

			// 다음 업데이트 알람
			if (alarm.Reservation == null)
			{
				TimeSpan ts = DateTime.Now.AddHours(6) - DateTime.Now; // 6시간 후
				alarm.SetAlarm((long)ts.TotalMilliseconds);
			}
		}

		/// <summary>
		/// Check completed fixture
		/// </summary>
		private CancellationTokenSource _completedFixtures_Cts = null;
		public async void Fv_item_check_completed_fixtures_Click(object sender, MouseButtonEventArgs e)
		{
			if (this._progRing_check_completed_fixtures.IsActive)
			{
				Dev.DebugString("Stoping check_completed_fixtures");

				_completedFixtures_Cts?.Cancel();
				_completedFixtures_Cts = null;

				this._lbl_check_completed_fixtures.Content = "Stoping...";
				return;
			}

			// 해당 로직에 알람이 이미 설정되 있는 상황이면 삭제
			var alarm = Singleton.Get<FootballAlarm.CheckCompletedFixtures>();
			if (alarm.Reservation != null)
			{
				alarm.CancelReservation();
			}

			string org_bannerText = this._lbl_check_completed_fixtures.Content.ToString();
			this._progRing_check_completed_fixtures.IsActive = true;

			_completedFixtures_Cts = new CancellationTokenSource();
			var token = _completedFixtures_Cts.Token;

			await FootballLogic.LogicFacade.CheckCompletedFixtures(token);

			await AsyncHelper.Async(Singleton.Get<FootballLogic.CheckValidation>().OutputErrorToJsonFile);

			this._lbl_check_completed_fixtures.Content = "Complete!";
			await Task.Delay(500);

			// 텍스트 원래대로 변경
			this._lbl_check_completed_fixtures.Content = org_bannerText;
			this._progRing_check_completed_fixtures.IsActive = false;

			// 다음 업데이트 알람
			if (alarm.Reservation == null)
			{
				TimeSpan ts = DateTime.Now.AddHours(1) - DateTime.Now; // 1시간 후
				alarm.SetAlarm((long)ts.TotalMilliseconds);
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
		#endregion
	}
}
