using Newtonsoft.Json;
using PoseSportsPredict.Models.Resources.Football;
using PoseSportsPredict.Utilities;
using PoseSportsPredict.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PoseSportsPredict.Logics
{
    public static class TableLoader
    {
        private static string _resourcePrefix = "PoseSportsPredict.";
        private static Assembly _assembly;
        private static bool IsLoaded = false;

        public static void Init(string rootPath)
        {
            if (IsLoaded)
                return;
#if __IOS__
            _resourcePrefix = "PoseSportsPredict.iOS.";
#elif __ANDROID__
            _resourcePrefix = "PoseSportsPredict.Android.";
#endif
            Debug.WriteLine("Using this resource prefix: " + _resourcePrefix);

            _assembly = IntrospectionExtensions.GetTypeInfo(typeof(LoadingPage)).Assembly;

            LoadTable(rootPath);

            IsLoaded = true;
        }

        private static void LoadTable(string rootPath)
        {
#if DEBUG
            foreach (var res in _assembly.GetManifestResourceNames())
            {
                System.Diagnostics.Debug.WriteLine("found resource: " + res);
            }
#endif
            // Football
            Load_Football_CoverageLeagues(rootPath);
        }

        private static void Load_Football_CoverageLeagues(string rootPath)
        {
            var CoverageLeagues = LoadJson<CoverageLeague[]>("Resources.Football.CoverageLeagues.json");

            Debug.Assert(CoverageLeagues.Length != 0);

            CoverageLeague.Load(CoverageLeagues);
        }

        #region JsonLoader

        private static T LoadJson<T>(string resourceName)
        {
            Stream stream = _assembly.GetManifestResourceStream(_resourcePrefix + resourceName);

            T resultValue = default;
            using (var reader = new System.IO.StreamReader(stream))
            {
                var jsonString = reader.ReadToEnd();
                resultValue = jsonString.JsonDeserialize<T>();
            }

            return resultValue;
        }

        #endregion JsonLoader
    }
}