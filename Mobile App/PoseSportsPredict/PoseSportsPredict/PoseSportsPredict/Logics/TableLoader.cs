using Newtonsoft.Json;
using PoseSportsPredict.Models.Resources.Common;
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
            // Common
            Load_Common_CoverageLanguage(rootPath);
            Load_Common_MembershipAdvantage(rootPath);

            // Football
            Load_Football_CoverageLeagues(rootPath);
            Load_Football_RecommendedLeagues(rootPath);
            Load_Football_StandingsDescription(rootPath);
            Load_Football_StandingsRankColor(rootPath);
        }

        private static void Load_Common_CoverageLanguage(string rootPath)
        {
            var coverageLanguage = LoadJson<CoverageLanguage[]>("Resources.Common.CoverageLanguage.json");

            Debug.Assert(coverageLanguage.Length != 0);

            CoverageLanguage.Load(coverageLanguage);
        }

        private static void Load_Common_MembershipAdvantage(string rootPath)
        {
            var advantages = LoadJson<MembershipAdvantage[]>("Resources.Common.MembershipAdvantage.json");

            Debug.Assert(advantages.Length != 0);

            MembershipAdvantage.Load(advantages);
        }

        private static void Load_Football_CoverageLeagues(string rootPath)
        {
            var coverageLeagues = LoadJson<CoverageLeague[]>("Resources.Football.CoverageLeagues.json");

            Debug.Assert(coverageLeagues.Length != 0);

            CoverageLeague.Load(coverageLeagues);
        }

        private static void Load_Football_RecommendedLeagues(string rootPath)
        {
            var recommendedLeagues = LoadJson<RecommendedLeague[]>("Resources.Football.RecommendedLeagues.json");

            Debug.Assert(recommendedLeagues.Length != 0);

            RecommendedLeague.Load(recommendedLeagues);
        }

        private static void Load_Football_StandingsDescription(string rootPath)
        {
            var standginsDescriptions = LoadJson<StandginsDescription[]>("Resources.Football.StandingsDescription.json");

            Debug.Assert(standginsDescriptions.Length != 0);

            StandginsDescription.Load(standginsDescriptions);
        }

        private static void Load_Football_StandingsRankColor(string rootPath)
        {
            var standingsRankColors = LoadJson<StandingsRankColor[]>("Resources.Football.StandingsRankColor.json");

            Debug.Assert(standingsRankColors.Length != 0);

            StandingsRankColor.Load(standingsRankColors);
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