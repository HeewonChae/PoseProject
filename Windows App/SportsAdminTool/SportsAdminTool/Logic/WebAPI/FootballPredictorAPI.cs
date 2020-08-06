using LogicCore.Debug;
using LogicCore.Utility;
using PredictorAPI;
using PredictorAPI.Models.Football;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsAdminTool.Logic.WebAPI
{
    public class FootballPredictorAPI : Singleton.INode
    {
        private readonly FootballPredictor _api = null;

        public FootballPredictorAPI()
        {
            string host_url = ConfigurationManager.AppSettings["predictor_api_url"];

            _api = new FootballPredictor();
            _api.Init(host_url);
        }

        public FootballPrediction PredictFixture(int fixture_id)
        {
            Dev.DebugString($"Call API - FootballPredictorAPI.PredictFixture, fixture_id = {fixture_id}");

            return _api.PredictFixture(fixture_id);
        }
    }
}