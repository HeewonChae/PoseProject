<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FootballPlayerList.ascx.cs" Inherits="Pose_sports_statistics.Controls.FootballPlayerList" %>

<asp:Panel ID="pnl_footballPlayerList" runat="server" >
    <h4><b>선수 목록</b></h4>
    <asp:UpdatePanel ID="upnl_playerList" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <asp:menu ID="menu_playerTeam" runat="server" orientation="Horizontal" 
                            StaticEnableDefaultPopOutImage="true"
                            OnMenuItemClick="Menu_playerTeamClick" 
                            BorderStyle="Outset"
                            ForeColor="Black">
                            <statichoverstyle backcolor="LightBlue" forecolor="Black" />
                        </asp:menu>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:MultiView ID="mview_playerList" runat="server" ActiveViewIndex ="0">
                            <asp:View ID="view_homePlayers" runat="server">
                                <div>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:menu ID="menu_homeLeagues" runat="server" orientation="Horizontal" 
                                                    StaticEnableDefaultPopOutImage="true"
                                                    OnMenuItemClick="Menu_HomeLeaguesClick" 
                                                    BorderStyle="Outset"
                                                    ForeColor="Black">
                                                    <statichoverstyle backcolor="LightBlue" forecolor="Black" />
                                                </asp:menu>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:MultiView ID="mview_homePlayers" runat="server" ActiveViewIndex ="0">
                                                    <asp:View ID="view_home_0" runat="server">
                                                        <asp:GridView ID="gv_home0"
                                                            GridLines="Horizontal" BorderStyle="None" 
                                                            AutoGenerateColumns="false" runat="server"
                                                            ItemType="Pose_sports_statistics.Models.FootballPlayer"
                                                            OnRowDataBound="GV_Players_RowDataBound"
                                                            RowStyle-Height="26">
                                                        </asp:GridView>
                                                    </asp:View>
                                                    <asp:View ID="view_home_1" runat="server">
                                                        <asp:GridView ID="gv_home1"
                                                            GridLines="Horizontal" BorderStyle="None" 
                                                            AutoGenerateColumns="false" runat="server"
                                                            ItemType="Pose_sports_statistics.Models.FootballPlayer"
                                                            OnRowDataBound="GV_Players_RowDataBound"
                                                            RowStyle-Height="26">
                                                        </asp:GridView>
                                                    </asp:View>
                                                    <asp:View ID="view_home_2" runat="server">
                                                        <asp:GridView ID="gv_home2"
                                                            GridLines="Horizontal" BorderStyle="None" 
                                                            AutoGenerateColumns="false" runat="server"
                                                            ItemType="Pose_sports_statistics.Models.FootballPlayer"
                                                            OnRowDataBound="GV_Players_RowDataBound"
                                                            RowStyle-Height="26">
                                                        </asp:GridView>
                                                    </asp:View>
                                                    <asp:View ID="view_home_3" runat="server">
                                                        <asp:GridView ID="gv_home3"
                                                            GridLines="Horizontal" BorderStyle="None" 
                                                            AutoGenerateColumns="false" runat="server"
                                                            ItemType="Pose_sports_statistics.Models.FootballPlayer"
                                                            OnRowDataBound="GV_Players_RowDataBound"
                                                            RowStyle-Height="26">
                                                        </asp:GridView>
                                                    </asp:View>
                                                    <asp:View ID="view_home_4" runat="server">
                                                        <asp:GridView ID="gv_home4"
                                                            GridLines="Horizontal" BorderStyle="None" 
                                                            AutoGenerateColumns="false" runat="server"
                                                            ItemType="Pose_sports_statistics.Models.FootballPlayer"
                                                            OnRowDataBound="GV_Players_RowDataBound"
                                                            RowStyle-Height="26">
                                                        </asp:GridView>
                                                    </asp:View>
                                                    <asp:View ID="view_home_5" runat="server">
                                                        <asp:GridView ID="gv_home5"
                                                            GridLines="Horizontal" BorderStyle="None" 
                                                            AutoGenerateColumns="false" runat="server"
                                                            ItemType="Pose_sports_statistics.Models.FootballPlayer"
                                                            OnRowDataBound="GV_Players_RowDataBound"
                                                            RowStyle-Height="26">
                                                        </asp:GridView>
                                                    </asp:View>
                                                </asp:MultiView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:View>
                            <asp:View ID="view_awayPlayers" runat="server">
                                <div>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:menu ID="menu_awayLeagues" runat="server" orientation="Horizontal" 
                                                    StaticEnableDefaultPopOutImage="true"
                                                    OnMenuItemClick="Menu_AwayLeaguesClick" 
                                                    BorderStyle="Outset"
                                                    ForeColor="Black">
                                                    <statichoverstyle backcolor="LightBlue" forecolor="Black" />
                                                </asp:menu>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:MultiView ID="mview_awayPlayers" runat="server" ActiveViewIndex ="0">
                                                    <asp:View ID="view_away_0" runat="server">
                                                        <asp:GridView ID="gv_away0"
                                                            GridLines="Horizontal" BorderStyle="None" 
                                                            AutoGenerateColumns="false" runat="server"
                                                            ItemType="Pose_sports_statistics.Models.FootballPlayer"
                                                            OnRowDataBound="GV_Players_RowDataBound"
                                                            RowStyle-Height="26">
                                                        </asp:GridView>
                                                    </asp:View>
                                                    <asp:View ID="view_away_1" runat="server">
                                                        <asp:GridView ID="gv_away1"
                                                            GridLines="Horizontal" BorderStyle="None" 
                                                            AutoGenerateColumns="false" runat="server"
                                                            ItemType="Pose_sports_statistics.Models.FootballPlayer"
                                                            OnRowDataBound="GV_Players_RowDataBound"
                                                            RowStyle-Height="26">
                                                        </asp:GridView>
                                                    </asp:View>
                                                    <asp:View ID="view_away_2" runat="server">
                                                        <asp:GridView ID="gv_away2"
                                                            GridLines="Horizontal" BorderStyle="None" 
                                                            AutoGenerateColumns="false" runat="server"
                                                            ItemType="Pose_sports_statistics.Models.FootballPlayer"
                                                            OnRowDataBound="GV_Players_RowDataBound"
                                                            RowStyle-Height="26">
                                                        </asp:GridView>
                                                    </asp:View>
                                                    <asp:View ID="view_away_3" runat="server">
                                                        <asp:GridView ID="gv_away3"
                                                            GridLines="Horizontal" BorderStyle="None" 
                                                            AutoGenerateColumns="false" runat="server"
                                                            ItemType="Pose_sports_statistics.Models.FootballPlayer"
                                                            OnRowDataBound="GV_Players_RowDataBound"
                                                            RowStyle-Height="26">
                                                        </asp:GridView>
                                                    </asp:View>
                                                    <asp:View ID="view_away_4" runat="server">
                                                        <asp:GridView ID="gv_away4"
                                                            GridLines="Horizontal" BorderStyle="None" 
                                                            AutoGenerateColumns="false" runat="server"
                                                            ItemType="Pose_sports_statistics.Models.FootballPlayer"
                                                            OnRowDataBound="GV_Players_RowDataBound"
                                                            RowStyle-Height="26">
                                                        </asp:GridView>
                                                    </asp:View>
                                                    <asp:View ID="view_away_5" runat="server">
                                                        <asp:GridView ID="gv_away5"
                                                            GridLines="Horizontal" BorderStyle="None" 
                                                            AutoGenerateColumns="false" runat="server"
                                                            ItemType="Pose_sports_statistics.Models.FootballPlayer"
                                                            OnRowDataBound="GV_Players_RowDataBound"
                                                            RowStyle-Height="26">
                                                        </asp:GridView>
                                                    </asp:View>
                                                </asp:MultiView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:View>
                            <asp:View ID="view_topScorer" runat="server">
                                <asp:GridView ID="gv_topScorer"
                                    GridLines="Horizontal" BorderStyle="None" 
                                    AutoGenerateColumns="false" runat="server"
                                    ItemType="Pose_sports_statistics.Models.FootballPlayer"
                                    OnRowDataBound="GV_TopScorer_RowDataBound"
                                    RowStyle-Height="26">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Team" ItemStyle-Height="30">
		                                    <ItemTemplate>
			                                    <span><%# Item.TeamName %>&nbsp&nbsp</span>
		                                    </ItemTemplate>
	                                    </asp:TemplateField>

	                                    <asp:TemplateField HeaderText="Name">
		                                    <ItemTemplate>
			                                    <span><%# Item.PalyerName %>&nbsp&nbsp</span>
		                                    </ItemTemplate>
	                                    </asp:TemplateField>

                                        <asp:BoundField HeaderText ="Pos" DataField="Position" />
                                        
                                        <asp:TemplateField HeaderText="Goal" ItemStyle-Width="50">
		                                    <ItemTemplate>
			                                    <span><%# Item.Goals.Total %>&nbsp&nbsp</span>
		                                    </ItemTemplate>
	                                    </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Assist" ItemStyle-Width="50">
		                                    <ItemTemplate>
			                                    <span><%# Item.Goals.Assists %>&nbsp&nbsp</span>
		                                    </ItemTemplate>
	                                    </asp:TemplateField>

                                         <asp:BoundField HeaderText ="Shot(%)" ItemStyle-Width="70"/>

                                        <asp:TemplateField HeaderText="Gamas" ItemStyle-Width="70">
		                                    <ItemTemplate>
			                                    <span><%# Item.Games.Appearences %>&nbsp&nbsp</span>
		                                    </ItemTemplate>
	                                    </asp:TemplateField>

	                                    <asp:TemplateField HeaderText="minutes" ItemStyle-Width="70">
		                                    <ItemTemplate>
			                                    <span>&nbsp<%# Item.Games.Minutes %>&nbsp</span>
		                                    </ItemTemplate>
	                                    </asp:TemplateField>

	                                    <asp:BoundField HeaderText ="Result" ItemStyle-Width="40"/>
                                    </Columns>
                                </asp:GridView>
                            </asp:View>
                        </asp:MultiView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>