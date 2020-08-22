using System;

namespace PoseSportsPredict
{
    public static class AppConfig
    {
        // Pose WebService
#if DEBUG
        public static string PoseWebBaseUrl = "http://121.142.201.8:8888/";
#else
        public static string PoseWebBaseUrl = "http://121.142.201.8:8888/";
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

        public const int Prediction_Unlocked_Time = 1; // Hour
    }
}