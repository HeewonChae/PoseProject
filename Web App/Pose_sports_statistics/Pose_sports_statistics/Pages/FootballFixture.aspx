<%@ Page Title="축구" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FootballFixture.aspx.cs" Inherits="Pose_sports_statistics.Pages.FootballFixture" %>

<%@ Register Src="~/Controls/FootballFixtureDetail.ascx" TagName="FootballFixtureDetail" TagPrefix="Ctrls"%>

<%@ Register Src="~/Controls/FootballTeamFixtureResults.ascx" TagName="FootballTeamFixtureResults" TagPrefix="Ctrls"%>

<%@ Register Src="~/Controls/FootballH2HFixtureList.ascx" TagName="FootballH2HFixtureList" TagPrefix="Ctrls"%>

<%@ Register Src="~/Controls/FootballPlayerList.ascx" TagName="FootballPlayerList" TagPrefix="Ctrls"%>

<%@ Register Src="~/Controls/FootballStanding.ascx" TagName="FootballStanding" TagPrefix="Ctrls"%>

<%@ Register Src="~/Controls/FootballPrediction.ascx" TagName="FootballPrediction" TagPrefix="Ctrls"%>

<%@ Register Src="~/Controls/FootballPredictionByOdds.ascx" TagName="FootballPredictionByOdds" TagPrefix="Ctrls"%>

<asp:Content ID="FootballFixtureInfo" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <table style="align-content:center; align-items:center; border-style:none; width:100%;">
        <tr>
            <td>
                <Ctrls:FootballFixtureDetail ID="ctrl_footballFixtureDetail" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <hr style="width:100%">
            </td>
        </tr>
        <tr>
            <td>
                <Ctrls:FootballPrediction ID="ctrl_footballPrediction" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <Ctrls:FootballPredictionByOdds ID="ctrl_footballPredictionByOdds" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <hr style="width:100%">
            </td>
        </tr>
        <tr>
            <td>
                <Ctrls:FootballStanding ID="ctrl_footballStanding" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <hr style="width:100%">
            </td>
        </tr>
        <tr>
            <td>
                <Ctrls:FootballH2HFixtureList ID="ctrl_FootballH2HFixtureList" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <hr style="width:100%">
            </td>
        </tr>
        <tr>
            <td>
                <Ctrls:FootballTeamFixtureResults ID="ctrl_footballTeamFixtureResults" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <hr style="width:100%">
            </td>
        </tr>
        <%--<tr>
            <td>
                <Ctrls:FootballPlayerList ID="ctrl_footballPlayerList" runat="server" />
            </td>
        </tr>--%>
    </table>
</asp:Content>

