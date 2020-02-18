<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FootballStanding.ascx.cs" Inherits="Pose_sports_statistics.Controls.FootballStanding" %>

<asp:Panel ID="pnl_footballStanding" runat="server" >
    <h4><b>리그 순위</b></h4>
    <asp:GridView GridLines="Horizontal" BorderStyle="None" ID="gv_standings" 
        AutoGenerateColumns="false" runat="server" SelectMethod="GetStandings" 
        ItemType="Pose_sports_statistics.Models.FootballStanding"
        OnRowDataBound="GV_Standings_RowDataBound">
        <Columns>
            <asp:BoundField HeaderText="Rank" DataField="Rank" ItemStyle-Height="25" ItemStyle-Width="40" ItemStyle-HorizontalAlign="Center"/>
            <asp:TemplateField HeaderText="Name">
                <ItemTemplate>
                    <span><%# Item.TeamName %>&nbsp&nbsp&nbsp</span>
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="P" ItemStyle-Width="40" >
                <ItemTemplate>
                    <span><%# Item.AllPlayedInfo.Played %></span>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="W" ItemStyle-Width="40" >
                <ItemTemplate>
                    <span><%# Item.AllPlayedInfo.Win %></span>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="D" ItemStyle-Width="40"  >
                <ItemTemplate>
                    <span><%# Item.AllPlayedInfo.Draw %></span>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="R" ItemStyle-Width="40"  >
                <ItemTemplate>
                    <span><%# Item.AllPlayedInfo.Lose %></span>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="GF" ItemStyle-Width="50"  >
                <ItemTemplate>
                    <span><%# Item.AllPlayedInfo.GoalsFor %></span>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="GA" ItemStyle-Width="50"  >
                <ItemTemplate>
                    <span><%# Item.AllPlayedInfo.GoalsAgainst %></span>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField HeaderText ="GD" DataField="GoalsDiff"  ItemStyle-Width="50" />
            <asp:BoundField HeaderText ="Pts" DataField="Points"  ItemStyle-Width="50" />
            <asp:TemplateField HeaderText="Form">
                <ItemTemplate>
                    <asp:Label ID="form_0" runat="server"/>
                    <asp:Label ID="form_1" runat="server"/>
                    <asp:Label ID="form_2" runat="server"/>
                    <asp:Label ID="form_3" runat="server"/>
                    <asp:Label ID="form_4" runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Panel>