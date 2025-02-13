<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Res11.aspx.cs" Inherits="HQCWeb.ResultMgt.Res11.Res11" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .searchCombo{
            text-align: center;
            z-index : 9999;
            top : 7%;
            position : absolute; 
        }
        .searchCombo2{
            text-align: center;
            z-index : 1;
            top : 57%;
            position : absolute; 
        }

        .number-column {
            text-align: right;
        }

        .string-column {
            text-align: left;
        }

        .total-column {
            background-color : #FFF2CC;
            border : 1px solid #BBC2CF;
        }

        .achive-column {
            background-color : #C6E0B4;
            border : 1px solid #BBC2CF;
        }

        .get-column {
            text-align: right;
            cursor: pointer;
            font-weight: bold;
            text-decoration: underline; 
        }

        .get-column-total {
            background-color : #FFF2CC;
            border : 1px solid #BBC2CF;
            text-align: right;
            cursor: pointer;
            font-weight: bold;
            text-decoration: underline; 
        }

        .division-column{
            cursor: default;
            font-weight: normal;
            text-decoration: none !important; 
        }
    </style>
    <script type="text/javascript">

        function fn_Validation() {
            fn_WatingCall();
            return true;
        }

        function fn_Chk(pkCode, seq) {

            var jsonData = JSON.stringify({ sParams: pkCode, sParams2: seq });

            $.ajax({
                type: "POST",
                url: "Res11.aspx/GetPkCode",
                data: jsonData,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    fn_Popup(msg.d);
                }
            });
        }

        function fn_Popup(pkCode) {
            fn_PostOpenPop(pkCode, '/ResultMgt/ResComm/ResComm.aspx', "1600", "750");
        }

        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }

        var column, field, data;
        var container, dataProvider, gridView;
        var cLine, cPart;

        // 그리드 생성
        function createGrid(_val) {
            container = document.getElementById('realgrid_c_2');
            dataProvider = new RealGrid.LocalDataProvider(false);
            gridView = new RealGrid.GridView(container);
            gridView.setDataSource(dataProvider);
            gridView.setColumns(column);

            //field 넣기 전 포맷변경(엑셀 문제)
            field.forEach(item => {
                if (item.fieldName == 'PLAN_SUM' || item.fieldName.substring(0, 8) == 'PLAN_DAY') {
                    item.dataType = 'number'; item.subType = 'int'
                }
            });

            //숫자 포맷 변경
            gridView.setFormatOptions({ numberFormat: '#,##0' });

            dataProvider.setFields(field);
            dataProvider.setRows(data);

            container.style.minHeight = "calc(100vh - 550px)"
            container.style.height = (dataProvider.getRowCount() > 0) ? (dataProvider.getRowCount() * 25 + 100) + "px" : "calc(100vh - 550px)";
            gridView.refresh(); // 그리드 새로 고침

            gridView.checkBar.visible = false;
            gridView.stateBar.visible = false;
            setContextMenu(gridView);
            setPaging();
            settingGrid(_val); if (data != undefined) document.getElementById('rowCnt').innerHTML = data.length;
        }

        function settingGrid(_val) {

            if (_val != undefined) {
                gridView.setFixedOptions({
                    colCount: _val
                });
            }

            gridView.setOptions({
                showInnerFocus: false, // 셀 선택 시 기본 테두리 강조 제거 (옵션)
                showLine: true,        // 셀 테두리 선 표시
                lineColor: "#BBC2CF"   // 셀 테두리 선 색상 설정
            });

            //PK 컬럼 클릭시 동작
            gridView.onCellClicked = function (grid, clickData) {

                if (clickData.cellType !== "data") {
                    return; // 데이터 셀이 아니면 이벤트 중단
                }
                if (clickData.column.substring(0, 8) == 'PLAN_DAY' || clickData.column == 'PLAN_SUM') {
                    var current = gridView.getCurrent();
                    var division = dataProvider.getValue(current.dataRow, "DIVISION");
                    if (division == '실적수량') {
                        if (dataProvider.getValue(current.dataRow, current.column) > 0) {
                            var value = dataProvider.getValue(current.dataRow, "KEY_HID");
                            var seq = dataProvider.getValue(current.dataRow, "PROD_DT") + ((clickData.column.substring(0, 8) == 'PLAN_DAY') ? clickData.column.substring(8, 10) : '');
                            fn_Chk(value, seq);
                        }
                        else {
                            alert("데이터가 없습니다.");
                        }
                    }
                }
            }

            gridView.layoutByColumn("SHOP_CD").spanCallback = function (grid, layout, itemIndex) {
                var value = grid.getValue(itemIndex, "SHOP_CD")
                if (value == "합계") {
                    return 3; //가로 병합 수
                }

                return 1;
            };

            //스타일 임의 설정
            gridView.setColumnProperty("PLANT_CD", "styleCallback", function (grid, dataCell) {
                var ret = {}
                ret.style = {
                    background: "white"
                };
                return ret;
            });

            gridView.setColumnProperty("DIVISION", "styleCallback", function (grid, dataCell) {
                var ret = {}
                ret.styleName = "division-column";
                return ret;
            });

            // 스타일 설정 콜백 함수
            gridView.setRowStyleCallback(function (grid, item, fixed) {
                var ret = {};

                var shopcd = grid.getValue(item.index, "SHOP_CD");
                var division = grid.getValue(item.index, "DIVISION");

                // SHOP_CD가 "합계"일 경우
                if (shopcd == '합계') {
                    if (division == '실적수량') {
                        ret.styleName = "get-column-total"
                    }
                    else {
                        ret.styleName = "total-column"
                    }
                    return ret;
                }
                else {

                    if (division == '달성율(%)') {
                        ret.styleName = "achive-column"
                    }
                    else if (division == '실적수량') {
                        ret.styleName = "get-column"
                    }
                    else {
                        ret.styleName = 'white'; 
                        ret.editable = false;
                    }
                    return ret;
                }

                return null;
            });

            gridView.columnByName("PLANT_CD").mergeRule = { criteria: "value" };
            gridView.columnByName("SHOP_CD").mergeRule = { criteria: "value" };
            gridView.columnByName("LINE_NM").mergeRule = { criteria: "value" };
            gridView.columnByName("PART_NO").mergeRule = { criteria: "prevvalues + value" };
            gridView.columnByName("PLAN_NM").mergeRule = { criteria: "prevvalues + value" };
            gridView.columnByName("PLAN_DETAIL_NM").mergeRule = { criteria: "prevvalues + value" };

            for (var i = 0; i < 31; i++) {
                gridView.columnByName("PLAN_DAY" + (i+1).toString().padStart(2, '0')).styleName = "number-column";
            }
            gridView.columnByName("PLAN_SUM").styleName = "number-column";

            gridView.columnByName("KEY_HID").visible = false;
            gridView.columnByName("PROD_DT").visible = false;
            gridView.columnByName("LINE_CD").visible = false;
            gridView.columnByName("P_SHOP_CD").visible = false;
            dataProvider.softDeleting = true;
            //gridView.displayOptions.fitStyle = "even";//그리드 채우기
            gridView.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gridView.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gridView.pasteOptions.enabled = false;//붙여넣기 금지
            //gridView.setRowGroup({ headerStatement: "${columnHeader}: ${groupValue} - ${rowCount} rows" });
            gridView.groupPanel.visible = true;//그룹핑 활성화
            gridView.filterPanel.visible = true;//필터패널 활성화
        }

        //콤보박스 초기화
        jQuery(document).ready(function ($) {
            $("#MainContent_txtLineCd").comboTree({
                source: cLine,
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true,
                onLoadAllChk: true,
                valueChange: true,
                hidCon: "MainContent_txtLineCdHidden",
                functionCall: true,                    // 콤보값 선택시 자바스크립스 함수 호출 여부
                functionCallFunc: "fn_Line_Checked"
            });

            $("#MainContent_txtPartNo").comboTree({
                source: [],
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true,
                onLoadAllChk: true,
                valueChange: true,
                hidCon: "MainContent_txtPartNoHidden"
            });
        });

        //라인코드 재설정
        function fn_Set_Line() {

            $("#MainContent_txtLineCd").comboTree({
                source: cLine,
                comboReload: true,
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true,
                onLoadAllChk: true,
                valueChange: true,
                hidCon: "MainContent_txtLineCdHidden",
                functionCall: true,                    // 콤보값 선택시 자바스크립스 함수 호출 여부
                functionCallFunc: "fn_Line_Checked"
            });
        }

        //라인코드 onchange
        function fn_Line_Checked() {
            $("#MainContent_btnFunctionCall").click();
        }

        //부품코드 재설정
        function fn_Set_Part() {

            $("#MainContent_txtPartNo").comboTree({
                source: cPart,
                comboReload: true,
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true,
                onLoadAllChk: true,
                valueChange: true,
                hidCon: "MainContent_txtPartNoHidden"
            });
        }

        var chart1, chart2, chartData1, chartData2;

        function createChart() {
            // SERIAL CHART
            chart1 = new AmCharts.AmSerialChart();
            chart1.dataProvider = chartData1;
            chart1.categoryField = "CATEGORY_FIELD";
            chart1.fontSize = 12;

            // SERIAL CHART
            chart2 = new AmCharts.AmSerialChart();
            chart2.dataProvider = chartData2;
            chart2.categoryField = "CATEGORY_FIELD";
            chart2.fontSize = 12;

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
            graph1.fillColors = "#FFD6A8";
            graph1.legendColor = "#FFD6A8";
            graph1.balloonText = "<span style='font-size:14px'>[[category]]: <b>[[value]]</b></span>";
            graph1.labelText = "[[value]]";
            graph1.type = "column";
            graph1.title = "계획";
            graph1.lineAlpha = 0;
            graph1.fillAlphas = 1;
            chart1.addGraph(graph1);

            var graph2 = new AmCharts.AmGraph();
            graph2.valueField = "PLAN_QTY";
            graph2.fillColors = "#FFD6A8";
            graph2.legendColor = "#FFD6A8";
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
            graph1.fillColors = "#E0EEFF";
            graph1.legendColor = "#E0EEFF";
            graph1.balloonText = "<span style='font-size:14px'>[[category]]: <b>[[value]]</b></span>";
            graph1.labelText = "[[value]]";
            graph1.type = "column";
            graph1.title = "실적";
            graph1.lineAlpha = 0;
            graph1.fillAlphas = 1;
            chart1.addGraph(graph1);

            var graph2 = new AmCharts.AmGraph();
            graph2.valueField = "ACT_QTY";
            graph2.fillColors = "#E0EEFF";
            graph2.legendColor = "#E0EEFF";
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

            chart1.columnWidth = 0.25
            //chart2.columnWidth = 0.5

            // WRITE
            chart1.write("chart1_div");
            chart2.write("chart2_div");
        }

        function adjustMaxValue() {
            var maxValue = 0;
            chartData1.forEach(function (data) {
                if (data.ACT_QTY > maxValue) maxValue = data.ACT_QTY;
                if (data.PLAN_QTY > maxValue) maxValue = data.PLAN_QTY;
            });
            var axis = chart1.valueAxes[0];
            axis.maximum = maxValue + (maxValue * 0.1);  // Add 10% headroom
            chart1.validateData();

            maxValue = 0;
            chartData2.forEach(function (data) {
                if (data.ACT_QTY > maxValue) maxValue = data.ACT_QTY;
                if (data.PLAN_QTY > maxValue) maxValue = data.PLAN_QTY;
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

    <asp:HiddenField ID="hidWidth" runat="server" Value="500" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="500" />

    <asp:HiddenField ID="hidGraphWidth" runat="server" />
    <asp:HiddenField ID="hidGraphHeight" runat="server" />

    <asp:TextBox ID="hidGridHeight" runat="server" style="display:none;" />

    <div class="contents" tabindex="0">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">Res11</asp:Label></h3>
            <div class ="al-r">
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('Res11'); return false;" />
                <asp:Button ID="btnFunctionCall" runat="server" Text="FunctionCall" OnClick="btnFunctionCall_Click"  style="display:none;"  />
            </div>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:60px;">
                    <col style="width:145px;">
                    <col style="width:60px;">
                    <col style="width:160px;">
                    <col style="width:60px;">
	                <col style="width:80px;">
	                <col style="width:220px;">
                    <col style="width:60px;">
                    <col style="width:120px;">
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbProdDt" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlYear" runat="server"></asp:DropDownList>
                        <asp:DropDownList ID="ddlMonth" runat="server"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbShopCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlShopCd" runat="server" OnSelectedIndexChanged="ddlShopCd_SelectedIndexChanged" AutoPostBack="true" Width="140"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbLineCd" runat="server"></asp:Label></th>
                    <td style="border-right: none;">
                        <asp:DropDownList ID="ddlEopFlag" runat="server" Width="70" OnSelectedIndexChanged="ddlEopFlag_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
                    </td>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="searchCombo" style="width:200px; font-size:12px;">
                                    <input type="text" ID="txtLineCd" runat="server" style="background-color:white; color:black;" readonly/>
                                    <asp:TextBox id="txtLineCdHidden" runat="server" style="display:none;"></asp:TextBox>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlEopFlag" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <th><asp:Label ID="lbDayNight" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDayNight" runat="server" Width="100"></asp:DropDownList>
                    </td>
                    <td class="al-r">
                        <%--<asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                        <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('Res11'); return false;" />
                        <asp:Button ID="btnFunctionCall" runat="server" Text="FunctionCall" OnClick="btnFunctionCall_Click"  style="display:none;"  />--%>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbPlanCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlPlanCd" runat="server" OnSelectedIndexChanged="ddlPlanCd_SelectedIndexChanged" AutoPostBack="true" Width="120"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbPlanDetailCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlPlanDetailCd" runat="server" Width="140"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlPlanCd" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <th><asp:Label ID="lbPartNo" runat="server"></asp:Label></th>
                    <td colspan="4">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="searchCombo2" style="width:430px; font-size:12px;">
                                    <input type="text" ID="txtPartNo" runat="server" style="background-color:white; color:black;" readonly/>
                                    <asp:TextBox id="txtPartNoHidden" runat="server" style="display:none;"></asp:TextBox>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="btnFunctionCall" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td class="al-r"></td>
                </tr>
            </table>
        </div>
        <div class="overflow_CT"><!-- 오버플로우 DIV 설정 -->
            <div class="graph_wrap mt14" style="height:300px; overflow:auto;" id="divChart">
                <asp:UpdatePanel ID="up2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td style="width:20%">
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

            <p class="sub_tit"></p><!--통계수치 테이블의 타이틀-->

            <div style="width:100%;">
                <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                    <ContentTemplate>
                        <%--<div id="realgrid_c_2" class="realgrid_c_2"></div>--%>
                        <div id="realgrid_c_2" class="realgrid_overflow"></div>
                        <table>
                            <tr>
                                <td>
                                    <div class="toolbar">
                                        <div id="gridPage" style="float:left" onselectstart="return false" ondragstart="return false"></div>
                                        &nbsp;
                                        <select id="current_page_value" onchange="setPaging(); return false;" style="font-size:12px; display:none; margin-top:10px; float:left" runat="server">
                                        </select>
                                        <input type="button" value="Set" id="btnSet" onclick="getCol('Res11'); return false;" style="display:none; margin-top:10px; float:left" />
                                        <div class="al-r total" ondragstart="return false">Total : <div id="rowCnt" class="f02" style="float:right"></div></div>
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
    </div>

</asp:Content>
