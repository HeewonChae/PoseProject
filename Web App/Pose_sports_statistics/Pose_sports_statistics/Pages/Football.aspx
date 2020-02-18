<%@ Page Title="축구" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Football.aspx.cs" Inherits="Pose_sports_statistics.Pages.Football" %>

<%@ Register Src="~/Controls/FootballFixtureList.ascx" TagName="FootballFixtures" TagPrefix="Ctrls"%>

<asp:Content ID="FootballMain" ContentPlaceHolderID="MainContent" runat="server">
	
    <Ctrls:FootballFixtures ID="ctrl_footballFixtures" runat="server" />

</asp:Content>
