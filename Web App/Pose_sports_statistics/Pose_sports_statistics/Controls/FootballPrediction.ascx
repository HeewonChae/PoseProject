<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FootballPrediction.ascx.cs" Inherits="Pose_sports_statistics.Controls.FootballPrediction" %>

<asp:Panel ID="pnl_footballFixtureDetail" runat="server" >
    <h4><b>경기 예측</b></h4>
    <asp:FormView ID="form_prediction" runat="server"
        SelectMethod="GetFootballPrediction"
        OnDataBound="Form_Prediction_DataBound"
        ItemType="Pose_sports_statistics.Models.FootballPrediction">
         <itemtemplate> 
             <asp:Table ID="tb_predictionDetail" 
                 BorderStyle="None"
                 runat="server">
                <asp:TableRow HorizontalAlign ="Center" VerticalAlign="Middle"> 
                    <asp:TableCell Width="100">
                        <b><asp:Label ID="lbl_winPG" runat="server"/></b>
                    </asp:TableCell>
                    <asp:TableCell Width="80">
                        <b><asp:Label ID="lbl_drawPG" runat="server"/></b>
                    </asp:TableCell >
                    <asp:TableCell Width="100">
                        <b><asp:Label ID="lbl_losePG" runat="server"/></b>
                    </asp:TableCell>
                </asp:TableRow>

                 <asp:TableRow HorizontalAlign ="Center" VerticalAlign="Middle">
                    <asp:TableCell ColumnSpan="3">
                        <hr style="width:100%">
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow VerticalAlign="Middle" Height="30">
                    <asp:TableCell HorizontalAlign ="Right">
                        <span><%#Item.GoalsHome %></span>
                    </asp:TableCell><asp:TableCell HorizontalAlign="Center" >
                        <b>Goals</b>
                    </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                        <span><%#Item.GoalsAway %></span>
                    </asp:TableCell>
                </asp:TableRow>

                 <asp:TableRow VerticalAlign="Middle" Height="30">
                    <asp:TableCell HorizontalAlign ="Right">
                        <span><%#Item.Comparison.Forme.Home %></span>
                    </asp:TableCell><asp:TableCell HorizontalAlign="Center" >
                        <b>Form</b>
                    </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                        <span><%#Item.Comparison.Forme.Away %></span>
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow VerticalAlign="Middle" Height="30">
                    <asp:TableCell HorizontalAlign ="Right">
                       <span><%#Item.Comparison.Attack.Home %></span>
                    </asp:TableCell><asp:TableCell HorizontalAlign="Center" >
                        <b>Attack</b>
                    </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                        <span><%#Item.Comparison.Attack.Away %></span>
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow VerticalAlign="Middle" Height="30">
                    <asp:TableCell HorizontalAlign ="Right">
                        <span><%#Item.Comparison.Defense.Home %></span>
                    </asp:TableCell><asp:TableCell HorizontalAlign="Center" >
                        <b>Defense</b>
                    </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                        <span><%#Item.Comparison.Defense.Away %></span>
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow VerticalAlign="Middle" Height="30">
                    <asp:TableCell HorizontalAlign ="Right">
                        <span><%#Item.Comparison.H2H.Home %></span>
                    </asp:TableCell><asp:TableCell HorizontalAlign="Center" >
                        <b>H2H</b>
                    </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                         <span><%#Item.Comparison.H2H.Away %></span>
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow VerticalAlign="Middle" Height="30">
                    <asp:TableCell HorizontalAlign ="Right">
                        <span><%#Item.Comparison.GoalsH2H.Home %></span>
                    </asp:TableCell><asp:TableCell HorizontalAlign="Center" >
                        <b>Goals H2H</b>
                    </asp:TableCell><asp:TableCell HorizontalAlign="Left">
                        <span><%#Item.Comparison.GoalsH2H.Away %></span>
                    </asp:TableCell>
                </asp:TableRow>

                 <asp:TableRow HorizontalAlign ="Center" VerticalAlign="Middle">
                    <asp:TableCell ColumnSpan="3">
                        <hr style="width:100%">
                    </asp:TableCell>
                </asp:TableRow>

                 <asp:TableRow> 
                    <asp:TableCell ColumnSpan="3">
                        <b><span><asp:Label ID="lbl_MatchWinner" runat="server"/></span></b></br>
                        <b><span><asp:Label ID="lbl_UnderOver" runat="server"/></span></b></br>
                        <b><span><asp:Label ID="lbl_Advice" runat="server"/></span></b></br>
                    </asp:TableCell>
                </asp:TableRow>
             </asp:Table>
         </itemtemplate>
    </asp:FormView>
</asp:Panel>