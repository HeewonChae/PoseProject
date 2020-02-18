<%@ Page Title="농구" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Basketball.aspx.cs" Inherits="Pose_sports_statistics.Pages.Basketball" %>

<asp:Content ID="BasketballMain" ContentPlaceHolderID="MainContent" runat="server">
 <div>
 <asp:UpdatePanel ID="UpdatePanel1" runat="server"
 UpdateMode="Conditional">
     <ContentTemplate>
     <asp:Label ID="Label1" runat="server" /><br />
     <asp:Button ID="Button1" runat="server"
     Text="Update Both Panels" OnClick="Button1_Click" />
     <asp:Button ID="Button2" runat="server"
     Text="Update This Panel" OnClick="Button2_Click" />
     <asp:CheckBox ID="cbDate" runat="server"
     Text="Include Date" AutoPostBack="false"
     OnCheckedChanged="cbDate_CheckedChanged" />
     </ContentTemplate>
 </asp:UpdatePanel>
 <asp:UpdatePanel ID="UpdatePanel2" runat="server"
 UpdateMode="Conditional">
     <ContentTemplate>
     <asp:Label ID="Label2" runat="server"
     ForeColor="red" />
     </ContentTemplate>
     <Triggers>
     <asp:AsyncPostBackTrigger ControlID="Button1" 
     EventName="Click" />
     <asp:AsyncPostBackTrigger ControlID="ddlColor" 
     EventName="SelectedIndexChanged" />
     </Triggers>
 </asp:UpdatePanel>

 <asp:DropDownList ID="ddlColor" runat="server"
 AutoPostBack="true"
 OnSelectedIndexChanged="ddlColor_SelectedIndexChanged">
 <asp:ListItem Selected="true" Value="Red" />
 <asp:ListItem Value="Blue" />
 <asp:ListItem Value="Green" />
 </asp:DropDownList>
 </div>
</asp:Content>
