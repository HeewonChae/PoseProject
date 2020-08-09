using LineNotify;
using LogicCore.Utility;
using LogicCore.Utility.Comparer;
using Repository.Mysql.FootballDB.Tables;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsAdminTool.Logic.WebAPI
{
    public enum LineNotifyType
    {
        Football_Picks,
    }

    public class LineNotifyAPI : Singleton.INode
    {
        private readonly static Dictionary<LineNotifyType, ILineNotify> _clients =
            new Dictionary<LineNotifyType, ILineNotify>(EnumComparer.For<LineNotifyType>());

        public LineNotifyAPI()

        {
            string host_url = ConfigurationManager.AppSettings["line_noti_url"];
            string football_picks_token = ConfigurationManager.AppSettings["line_noti_token_football_pick"];

            var football_picks = new LineNotify.LineNotify();
            football_picks.Init(host_url, football_picks_token);

            _clients.Add(LineNotifyType.Football_Picks, football_picks);
        }

        public void SendMessageAsync(LineNotifyType clientType, string msg)
        {
            if (_clients.ContainsKey(clientType))
            {
                _clients[clientType].SendMessageAsync(msg);
            }
        }

        public void SendMessage(LineNotifyType clientType, string msg)
        {
            if (_clients.ContainsKey(clientType))
            {
                _clients[clientType].SendMessage(msg);
            }
        }
    }
}