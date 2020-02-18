<%@ Page Title="축구" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FootBallByCountry.aspx.cs" Inherits="Pose_sports_statistics.Pages.WebForm1" %>

<%@ Register Src="~/Controls/FootballFixtureList.ascx" TagName="FootballFixtures" TagPrefix="Ctrls"%>

<asp:Content ID="FootballByCountry" ContentPlaceHolderID="MainContent" runat="server">

    <Ctrls:FootballFixtures ID="ctrl_footballFixtures" runat="server" />

</asp:Content>
