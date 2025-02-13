<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Qua98.aspx.cs" Inherits="HQCWeb.QualityMgt.Qua98.Qua98" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function fn_Validation() {
            if ($("#MainContent_ddlShopCd").val() == "") {
                alert("Shop Code를 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlType").val() == "") {
                alert("검색조건을 선택하세요.");
                return false;
                txtSearchComboHidden
            } else if ( ($("#MainContent_txtSearchComboHidden").val() == "" && $("#MainContent_ddlSearchType2").val() == "1") || ($("#MainContent_txtSearchBarcode").val() == "" && $("#MainContent_ddlSearchType2").val() == "2") ) {
                alert("검색 데이터를 입력하세요.");
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
        /* 다중콤보 시작 */
        // 검색어 콤보
        jQuery(document).ready(function ($) {

            $("#MainContent_txtSearchCombo").comboTree({
                source: [],
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true,
                onLoadAllChk: true,
                hidCon: "MainContent_txtSearchComboHidden"
            });

            $("#txtSearchValue").val(startBarcode); fn_Set();

            $("#type2").hide();
        });

        function fn_searchType_change(ddl) {
            if (ddl.value === '1') {
                $("#type1").show();
                $("#type2").hide();
            }
            else {
                $("#type1").hide();
                $("#type2").show();
            }
        }

        function fn_SearchSetStyle() {
            if ($("#divSearchSet").css("display") == "none") {
                $("#divSearchSet").show();
            }
            else {
                $("#divSearchSet").hide();
            }
        }

        function fn_Set() {
            $("#MainContent_txtSearchCombo").comboTree({
                source: [],
                comboReload: true
            });

            var txtSearch = $("#txtSearchValue").val();

            txtSearch = txtSearch.replace(/(?:\r\n|\r|\n)/g, ',');

            const arr = txtSearch.split(',');
            const arrTxtSearch = arr.filter((el, index) => arr.indexOf(el) === index);

            var tList = new Array();

            var iLen = 0;

            for (var i = 0; i < arrTxtSearch.length; i++) {
                if (arrTxtSearch[i] != "") {
                    iLen++;

                    var comboData = new Object();

                    comboData.id = arrTxtSearch[i];
                    comboData.title = arrTxtSearch[i];

                    tList.push(comboData);
                }
            }

            if (iLen > 50) {
                alert("검색어 셋팅 가능 숫자는 50개 입니다.");

                $("#MainContent_txtSearchCombo").comboTree({
                    source: [],
                    isMultiple: true,
                    cascadeSelect: false,
                    collapse: false,
                    selectAll: true,
                    onLoadAllChk: true
                });

                return false;
            } else {
                $("#MainContent_txtSearchCombo").comboTree({
                    source: tList,
                    isMultiple: true,
                    cascadeSelect: false,
                    collapse: false,
                    selectAll: true,
                    onLoadAllChk: true,
                    hidCon: "MainContent_txtSearchComboHidden"
                });

                $("#divSearchSet").hide();
            }
        }

        /* 다중콤보 끝 */

        var column1, field1, data1,
           column2, field2, data2,
           column3, field3, data3,
           column4, field4, data4,
           column5, field5, data5,
           column6, field6, data6,
           column7, field7, data7;

        var container, dataProvider, gridView;
        var startBarcode;

        //dataProvider, gridView 저장용
        var dP1, gV1,
            dP2, gV2,
            dP3, gV3,
            dP4, gV4,
            dP5, gV5,
            dP6, gV6,
            dP7, gV7;
            
        var tabNum = 1;//초기값 1

        //통합 엑셀 출력
        function fn_excelExport_override(menuId) {
            //fn_excelExport(menuId + '_' + tabNum);

            var excelType = true;
            var showProgress = true;
            var itemType = true;//숨겨진값 
            var pageType = true;//전체페이지 내보내기
            var indicator = 'default';
            var header = 'default';
            var footer = 'hidden';//아래 footer
            
            RealGrid.exportGrid({
                type: "excel",
                target: "local",
                fileName: menuId + ".xlsx",
                showProgress: showProgress,
                progressMessage: "Exporting....",
                indicator: indicator,
                header: header,
                footer: footer,
                compatibility: excelType,
                allItems: itemType,
                pagingAllItems: pageType,
                exportGrids: [
                    { grid: gV1, sheetName: $("#MainContent_lb1").text() },
                    { grid: gV2, sheetName: $("#MainContent_lb2").text() },
                    { grid: gV3, sheetName: $("#MainContent_lb3").text() },
                    { grid: gV4, sheetName: $("#MainContent_lb4").text() },
                    { grid: gV5, sheetName: $("#MainContent_lb5").text() }
                ]
            });
        }

        //탭에 따라 세팅 변경
        function getCol_override(menuId) {
            getCol(menuId + '_' + tabNum);
        }

        //탭 열기
        function openTab(evt, tabName) {
            var i, tabContent, tabButtons;

            // Hide all tab content
            tabContent = document.getElementsByClassName("grid-tab-content");
            for (i = 0; i < tabContent.length; i++) {
                tabContent[i].style.display = "none";
                tabContent[i].classList.remove("active");
            }

            // Remove the 'active' class from all buttons
            tabButtons = document.getElementsByClassName("grid-tab-button");
            for (i = 0; i < tabButtons.length; i++) {
                tabButtons[i].classList.remove("active");
            }

            // Show the current tab and add 'active' class to the button
            document.getElementById(tabName).style.display = "block";
            document.getElementById(tabName).classList.add("active");
            evt.currentTarget.classList.add("active");

            //탭번호로 그리드 변경
            updateGridReferences(tabName);
        }

        function updateGridReferences(tabName) {
            $(".rg-group-panel-message").css("top", "10px");//버그 수정을 위해 강제전환
            const tabNumber = tabName.replace('Tab', '');
            var tmpData = window['data' + tabNumber];
            dataProvider = window['dP' + tabNumber];
            gridView = window['gV' + tabNumber];
            tabNum = tabNumber;

            if (tmpData !== undefined)
                document.getElementById('rowCnt').innerHTML = tmpData.length;
            setPaging();
            $("#MainContent_txtTabCount").val(tabNumber);
        }

        // 그리드 생성
        function createGrid1(_val) {
            container = document.getElementById('realgrid1');
            dataProvider = new RealGrid.LocalDataProvider(false);
            gridView = new RealGrid.GridView(container);
            gridView.setDataSource(dataProvider);
            gridView.setColumns(column1);
            dataProvider.setFields(field1);
            dataProvider.setRows(data1);
            gridView.checkBar.visible = false;
            gridView.stateBar.visible = false;
            setContextMenu(gridView);
            setPaging();
            settingGrid1(_val); if (data1 != undefined && $("#MainContent_txtTabCount").val() == '1') document.getElementById('rowCnt').innerHTML = data1.length;
            dP1 = dataProvider;
            gV1 = gridView;
        }

        function settingGrid1(_val) {
            //PK 컬럼 클릭시 동작
            gridView.onCellClicked = function (grid, clickData) {

                if (clickData.cellType !== "data") {
                    return; // 데이터 셀이 아니면 이벤트 중단
                }

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

            var carTp = '';

            if(data1 !== undefined)
                carTp = dataProvider.getValue(0, "BSA_CAR_TYPE").substring(0, 4);

            //gridView.columnByName("COL_1").styleName = "grid-primary-column"
            gridView.columnByName("KEY_HID").visible = false;

            gridView.columnByName("BSA_LINE_CD").styleName = "string-column";
            gridView.columnByName("BSA_CAR_TYPE").styleName = "string-column";
            gridView.columnByName("BSA_COMPLETE_NO").styleName = "string-column";
            gridView.columnByName("BSA_SERIAL_NO").styleName = "string-column";

            if (carTp === 'HR00')
                gridView.columnByName("BSA_CASE_BARCODE").styleName = "string-column";
            else
                gridView.columnByName("BSA_CASE_BARCODE").visible = false;

            gridView.columnByName("BPA_LINE_CD").styleName = "string-column";
            gridView.columnByName("BPA_CAR_TYPE").styleName = "string-column";
            gridView.columnByName("BPA_COMPLETE_NO").styleName = "string-column";
            gridView.columnByName("BPA_SERIAL_NO").styleName = "string-column";
            gridView.columnByName("BMA_LINE_CD").styleName = "string-column";
            gridView.columnByName("BMA_CAR_TYPE").styleName = "string-column";
            gridView.columnByName("BMA_COMPLETE_NO").styleName = "string-column";
            gridView.columnByName("BMA_SERIAL_NO").styleName = "string-column";
            gridView.columnByName("CELL_COMPLETE_NO").styleName = "string-column";

            dataProvider.softDeleting = true;
            //gridView.displayOptions.fitStyle = "even";//그리드 채우기
            gridView.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gridView.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gridView.pasteOptions.enabled = false;//붙여넣기 금지
            gridView.groupPanel.visible = true;//그룹핑 활성화
            gridView.filterPanel.visible = true;//필터패널 활성화
            gridView.groupPanel.minHeight = 34;//그룹핑 영역 최소높이
        }

        // 그리드 생성
        function createGrid2(_val) {
            container = document.getElementById('realgrid2');
            dataProvider = new RealGrid.LocalDataProvider(false);
            gridView = new RealGrid.GridView(container);
            gridView.setDataSource(dataProvider);
            gridView.setColumns(column2);
            dataProvider.setFields(field2);
            dataProvider.setRows(data2);
            gridView.checkBar.visible = false;
            gridView.stateBar.visible = false;
            setContextMenu(gridView);
            setPaging();
            settingGrid2(_val); if (data2 != undefined && $("#MainContent_txtTabCount").val() == '2') document.getElementById('rowCnt').innerHTML = data2.length;
            dP2 = dataProvider;
            gV2 = gridView;
        }

        function settingGrid2(_val) {
            //PK 컬럼 클릭시 동작
            gridView.onCellClicked = function (grid, clickData) {

                if (clickData.cellType !== "data") {
                    return; // 데이터 셀이 아니면 이벤트 중단
                }
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

            //gridView.columnByName("COL_1").styleName = "grid-primary-column"
            gridView.columnByName("KEY_HID").visible = false;
            //gridView.columnByName("BSA_COMPLETE_NO").styleName = "string-column";
            //gridView.columnByName("BSA_SERIAL_NO").styleName = "string-column";
            gridView.columnByName("LINE_CD").styleName = "string-column";
            gridView.columnByName("CAR_TYPE").styleName = "string-column";
            gridView.columnByName("COMPLETE_NO").styleName = "string-column";
            gridView.columnByName("SERIAL_NO").styleName = "string-column";
            gridView.columnByName("STN_NM").styleName = "string-column";
            gridView.columnByName("PART_SERIAL_NO").styleName = "string-column";
            gridView.columnByName("DIFF_MSG").styleName = "string-column";
            
            dataProvider.softDeleting = true;
            //gridView.displayOptions.fitStyle = "even";//그리드 채우기
            gridView.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gridView.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gridView.pasteOptions.enabled = false;//붙여넣기 금지
            gridView.groupPanel.visible = true;//그룹핑 활성화
            gridView.filterPanel.visible = true;//필터패널 활성화
            gridView.groupPanel.minHeight = 34;//그룹핑 영역 최소높이
        }

        // 그리드 생성
        function createGrid3(_val) {
            container = document.getElementById('realgrid3');
            dataProvider = new RealGrid.LocalDataProvider(false);
            gridView = new RealGrid.GridView(container);
            gridView.setDataSource(dataProvider);
            gridView.setColumns(column3);
            dataProvider.setFields(field3);
            dataProvider.setRows(data3);
            gridView.checkBar.visible = false;
            gridView.stateBar.visible = false;
            setContextMenu(gridView);
            setPaging();
            settingGrid3(_val); if (data3 != undefined && $("#MainContent_txtTabCount").val() == '3') document.getElementById('rowCnt').innerHTML = data3.length;
            dP3 = dataProvider;
            gV3 = gridView;
        }

        function settingGrid3(_val) {
            //PK 컬럼 클릭시 동작
            gridView.onCellClicked = function (grid, clickData) {

                if (clickData.cellType !== "data") {
                    return; // 데이터 셀이 아니면 이벤트 중단
                }
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

            //gridView.columnByName("COL_1").styleName = "grid-primary-column"
            if(gridView.columnByName("KEY_HID") != undefined)
                gridView.columnByName("KEY_HID").visible = false;
            //gridView.columnByName("BSA_COMPLETE_NO").styleName = "string-column";
            //gridView.columnByName("BSA_SERIAL_NO").styleName = "string-column";
            gridView.columnByName("LINE_CD").styleName = "string-column";
            gridView.columnByName("CAR_TYPE").styleName = "string-column";
            gridView.columnByName("COMPLETE_NO").styleName = "string-column";
            gridView.columnByName("SERIAL_NO").styleName = "string-column";
            gridView.columnByName("STN_NM").styleName = "string-column";

            gridView.columnByName("ITEM_NM").styleName = "string-column";
            gridView.columnByName("ITEM_VALUE").styleName = "string-column";


            //for (var i = 0; i < 300; i++) {
            //    if (gridView.columnByName("ITEM_NM_" + (i + 1).toString().padStart(2, '0')) != undefined)
            //        gridView.columnByName("ITEM_NM_" + (i + 1).toString().padStart(2, '0')).styleName = "string-column";
            //    else
            //        break;
            //}

            dataProvider.softDeleting = true;
            //gridView.displayOptions.fitStyle = "even";//그리드 채우기
            gridView.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gridView.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gridView.pasteOptions.enabled = false;//붙여넣기 금지
            gridView.groupPanel.visible = true;//그룹핑 활성화
            gridView.filterPanel.visible = true;//필터패널 활성화
            gridView.groupPanel.minHeight = 34;//그룹핑 영역 최소높이
        }

        // 그리드 생성
        function createGrid4(_val) {
            container = document.getElementById('realgrid4');
            dataProvider = new RealGrid.LocalDataProvider(false);
            gridView = new RealGrid.GridView(container);
            gridView.setDataSource(dataProvider);
            gridView.setColumns(column4);
            //field 넣기 전 포맷변경(엑셀 문제)
            field4.forEach(item => {
                if (item.fieldName == 'TORQUE' || item.fieldName == 'TORQUE_MAX' || item.fieldName == 'TORQUE_MIN') {
                    item.dataType = 'number'; item.subType = 'unum'
                } else if (item.fieldName == 'ANGLE_VALUE' || item.fieldName == 'ANGLE_MIN' || item.fieldName == 'ANGLE_MAX' || item.fieldName == 'WORK_SEQ'
                    || item.fieldName == 'R_ANGLE' || item.fieldName == 'R_ANGLE_MIN' || item.fieldName == 'R_ANGLE_MAX'
                    ) {
                    item.dataType = 'number'; item.subType = 'int'
                }
            });

            //숫자 포맷 변경
            gridView.setFormatOptions({ numberFormat: '#,##0' });

            dataProvider.setFields(field4);
            dataProvider.setRows(data4);
            gridView.checkBar.visible = false;
            gridView.stateBar.visible = false;
            setContextMenu(gridView);
            setPaging();
            settingGrid4(_val); if (data4 != undefined && $("#MainContent_txtTabCount").val() == '4') document.getElementById('rowCnt').innerHTML = data4.length;
            dP4 = dataProvider;
            gV4 = gridView;
        }

        function settingGrid4(_val) {
            //PK 컬럼 클릭시 동작
            gridView.onCellClicked = function (grid, clickData) {

                if (clickData.cellType !== "data") {
                    return; // 데이터 셀이 아니면 이벤트 중단
                }
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

            //gridView.columnByName("COL_1").styleName = "grid-primary-column"
            gridView.columnByName("KEY_HID").visible = false;
            //gridView.columnByName("BSA_COMPLETE_NO").styleName = "string-column";
            //gridView.columnByName("BSA_SERIAL_NO").styleName = "string-column";
            gridView.columnByName("LINE_CD").styleName = "string-column";
            gridView.columnByName("CAR_TYPE").styleName = "string-column";
            gridView.columnByName("COMPLETE_NO").styleName = "string-column";
            gridView.columnByName("SERIAL_NO").styleName = "string-column";
            gridView.columnByName("STN_NM").styleName = "string-column";

            gridView.columnByName("WORK_NM").styleName = "string-column";
            gridView.columnByName("DEV_NM").styleName = "string-column";
            gridView.columnByName("REASON").styleName = "string-column";

            gridView.columnByName("ANGLE_VALUE").styleName = "number-column";
            gridView.columnByName("ANGLE_MIN").styleName = "number-column";
            gridView.columnByName("ANGLE_MAX").styleName = "number-column";
            gridView.columnByName("TORQUE_VALUE").styleName = "number-column";
            gridView.columnByName("TORQUE_MIN").styleName = "number-column";
            gridView.columnByName("TORQUE_MAX").styleName = "number-column";
            gridView.columnByName("R_ANGLE").styleName = "number-column";
            gridView.columnByName("R_ANGLE_MIN").styleName = "number-column";
            gridView.columnByName("R_ANGLE_MAX").styleName = "number-column";

            gridView.columnByName("WORK_SEQ").styleName = "number-column";

            dataProvider.softDeleting = true;
            //gridView.displayOptions.fitStyle = "even";//그리드 채우기
            gridView.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gridView.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gridView.pasteOptions.enabled = false;//붙여넣기 금지
            gridView.groupPanel.visible = true;//그룹핑 활성화
            gridView.filterPanel.visible = true;//필터패널 활성화
            gridView.groupPanel.minHeight = 34;//그룹핑 영역 최소높이
        }

        // 그리드 생성
        function createGrid5(_val) {
            container = document.getElementById('realgrid5');
            dataProvider = new RealGrid.LocalDataProvider(false);
            gridView = new RealGrid.GridView(container);
            gridView.setDataSource(dataProvider);
            gridView.setColumns(column5);
            dataProvider.setFields(field5);
            dataProvider.setRows(data5);
            gridView.checkBar.visible = false;
            gridView.stateBar.visible = false;
            setContextMenu(gridView);
            setPaging();
            settingGrid5(_val); if (data5 != undefined && $("#MainContent_txtTabCount").val() == '5') document.getElementById('rowCnt').innerHTML = data5.length;
            dP5 = dataProvider;
            gV5 = gridView;
        }

        function settingGrid5(_val) {
            //PK 컬럼 클릭시 동작
            gridView.onCellClicked = function (grid, clickData) {

                if (clickData.cellType !== "data") {
                    return; // 데이터 셀이 아니면 이벤트 중단
                }
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

            //gridView.columnByName("COL_1").styleName = "grid-primary-column"
            gridView.columnByName("KEY_HID").visible = false;
            //gridView.columnByName("BSA_COMPLETE_NO").styleName = "string-column";
            //gridView.columnByName("BSA_SERIAL_NO").styleName = "string-column";
            gridView.columnByName("LINE_CD").styleName = "string-column";
            gridView.columnByName("CAR_TYPE").styleName = "string-column";
            gridView.columnByName("COMPLETE_NO").styleName = "string-column";
            gridView.columnByName("SERIAL_NO").styleName = "string-column";
            gridView.columnByName("STN_NM").styleName = "string-column";

            gridView.columnByName("CAUSE1").styleName = "string-column";
            gridView.columnByName("CAUSE2").styleName = "string-column";
            gridView.columnByName("CAUSE3").styleName = "string-column";
            gridView.columnByName("CAUSE4").styleName = "string-column";
            gridView.columnByName("CAUSE5").styleName = "string-column";
            gridView.columnByName("MEMO").styleName = "string-column";

            dataProvider.softDeleting = true;
            //gridView.displayOptions.fitStyle = "even";//그리드 채우기
            gridView.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gridView.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gridView.pasteOptions.enabled = false;//붙여넣기 금지
            gridView.groupPanel.visible = true;//그룹핑 활성화
            gridView.filterPanel.visible = true;//필터패널 활성화
            gridView.groupPanel.minHeight = 34;//그룹핑 영역 최소높이
        }

        // 그리드 생성
        function createGrid6(_val) {
            if (!gV6) {
                container = document.getElementById('realgrid6');
                dP6 = new RealGrid.LocalDataProvider(false);
                gV6 = new RealGrid.GridView(container);
            }
            gV6.setDataSource(dP6);
            gV6.setColumns(column6);
            dP6.setFields(field6);
            dP6.setRows(data6);
            gV6.checkBar.visible = false;
            gV6.stateBar.visible = false;
            setContextMenu(gV6);
            setPaging();
            settingGrid6(_val); if (data6 != undefined && $("#MainContent_txtTabCount").val() == '6') document.getElementById('rowCnt').innerHTML = data6.length;
        }

        function settingGrid6(_val) {

            if (_val != undefined) {
                gV6.setFixedOptions({
                    colCount: _val
                });
            }

            gV6.columnByName("KEY_HID").visible = false;
            gV6.columnByName("LINE_CD").styleName = "string-column";
            gV6.columnByName("CAR_TYPE").styleName = "string-column";
            gV6.columnByName("STN_CD").styleName = "string-column";

            gV6.columnByName("D_SYMPTOM").styleName = "string-column";
            gV6.columnByName("D_REASON").styleName = "string-column";
            gV6.columnByName("D_RESP").styleName = "string-column";
            gV6.columnByName("REWORK_MSG").styleName = "string-column";

            dP6.softDeleting = true;
            //gV6.displayOptions.fitStyle = "even";//그리드 채우기
            gV6.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gV6.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gV6.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gV6.pasteOptions.enabled = false;//붙여넣기 금지
            gV6.groupPanel.visible = true;//그룹핑 활성화
            gV6.filterPanel.visible = true;//필터패널 활성화
            gV6.groupPanel.minHeight = 34;//그룹핑 영역 최소높이
        }

        // 그리드 생성
        function createGrid7(_val) {
            if (!gV7) {
                container = document.getElementById('realgrid7');
                dP7 = new RealGrid.LocalDataProvider(false);
                gV7 = new RealGrid.GridView(container);
            }
            gV7.setDataSource(dP7);
            gV7.setColumns(column7);
            dP7.setFields(field7);
            dP7.setRows(data7);
            gV7.checkBar.visible = false;
            gV7.stateBar.visible = false;
            setContextMenu(gV7);
            setPaging();
            settingGrid7(_val); if (data7 != undefined && $("#MainContent_txtTabCount").val() == '7') document.getElementById('rowCnt').innerHTML = data7.length;
        }

        function settingGrid7(_val) {

            if (_val != undefined) {
                gV7.setFixedOptions({
                    colCount: _val
                });
            }

            gV7.columnByName("KEY_HID").visible = false;
            gV7.columnByName("LINE_CD").styleName = "string-column";

            gV7.columnByName("D_SYMPTOM").styleName = "string-column";
            gV7.columnByName("D_REASON").styleName = "string-column";
            gV7.columnByName("D_RESP").styleName = "string-column";
            gV7.columnByName("REWORK_MSG").styleName = "string-column";

            dP7.softDeleting = true;
            //gV6.displayOptions.fitStyle = "even";//그리드 채우기
            gV7.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gV7.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gV7.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gV7.pasteOptions.enabled = false;//붙여넣기 금지
            gV7.groupPanel.visible = true;//그룹핑 활성화
            gV7.filterPanel.visible = true;//필터패널 활성화
            gV7.groupPanel.minHeight = 34;//그룹핑 영역 최소높이
        }
    </script>

    <style>
        .grid-primary-column {
            font-weight: bold;
            text-decoration: underline; cursor: pointer;
        }
        
        .searchCombo{
            text-align: center;
            z-index : 9999;
            top : 11%;
            position : absolute; 
        }

        .string-column {
            text-align: left;
        }
    </style>

    <asp:HiddenField ID="hidWidth" runat="server" Value="500" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="500" />
   
    <div class="contents" tabindex="0">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">Qua98</asp:Label></h3>
            <div class="al-r">
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport_override('Qua98'); return false;" />
            </div>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:60px;">
                    <col style="width:120px;">
                    <col style="width:60px;">
                    <col style="width:60px;">
                    <col style="width:100px;">
                    <col style="width:400px;">
                    <col>
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbShopCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlShopCd" runat="server" Width="100"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbSearchType" runat="server"></asp:Label></th>
                    <td style="border-right: none;">
                        <asp:DropDownList ID="ddlSearchType2" runat="server" onchange="fn_searchType_change(this)" AutoPostBack="false"></asp:DropDownList>
                    </td>
                    <td style="border-right: none;">
                        <asp:DropDownList ID="ddlSearchType" runat="server" ></asp:DropDownList>
                        <asp:TextBox id="txtTabCount" runat="server" style="display:none"></asp:TextBox>
                    </td>
                    <td id="type1">
                        <div class="searchCombo" style="width:270px; font-size:12px;">
                            <asp:TextBox ID="txtSearchCombo" runat="server" ReadOnly="true" style="background-color:white; color:black;"></asp:TextBox>
                            <asp:TextBox id="txtSearchComboHidden" runat="server" style="display:none;"></asp:TextBox>
                        </div>
                        <img src="/img/gnb_on_arrow.png" style="float:left; padding-left:270px;" onclick="javascript:fn_SearchSetStyle();" />
                        <div id="divSearchSet" class="searchCombo" style="display:none;">
                            <textarea id="txtSearchValue" rows="5" style="width:270px;"></textarea>
                            <br />
                            <input type="button" value="적용" onclick="javascript:fn_Set();" />
                            <input type="button" value="닫기" onclick="javascript:fn_SearchSetStyle();" />
                        </div>
                    </td>
                    <td id="type2">
                        <asp:TextBox id="txtSearchBarcode" runat="server" Width="270"></asp:TextBox>
                    </td>
                    <td class="al-r">
                    </td>
                </tr>
            </table>
        </div>
        <br />

        <!-- 탭 설정 -->
        <div class="grid-tab-container">
            <div class="grid-tabs-wrap">
	            <ul class="tabs">
		            <li class="tab" ><p class="grid-tab-button active" onclick="javascript:openTab(event, 'Tab1');"><asp:Label ID="lb1" runat="server"></asp:Label></p></li>
	                <li class="tab" ><p class="grid-tab-button " onclick="javascript:openTab(event, 'Tab2');"><asp:Label ID="lb2" runat="server"></asp:Label></p></li>
                    <li class="tab" ><p class="grid-tab-button " onclick="javascript:openTab(event, 'Tab3');"><asp:Label ID="lb3" runat="server"></asp:Label></p></li>
	                <li class="tab" ><p class="grid-tab-button " onclick="javascript:openTab(event, 'Tab4');"><asp:Label ID="lb4" runat="server"></asp:Label></p></li>
                    <li class="tab" ><p class="grid-tab-button " onclick="javascript:openTab(event, 'Tab5');"><asp:Label ID="lb5" runat="server"></asp:Label></p></li>
                    <li class="tab" ><p class="grid-tab-button " onclick="javascript:openTab(event, 'Tab6');"><asp:Label ID="lb6" runat="server"></asp:Label></p></li>
                    <li class="tab" ><p class="grid-tab-button " onclick="javascript:openTab(event, 'Tab7');"><asp:Label ID="lb7" runat="server"></asp:Label></p></li>
	            </ul>
            </div>

            <div class="grid-tab-content active" id="Tab1">
                <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                     <ContentTemplate>
                        <div id="realgrid1" class="realgrid_2"></div>
                     </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="grid-tab-content" id="Tab2">
                <asp:UpdatePanel runat="server" ID="up2" UpdateMode="Conditional">
                     <ContentTemplate>
                        <div id="realgrid2" class="realgrid_2"></div>
                     </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="grid-tab-content" id="Tab3">
                <asp:UpdatePanel runat="server" ID="up3" UpdateMode="Conditional">
                     <ContentTemplate>
                        <div id="realgrid3" class="realgrid_2"></div>
                     </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="grid-tab-content" id="Tab4">
                <asp:UpdatePanel runat="server" ID="up4" UpdateMode="Conditional">
                     <ContentTemplate>
                        <div id="realgrid4" class="realgrid_2"></div>
                     </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="grid-tab-content" id="Tab5">
                <asp:UpdatePanel runat="server" ID="up5" UpdateMode="Conditional">
                     <ContentTemplate>
                        <div id="realgrid5" class="realgrid_2"></div>
                     </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="grid-tab-content" id="Tab6">
                <asp:UpdatePanel runat="server" ID="up6" UpdateMode="Conditional">
                     <ContentTemplate>
                        <div id="realgrid6" class="realgrid_2"></div>
                     </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="grid-tab-content" id="Tab7">
                <asp:UpdatePanel runat="server" ID="up7" UpdateMode="Conditional">
                     <ContentTemplate>
                        <div id="realgrid7" class="realgrid_2"></div>
                     </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <asp:UpdatePanel runat="server" ID="up8" UpdateMode="Conditional">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <div class="toolbar">
                                <div id="gridPage" style="float:left" onselectstart="return false" ondragstart="return false"></div>
                                &nbsp;
                                <select id="current_page_value" onchange="setPaging(); return false;" style="font-size:12px; display:none; margin-top:10px; float:left" runat="server">
                                </select>
                                <input type="button" value="Set" id="btnSet" onclick="getCol_override('Qua98'); return false;" style="display:none; margin-top:10px; float:left" />
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

</asp:Content>