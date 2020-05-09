using System;

namespace PoseSportsPredict
{
    public static class AppConfig
    {
        // Pose WebService
        public static string PoseWebBaseUrl = "http://192.168.0.157:8888/";

        // SQLite
        public static string SQLiteServicePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public static string SQLiteScheme = "PoseSportsPredict.db3";

        // Syncfusion v18.1.0.46
        public static string SyncfusionKey = "MjQ4OTA2QDMxMzgyZTMxMmUzMGRHQ1NJem5RNmgrYTBwQ0Z3TDdXcm9NdW81TVdDLzcveHordTVYOE5UQWM9";

        //LocalNoti Channel
        public static string Psoe_Noti_Channel_01 = "psoe_channel_01";
    }
}