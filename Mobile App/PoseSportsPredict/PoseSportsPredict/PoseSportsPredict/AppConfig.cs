﻿using System;

namespace PoseSportsPredict
{
    public static class AppConfig
    {
        // Pose WebService
        public static string PoseWebBaseUrl = "http://192.168.0.157:8888/";

        // SQLite
        public static string SQLiteServicePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public static string SQLiteScheme = "PoseSportsPredict.db3";

        public static string Syncfusion_18_1_0_46 = "MjQ4OTA2QDMxMzgyZTMxMmUzMGRHQ1NJem5RNmgrYTBwQ0Z3TDdXcm9NdW81TVdDLzcveHordTVYOE5UQWM9";
    }
}