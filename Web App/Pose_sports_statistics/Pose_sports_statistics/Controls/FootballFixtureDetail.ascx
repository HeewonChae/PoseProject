<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FootballFixtureDetail.ascx.cs" Inherits="Pose_sports_statistics.Controls.FootballFixtureDetail" %>

<asp:Panel ID="pnl_footballFixtureDetail" runat="server">
    <asp:FormView ID="form_fixture" runat="server"
        SelectMethod="GetFootballFixture"
        OnDataBound="Form_fixture_DataBound"
        ItemType="Pose_sports_statistics.Models.FootballFixture">
        <ItemTemplate>
            <h3>
                <img src="<%# Item.League.Flag %>" height="21" width="28">
                <b><%# Item.League.Name %> | <%# Item.Round %></b></h3>
            <br />
            <asp:Table ID="tb_fixtureDetail" Style="width: 100%"
                BorderStyle="None"
                runat="server">

                <asp:TableRow HorizontalAlign="Center" VerticalAlign="Middle">
                    <asp:TableCell Width="300">
                        <img src="<%# Item.HomeTeam.Logo %>" height="90" width="120">
                        <br />
                        <a href="<%#: GetRouteUrl("FootballTeamById", new {TeamId = Item.HomeTeam.TeamId}) %>"><%# Item.HomeTeam.TeamName %></a>
                    </asp:TableCell>
                    <asp:TableCell Width="200">
                        <b>VS</b>
                        <br />
                        <b><%# Eval("MatchTime", "{0:yyyy/MM/dd HH:mm}") %></b>
                        <br />
                        <b><%# Item.Venue %></b>
                        <br />
                        <b><%# Item.Status %></b>
                    </asp:TableCell>
                    <asp:TableCell Width="300">
                        <img src="<%# Item.AwayTeam.Logo %>" height="90" width="120">
                        <br />
                        <a href="<%#: GetRouteUrl("FootballTeamById", new {TeamId = Item.AwayTeam.TeamId}) %>"><%# Item.AwayTeam.TeamName %></a>
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow HorizontalAlign="Center" VerticalAlign="Middle">
                    <asp:TableCell ColumnSpan="3">
                        <hr style="width:100%">
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow VerticalAlign="Middle" Height="30">
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="lbl_homeRank" runat="server" />
                    </asp:TableCell><asp:TableCell HorizontalAlign="Center">
                        <b>순위</b>
                    </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                        <asp:Label ID="lbl_awyaRank" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow VerticalAlign="Middle" Height="30">
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="lbl_homePoint" runat="server" />
                    </asp:TableCell><asp:TableCell HorizontalAlign="Center">
                        <b>승점</b>
                    </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                        <asp:Label ID="lbl_awayPoint" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow VerticalAlign="Middle" Height="30">
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="home_form_0" runat="server" />
                        <asp:Label ID="home_form_1" runat="server" />
                        <asp:Label ID="home_form_2" runat="server" />
                        <asp:Label ID="home_form_3" runat="server" />
                        <asp:Label ID="home_form_4" runat="server" />
                    </asp:TableCell><asp:TableCell HorizontalAlign="Center">
                        <b>최근 결과</b>
                    </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                        <asp:Label ID="away_form_0" runat="server" />
                        <asp:Label ID="away_form_1" runat="server" />
                        <asp:Label ID="away_form_2" runat="server" />
                        <asp:Label ID="away_form_3" runat="server" />
                        <asp:Label ID="away_form_4" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow VerticalAlign="Middle" Height="30">
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="lbl_homeTotalRecord" runat="server" />
                    </asp:TableCell><asp:TableCell HorizontalAlign="Center">
                        <b>전적</b>
                    </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                        <asp:Label ID="lbl_awayTotalRecord" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow VerticalAlign="Middle" Height="30">
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="lbl_homeGoalsAvg" runat="server" />
                    </asp:TableCell><asp:TableCell HorizontalAlign="Center">
                        <b>평균 득점</b>
                    </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                        <asp:Label ID="lbl_awayGoalsAvg" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow VerticalAlign="Middle" Height="30">
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="lbl_homeGoalAgainst" runat="server" />
                    </asp:TableCell><asp:TableCell HorizontalAlign="Center">
                        <b>평균 실점</b>
                    </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                        <asp:Label ID="lbl_awayGoalAgainst" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow VerticalAlign="Middle" Height="30">
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="lbl_homeLastSixPoints" runat="server" />
                    </asp:TableCell><asp:TableCell HorizontalAlign="Center">
                        <b>6경기 득실</b>
                    </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                        <asp:Label ID="lbl_awayLastSixPoints" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow VerticalAlign="Middle" Height="30">
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="lbl_homeLastThreePoints" runat="server" />
                    </asp:TableCell><asp:TableCell HorizontalAlign="Center">
                        <b>H/A 3경기 득실</b>
                    </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                        <asp:Label ID="lbl_awayLastThreePoints" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow VerticalAlign="Middle" Height="30">
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="lbl_homeRecoveryDays" runat="server" />
                    </asp:TableCell><asp:TableCell HorizontalAlign="Center">
                        <b>회복기간</b>
                    </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                        <asp:Label ID="lbl_awayRecoveryDays" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </ItemTemplate>
    </asp:FormView>
</asp:Panel>