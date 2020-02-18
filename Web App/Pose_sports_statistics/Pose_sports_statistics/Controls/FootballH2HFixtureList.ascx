<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FootballH2HFixtureList.ascx.cs" Inherits="Pose_sports_statistics.Controls.FootballH2HFixtureList" %>

<asp:Panel ID="pnl_footballH2HFixtureList" runat="server" >
    <h4><b>H2H</b></h4>
    <asp:GridView GridLines="Horizontal" BorderStyle="None" ID="gv_h2hFixtures" 
        AutoGenerateColumns="false" runat="server" SelectMethod="GetH2HFixtures" 
        ItemType="Pose_sports_statistics.Models.FootballFixture"
        OnRowDataBound="GV_h2hFixtures_RowDataBound"
        ShowHeader="false"
        RowStyle-Height="30"
        RowStyle-HorizontalAlign="Center">
        <Columns>
            <asp:TemplateField HeaderText="League" ItemStyle-Height="30">
                <ItemTemplate>
                    <span><%# Item.League.Name %>&nbsp</span>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Date">
                <ItemTemplate>
                    <span>&nbsp<%# Eval("EventDate", "{0:yyyy/MM/dd HH:mm}") %>&nbsp&nbsp</span>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField HeaderText ="Home" />

            <asp:TemplateField HeaderText="Full" ItemStyle-Width="50">
                <ItemTemplate>
                    <span>&nbsp<%# Item.Score.FullTime %>&nbsp</span>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField HeaderText ="Away" />

            <asp:BoundField HeaderText ="Result" ItemStyle-Width="35"/>
        </Columns>
    </asp:GridView>
</asp:Panel>