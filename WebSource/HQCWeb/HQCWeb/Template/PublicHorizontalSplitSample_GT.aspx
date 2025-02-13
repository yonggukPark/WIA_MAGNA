<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="PublicHorizontalSplitSample_GT.aspx.cs" Inherits="HQCWeb.Template.PublicHorizontalSplitSample_GT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }

        var column, field, data;
        var container, dataProvider, gridView;

        // 그리드 생성
        function createGrid(_val) {
            container = document.getElementById('realgrid_c_2');
            dataProvider = new RealGrid.LocalDataProvider(false);
            gridView = new RealGrid.GridView(container);
            gridView.setDataSource(dataProvider);
            gridView.setColumns(column);
            dataProvider.setFields(field);
            dataProvider.setRows(data);
            gridView.checkBar.visible = false;
            gridView.stateBar.visible = false;
            setContextMenu(gridView);
            setPaging();
            settingGrid(_val); if (data != undefined) document.getElementById('rowCnt').innerHTML = data.length;
        }

        function settingGrid(_val) {
            //PK 컬럼 클릭시 동작
            gridView.onCellClicked = function (grid, clickData) {
                if (clickData.column == 'COL_1') {
                    var current = gridView.getCurrent();
                    var value = dataProvider.getValue(current.dataRow, "KEY_HID");
                    fn_Modify(value);
                }
            }

            if (_val != undefined) {
                gridView.setFixedOptions({
                    colCount: _val
                });
            }

            gridView.columnByName("COL_1").styleName = "grid-primary-column"
            gridView.columnByName("KEY_HID").visible = false;
            dataProvider.softDeleting = true;
            //gridView.displayOptions.fitStyle = "even";//그리드 채우기
            gridView.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gridView.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gridView.pasteOptions.enabled = false;//붙여넣기 금지
        }

        var chart1, chart2, chartData1, chartData2;

        function createChart() {
            // SERIAL CHART
            chart1 = new AmCharts.AmSerialChart();
            chart1.dataProvider = chartData1;
            chart1.categoryField = "CATEGORY_FIELD";

            // SERIAL CHART
            chart2 = new AmCharts.AmSerialChart();
            chart2.dataProvider = chartData2;
            chart2.categoryField = "CATEGORY_FIELD";
            // the following two lines makes chart 3D
            //chart2.depth3D = 20;
            //chart2.angle = 30;

            // AXES
            // category
            var categoryAxis = chart1.categoryAxis;
            categoryAxis.labelRotation = 60;
            categoryAxis.dashLength = 5;
            categoryAxis.gridPosition = "start";

            var categoryAxis = chart2.categoryAxis;
            categoryAxis.labelRotation = 60;
            categoryAxis.dashLength = 5;
            categoryAxis.gridPosition = "start";

            var valueAxis = chart1.valueAxes;
            valueAxis.minimum = 0;

            var valueAxis = chart2.valueAxes;
            valueAxis.minimum = 0;

            // 계획그래프
            var graph1 = new AmCharts.AmGraph();
            graph1.valueField = "PLAN_QTY";
            graph1.fillColors = "#ffb25b";
            graph1.legendColor = "#ffb25b";
            graph1.balloonText = "<span style='font-size:14px'>[[category]]: <b>[[value]]</b></span>";
            graph1.labelText = "[[value]]";
            graph1.type = "column";
            graph1.title = "계획";
            graph1.lineAlpha = 0;
            graph1.fillAlphas = 1;
            chart1.addGraph(graph1);

            var graph2 = new AmCharts.AmGraph();
            graph2.valueField = "PLAN_QTY";
            graph2.fillColors = "#ffb25b";
            graph1.legendColor = "#ffb25b";
            graph2.balloonText = "<span style='font-size:14px'>[[category]]: <b>[[value]]</b></span>";
            graph2.labelText = "[[value]]";
            graph2.type = "column";
            graph2.title = "계획";
            graph2.lineAlpha = 0;
            graph2.fillAlphas = 1;
            chart2.addGraph(graph2);

            // 실적그래프
            var graph1 = new AmCharts.AmGraph();
            graph1.valueField = "ACT_QTY";
            graph1.fillColors = "#94c5ff";
            graph1.legendColor = "#94c5ff";
            graph1.balloonText = "<span style='font-size:14px'>[[category]]: <b>[[value]]</b></span>";
            graph1.labelText = "[[value]]";
            graph1.type = "column";
            graph1.title = "실적";
            graph1.lineAlpha = 0;
            graph1.fillAlphas = 1;
            chart1.addGraph(graph1);

            var graph2 = new AmCharts.AmGraph();
            graph2.valueField = "ACT_QTY";
            graph2.fillColors = "#94c5ff";
            graph1.legendColor = "#94c5ff";
            graph2.balloonText = "<span style='font-size:14px'>[[category]]: <b>[[value]]</b></span>";
            graph2.labelText = "[[value]]";
            graph2.type = "column";
            graph2.title = "실적";
            graph2.lineAlpha = 0;
            graph2.fillAlphas = 1;
            chart2.addGraph(graph2);

            var legend1 = new AmCharts.AmLegend();
            legend1.useGraphSettings = true;
            legend1.position = "top";
            legend1.align = "center";

            var legend2 = new AmCharts.AmLegend();
            legend2.useGraphSettings = true;
            legend2.position = "top";
            legend2.align = "center";

            chart1.addLegend(legend1);
            chart2.addLegend(legend2);


            // CURSOR
            var chartCursor = new AmCharts.ChartCursor();
            chartCursor.cursorAlpha = 0;
            chartCursor.zoomable = false;
            chartCursor.categoryBalloonEnabled = false;
            chart1.addChartCursor(chartCursor);
            chart2.addChartCursor(chartCursor);

            //chart2.creditsPosition = "top-right";
            //chart2.chartFile = "Sample02";

            // WRITE
            chart1.write("chart1_div");
            chart2.write("chart2_div");
        }

        function adjustMaxValue() {
            var maxValue = 0;
            chartData1.forEach(function (data) {
                if (data.ACT_QTY > maxValue) maxValue = data.ACT_QTY;
                else if (data.PLAN_QTY > maxValue) maxValue = data.PLAN_QTY;
            });
            var axis = chart1.valueAxes[0];
            axis.maximum = maxValue + (maxValue * 0.1);  // Add 10% headroom
            chart1.validateData();

            maxValue = 0;
            chartData2.forEach(function (data) {
                if (data.ACT_QTY > maxValue) maxValue = data.ACT_QTY;
                else if (data.PLAN_QTY > maxValue) maxValue = data.PLAN_QTY;
            });
            var axis = chart2.valueAxes[0];
            axis.maximum = maxValue + (maxValue * 0.1);  // Add 10% headroom
            chart2.validateData();
        }

        function resetChart() {
            createChart();
            adjustMaxValue();
        }
    </script>

    <style>
        .grid-primary-column {
            font-weight: bold;
            text-decoration: underline; cursor: pointer;
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
            <h3><asp:Label ID="lbTitle" runat="server">MENU_ID</asp:Label></h3>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:100px;" />
                    <col style="width:190px;" />
                    <col />
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbCond_01" runat="server"></asp:Label></th>
                    <td><asp:TextBox ID="txtCondi1" runat="server" style="width:150px;"></asp:TextBox></td>
                    <td class="al-r">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="fn_WatingCall();" Visible="false" />
                        <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('GridExcel'); return false;" />
                    </td>
                </tr>
            </table>
        </div>

        <div class="graph_wrap mt14" style="height:300px; overflow:auto;" id="divChart">
            <asp:UpdatePanel ID="up2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td style="width:25%">
                                <div id="chart1_div" style="height: 280px;"></div>
                            </td>
                            <td>
                                <div id="chart2_div" style="height: 280px;"></div>
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

        <p class="sub_tit">타이틀</p><!--통계수치 테이블의 타이틀-->

        <div style="width:100%;">
            <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="realgrid_c_2" class="realgrid_c_2"></div>
                    <table>
                        <tr>
                            <td>
                                <div class="toolbar">
                                    <div id="gridPage" style="float:left" onselectstart="return false" ondragstart="return false"></div>
                                    &nbsp;
                                    <select id="current_page_value" onchange="setPaging(); return false;" style="font-size:12px; display:none; margin-top:10px; float:left" runat="server">
                                    </select>
                                    <input type="button" value="Set" id="btnSet" onclick="getCol('MENU_ID'); return false;" style="display:none;" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>
