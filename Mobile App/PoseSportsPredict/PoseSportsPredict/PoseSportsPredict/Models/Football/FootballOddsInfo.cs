using PosePacket.Service.Football.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Models.Football
{
    public class FootballOddsInfo
    {
        public int FixtureId { get; set; }
        public FootballBookmakerType BookmakerType { get; set; }
        public FootballOddsLabelType OddsLabelType { get; set; }
        public string BookmakerImageUrl { get; set; }
        public Color BookmakerColor { get; set; }
        public double RefundRate { get; set; }
        public double WinOdds { get; set; }
        public double DrawOdds { get; set; }
        public double LoseOdds { get; set; }
    }
}