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
            string list_noti_url = ConfigurationManager.AppSettings["line_noti_url"];
            string line_noti_token = ConfigurationManager.AppSettings["line_noti_token_Dev"];

            _api = new FootballPredictor();
            _api.Init(host_url, list_noti_url, line_noti_token);
        }

        public FootballPrediction PredictFixture(int fixture_id)
        {
            //Dev.DebugString($"Call API - FootballPredictorAPI.PredictFixture, fixture_id = {fixture_id}");

            return _api.PredictFixture(fixture_id);
        }
    }
}