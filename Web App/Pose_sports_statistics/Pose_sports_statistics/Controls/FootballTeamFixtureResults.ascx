<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FootballTeamFixtureResults.ascx.cs" Inherits="Pose_sports_statistics.Controls.FootballTeamFixtureList" %>

<asp:Panel ID="pnl_footballTeamFixtureResults" runat="server" >
    <h4><b>경기 일정</b></h4>
    <asp:UpdatePanel ID="upnl_teamFixtureResults" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <asp:menu ID="tabContainer" runat="server" orientation="Horizontal" 
                            StaticEnableDefaultPopOutImage="true"
                            OnMenuItemClick="Menu1_MenuItemClick" 
                            BorderStyle="Outset"
                            ForeColor="Black">
                            <statichoverstyle backcolor="LightBlue" forecolor="Black" />
                        </asp:menu>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:MultiView ID="mview_Teamfixtures" runat="server" ActiveViewIndex ="0">
                            <asp:View ID="view_homeFixtures" runat="server">
                                <h5><b>지난 경기</b></h5>
                                <asp:GridView ID="gv_homeLateFixtures"
                                    GridLines="Horizontal" BorderStyle="None" 
                                    AutoGenerateColumns="false" runat="server"
                                    ItemType="Pose_sports_statistics.Models.FootballFixture"
                                    OnRowDataBound="GV_HomeFixtures_RowDataBound"
                                    RowStyle-Height="30"
                                    RowStyle-HorizontalAlign="Center"
                                    ShowHeader="false">
                                    <Columns>
                                        <asp:BoundField HeaderText="League"/>
                                        <asp:BoundField HeaderText="Date"/>
                                        <asp:BoundField HeaderText="Home" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField HeaderText="Score"/>
                                        <asp:BoundField HeaderText="Away" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="Result" ItemStyle-Width="35"/>
                                    </Columns>
                                </asp:GridView>
                                </br>
                                <h5><b>예정된 경기</b></h5>
                                <asp:GridView ID="gv_homeReserveFixtures"
                                    GridLines="Horizontal" BorderStyle="None" 
                                    AutoGenerateColumns="false" runat="server"
                                    ItemType="Pose_sports_statistics.Models.FootballFixture"
                                    OnRowDataBound="GV_HomeReserveFixtures_RowDataBound"
                                    RowStyle-Height="30"
                                    RowStyle-HorizontalAlign="Center"
                                    ShowHeader="false">
                                    <Columns>
                                        <asp:BoundField HeaderText="League"/>
                                        <asp:BoundField HeaderText="Date"/>
                                        <asp:BoundField HeaderText="Home"/>
                                        <asp:BoundField HeaderText="Away"/>
                                        <asp:BoundField HeaderText="D_Day"/>
                                    </Columns>
                                </asp:GridView>

                            </asp:View>
                            <asp:View ID="view_awayFixtures" runat="server">
                                <h5><b>지난 경기</b></h5>
                                <asp:GridView ID="gv_awayLateFixtures"
                                    GridLines="Horizontal" BorderStyle="None" 
                                    AutoGenerateColumns="false" runat="server"
                                    ItemType="Pose_sports_statistics.Models.FootballFixture"
                                    OnRowDataBound="GV_AwayFixtures_RowDataBound"
                                    RowStyle-Height="30"
                                    RowStyle-HorizontalAlign="Center"
                                    ShowHeader="false">
                                    <Columns>
                                        <asp:BoundField HeaderText="League"/>
                                        <asp:BoundField HeaderText="Date"/>
                                        <asp:BoundField HeaderText="Home" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField HeaderText="Score"/>
                                        <asp:BoundField HeaderText="Away" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderText="Result" ItemStyle-Width="35"/>
                                    </Columns>
                                </asp:GridView>
                                </br>
                                <h5><b>예정된 경기</b></h5>
                                <asp:GridView ID="gv_awayReserveFixtures"
                                    GridLines="Horizontal" BorderStyle="None" 
                                    AutoGenerateColumns="false" runat="server"
                                    ItemType="Pose_sports_statistics.Models.FootballFixture"
                                    OnRowDataBound="GV_AwayReserveFixtures_RowDataBound"
                                    RowStyle-Height="30"
                                    RowStyle-HorizontalAlign="Center"
                                    ShowHeader="false">
                                    <Columns>
                                        <asp:BoundField HeaderText="League"/>
                                        <asp:BoundField HeaderText="Date"/>
                                        <asp:BoundField HeaderText="Home"/>
                                        <asp:BoundField HeaderText="Away"/>
                                        <asp:BoundField HeaderText="D_Day"/>
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
