using LogicCore.DataMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ApiModel = RapidAPI.Models;

namespace SportsAdminTool.Model.Football
{
    public class Odds
    {
        public class FixtureInfoMini
        {
            public int LeagueId { get; set; }
            public int FixtureId { get; set; }
            public long UpdateAt { get; set; }
        }

        public class BookmakerInfo
        {
            public class BetInfo
            {
                public class BetValue
                {
                    public string Name;
                    public float Odds;
                }

                public ApiModel.Football.Enums.OddsLabelType LabelType { get; set; }
                public BetValue[] BetValues { get; set; }

                public void SetBetValues(ApiModel.Football.Odds.BookmakerInfo.BetInfo.BetValue[] api_betValues)
                {
                    List<BetValue> items = new List<BetValue>();

                    foreach (var api_betValue in api_betValues)
                    {
                        var item = new BetValue
                        {
                            Name = api_betValue.Name,
                            Odds = api_betValue.Odds
                        };

                        items.Add(item);
                    }

                    BetValues = items.ToArray();
                }
            }

            public ApiModel.Football.Enums.BookmakerType BookmakerType { get; set; }
            public BetInfo[] BetInfos { get; set; }

            public void SetBetInfos(ApiModel.Football.Odds.BookmakerInfo.BetInfo[] api_betInfos)
            {
                var items = new List<BetInfo>();

                foreach (var api_betInfo in api_betInfos)
                {
                    var item = (BetInfo)DataMapper.Map(api_betInfo, typeof(BetInfo));
                    items.Add(item);
                }

                BetInfos = items.ToArray();
            }
        }

        public FixtureInfoMini FixtureMini { get; set; }
        public BookmakerInfo[] Bookmakers { get; set; }

        public void SetBookmakers(ApiModel.Football.Odds.BookmakerInfo[] api_bookMakerInfos)
        {
            List<BookmakerInfo> items = new List<BookmakerInfo>();

            foreach (var api_bookMaker in api_bookMakerInfos)
            {
                if (api_bookMaker.BookmakerType == ApiModel.Football.Enums.BookmakerType._NONE_
                    || api_bookMaker.BookmakerType == ApiModel.Football.Enums.BookmakerType._MAX_)
                    continue;

                var item = (BookmakerInfo)DataMapper.Map(api_bookMaker, typeof(BookmakerInfo));

                if (item.BetInfos.Length == 0)
                    continue;

                items.Add(item);
            }

            Bookmakers = items.ToArray();
        }
    }
}