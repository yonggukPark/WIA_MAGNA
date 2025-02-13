<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Mon16.aspx.cs" Inherits="HQCWeb.MonitorMgt.Mon16.Mon16" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function fn_Validation() {

            if ($("#MainContent_txtLineCdHidden").val() == "") {
                alert("라인을 선택하세요.");
                return false;
            } else if ($("#MainContent_txtBarcode").val() != "" && $("#MainContent_txtSerialNo").val() != "" ) {
                alert("바코드와 시리얼을 동시에 검색할 수 없습니다.");
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

        function fn_Modify(pkCode) {
            // 단일 수정은 사용하지 않음(20241121)
            //fn_PostOpenPop(pkCode, 'MonitorMgt/Mon16/Mon16_p01.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        function fn_Lock() {
            var pkString = '';
            var rows = gridView.getCheckedRows();

            if (rows.length > 0) {

                if (rows.length > 100) {
                    alert("100건 이하의 데이터만 일괄수정 가능합니다 : 현재 " + rows.length + "건 ");
                    return false;
                }

                for (var i in rows) {
                    var data = dataProvider.getJsonRow(rows[i]);

                    //if (data.BARCODE_FLAG == "N") {
                    //    alert("단품 조회의 경우에만 일괄삭제 가능합니다.");
                    //    return false;
                    //}

                    pkString += data.KEY_HID;
                    pkString += '|';
                }

                pkString = pkString.substring(0, pkString.length - 1);
                fn_PostOpenPop(pkString, 'MonitorMgt/Mon16/Mon16_p02.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
            }

        }

        function fn_parentReload() {
            fn_WatingCall();
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }

        var column, field, data;
        var container, dataProvider, gridView;
        var cLine;

        /* 다중콤보 시작 */

        //콤보박스 초기화
        jQuery(document).ready(function ($) {
            fn_Set_Line();

            //라벨 설정
            $("#spanTabLabel").text("(최대 조회 기간은 1개월 입니다.)")
        });

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
                //functionCall: true,                    // 콤보값 선택시 자바스크립스 함수 호출 여부
                //functionCallFunc: "fn_Line_Checked"
            });
        }

        //Line코드 onchange
        //function fn_Line_Checked() {
        //    $("#MainContent_btnFunctionCall").click();
        //}

        /* 다중콤보 끝 */

        /* 그리드 시작 */

        // 그리드 생성
        function createGrid(_val) {
            container = document.getElementById('realgrid');
            dataProvider = new RealGrid.LocalDataProvider(false);
            gridView = new RealGrid.GridView(container);
            gridView.setDataSource(dataProvider);
            gridView.setColumns(column);
            dataProvider.setFields(field);
            dataProvider.setRows(data);
            //gridView.checkBar.visible = false;
            gridView.stateBar.visible = false;
            gridView.setCheckBar({ useImages: true });
            setContextMenu(gridView);
            setPaging();
            settingGrid(_val); if (data != undefined) document.getElementById('rowCnt').innerHTML = data.length;
        }

        function settingGrid(_val) {
            //PK 컬럼 클릭시 동작 (20241121 사용안함)
            //gridView.onCellClicked = function (grid, clickData) {
            //    if (clickData.column == 'COMPLETE_NO') {
            //        var current = gridView.getCurrent();
            //        var value = dataProvider.getValue(current.dataRow, "KEY_HID");
            //        fn_Modify(value);
            //    }
            //}

            if (_val != undefined) {
                gridView.setFixedOptions({
                    colCount: _val
                });
            }

            //클릭 비활성(20241121)
            //gridView.columnByName("COMPLETE_NO").styleName = "grid-primary-column"
            gridView.columnByName("LINE_CD").styleName = "string-column"
            //gridView.columnByName("STN_CD").styleName = "string-column"
            gridView.columnByName("KEY_HID").visible = false;
            gridView.columnByName("BARCODE_FLAG").visible = false;
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
    <asp:HiddenField ID="hidHeight" runat="server" Value="250" />

    <div class="contents">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">Mon16</asp:Label> <span id="spanTabLabel"></span> </h3>
            <div class="al-r">
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                <input type="button" value="Lock" onclick="javascript:fn_Lock();" ID="btnLock" runat="server" visible="false" />
                <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('Mon16'); return false;" />
                <%--<asp:Button ID="btnFunctionCall" runat="server" Text="FunctionCall" OnClick="btnFunctionCall_Click"  style="display:none;"  />--%>
            </div>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:90px;">
	                <col style="width:290px;">
	                <col style="width:80px;">
	                <col style="width:120px;">
	                <col style="width:80px;">
	                <col style="width:80px;">
	                <col style="width:220px;">
	                <col style="width:90px;">
	                <col style="width:35px;">
                    <col>
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
                        <asp:DropDownList ID="ddlShopCd" runat="server" OnSelectedIndexChanged="ddlShopCd_SelectedIndexChanged" AutoPostBack="true" Width="100"></asp:DropDownList>
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
                    <th><asp:Label ID="lbLockedData" runat="server"></asp:Label></th>
                    <td><asp:CheckBox ID="chkLockedData" runat="server" /></td>
                    <td class="al-r">
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbBarcode" runat="server"></asp:Label></th>
                    <td><asp:TextBox ID="txtBarcode" runat="server" Width="270" MaxLength="30"></asp:TextBox></td>
                    <th><asp:Label ID="lbSerialNo" runat="server"></asp:Label></th>
                    <td colspan="6"><asp:TextBox ID="txtSerialNo" runat="server" Width="270" MaxLength="30"></asp:TextBox></td>
                    <td class="al-r">
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div>
            <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="realgrid" class="realgrid_3"></div>
                    <table>
                        <tr>
                            <td>
                                <div class="toolbar">
                                    <div id="gridPage" style="float:left" onselectstart="return false" ondragstart="return false"></div>
                                    &nbsp;
                                    <select id="current_page_value" onchange="setPaging(); return false;" style="font-size:12px; display:none; margin-top:10px; float:left" runat="server">
                                    </select>
                                    <input type="button" value="Set" id="btnSet" onclick="getCol('Mon16'); return false;" style="display:none; margin-top:10px; float:left" />
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

</asp:Content>