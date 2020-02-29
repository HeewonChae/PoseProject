using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.InfraStructure
{
	public static class AppConfig
	{
		// Pose WebService
		public static string PoseWebBaseUrl = "http://192.168.0.157:8888/";

		// SQLite
		public static string SQLiteServicePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

		public static string SQLiteScheme = "PoseSportsPredict.db3";
	}
}