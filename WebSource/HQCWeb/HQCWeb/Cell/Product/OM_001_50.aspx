<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="OM_001_50.aspx.cs" Inherits="HQCWeb.Cell.Product.OM_001_50" %>

<%--<%@ Register Src="~/UserContorls/GridControl.ascx" TagPrefix="uc1" TagName="GridControl" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .searchCombo_2{
            text-align: center;
            z-index : 1;
            top : 37%;
            position : absolute; 
        }
    </style>

    <script type="text/javascript">

        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }

        function fn_hide() {
            if ($("#div_Search").css("display") == "none") {
                $("#div_Search").show();

                if (screen.width == 1280) {
                    $(".dxgvCSD").attr("style", "height:272px; overflow: scroll;");
                } else {
                    $(".dxgvCSD").attr("style", "height:260px; overflow: scroll;");
                }
            } else {
                $("#div_Search").hide();

                if (screen.width == 1280) {
                    $(".dxgvCSD").attr("style", "height:408px; overflow: scroll;");
                } else {
                    $(".dxgvCSD").attr("style", "height:396px; overflow: scroll;");
                }
            }
        }

    </script>
    <asp:HiddenField ID="hidWidth" runat="server" Value="500" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="500" />

    <%--<asp:HiddenField ID="hidGraphWidth" runat="server" />
    <asp:HiddenField ID="hidGraphHeight" runat="server" />--%>

    <asp:TextBox ID="hidGraphWidth" runat="server" style="display:none;" />
    <asp:TextBox ID="hidGraphHeight" runat="server" style="display:none;" />

    <asp:TextBox ID="hidGridHeight" runat="server" style="display:none;" />



    <div class="contents">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">WEB_11200</asp:Label></h3>
            <ul class="win_btn">
                <li><a href="javascript:fn_hide();" class="reduce">축소</a></li>
            </ul>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13" id="div_Search">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:100px;" />
                    <col style="width:190px;" />
                    <col style="width:100px;" />
                    <col style="width:190px;" />
                    <col style="width:100px;" />
                    <col style="width:190px;" />
                    <col />
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbWorkDate" runat="server"></asp:Label></th>
                    <td><asp:TextBox ID="txtDate" runat="server" style="width:80px; z-index:200;"></asp:TextBox></td>
                    <th><asp:Label ID="lbShift" runat="server"></asp:Label></th>
                    <td><asp:DropDownList ID="ddlShift" runat="server"></asp:DropDownList></td>
                    <th><asp:Label ID="lbOperation" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel ID="up3" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlOperation" runat="server" OnSelectedIndexChanged="ddlOperation_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbPOType" runat="server"></asp:Label></th>
                    <td>
                        <div class="searchCombo_2" style="width:125px;">
                            <asp:TextBox id="txtPOType" runat="server" placeholder="Select" autocomplete="off"></asp:TextBox>
                        </div>
                    </td>
                    <th><asp:Label ID="lbPO" runat="server"></asp:Label></th>
                    <td>
                        <div class="searchCombo_2" style="width:150px;">
                            <asp:TextBox id="txtPO" runat="server" placeholder="Select" autocomplete="off"></asp:TextBox>
                            <asp:TextBox id="txtPOHidden" runat="server" style="display:none;"></asp:TextBox>
                        </div>
                    </td>
                    <th><asp:Label ID="lbSearchType" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel ID="up4" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlSearchType" runat="server" OnSelectedIndexChanged="ddlSearchType_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:RadioButton ID="rb01" runat="server" Text="Move Hourly"            GroupName="rbChart" OnCheckedChanged="rb01_CheckedChanged" AutoPostBack="true"  /> &nbsp;
                        <asp:RadioButton ID="rb02" runat="server" Text="Move By Sum Hour"       GroupName="rbChart" OnCheckedChanged="rb02_CheckedChanged" AutoPostBack="true"  /> &nbsp;
                        <asp:RadioButton ID="rb03" runat="server" Text="Move By DRUK Tools"     GroupName="rbChart" OnCheckedChanged="rb03_CheckedChanged" AutoPostBack="true"  /> &nbsp;&nbsp;&nbsp;
                    </td>
                    <td class="al-r">
                        <asp:Button ID="btnSearch" runat="server" Text="조회" OnClick="btnSearch_Click" OnClientClick="fn_WatingCall();" Visible="false" />
                        <asp:Button ID="btnReload" runat="server" Text="Reload" OnClick="btnReload_Click" style="display:none;" />
                        <asp:Button ID="btnFunctionCall" runat="server" Text="FunctionCall" OnClick="btnFunctionCall_Click"  style="display:none;"  />
                    </td>
                </tr>
            </table>
        </div>

        <div class="graph_wrap mt14" style="height:340px;" id="divChart">
            <asp:UpdatePanel ID="up2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <dxchartsui:WebChartControl ID="chart_01" runat="server" CrosshairEnabled="false" Visible="true">
                    </dxchartsui:WebChartControl>


                    <dxchartsui:WebChartControl ID="chart_02" runat="server" CrosshairEnabled="false" Visible="false">
                    </dxchartsui:WebChartControl>


                    <dxchartsui:WebChartControl ID="chart_03" runat="server" CrosshairEnabled="false" Visible="false">
                    </dxchartsui:WebChartControl>

                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnReload" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnFunctionCall" EventName="Click" />

                    <asp:AsyncPostBackTrigger ControlID="rb01" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="rb02" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="rb03" EventName="CheckedChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
       
        <p class="sub_tit" style="padding-top:10px;">시간별투입수량정보</p><!--통계수치 테이블의 타이틀-->

        <div style="width:100%; height:300px;">
            <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                <ContentTemplate>
                    <%--<uc1:GridControl runat="server" ID="grid" Height="265" Horizontal="Y" />--%>

                    <%--
                        OnHtmlRowPrepared="grid_HtmlRowPrepared"
                        OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared"
                    --%>

                    <dx:ASPxGridView ID="grid" ClientInstanceName="grid" 
                        runat="server" Width="100%" 
                        AutoGenerateColumns="True" 
                        EnableRowsCache="false"
                        OnHtmlRowPrepared="grid_HtmlRowPrepared">
                        <SettingsLoadingPanel ImagePosition="Top" Delay="1000" />    
                        <Settings VerticalScrollBarMode="Visible" VerticalScrollBarStyle="VirtualSmooth"  HorizontalScrollBarMode="Visible" /> 
                        <SettingsPager Mode="EndlessPaging"  PageSize="100000"/>

                        <Styles>
                            <Header HorizontalAlign="Center"  BackColor="#edf5fa" ForeColor="#2e6e99" />  
                        </Styles>
                    </dx:ASPxGridView>

                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnFunctionCall" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

    <script>

        if (screen.width == 1280) {
            $("#MainContent_hidGraphWidth").val(screen.width - 144);
            $("#MainContent_hidGraphHeight").val("260");

            $("#divChart").attr("style", "height:260px;");

            $("#MainContent_hidGridHeight").val("272");
        }
        else {
            $("#MainContent_hidGraphWidth").val(screen.width - 316);
            $("#MainContent_hidGraphHeight").val("338");

            $("#divChart").attr("style", "height:340px;");

            $("#MainContent_hidGridHeight").val("260");
        }

        var comType1;

        jQuery(document).ready(function ($) {
            //$("#MainContent_btnReload").click();

            $("#MainContent_rb01").prop("checked", true);
            
            $("#MainContent_txtPOType").comboTree({
                source: <%=strPOTypeJson%>,
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true,
                valueChange: false,
                functionCall: true,
                functionCallFunc: "FuncCall" 
            });

            $("#MainContent_txtPO").comboTree({
                source: [{ "id": "11", "title": "11" }, { "id": "22", "title": "22" }],
                isMultiple: true,
                cascadeSelect: true,
                collapse: false,
                selectAll: true,
                valueChange: false
            });
        });

        function FuncCall() {
            $("#MainContent_btnFunctionCall").click();
        }

        function fn_SetPODDL(jsonval) {
            $("#MainContent_txtPO").comboTree({
                source: []
            });

            $("#MainContent_txtPO").comboTree({
                source: jsonval,
                isMultiple: true,
                cascadeSelect: true,
                collapse: false,
                selectAll: true,
                valueChange: false
            });
        }

        function fn_ConditionSH(_val) {

            if (_val == "S") {
                $("#MainContent_lbSearchType").show();
                $("#MainContent_up4").show();
            } else {
                $("#MainContent_lbSearchType").hide();
                $("#MainContent_up4").hide();
            }

        }


    </script>

</asp:Content>

