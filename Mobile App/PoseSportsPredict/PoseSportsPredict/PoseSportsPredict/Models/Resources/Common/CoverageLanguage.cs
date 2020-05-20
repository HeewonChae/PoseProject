using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PoseSportsPredict.Models.Resources.Common
{
    internal class CoverageLanguage
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NativeName { get; set; }

        public static Dictionary<string, CoverageLanguage> CoverageLanguages { get; } = new Dictionary<string, CoverageLanguage>();

        public static void Load(params CoverageLanguage[] coverageLeagues)
        {
            foreach (var covaerageLeague in coverageLeagues)
            {
                Debug.Assert(!CoverageLanguages.ContainsKey(covaerageLeague.Id));

                CoverageLanguages.Add(covaerageLeague.Id, covaerageLeague);
            }
        }
    }
}