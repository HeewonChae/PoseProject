<%@ Page Title="관심 경기" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InterestedEvent.aspx.cs" Inherits="Pose_sports_statistics.Pages.InterestedEvent" %>

<%@ Register Src="~/Controls/InterestedEventList.ascx" TagName="InterestedEventList" TagPrefix="Ctrls"%>

<asp:Content ID="InterestEvent" ContentPlaceHolderID="MainContent" runat="server">

    <Ctrls:InterestedEventList ID="ctrl_interestedEventList" runat="server" />

</asp:Content>
