<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FootballPredictionByOdds.ascx.cs" Inherits="Pose_sports_statistics.Controls.FootballPredictionByOdds" %>

<asp:Panel ID="pnl_footballPredictionOdds" runat="server">
    <table>
        <tr>
            <td>
                <asp:ListView ID="lv_footballPredictionByOdds" runat="server"
                    SelectMethod="GetPredictionByOdds">
                    <LayoutTemplate>
                        <table>
                            <tr>
                                <td>
                                    <table style="padding:15px" runat="server">
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
                            <br />
                            <table border="1">
                                <tr style="text-align:center">
                                    <td rowspan="4" style="width:140px"><%# Eval("BookMakerName") %></td>
                                    <td style="width:85px">Home</td>
                                    <td style="width:85px">Draw</td>
                                    <td style="width:85px">Away</td>
                                </tr>
                                <tr style="text-align:center">
                                    <td><%# Eval("HomeOdds") %></td>
                                    <td><%# Eval("DrawOdds") %></td>
                                    <td><%# Eval("AwayOdds") %></td>
                                </tr>
                                <tr style=" text-align:center">
                                    <td colspan="3"><%# Eval("RefundRate", "{0:#.0}") %> %</td>
                                </tr>
                                <tr style="text-align:center">
                                    <td><%# Eval("PredictionPG_Home", "{0:#.0}") %> %</td>
                                    <td><%# Eval("PredictionPG_Draw", "{0:#.0}") %> %</td>
                                    <td><%# Eval("PredictionPG_Away", "{0:#.0}") %> %</td>
                                </tr>
                            </table>
                        </td>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <table>
                            <tr>
                                <td>No odds data</td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                </asp:ListView>
            </td>
        </tr>
    </table>
</asp:Panel>