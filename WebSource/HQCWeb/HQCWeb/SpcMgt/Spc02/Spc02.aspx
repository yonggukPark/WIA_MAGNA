<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Spc02.aspx.cs" Inherits="HQCWeb.SpcMgt.Spc02" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        function fn_Validation() {
            if ($("#MainContent_ddlShopCd").val() == "") {
                alert("Shop Code를 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlLineCd").val() == "") {
                alert("라인을 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlCarType").val() == "") {
                alert("차종을 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlCarType").val() == "") {
                alert("공정을 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlDevId").val() == "") {
                alert("검사기를 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlWorkCd").val() == "") {
                alert("작업순서를 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlItemCd").val() == "") {
                alert("검사항목을 선택하세요.");
                return false;
            } else {
                fn_WatingCall();
                return true;
            }
            return true;
        }

        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }

        var chart, chartData;

        function createChart() {
            // SERIAL CHART
            chart = new AmCharts.AmSerialChart();
            chart.dataProvider = chartData;
            chart.categoryField = "ID";
            chart.fontSize = 12;

            // AXES
            // category
            var categoryAxis = chart.categoryAxis;
            categoryAxis.labelRotation = 60;
            categoryAxis.dashLength = 5;
            categoryAxis.gridPosition = "start";

            // first value axis (on the left)
            var valueAxis1 = new AmCharts.ValueAxis();
            valueAxis1.axisColor = "#FF6600";
            valueAxis1.axisThickness = 2;
            chart.addValueAxis(valueAxis1);

            // GRAPHS
            $("#divChart").show();

            // first graph
            var graph1 = new AmCharts.AmGraph();
            graph1.type = "column";
            graph1.valueAxis = valueAxis1; // we have to indicate which value axis should be used
            graph1.lineColor = "#a668d5";
            graph1.valueField = "VAL";
            //graph1.title = "X bar";
            graph1.lineAlpha = 1;
            graph1.fillAlphas = 1;
            chart.addGraph(graph1);

            // second graph
            var graph2 = new AmCharts.AmGraph();
            graph2.valueAxis = valueAxis1; // we have to indicate which value axis should be used
            //graph2.title = "UCL";
            graph2.valueField = "VAL";
            graph2.bullet = "square";
            graph2.hideBulletsCount = 30;
            graph2.bulletBorderThickness = 1;
            chart.addGraph(graph2);

            // CURSOR
            var chartCursor = new AmCharts.ChartCursor();
            chartCursor.cursorAlpha = 0.1;
            chartCursor.fullWidth = true;
            chartCursor.valueLineBalloonEnabled = true;
            chart.addChartCursor(chartCursor);

            //// LEGEND
            //var legend = new AmCharts.AmLegend();
            //legend.marginLeft = 110;
            //legend.useGraphSettings = true;
            //chart.addLegend(legend);

            // WRITE
            chart.write("chart_div");
        }

        function resetChart() {
            createChart();
            //adjustMaxValue();
        }
    </script>

    <style>
        .grid-primary-column {
            font-weight: bold;
            text-decoration: underline; cursor: pointer;
        }

        .ellipsis-dropdown {
        width: 250px; /* 고정 너비를 설정합니다 */
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
        }
        .ellipsis-dropdown option {
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }

        .number-column {
            text-align: right;
        }

        .string-column {
            text-align: left;
        }
    </style>

    <asp:HiddenField ID="hidWidth" runat="server" Value="500" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="500" />

    <asp:HiddenField ID="hidGraphWidth" runat="server" />
    <asp:HiddenField ID="hidGraphHeight" runat="server" />

    <asp:TextBox ID="hidGridHeight" runat="server" style="display:none;" />

    <div class="contents" tabindex="0">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">Spc02</asp:Label></h3>
            <div class="al-r">
               <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                <input type="button" value="New" onclick="javascript:fn_Register();" ID="btnNew" runat="server" visible="false" />
                <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('Spc02'); return false;" />
                <%--<asp:Button ID="btnFunctionCall" runat="server" Text="FunctionCall" OnClick="btnFunctionCall_Click"  style="display:none;"  />--%>
            </div>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:60px;">
                    <col style="width:290px;">
                    <col style="width:60px;">
                    <col style="width:270px;">
                    <col style="width:60px;">
	                <col style="width:80px;">
	                <col style="width:220px;">
                    <col style="width:60px;">
                    <col style="width:270px;">
                    <col />
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbProdDt" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                            <ContentTemplate>
                            <asp:TextBox ID="txtFromDt" runat="server" Width="90" onkeydown="return fn_EventKeyChk(event);" onkeyup="removeChar(event);" style="IME-MODE:disabled; background-color:white; color:black; padding-left:8px; padding-right:8px;"></asp:TextBox> 
                            <span id="spBetween">~</span>
                            <asp:TextBox ID="txtToDt" runat="server" Width="90" onkeydown="return fn_EventKeyChk(event);" onkeyup="removeChar(event);" style="background-color:white; color:black; padding-left:8px; padding-right:8px;"></asp:TextBox>
                            <asp:DropDownList ID="ddlWctCd" runat="server" OnSelectedIndexChanged="ddlWct_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlWctCd" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <th><asp:Label ID="lbShopCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlShopCd" runat="server" OnSelectedIndexChanged="ddlShopCd_SelectedIndexChanged" AutoPostBack="true" Width="250"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbLineCd" runat="server"></asp:Label></th>
                    <td style="border-right: none;">
                        <asp:DropDownList ID="ddlEopFlag" runat="server" Width="70" OnSelectedIndexChanged="ddlEopFlag_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
                    </td>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlLineCd" runat="server" OnSelectedIndexChanged="ddlLineCd_SelectedIndexChanged" AutoPostBack="true" Width="250"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlEopFlag" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <th><asp:Label ID="lbCarType" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlCarType" runat="server" OnSelectedIndexChanged="ddlStnCd_ddlCarType_SelectedIndexChanged" AutoPostBack="true" CssClass="ellipsis-dropdown"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlLineCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlEopFlag" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td class="al-r">
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbStnCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel7" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlStnCd" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStnCd_ddlCarType_SelectedIndexChanged" CssClass="ellipsis-dropdown" Width="270"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlLineCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlEopFlag" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <th><asp:Label ID="lbDevId" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlDevId" runat="server" OnSelectedIndexChanged="ddlDevId_SelectedIndexChanged" AutoPostBack="true" CssClass="ellipsis-dropdown"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlLineCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlCarType" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlStnCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlEopFlag" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <th><asp:Label ID="lbWorkCd" runat="server"></asp:Label></th>
                    <td colspan="2">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel5" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlWorkCd" runat="server" width="340" OnSelectedIndexChanged="ddlWorkCd_SelectedIndexChanged" AutoPostBack="true" CssClass="ellipsis-dropdown"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlLineCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlCarType" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlStnCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlDevId" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlEopFlag" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <th><asp:Label ID="lbItemCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel6" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlItemCd" runat="server" AutoPostBack="true" CssClass="ellipsis-dropdown"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlLineCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlCarType" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlStnCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlDevId" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlWorkCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlEopFlag" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td class="al-r">
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbDayNight" runat="server"></asp:Label></th>
                    <td colspan = "8">
                        <asp:RadioButton ID="rdDay" runat="server" Text="주간"    AutoPostBack="false" GroupName="dayGrp" Width="50px" />
                        <asp:RadioButton ID="rdNight" runat="server" Text="야간"    AutoPostBack="false" GroupName="dayGrp" Width="60px" />
                    </td>
                    <td class="al-r">
                    </td>
                </tr>
            </table>
        </div>

        <table>
        <tr>
            <td  style="vertical-align: top; padding-top: 13px;">
                <div style="width:190px; height:150px ">
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table class="tbl_left">
                                <tr>
                                    <th>총시료수</th>
                                    <td>
                                        <asp:Label ID="lblTotal" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th>규격상한(SU)</th>
                                    <td>
                                        <asp:Label ID="lblSU" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th>규격하한(SL)</th>
                                    <td>
                                        <asp:Label ID="lblSL" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th>횟수</th>
                                    <td>
                                        <asp:Label  ID="lblN" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div style="width:190px;height:75px">
                    <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table class="tbl_left">
                            <tr>
                                <th>X bar</th>
                                <td>
                                    <asp:Label ID="lblcopyX"  runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <th>R</th>
                                <td>
                                    <asp:Label ID="lblcopyR"  runat="server"></asp:Label>
                                </td>
                            </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div style="width:190px; height:100px">
                    <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table class="tbl_left">
                            <tr>
                                <th colspan="2">공정능력지수(Pp, Ppk)의 계산</th>
                            </tr>
                            <tr>
                                <th>Pp</th>
                                <td>
                                    <asp:Label ID="lblPp" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <th>Ppk</th>
                                <td>
                                    <asp:Label ID="lblPpk" runat="server"></asp:Label>
                                </td>
                            </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </td>
            <td style="width:15px">&nbsp;</td>
            <td  style="vertical-align: top;">
                <div class="graph_wrap mt14" style="height:500px; overflow:auto; display:none" id="divChart">
                    <asp:UpdatePanel ID="up2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <div id="chart_div" style="height: 480px;"></div>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>
        </table>

    </div>

</asp:Content>
