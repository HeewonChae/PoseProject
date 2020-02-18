<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FootballFixtureList.ascx.cs" Inherits="Pose_sports_statistics.Controls.FootballFixtureList" %>

<asp:Panel ID="pnl_footballFixtureList" runat="server">
    <br/>
    <asp:DropDownList ID="ddl_kindCountry"  
        OnSelectedIndexChanged="DDL_KindCountry_SelectedIndexChanged"
        Height="40" AutoPostBack="true" runat="server">
    </asp:DropDownList>
    <br/>
    <table>
        <tr>
            <td>
                <asp:ListView ID="lv_footballFixtureList" runat="server"
                    SelectMethod="GetFixtures" 
                    OnPagePropertiesChanging="lv_footballFixtureList_PagePropertiesChanging">
                    <LayoutTemplate>
                        <table>
                            <tr>
                                <td>
                                    <table style="width:100%; padding:15px" runat="server">
                                        <tr id="groupPlaceholder" runat="server"/>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <GroupTemplate>
                        <tr runat="server">
                            <td id="itemPlaceholder" runat="server"></td>
                        </tr>
                    </GroupTemplate>
                    <ItemTemplate>
                        <td>
                            <h5>
                                <b><%# Eval("StartTime", "{0:MM/dd HH:mm}") %> </b>
                                <img src="<%# Eval("Flag") %>" height="12" width="16"></img>
                                <b>  <%# Eval("League") %></b>
                            </h5>
                
                            <asp:GridView GridLines="Horizontal" BorderStyle="None" ID="gv_footballFixtures" 
                                AutoGenerateColumns="false" runat="server" DataSource = '<%# Eval("Fixtures") %>'
                                ItemType="Pose_sports_statistics.Models.FootballFixture"
                                OnRowDataBound="GV_FootballFixtures_RowDataBound"
                                ShowHeader="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="SelectFixture" ItemStyle-Width="50" ItemStyle-Height="30" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk_interestedFixture" runat="server" OnCheckedChanged="chk_fixture_InterestedIndexChanged" AutoPostBack="true" ToolTip="<%# Item.FixtureID %>"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="HomeTeam" ItemStyle-Width="250" ItemStyle-Height="30" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <img src="<%# Item.HomeTeam.Logo %>" height="12" width="16">
                                            <asp:HyperLink id="hl_homeTeamLink" ForeColor="Black"
                                                Text="<%# Item.HomeTeam.TeamName %>" runat="server"/> 
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField HeaderText ="HomeOdds" DataField="HomeOdds" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center"/>
                                    <asp:BoundField HeaderText ="DrawOdds" DataField="DrawOdds" ItemStyle-Width="80"  ItemStyle-HorizontalAlign="Center"/>
                                    <asp:BoundField HeaderText ="AwayOdds" DataField="AwayOdds" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center"/>--%>
                                    <asp:TemplateField HeaderText="AwayTeam" ItemStyle-Width="250" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:HyperLink id="hl_awayTeamLink" ForeColor="Black"
                                                Text="<%# Item.AwayTeam.TeamName %>" runat="server"/> 
                                            <img src="<%# Item.AwayTeam.Logo %>" height="12" width="16">
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText ="Status" DataField="StatusShort" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center"/>
                                    <asp:TemplateField HeaderText="Detail" ItemStyle-Width="50">
                                        <ItemTemplate>
                                            <a href="<%#: GetRouteUrl("FootballFixtureByID", new {FixtureID = Item.FixtureID}) %>">
                                                Detail
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <br />
                        </td>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <table>
                            <tr>
                                <td>No data</td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                </asp:ListView>
                <asp:DataPager ID="lv_DataPager" runat="server" PagedControlID="lv_footballFixtureList" PageSize="10">
                        <Fields>
                            <asp:NumericPagerField ButtonType="Link" />
                        </Fields>
                </asp:DataPager>
            </td>
        </tr>
    </table>
</asp:Panel>