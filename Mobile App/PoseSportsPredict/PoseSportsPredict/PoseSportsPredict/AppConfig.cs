using System;

namespace PoseSportsPredict
{
    public static class AppConfig
    {
        // Pose WebService
#if DEBUG

        public static string PoseWebBaseUrl = "http://112.172.16.53:8888/";
        //public static string PoseWebBaseUrl = "http://112.172.16.53:8080/";

#else
        public static string PoseWebBaseUrl = "http://34.123.230.143:8080/";
#endif

        // SQLite

        public static string SQLiteServicePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static string SQLiteScheme = "PoseSportsPredict.db3";

        // Syncfusion v18.1.0.46
        public static string SyncfusionKey = "MjQ4OTA2QDMxMzgyZTMxMmUzMGRHQ1NJem5RNmgrYTBwQ0Z3TDdXcm9NdW81TVdDLzcveHordTVYOE5UQWM9";

        // LocalNoti Channel
        public static string Psoe_Noti_Channel_01 = "psoe_channel_01";

        // For Culture

        public const string DEFAULT_LANGUAGE = "en";
        public const string CULTURE_CHANGED_MSG = "UICultureChanged";

        // 예측데이터 광고보고 해제 유지할 시간
        public const int Prediction_Unlocked_Time = 1; // Hour

        // Ads

        public const string ADMOB_NATIVE_ADS_ID = "ca-app-pub-3381862928005780/5854172385";
        public const string ADMOB_BANNER_ADS_ID = "ca-app-pub-3381862928005780/2467669138";
        public const string ADMOB_REWARD_ADS_ID = "ca-app-pub-3381862928005780/8292633466";

        // InAppBilling ProductId
        public readonly static string[] ANDROID_PRODUCT_IDS = new string[] { "pose_poseidon_picks_0_1", "pose_poseidon_picks_1_1" };

        // MemberRoleType Change Message
        public const string MEMBERSHIP_TYPE_CHANGED = "Membership_Type_Changed";

        // Firebase Deep Link

        public const string APP_LINK = "https://poseidonpicks.page.link/app";
        public const string PAGE_BASE_LINK = "https://poseidonpicks.page.link/pagelinker";
    }
}