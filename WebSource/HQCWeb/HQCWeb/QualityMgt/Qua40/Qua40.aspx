<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Qua40.aspx.cs" Inherits="HQCWeb.QualityMgt.Qua40.Qua40" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        function fn_Validation() {

            if ($("#MainContent_ddlShopCd").val() == "" && $("#MainContent_txtTabCount").val() != "4") {
                alert("Shop Code를 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlLineCd").val() == "" && $("#MainContent_txtTabCount").val() != "4") {
                alert("라인을 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlLineCd").val() == "" && $("#MainContent_txtTabCount").val() == "4") {
                alert("품번을 선택하세요.");
                return false;
            } else if (($("#MainContent_txtBarcode").val() != "" && $("#MainContent_txtSerialNo").val() != "" && $("#MainContent_txtPartSerialNo").val() != "" && $("#MainContent_txtPartBarcodeNo").val() != "") ||
                       ($("#MainContent_txtBarcode").val() != "" && $("#MainContent_txtSerialNo").val() != "" && $("#MainContent_txtPartSerialNo").val() != "" && $("#MainContent_txtPartBarcodeNo").val() == "") ||
                       ($("#MainContent_txtBarcode").val() != "" && $("#MainContent_txtSerialNo").val() != "" && $("#MainContent_txtPartSerialNo").val() == "" && $("#MainContent_txtPartBarcodeNo").val() != "") ||
                       ($("#MainContent_txtBarcode").val() != "" && $("#MainContent_txtSerialNo").val() == "" && $("#MainContent_txtPartSerialNo").val() != "" && $("#MainContent_txtPartBarcodeNo").val() != "") ||
                       ($("#MainContent_txtBarcode").val() == "" && $("#MainContent_txtSerialNo").val() != "" && $("#MainContent_txtPartSerialNo").val() != "" && $("#MainContent_txtPartBarcodeNo").val() != "") ||
                       ($("#MainContent_txtBarcode").val() != "" && $("#MainContent_txtSerialNo").val() != "" && $("#MainContent_txtPartSerialNo").val() == "" && $("#MainContent_txtPartBarcodeNo").val() == "") ||
                       ($("#MainContent_txtBarcode").val() != "" && $("#MainContent_txtSerialNo").val() == "" && $("#MainContent_txtPartSerialNo").val() == "" && $("#MainContent_txtPartBarcodeNo").val() != "") ||
                       ($("#MainContent_txtBarcode").val() == "" && $("#MainContent_txtSerialNo").val() == "" && $("#MainContent_txtPartSerialNo").val() != "" && $("#MainContent_txtPartBarcodeNo").val() != "") ||
                       ($("#MainContent_txtBarcode").val() == "" && $("#MainContent_txtSerialNo").val() != "" && $("#MainContent_txtPartSerialNo").val() != "" && $("#MainContent_txtPartBarcodeNo").val() == "") 
                 ) {
                alert("바코드, 시리얼, 부품바코드는 동시에 검색할 수 없습니다.");
                return false;
            } else {
                // 날짜를 가져옵니다
                var fromDateStr = $("#MainContent_txtFromDt").val();
                var toDateStr = $("#MainContent_txtToDt").val();
                var wctFlag = $("#MainContent_ddlWctCd").val();

                // 날짜 유효성 검사
                if ((!fromDateStr || !toDateStr) && wctFlag == "H") {
                    alert("날짜 데이터가 비어있습니다.");
                    return false;
                } else {
                    // 문자열 값을 Date 객체로 변환
                    var fromDate = new Date(fromDateStr);
                    var toDate = new Date(toDateStr);

                    // 변환된 날짜 유효성 확인
                    if (isNaN(fromDate) || isNaN(toDate)) {
                        alert("날짜 형식이 올바르지 않습니다.");
                        return false;
                    }

                    // 날짜 차이 계산
                    var timeDifference = toDate.getTime() - fromDate.getTime();
                    var dayDifference = timeDifference / (1000 * 60 * 60 * 24);

                    // 종료일이 시작일보다 이전인지 확인
                    if (toDate < fromDate) {
                        alert("시작일보다 종료일이 작습니다.");
                        return false;
                    }

                    // 1개월 이내인지 확인 (1개월을 30일로 계산)
                    if (dayDifference > 30) {
                        alert("최대 조회 기간은 1개월입니다.");
                        return false;
                    }
                }

                fn_WatingCall();
                return true;
            }
            return true;
        }

        function fn_Register() {
            fn_OpenPop('/QualityMgt/Qua40/Qua40_p03.aspx', $("#MainContent_hidWidth").val(), "670");
        }

        function fn_Modify(pkCode,seq) {
            // 탭에 따라 다른 팝업 호출
            if(seq === 2)
                fn_PostOpenPop(pkCode, 'QualityMgt/Qua40/Qua40_p01.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
            else if (seq === 3)
                fn_PostOpenPop(pkCode, 'QualityMgt/Qua40/Qua40_p02.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
            else if (seq === 4)
                fn_PostOpenPop(pkCode, 'QualityMgt/Qua40/Qua40_p04.aspx', $("#MainContent_hidWidth").val(), "670");
        }

        function fn_Delete() {
            var tabNum = $("#MainContent_txtTabCount").val();
            var pkString = '';
            var rows = gridView.getCheckedRows();

            if (rows.length > 0) {

                if (rows.length > 100) {
                    alert("100건 이하의 데이터만 일괄삭제 가능합니다 : 현재 " + rows.length + "건 ");
                    return false;
                }

                for (var i in rows) {
                    var data = dataProvider.getJsonRow(rows[i]);

                    if (data.DIV == "삭제") {
                        if (tabNum === "2")
                            alert("이미 삭제된 데이터가 존재합니다 : " + data.PART_SERIAL_NO);
                        else
                            alert("이미 삭제된 데이터가 존재합니다 : " + data.SERIAL_NO_C);
                        return false;
                    }
                    else if (data.BARCODE_FLAG == "N") {
                        alert("단품 조회의 경우에만 일괄삭제 가능합니다.");
                        return false;
                    }

                    pkString += data.KEY_HID;
                    pkString += '|';
                }

                pkString = pkString.substring(0, pkString.length - 1);


                if (tabNum === "2") fn_PostOpenPop(pkString, 'QualityMgt/Qua40/Qua40_p05.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());

                else fn_PostOpenPop(pkString, 'QualityMgt/Qua40/Qua40_p06.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
            }

        }

        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }

        var column1, field1, data1,
           column2, field2, data2,
           column3, field3, data3,
           column4, field4, data4,
           column5, field5, data5;

        var container, dataProvider, gridView;
        var startBarcode;

        //dataProvider, gridView 저장용
        var dP1, gV1,
            dP2, gV2,
            dP3, gV3,
            dP4, gV4,
            dP5, gV5;
        var cShop, cLine, cPart;

        var tabNum = 1;//초기값 1
        let btnNewAuth;// 버튼 권한 저장
        let btnDeleteAuth;// 버튼 권한 저장

        /* 다중콤보 시작 */

        //콤보박스 초기화
        jQuery(document).ready(function ($) {
            fn_Set_Shop();
            fn_Set_Line();
            fn_Set_Part();

            btnNewAuth = $("#MainContent_btnNew").is(':visible');
            $("#MainContent_btnNew").hide();

            btnDeleteAuth = $("#MainContent_btnDeleteNew").is(':visible');
            $("#MainContent_btnDeleteNew").hide();

            //PART 조건 숨기기
            $('#MainContent_lbPartNo').parent().hide();
            $('#divPartNo').parent().hide();

            //결과 숨기기
            $('#MainContent_lbResultCd').parent().hide();
            $('#MainContent_ddlResultCd').parent().hide();

            //부품바코드 숨기기
            $('#MainContent_lbPartSerialNo').parent().hide();
            $('#MainContent_txtPartSerialNo').parent().hide();

            //부품바코드 숨기기
            $('#MainContent_lbPartBarcodeNo').parent().hide();
            $('#MainContent_txtPartBarcodeNo').parent().hide();

            //하위레벨체크 숨기기
            $('#MainContent_lbDownLevel').parent().hide();
            $('#MainContent_rdNo').parent().parent().hide();

            $('#tdGap1').hide();
            $('#tdGap2').hide();

            //라벨 설정
            $("#spanTabLabel").text("(최대 조회 기간은 1개월 입니다.)")

        });

        //Shop코드 재설정
        function fn_Set_Shop() {
            $("#MainContent_txtShopCd").comboTree({
                source: cShop,
                comboReload: true,
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true,
                onLoadAllChk: true,
                valueChange: true,
                hidCon: "MainContent_txtShopCdHidden",
                functionCall: true,                    // 콤보값 선택시 자바스크립스 함수 호출 여부
                functionCallFunc: "fn_Shop_Checked"
            });
        }

        //Shop코드 onchange
        function fn_Shop_Checked() {
            $("#MainContent_btnFunctionCall").click();
        }

        //부품코드 재설정
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

        //Line코드 onchange
        function fn_Line_Checked() {
            $("#MainContent_btnFunctionCall_2").click();
        }

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

        /* 다중콤보 끝 */

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

        // 탭에 따라 기타사항 변경
        function updateGridReferences(tabName) {
            $(".rg-group-panel-message").css("top", "10px");//버그 수정을 위해 강제전환
            const tabNumber = tabName.replace('Tab', '');
            var tmpData = window['data' + tabNumber];
            dataProvider = window['dP' + tabNumber];
            gridView = window['gV' + tabNumber];
            tabNum = tabNumber;

            if (tmpData !== undefined)
                document.getElementById('rowCnt').innerHTML = tmpData.length;

            if (tabNumber === "4") {
                if(btnNewAuth)
                    $("#MainContent_btnNew").show();
                // SHOP 조건 숨기기
                $('#MainContent_lbShopCd').parent().hide(); 
                $('#divShopCd').parent().hide();

                //LINE 조건 숨기기
                $('#MainContent_lbLineCd').parent().hide();
                $('#MainContent_UpdatePanel1').parent().hide();
                $('#MainContent_ddlEopFlag').parent().hide();

                //PART 조건 보이기
                $('#MainContent_lbPartNo').parent().show();
                $('#divPartNo').parent().show();

                //결과 보이기
                $('#MainContent_lbResultCd').parent().show();
                $('#MainContent_ddlResultCd').parent().show();

                $('#MainContent_txtSerialNo').parent().attr("colspan", 6);

                $("#MainContent_btnDeleteNew").hide();

                //부품바코드 숨기기
                $('#MainContent_lbPartSerialNo').parent().hide();
                $('#MainContent_txtPartSerialNo').parent().hide();

                //반제품 부품바코드 숨기기
                $('#MainContent_lbPartBarcodeNo').parent().hide();
                $('#MainContent_txtPartBarcodeNo').parent().hide();

                //하위레벨체크 숨기기
                $('#MainContent_lbDownLevel').parent().hide();
                $('#MainContent_rdNo').parent().parent().hide();

                $('#tdGap1').hide();
                $('#tdGap2').hide();
            }
            else {
                $("#MainContent_btnNew").hide();
                // SHOP 조건 보이기
                $('#MainContent_lbShopCd').parent().show();
                $('#divShopCd').parent().show();

                // LINE 조건 보이기
                $('#MainContent_lbLineCd').parent().show(); 
                $('#MainContent_UpdatePanel1').parent().show();
                $('#MainContent_ddlEopFlag').parent().show();

                //PART 조건 숨기기
                $('#MainContent_lbPartNo').parent().hide();
                $('#divPartNo').parent().hide();

                //결과 숨기기
                $('#MainContent_lbResultCd').parent().hide();
                $('#MainContent_ddlResultCd').parent().hide();
                
                if (tabNumber === "2" || tabNumber === "3") {
                    if (btnDeleteAuth)
                        $("#MainContent_btnDeleteNew").show();

                    if (tabNumber === "2") {
                        //부품바코드 보이기
                        $('#MainContent_lbPartSerialNo').parent().show();
                        $('#MainContent_txtPartSerialNo').parent().show();

                        //반제품 부품바코드 숨기기
                        $('#MainContent_lbPartBarcodeNo').parent().hide();
                        $('#MainContent_txtPartBarcodeNo').parent().hide();

                        //하위레벨체크 숨기기
                        $('#MainContent_lbDownLevel').parent().hide();
                        $('#MainContent_rdNo').parent().parent().hide();

                        $('#tdGap1').hide();
                        $('#tdGap2').hide();
                    }
                    else {
                        //부품바코드 숨기기
                        $('#MainContent_lbPartSerialNo').parent().hide();
                        $('#MainContent_txtPartSerialNo').parent().hide();

                        //반제품 부품바코드 보이기
                        $('#MainContent_lbPartBarcodeNo').parent().show();
                        $('#MainContent_txtPartBarcodeNo').parent().show();

                        //하위레벨체크 보이기
                        $('#MainContent_lbDownLevel').parent().show();
                        $('#MainContent_rdNo').parent().parent().show();

                        $('#tdGap1').show();
                        $('#tdGap2').show();
                    }

                    $('#MainContent_txtSerialNo').parent().attr("colspan", 1);
                }
                else {
                    $("#MainContent_btnDeleteNew").hide();

                    //부품바코드 숨기기
                    $('#MainContent_lbPartSerialNo').parent().hide();
                    $('#MainContent_txtPartSerialNo').parent().hide();

                    //반제품 부품바코드 숨기기
                    $('#MainContent_lbPartBarcodeNo').parent().hide();
                    $('#MainContent_txtPartBarcodeNo').parent().hide();

                    //하위레벨체크 숨기기
                    $('#MainContent_lbDownLevel').parent().hide();
                    $('#MainContent_rdNo').parent().parent().hide();

                    $('#MainContent_txtSerialNo').parent().attr("colspan", 4);

                    $('#tdGap1').hide();
                    $('#tdGap2').hide();
                }
            }

            setPaging();
            $("#MainContent_txtTabCount").val(tabNumber);
        }

        /* 그리드 시작 */

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
                    fn_Modify(value,1);
                }
            }

            if (_val != undefined) {
                gridView.setFixedOptions({
                    colCount: _val
                });
            }

            gridView.columnByName("KEY_HID").visible = false;
            gridView.columnByName("LINE_CD").styleName = "string-column";
            gridView.columnByName("CAR_TYPE").styleName = "string-column";
            gridView.columnByName("STN_CD").styleName = "string-column";

            gridView.columnByName("D_SYMPTOM").styleName = "string-column";
            gridView.columnByName("D_REASON").styleName = "string-column";
            gridView.columnByName("D_RESP").styleName = "string-column";
            gridView.columnByName("REWORK_MSG").styleName = "string-column";

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
            //gridView.checkBar.visible = false;
            gridView.stateBar.visible = false;
            gridView.setCheckBar({ useImages: true });
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

                if (clickData.column == 'PART_SERIAL_NO') {
                    var current = gridView.getCurrent();
                    var value = dataProvider.getValue(current.dataRow, "KEY_HID");
                    var chk = dataProvider.getValue(current.dataRow, "DIV");
                    //if (chk == "삭제") {
                    //    alert("구분이 등록인 항목만 제품 바코드 번호를 수정할 수 있습니다.")
                    //    return false
                    //}
                    //else {
                    //    fn_Modify(value, 2);
                    //}
                    fn_Modify(value, 2);
                }
                else if (clickData.cellType == "check") {
                    var current = gridView.getCurrent();
                    var chk = dataProvider.getValue(current.dataRow, "DIV");
                    if (chk == "삭제") {
                        alert("이미 삭제된 데이터는 선택할 수 없습니다.")
                        return false
                    }
                }
            }

            if (_val != undefined) {
                gridView.setFixedOptions({
                    colCount: _val
                });
            }

            gridView.columnByName("KEY_HID").visible = false;
            gridView.columnByName("BARCODE_FLAG").visible = false;
            gridView.columnByName("LINE_CD").styleName = "string-column";
            gridView.columnByName("PART_SERIAL_NO").styleName = "grid-primary-column"

            gridView.columnByName("D_SYMPTOM").styleName = "string-column";
            gridView.columnByName("D_REASON").styleName = "string-column";
            gridView.columnByName("D_RESP").styleName = "string-column";
            gridView.columnByName("REWORK_MSG").styleName = "string-column";
            
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
            //gridView.checkBar.visible = false;
            gridView.stateBar.visible = false;
            gridView.setCheckBar({ useImages: true });
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

                if (clickData.column == 'SERIAL_NO_C') {
                    var current = gridView.getCurrent();
                    var value = dataProvider.getValue(current.dataRow, "KEY_HID");
                    var chk = dataProvider.getValue(current.dataRow, "DIV");
                    //if (chk == "삭제") {
                    //    alert("구분이 등록인 항목만 수정할 수 있습니다.")
                    //    return false
                    //}
                    //else {
                    //    fn_Modify(value, 3);
                    //}
                    fn_Modify(value, 3);
                }
            }

            if (_val != undefined) {
                gridView.setFixedOptions({
                    colCount: _val
                });
            }

            gridView.columnByName("KEY_HID").visible = false;
            gridView.columnByName("BARCODE_FLAG").visible = false;
            gridView.columnByName("LINE_CD").styleName = "string-column";
            gridView.columnByName("SERIAL_NO_C").styleName = "grid-primary-column"

            gridView.columnByName("D_SYMPTOM").styleName = "string-column";
            gridView.columnByName("D_REASON").styleName = "string-column";
            gridView.columnByName("D_RESP").styleName = "string-column";
            gridView.columnByName("REWORK_MSG").styleName = "string-column";

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

                if (clickData.column == 'LOG_SEQ') {
                    var current = gridView.getCurrent();
                    var value = dataProvider.getValue(current.dataRow, "KEY_HID");
                    fn_Modify(value,4);
                }
            }

            if (_val != undefined) {
                gridView.setFixedOptions({
                    colCount: _val
                });
            }

            gridView.columnByName("KEY_HID").visible = false;
            gridView.columnByName("LOG_SEQ").styleName = "grid-primary-column"

            gridView.columnByName("D_SYMPTOM").styleName = "string-column";
            gridView.columnByName("D_REASON").styleName = "string-column";
            gridView.columnByName("D_RESP").styleName = "string-column";
            gridView.columnByName("REWORK_MSG").styleName = "string-column";

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
            //if (!gV5) {
            //    container = document.getElementById('realgrid5');
            //    dP5 = new RealGrid.LocalDataProvider(false);
            //    gV5 = new RealGrid.GridView(container);
            //}

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

            if (_val != undefined) {
                gridView.setFixedOptions({
                    colCount: _val
                });
            }

            gridView.columnByName("KEY_HID").visible = false;
            gridView.columnByName("LINE_CD").styleName = "string-column";

            gridView.columnByName("D_SYMPTOM").styleName = "string-column";
            gridView.columnByName("D_REASON").styleName = "string-column";
            gridView.columnByName("D_RESP").styleName = "string-column";
            gridView.columnByName("REWORK_MSG").styleName = "string-column";

            dataProvider.softDeleting = true;
            //gV6.displayOptions.fitStyle = "even";//그리드 채우기
            gridView.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gridView.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gridView.pasteOptions.enabled = false;//붙여넣기 금지
            gridView.groupPanel.visible = true;//그룹핑 활성화
            gridView.filterPanel.visible = true;//필터패널 활성화
            gridView.groupPanel.minHeight = 34;//그룹핑 영역 최소높이
        }

        /* 그리드 끝 */

    </script>

    <style>
        .grid-primary-column {
            font-weight: bold;
            text-decoration: underline; cursor: pointer;
        }
        
        .searchCombo{
            text-align: center;
            z-index : 9999;
            top : 4%;
            position : absolute; 
        }

        .string-column {
            text-align: left;
        }
    </style>

    <asp:HiddenField ID="hidWidth" runat="server" Value="500" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="380" />
   
    <div class="contents" tabindex="0">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">Qua40</asp:Label> <span id="spanTabLabel"></span> </h3>
            <div class="al-r">
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                <input type="button" value="New" onclick="javascript:fn_Register();" ID="btnNew" runat="server" visible="false" />
                <input type="button" value="Delete" onclick="javascript:fn_Delete();" ID="btnDeleteNew" runat="server" visible="false" />
                <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport_override('Qua40'); return false;" />
                <asp:Button ID="btnFunctionCall" runat="server" Text="FunctionCall" OnClick="btnFunctionCall_Click"  style="display:none;"  />
                <asp:TextBox id="txtTabCount" runat="server" style="display:none"></asp:TextBox>
            </div>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:120px;">
	                <col style="width:290px;">
	                <col style="width:120px;">
	                <col style="width:135px;">
	                <col style="width:120px;">
	                <col style="width:80px;">
	                <col style="width:220px;">
                    <col style="width:170px;">
                    <col style="width:250px;">
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbSearchDt" runat="server"></asp:Label></th>
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
                        <div id="divShopCd" class="searchCombo" style="width:115px; font-size:12px;">
                            <input type="text" ID="txtShopCd" runat="server" style="background-color:white; color:black;" readonly/>
                            <asp:TextBox id="txtShopCdHidden" runat="server" style="display:none;"></asp:TextBox>
                        </div>
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
                                <asp:AsyncPostBackTrigger ControlID="btnFunctionCall" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="ddlEopFlag" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <th><asp:Label ID="lbPartNo" runat="server"></asp:Label></th>
                    <td colspan="6">
                        <div id="divPartNo" class="searchCombo" style="width:430px; font-size:12px;">
                            <input type="text" ID="txtPartNo" runat="server" style="background-color:white; color:black;" readonly/>
                            <asp:TextBox id="txtPartNoHidden" runat="server" style="display:none;"></asp:TextBox>
                        </div>
                    </td>
                    <td colspan="2" id="tdGap1"></td>
                    <td class="al-r">
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbDCode" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDCode" runat="server" Width="270"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbDReasonCode" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDReasonCode" runat="server" Width="100"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbDRespCd" runat="server"></asp:Label></th>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlDRespCd" runat="server" Width="150"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbResultCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlResultCd" runat="server" Width="100"></asp:DropDownList>
                    </td>
                    <td colspan="2" id="tdGap2"></td>
                    <td class="al-r">
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbBarcode" runat="server"></asp:Label></th>
                    <td><asp:TextBox ID="txtBarcode" runat="server" Width="270" MaxLength="30"></asp:TextBox></td>
                    <th><asp:Label ID="lbSerialNo" runat="server"></asp:Label></th>
                    <td colspan="4"><asp:TextBox ID="txtSerialNo" runat="server" Width="270" MaxLength="30"></asp:TextBox></td>
                    <th><asp:Label ID="lbPartSerialNo" runat="server"></asp:Label></th>
                    <td colspan="2"><asp:TextBox ID="txtPartSerialNo" runat="server" Width="420" MaxLength="60"></asp:TextBox></td>
                    <th><asp:Label ID="lbDownLevel" runat="server"></asp:Label></th>
                    <td colspan="2">
                        <asp:RadioButton ID="rdNo" runat="server" Text="미포함" GroupName="dayGrp" Width="60px" />
                        <asp:RadioButton ID="rdYes" runat="server" Text="포함"  GroupName="dayGrp" Width="60px" />
                    </td>
                    <th><asp:Label ID="lbPartBarcodeNo" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtPartBarcodeNo" runat="server" Width="420" MaxLength="60"></asp:TextBox>
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
	                <li class="tab" ><p class="grid-tab-button " onclick="javascript:openTab(event, 'Tab4');" ><asp:Label ID="lb4" runat="server"></asp:Label></p></li>
	                <li class="tab" ><p class="grid-tab-button " onclick="javascript:openTab(event, 'Tab5');" ><asp:Label ID="lb5" runat="server"></asp:Label></p></li>
	            </ul>
            </div>

            <div class="grid-tab-content active" id="Tab1">
                <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                     <ContentTemplate>
                        <div id="realgrid1" class="realgrid_4"></div>
                     </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="grid-tab-content" id="Tab2">
                <asp:UpdatePanel runat="server" ID="up2" UpdateMode="Conditional">
                     <ContentTemplate>
                        <div id="realgrid2" class="realgrid_4"></div>
                     </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="grid-tab-content" id="Tab3">
                <asp:UpdatePanel runat="server" ID="up3" UpdateMode="Conditional">
                     <ContentTemplate>
                        <div id="realgrid3" class="realgrid_4"></div>
                     </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="grid-tab-content" id="Tab4">
                <asp:UpdatePanel runat="server" ID="up4" UpdateMode="Conditional">
                     <ContentTemplate>
                        <div id="realgrid4" class="realgrid_4"></div>
                     </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="grid-tab-content" id="Tab5">
                <asp:UpdatePanel runat="server" ID="up5" UpdateMode="Conditional">
                     <ContentTemplate>
                        <div id="realgrid5" class="realgrid_4"></div>
                     </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <asp:UpdatePanel runat="server" ID="up7" UpdateMode="Conditional">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <div class="toolbar">
                                <div id="gridPage" style="float:left" onselectstart="return false" ondragstart="return false"></div>
                                &nbsp;
                                <select id="current_page_value" onchange="setPaging(); return false;" style="font-size:12px; display:none; margin-top:10px; float:left" runat="server">
                                </select>
                                <input type="button" value="Set" id="btnSet" onclick="getCol_override('Qua40'); return false;" style="display:none; margin-top:10px; float:left" />
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