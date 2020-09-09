﻿using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using MarcTron.Plugin;
using PosePacket.Service.Football;
using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.InfraStructure.Cache;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Models;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Models.Resources.Common;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Services;
using PoseSportsPredict.Services.Cache.Loader;
using PoseSportsPredict.Utilities.LocalStorage;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football.Match.Detail;
using Sharpnado.Presentation.Forms;
using Shiny;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WebServiceShare.WebServiceClient;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace PoseSportsPredict.ViewModels.Football.Match.Detail
{
    public class FootballMatchDetailPredictionsViewModel : BaseViewModel
    {
        #region BaseViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            PredictionsTaskLoaderNotifier = new TaskLoaderNotifier<IReadOnlyCollection<FootballPredictionInfo>>();
            _isSetRewardEvent = false;
            AdsPlayed = false;
            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            if (PredictionsTaskLoaderNotifier.IsNotStarted)
                PredictionsTaskLoaderNotifier.Load(InitPredictionData);

            if (!_isSetRewardEvent)
            {
                _isSetRewardEvent = true;
                CrossMTAdmob.Current.OnRewardedVideoAdLoaded += OnRewardedVideoAdLoaded;
                CrossMTAdmob.Current.OnRewarded += OnRewarded;
                CrossMTAdmob.Current.OnRewardedVideoAdClosed += OnRewardedVideoAdClosed;
                CrossMTAdmob.Current.OnRewardedVideoAdFailedToLoad += OnRewardedVideoAdFailedToLoad;
            }
        }

        public override void OnDisAppearing(params object[] datas)
        {
            if (!AdsPlayed && _isSetRewardEvent)
            {
                _isSetRewardEvent = false;
                CrossMTAdmob.Current.OnRewardedVideoAdLoaded -= OnRewardedVideoAdLoaded;
                CrossMTAdmob.Current.OnRewarded -= OnRewarded;
                CrossMTAdmob.Current.OnRewardedVideoAdClosed -= OnRewardedVideoAdClosed;
                CrossMTAdmob.Current.OnRewardedVideoAdFailedToLoad -= OnRewardedVideoAdFailedToLoad;
            }
        }

        #endregion BaseViewModel

        #region Services

        private ICacheService _cacheService;
        private ISQLiteService _sqliteService;
        private MembershipService _membershipService;

        #endregion Services

        #region Fields

        private FootballMatchInfo _matchInfo;
        private TaskLoaderNotifier<IReadOnlyCollection<FootballPredictionInfo>> _predictionsTaskLoaderNotifier;
        private List<FootballPredictionInfo> _allPredictions;
        private FootballPredictionGroup _selectedPrediction;
        private bool _isSetRewardEvent;

        private FootballPredictionGroup _finalScorePredictions;
        private FootballPredictionGroup _matchWinnerPredictions;
        private FootballPredictionGroup _bothToScorePredictions;
        private FootballPredictionGroup _underOverPredictions;

        private bool _isFinalScoreUnlocked;
        private bool _isMatchWinnerUnlocked;
        private bool _isBothToScoreUnlocked;
        private bool _isUnderOverUnlocked;

        #endregion Fields

        #region Properties

        public bool AdsPlayed;

        public TaskLoaderNotifier<IReadOnlyCollection<FootballPredictionInfo>> PredictionsTaskLoaderNotifier { get => _predictionsTaskLoaderNotifier; set => SetValue(ref _predictionsTaskLoaderNotifier, value); }
        public bool IsFinalScoreUnlocked { get => _isFinalScoreUnlocked; set => SetValue(ref _isFinalScoreUnlocked, value); }
        public bool IsMatchWinnerUnlocked { get => _isMatchWinnerUnlocked; set => SetValue(ref _isMatchWinnerUnlocked, value); }
        public bool IsBothToScoreUnlocked { get => _isBothToScoreUnlocked; set => SetValue(ref _isBothToScoreUnlocked, value); }
        public bool IsUnderOverUnlocked { get => _isUnderOverUnlocked; set => SetValue(ref _isUnderOverUnlocked, value); }

        public FootballPredictionGroup FinalScorePredictions { get => _finalScorePredictions; set => SetValue(ref _finalScorePredictions, value); }
        public FootballPredictionGroup MatchWinnerPredictions { get => _matchWinnerPredictions; set => SetValue(ref _matchWinnerPredictions, value); }
        public FootballPredictionGroup BothToScorePredictions { get => _bothToScorePredictions; set => SetValue(ref _bothToScorePredictions, value); }
        public FootballPredictionGroup UnderOverPredictions { get => _underOverPredictions; set => SetValue(ref _underOverPredictions, value); }

        #endregion Properties

        #region Commands

        public ICommand FinalScoreUnlockCommand { get => new RelayCommand(FinalScoreUnlock); }

        private async void FinalScoreUnlock()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            var isUnlock = await UnlockProcess(FinalScorePredictions);

            if (isUnlock)
                await PageSwitcher.PushPopupAsync(ShinyHost.Resolve<FootballPredictionFinalScoreViewModel>(), FinalScorePredictions);

            SetIsBusy(false);
        }

        public ICommand MatchWinnerUnlockCommand { get => new RelayCommand(MatchWinnerUnlock); }

        private async void MatchWinnerUnlock()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            var isUnlock = await UnlockProcess(MatchWinnerPredictions);

            if (isUnlock)
                await PageSwitcher.PushPopupAsync(ShinyHost.Resolve<FootballPredictionMatchWinnerViewModel>(), MatchWinnerPredictions);

            SetIsBusy(false);
        }

        public ICommand BothToScoreUnlockCommand { get => new RelayCommand(BothToScoreUnlock); }

        private async void BothToScoreUnlock()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            var isUnlock = await UnlockProcess(BothToScorePredictions);

            if (isUnlock)
                await PageSwitcher.PushPopupAsync(ShinyHost.Resolve<FootballPredictionBothToScoreViewModel>(), BothToScorePredictions);

            SetIsBusy(false);
        }

        public ICommand UnderOverUnlockCommand { get => new RelayCommand(UnderOverUnlock); }

        private async void UnderOverUnlock()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            var isUnlock = await UnlockProcess(UnderOverPredictions);

            if (isUnlock)
                await PageSwitcher.PushPopupAsync(ShinyHost.Resolve<FootballPredictionUnderOverViewModel>(), UnderOverPredictions);

            SetIsBusy(false);
        }

        #endregion Commands

        #region Constructors

        public FootballMatchDetailPredictionsViewModel(
            FootballMatchDetailPredictionsView view,
            ICacheService cacheService,
            ISQLiteService sqliteService,
            MembershipService membershipService) : base(view)
        {
            _cacheService = cacheService;
            _sqliteService = sqliteService;
            _membershipService = membershipService;

            OnInitializeView();
        }

        #endregion Constructors

        #region Methods

        public async Task<bool> UnlockProcess(FootballPredictionGroup predictionGroup)
        {
            _selectedPrediction = predictionGroup;

            MembershipAdvantage.TryGetValue(_membershipService.MemberRoleType, out MembershipAdvantage advantage);
            if ((!advantage.IsVideoAdRemove)
                && predictionGroup.UnlockedTime.AddHours(AppConfig.Prediction_Unlocked_Time) < DateTime.UtcNow)
            {
                // 동영상 광고
                if (CrossMTAdmob.Current.IsEnabled)
                {
                    LocalStorage.Storage.GetValueOrDefault(LocalStorageKey.IsDismissVideoAdsInfoPopup, out bool isDismiss);

                    if (!isDismiss)
                    {
                        var ret = await MaterialDialog.Instance.ConfirmAsync(LocalizeString.Unlocked_Prediction_Keep_Hour,
                        LocalizeString.App_Title,
                        LocalizeString.Ok,
                        LocalizeString.Dont_Show_Again,
                        DialogConfiguration.AppTitleAlterDialogConfiguration);

                        if (ret.HasValue && !ret.Value)
                            LocalStorage.Storage.AddOrUpdateValue(LocalStorageKey.IsDismissVideoAdsInfoPopup, true);
                    }

                    CrossMTAdmob.Current.LoadRewardedVideo(AppConfig.ADMOB_REWARD_ADS_ID);
                    UserDialogs.Instance.ShowLoading(LocalizeString.Loading);
                }

                return false;
            }

            return true;
        }

        /// <summary>s
        /// OnReward event handler
        /// </summary>
        /// <param name="matchInfo"></param>
        /// <returns></returns>
        ///

        private void OnRewardedVideoAdLoaded(object sender, EventArgs e)
        {
            AdsPlayed = true;
            CrossMTAdmob.Current.ShowRewardedVideo();
        }

        private async void OnRewarded(object sender, EventArgs e)
        {
            // Save SQLite
            _selectedPrediction.UnlockedTime = DateTime.UtcNow;
            await _sqliteService.InsertOrUpdateAsync(_selectedPrediction);

            switch (_selectedPrediction.MainLabel)
            {
                case FootballPredictionType.Final_Score:
                    IsFinalScoreUnlocked = true;
                    break;

                case FootballPredictionType.Match_Winner:
                    IsMatchWinnerUnlocked = true;
                    break;

                case FootballPredictionType.Both_Teams_to_Score:
                    IsBothToScoreUnlocked = true;
                    break;

                case FootballPredictionType.Under_Over:
                    IsUnderOverUnlocked = true;
                    break;

                default:
                    break;
            }
        }

        private void OnRewardedVideoAdClosed(object sender, EventArgs e)
        {
            AdsPlayed = false;
            UserDialogs.Instance.HideLoading();
        }

        private async void OnRewardedVideoAdFailedToLoad(object sender, EventArgs e)
        {
            AdsPlayed = false;
            UserDialogs.Instance.HideLoading();

            await MaterialDialog.Instance.AlertAsync(LocalizeString.VidonAd_Load_Fail,
                    LocalizeString.App_Title,
                    LocalizeString.Ok,
                    DialogConfiguration.AppTitleAlterDialogConfiguration);
        }

        public FootballMatchDetailPredictionsViewModel SetMatchInfo(FootballMatchInfo matchInfo)
        {
            _matchInfo = matchInfo;
            return this;
        }

        private async Task<IReadOnlyCollection<FootballPredictionInfo>> InitPredictionData()
        {
            SetIsBusy(true);

            await Task.Delay(300);

            if (!_matchInfo.IsPredicted)
            {
                SetIsBusy(false);
                return null;
            }

            // call server
            var server_result = await _cacheService.GetAsync<O_GET_MATCH_PREDICTIONS>(
              loader: () =>
              {
                  return FootballDataLoader.MatchPredictions(_matchInfo.Id);
              },
              key: $"P_GET_MATCH_PREDICTIONS:{_matchInfo.PrimaryKey}",
              expireTime: DateTime.Now.Date.AddDays(1) - DateTime.Now,
              serializeType: SerializeType.MessagePack);

            if (server_result == null)
                throw new Exception(LocalizeString.Occur_Error);

            if (server_result.PredictionDetails.Length == 0)
            {
                SetIsBusy(false);
                return null;
            }

            var dic_predictionGroup = ShinyHost.Resolve<PredictionDetailsToPredictionGroup>().Convert(server_result.PredictionDetails);
            FinalScorePredictions = dic_predictionGroup[FootballPredictionType.Final_Score];
            MatchWinnerPredictions = dic_predictionGroup[FootballPredictionType.Match_Winner];
            BothToScorePredictions = dic_predictionGroup[FootballPredictionType.Both_Teams_to_Score];
            UnderOverPredictions = dic_predictionGroup[FootballPredictionType.Under_Over];

            _allPredictions = dic_predictionGroup.Values.SelectMany(elem => elem.Predictions).ToList();

            MembershipAdvantage.TryGetValue(_membershipService.MemberRoleType, out MembershipAdvantage advantage);
            if (advantage.IsVideoAdRemove)
            {
                IsFinalScoreUnlocked = true;
                IsMatchWinnerUnlocked = true;
                IsBothToScoreUnlocked = true;
                IsUnderOverUnlocked = true;
            }
            else
            {
                var sql_finalScoreGroup = await _sqliteService.SelectAsync<FootballPredictionGroup>(FinalScorePredictions.PrimaryKey);
                if (sql_finalScoreGroup != null)
                {
                    IsFinalScoreUnlocked = sql_finalScoreGroup.UnlockedTime.AddHours(AppConfig.Prediction_Unlocked_Time) > DateTime.UtcNow;
                    FinalScorePredictions.UnlockedTime = sql_finalScoreGroup.UnlockedTime;
                }

                var sql_matchWinnerGroup = await _sqliteService.SelectAsync<FootballPredictionGroup>(MatchWinnerPredictions.PrimaryKey);
                if (sql_matchWinnerGroup != null)
                {
                    IsMatchWinnerUnlocked = sql_matchWinnerGroup.UnlockedTime.AddHours(AppConfig.Prediction_Unlocked_Time) > DateTime.UtcNow;
                    MatchWinnerPredictions.UnlockedTime = sql_matchWinnerGroup.UnlockedTime;
                }

                var sql_bothToScoreGroup = await _sqliteService.SelectAsync<FootballPredictionGroup>(BothToScorePredictions.PrimaryKey);
                if (sql_bothToScoreGroup != null)
                {
                    IsBothToScoreUnlocked = sql_bothToScoreGroup.UnlockedTime.AddHours(AppConfig.Prediction_Unlocked_Time) > DateTime.UtcNow;
                    BothToScorePredictions.UnlockedTime = sql_bothToScoreGroup.UnlockedTime;
                }

                var sql_UnderOverGroup = await _sqliteService.SelectAsync<FootballPredictionGroup>(UnderOverPredictions.PrimaryKey);
                if (sql_UnderOverGroup != null)
                {
                    IsUnderOverUnlocked = sql_UnderOverGroup.UnlockedTime.AddHours(AppConfig.Prediction_Unlocked_Time) > DateTime.UtcNow;
                    UnderOverPredictions.UnlockedTime = sql_UnderOverGroup.UnlockedTime;
                }
            }

            SetIsBusy(false);

            return _allPredictions;
        }

        #endregion Methods
    }
}