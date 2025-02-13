<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Qua13.aspx.cs" Inherits="HQCWeb.QualityMgt.Qua13.Qua13" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .searchCombo{
            text-align: center;
            z-index : 1;
            top : 6.5%;
            position : absolute; 
        }
    </style>
    <script type="text/javascript">
        function fn_Validation() {
            if ($("#MainContent_ddlShopCd").val() == "") {
                alert("Shop Code를 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlLineCd").val() == "") {
                alert("라인을 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlStnCd").val() == "") {
                alert("공정을 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlCarType").val() == "") {
                alert("차종을 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlPset").val() == "") {
                alert("P Set를 선택하세요.");
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

        var chart1, chart2, char3, chartData1, chartData2, chartData3;
        var cStn, cCarType;

        function createChart() {

            function convertToChartData(originalData) {
                return originalData.map(item => ({
                    X: item.X,
                    Y: parseFloat(item.Y), // Y값을 숫자로 변환
                    MAX: parseFloat(item.Y_MAX),
                    MIN: parseFloat(item.Y_MIN)
                }));
            }

            // XY CHART 1
            chart1 = new AmCharts.AmXYChart();
            chart1.dataProvider = convertToChartData(chartData1);
            $("#spanChart1").text("(최대값 : " + chartData1[0].Y_MAX + " 최소값 : " + chartData1[0].Y_MIN + ")")

            // XY CHART 2
            chart2 = new AmCharts.AmXYChart();
            chart2.dataProvider = convertToChartData(chartData2);
            $("#spanChart2").text("(최대값 : " + chartData2[0].Y_MAX + " 최소값 : " + chartData2[0].Y_MIN + ")")

            // XY CHART 3
            chart3 = new AmCharts.AmXYChart();
            chart3.dataProvider = convertToChartData(chartData3);
            $("#spanChart3").text("(최대값 : " + chartData3[0].Y_MAX + " 최소값 : " + chartData3[0].Y_MIN + ")")

            // AXES
            var valueAxisX1 = new AmCharts.ValueAxis();
            valueAxisX1.position = "bottom";
            valueAxisX1.axisAlpha = 0;
            valueAxisX1.autoGridCount = true; // 자동으로 그리드 개수 조정
            chart1.addValueAxis(valueAxisX1);

            var valueAxisY1 = new AmCharts.ValueAxis();
            valueAxisY1.position = "left";
            valueAxisY1.axisAlpha = 0;
            valueAxisY1.precision = 1; // 소수점 1자리 표시
            valueAxisY1.labelFunction = function (value) {
                return value.toFixed(1); // 원하는 소수점 자리수로 출력
            };
            chart1.addValueAxis(valueAxisY1);

            var valueAxisX2 = new AmCharts.ValueAxis();
            valueAxisX2.position = "bottom";
            valueAxisX2.axisAlpha = 0;
            valueAxisX2.autoGridCount = true; // 자동으로 그리드 개수 조정
            chart2.addValueAxis(valueAxisX2);

            var valueAxisY2 = new AmCharts.ValueAxis();
            valueAxisY2.position = "left";
            valueAxisY2.axisAlpha = 0;
            valueAxisY2.precision = 1; // 소수점 1자리 표시
            valueAxisY2.labelFunction = function (value) {
                return value.toFixed(1); // 원하는 소수점 자리수로 출력
            };
            chart2.addValueAxis(valueAxisY2);

            var valueAxisX3 = new AmCharts.ValueAxis();
            valueAxisX3.position = "bottom";
            valueAxisX3.axisAlpha = 0;
            valueAxisX3.autoGridCount = true; // 자동으로 그리드 개수 조정
            chart3.addValueAxis(valueAxisX3);

            var valueAxisY3 = new AmCharts.ValueAxis();
            valueAxisY3.position = "left";
            valueAxisY3.axisAlpha = 0;
            valueAxisY3.precision = 1; // 소수점 1자리 표시
            valueAxisY3.labelFunction = function (value) {
                return value.toFixed(1); // 원하는 소수점 자리수로 출력
            };
            chart3.addValueAxis(valueAxisY3);

            // 개수 그래프
            var graph1 = new AmCharts.AmGraph();
            graph1.xField = "X";
            graph1.yField = "Y";
            graph1.lineAlpha = 0;
            graph1.bullet = "circle";
            graph1.bulletColor = "#8F6F4D";
            graph1.balloonText = "순번:[[X]] 앵글값:[[Y]]";
            graph1.balloon = { "borderColor": "#8F6F4D" }; // 외곽선 색상 지정
            chart1.addGraph(graph1);

            var minGraph1 = new AmCharts.AmGraph();
            minGraph1.xField = "X";  // 순번
            minGraph1.yField = "MIN"; // 최소값
            minGraph1.lineColor = "#FF0000";
            minGraph1.lineThickness = 1;
            minGraph1.bullet = "none";     // 점 표시 없음
            minGraph1.balloonText = "최소값:[[MIN]]";
            chart1.addGraph(minGraph1);

            var maxGraph1 = new AmCharts.AmGraph();
            maxGraph1.xField = "X";  // 순번
            maxGraph1.yField = "MAX"; // 최대값
            maxGraph1.lineColor = "#0000FF";
            maxGraph1.lineThickness = 1;
            maxGraph1.bullet = "none";     // 점 표시 없음
            maxGraph1.balloonText = "최대값:[[MAX]]";
            chart1.addGraph(maxGraph1);

            var graph2 = new AmCharts.AmGraph();
            graph2.xField = "X";
            graph2.yField = "Y";
            graph2.lineAlpha = 0;
            graph2.bullet = "circle";
            graph2.bulletColor = "#DCEBF8";
            graph2.balloonText = "순번:[[X]] 앵글값:[[Y]]";
            graph2.balloon = { "borderColor": "#DCEBF8" }; // 외곽선 색상 지정
            chart2.addGraph(graph2);

            var minGraph2 = new AmCharts.AmGraph();
            minGraph2.xField = "X";  // 순번
            minGraph2.yField = "MIN"; // 최소값
            minGraph2.lineColor = "#FF0000";
            minGraph2.lineThickness = 1;
            minGraph2.bullet = "none";     // 점 표시 없음
            minGraph2.balloonText = "최소값:[[MIN]]";
            chart2.addGraph(minGraph2);

            var maxGraph2 = new AmCharts.AmGraph();
            maxGraph2.xField = "X";  // 순번
            maxGraph2.yField = "MAX"; // 최대값
            maxGraph2.lineColor = "#0000FF";
            maxGraph2.lineThickness = 1;
            maxGraph2.bullet = "none";     // 점 표시 없음
            maxGraph2.balloonText = "최대값:[[MAX]]";
            chart2.addGraph(maxGraph2);

            var graph3 = new AmCharts.AmGraph();
            graph3.xField = "X";
            graph3.yField = "Y";
            graph3.lineAlpha = 0;
            graph3.bullet = "circle";
            graph3.bulletColor = "#DCDDDD";
            graph3.balloonText = "순번:[[X]] 토크:[[Y]]";
            graph3.balloon = { "borderColor": "#DCDDDD" }; // 외곽선 색상 지정
            chart3.addGraph(graph3);

            var minGraph3 = new AmCharts.AmGraph();
            minGraph3.xField = "X";  // 순번
            minGraph3.yField = "MIN"; // 최소값
            minGraph3.lineColor = "#FF0000";
            minGraph3.lineThickness = 1;
            minGraph3.bullet = "none";     // 점 표시 없음
            minGraph3.balloonText = "최소값:[[MIN]]";
            chart3.addGraph(minGraph3);

            var maxGraph3 = new AmCharts.AmGraph();
            maxGraph3.xField = "X";  // 순번
            maxGraph3.yField = "MAX"; // 최대값
            maxGraph3.lineColor = "#0000FF";
            maxGraph3.lineThickness = 1;
            maxGraph3.bullet = "none";     // 점 표시 없음
            maxGraph3.balloonText = "최대값:[[MAX]]";
            chart3.addGraph(maxGraph3);

            // CURSOR
            var chartCursor = new AmCharts.ChartCursor();
            chartCursor.cursorAlpha = 0;
            chartCursor.zoomable = true;
            chartCursor.categoryBalloonEnabled = false;
            chart1.addChartCursor(chartCursor);
            chart2.addChartCursor(chartCursor);
            chart3.addChartCursor(chartCursor);

            // WRITE
            chart1.validateData();
            chart2.validateData();
            chart3.validateData();

            chart1.write("chart1_div");
            chart2.write("chart2_div");
            chart3.write("chart3_div");
        }

        function resetChart() {
            createChart();
        }

        function wait(milisec) {
            let start = Date.now(), now = start;
            while (now - start < milisec) {
                now = Date.now();
            }
        }
    </script>

    <asp:HiddenField ID="hidWidth" runat="server" Value="500" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="500" />

    <asp:HiddenField ID="hidGraphWidth" runat="server" />
    <asp:HiddenField ID="hidGraphHeight" runat="server" />

    <asp:TextBox ID="hidGridHeight" runat="server" style="display:none;" />

    <div class="contents" tabindex="0">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">Qua13</asp:Label></h3>
            <div class="al-r">
               <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                <input type="button" value="New" onclick="javascript:fn_Register();" ID="btnNew" runat="server" visible="false" />
                <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('Qua13'); return false;" />
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
	                <col style="width:80px;">
                    <col style="width:220px;">
                    <col style="width:70px;">
                    <col style="width:270px;">
                    <col style="width:270px;">
                    <col style="width:60px;">
                    <col style="width:120px;">
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbShopCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlShopCd" runat="server" OnSelectedIndexChanged="ddlShopCd_SelectedIndexChanged" AutoPostBack="true" Width="270"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbLineCd" runat="server"></asp:Label></th>
                    <td style="border-right: none;">
                        <asp:DropDownList ID="ddlEopFlag" runat="server" Width="70" OnSelectedIndexChanged="ddlEopFlag_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
                    </td>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlLineCd" runat="server" OnSelectedIndexChanged="ddlLineCd_SelectedIndexChanged" AutoPostBack="true" Width="270"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <th><asp:Label ID="lbStnCd" runat="server"></asp:Label></th>
                    <td style = "border-right: 0px;">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlStnCd" runat="server" OnSelectedIndexChanged="ddlStnCd_SelectedIndexChanged" AutoPostBack="true" Width="250"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlLineCd" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                        </td>
                        <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlCarType" runat="server" OnSelectedIndexChanged="ddlCarType_SelectedIndexChanged" AutoPostBack="true" Width="250"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlLineCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlStnCd" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <th><asp:Label ID="lbPset" runat="server"></asp:Label></th>
                    <td><asp:UpdatePanel runat="server" ID="UpdatePanel5" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlPset" runat="server" Width="100"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlLineCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlStnCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlCarType" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td class="al-r">
                    </td>
                </tr>
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
                    <th><asp:Label ID="lbRslt1" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlRslt1" runat="server" Width="100"></asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>

        <div class="graph_wrap mt14" style="height:calc(100vh - 220px); overflow:auto;" id="divChart">
            <asp:UpdatePanel ID="up2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table>
                            <tr>
                                <td>
                                    <p class="sub_tit">런다운 앵글 <span id="spanChart1"></span></p>
                                    <div id="chart1_div" style="height: calc(100vh - 650px); width : calc(100vw - 100px)"></div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <p class="sub_tit">파이널 앵글 <span id="spanChart2"></span></p>
                                    <div id="chart2_div" style="height: calc(100vh - 650px); width : calc(100vw - 100px)"></div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <p class="sub_tit">체결 토크 <span id="spanChart3"></span></p>
                                    <div id="chart3_div" style="height: calc(100vh - 650px); width : calc(100vw - 100px)"></div>
                                </td>
                            </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>

        <br />
    </div>

</asp:Content>
