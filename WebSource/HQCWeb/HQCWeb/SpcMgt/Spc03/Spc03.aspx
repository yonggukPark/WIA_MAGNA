<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Spc03.aspx.cs" Inherits="HQCWeb.SpcMgt.Spc03" %>

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

        var chart, chartData1, chartData2;

        function createChart() {
            // SERIAL CHART
            chart = new AmCharts.AmSerialChart();
            chart.dataProvider = chartData1;
            chart.categoryField = "PROD_DT";
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

            //// second value axis (on the right)
            //var valueAxis2 = new AmCharts.ValueAxis();
            //valueAxis2.position = "right"; // this line makes the axis to appear on the right
            //valueAxis2.axisColor = "#FCD202";
            //valueAxis2.gridAlpha = 0;
            //valueAxis2.axisThickness = 2;
            //chart.addValueAxis(valueAxis2);

            //// third value axis (on the left, detached)
            //var valueAxis3 = new AmCharts.ValueAxis();
            //valueAxis3.offset = 50; // this line makes the axis to appear detached from plot area
            //valueAxis3.gridAlpha = 0;
            //valueAxis3.axisColor = "#B0DE09";
            //valueAxis3.axisThickness = 2;
            //chart.addValueAxis(valueAxis3);

            // GRAPHS
            $("#divChart").show();

            // first graph
            var graph1 = new AmCharts.AmGraph();
            graph1.valueAxis = valueAxis1; // we have to indicate which value axis should be used
            graph1.title = "X bar";
            graph1.valueField = "X_BAR";
            graph1.bullet = "round";
            graph1.hideBulletsCount = 30;
            graph1.bulletBorderThickness = 1;
            chart.addGraph(graph1);

            // second graph
            var graph2 = new AmCharts.AmGraph();
            graph2.valueAxis = valueAxis1; // we have to indicate which value axis should be used
            graph2.title = "UCL";
            graph2.valueField = "X_UCL";
            graph2.bullet = "square";
            graph2.hideBulletsCount = 30;
            graph2.bulletBorderThickness = 1;
            chart.addGraph(graph2);

            // third graph
            var graph3 = new AmCharts.AmGraph();
            graph3.valueAxis = valueAxis1; // we have to indicate which value axis should be used
            graph3.title = "LCL";
            graph3.valueField = "X_LCL";
            graph3.bullet = "triangleUp";
            graph3.hideBulletsCount = 30;
            graph3.bulletBorderThickness = 1;
            chart.addGraph(graph3);

            // SU graph
            var graph4 = new AmCharts.AmGraph();
            graph4.valueAxis = valueAxis1; // we have to indicate which value axis should be used
            graph4.title = "SU";
            graph4.valueField = "SU";
            //graph2.bullet = "square";
            //graph2.hideBulletsCount = 30;
            //graph2.bulletBorderThickness = 1;
            chart.addGraph(graph4);

            // SL graph
            var graph5 = new AmCharts.AmGraph();
            graph5.valueAxis = valueAxis1; // we have to indicate which value axis should be used
            graph5.title = "SL";
            graph5.valueField = "SL";
            //graph2.bullet = "square";
            //graph2.hideBulletsCount = 30;
            //graph2.bulletBorderThickness = 1;
            chart.addGraph(graph5);

            // CURSOR
            var chartCursor = new AmCharts.ChartCursor();
            chartCursor.cursorAlpha = 0.1;
            chartCursor.fullWidth = true;
            chartCursor.valueLineBalloonEnabled = true;
            chart.addChartCursor(chartCursor);

            // LEGEND
            var legend = new AmCharts.AmLegend();
            legend.marginLeft = 110;
            legend.useGraphSettings = true;
            chart.addLegend(legend);

            // WRITE
            chart.write("chart_div1");
        }

        function createChart2() {
            // SERIAL CHART
            chart = new AmCharts.AmSerialChart();
            chart.dataProvider = chartData2;
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
            chart.write("chart_div2");
        }

        function createChart3() {
            // SERIAL CHART
            chart = new AmCharts.AmSerialChart();
            chart.dataProvider = chartData1;
            chart.categoryField = "PROD_DT";
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

            // second value axis (on the right)
            //var valueAxis2 = new AmCharts.ValueAxis();
            //valueAxis2.position = "right"; // this line makes the axis to appear on the right
            //valueAxis2.axisColor = "#FCD202";
            //valueAxis2.gridAlpha = 0;
            //valueAxis2.axisThickness = 2;
            //chart.addValueAxis(valueAxis2);

            //// third value axis (on the left, detached)
            //var valueAxis3 = new AmCharts.ValueAxis();
            //valueAxis3.offset = 50; // this line makes the axis to appear detached from plot area
            //valueAxis3.gridAlpha = 0;
            //valueAxis3.axisColor = "#B0DE09";
            //valueAxis3.axisThickness = 2;
            //chart.addValueAxis(valueAxis3);

            // GRAPHS
            $("#divChart").show();

            // first graph
            var graph1 = new AmCharts.AmGraph();
            graph1.valueAxis = valueAxis1; // we have to indicate which value axis should be used
            graph1.title = "R";
            graph1.valueField = "R_DATA";
            graph1.bullet = "round";
            graph1.hideBulletsCount = 30;
            graph1.bulletBorderThickness = 1;
            chart.addGraph(graph1);

            // second graph
            var graph2 = new AmCharts.AmGraph();
            graph2.valueAxis = valueAxis1; // we have to indicate which value axis should be used
            graph2.title = "UCL";
            graph2.valueField = "R_UCL";
            graph2.bullet = "square";
            graph2.hideBulletsCount = 30;
            graph2.bulletBorderThickness = 1;
            chart.addGraph(graph2);

            // third graph
            //var graph3 = new AmCharts.AmGraph();
            //graph3.valueAxis = valueAxis1; // we have to indicate which value axis should be used
            //graph3.title = "LCL";
            //graph3.valueField = "R_LCL";
            //graph3.bullet = "triangleUp";
            //graph3.hideBulletsCount = 30;
            //graph3.bulletBorderThickness = 1;
            //chart.addGraph(graph3);
            

            // CURSOR
            var chartCursor = new AmCharts.ChartCursor();
            chartCursor.cursorAlpha = 0.1;
            chartCursor.fullWidth = true;
            chartCursor.valueLineBalloonEnabled = true;
            chart.addChartCursor(chartCursor);

            // LEGEND
            var legend = new AmCharts.AmLegend();
            legend.marginLeft = 110;
            legend.useGraphSettings = true;
            chart.addLegend(legend);

            // WRITE
            chart.write("chart_div3");
        }

        function adjustMaxValue(param) {
            var maxValue = 0;
            chartData1.forEach(function (data) {
                if (param == "X") {
                    if (data.X_BAR > maxValue) maxValue = data.X_BAR;
                    if (data.X_UCL > maxValue) maxValue = data.X_UCL;
                    if (data.X_LCL > maxValue) maxValue = data.X_LCL;
                }
                else {
                    if (data.R_DATA > maxValue) maxValue = data.R_DATA;
                    if (data.R_UCL > maxValue) maxValue = data.R_UCL;
                    if (data.R_LCL > maxValue) maxValue = data.R_LCL;
                }
            });
            var axis = chart.valueAxes[0];
            axis.maximum = maxValue + (maxValue * 0.1);  // Add 10% headroom
            chart.validateData();
        }

        function resetChart() {
            createChart();
            adjustMaxValue('X');
            createChart2();
            createChart3();
            adjustMaxValue('R');
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
            <h3><asp:Label ID="lbTitle" runat="server">Spc03</asp:Label></h3>
            <div class="al-r">
               <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                <input type="button" value="New" onclick="javascript:fn_Register();" ID="btnNew" runat="server" visible="false" />
                <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('Spc03'); return false;" />
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
                                <asp:DropDownList ID="ddlWorkCd" runat="server" OnSelectedIndexChanged="ddlWorkCd_SelectedIndexChanged" AutoPostBack="true" CssClass="ellipsis-dropdown"></asp:DropDownList>
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
                <div>
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table class="tbl_center">
                                <tr>
                                    <th>총시료수</th>
                                    <td>
                                        <asp:Label ID="lblTotal" runat="server"></asp:Label>
                                    </td>
                                    <th>규격상한(SU)</th>
                                    <td>
                                        <asp:Label ID="lblSU" runat="server"></asp:Label>
                                    </td>
                                    <th>규격하한(SL)</th>
                                    <td>
                                        <asp:Label ID="lblSL" runat="server"></asp:Label>
                                    </td>
                                    <th>횟수</th>
                                    <td>
                                        <asp:Label  ID="lblN" runat="server"></asp:Label>
                                    </td>
                                    <th>X bar</th>
                                    <td>
                                        <asp:Label ID="lblcopyX"  runat="server"></asp:Label>
                                    </td>
                                    <th>R</th>
                                    <td>
                                        <asp:Label ID="lblcopyR"  runat="server"></asp:Label>
                                    </td>
                                    <th>Pp</th>
                                    <td>
                                        <asp:Label ID="lblPp" runat="server"></asp:Label>
                                    </td>
                                    <th>Ppk</th>
                                    <td>
                                        <asp:Label ID="lblPpk" runat="server"></asp:Label>
                                    </td>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>
         <tr>
            <td  style="vertical-align: top;">
                <div class="graph_wrap mt14" style="height:500px; overflow:auto; width: calc(100vw - 80px); display:none" id="divChart">
                    <asp:UpdatePanel ID="up2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td style="width: 33%;">
                                        <div id="chart_div1" style="height: 480px;"></div>
                                    </td>
                                    <td style="width: 33%;">
                                        <div id="chart_div2" style="height: 480px;"></div>
                                    </td>
                                    <td>
                                        <div id="chart_div3" style="height: 480px;"></div>
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
