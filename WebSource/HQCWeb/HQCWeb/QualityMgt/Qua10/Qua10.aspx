<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Qua10.aspx.cs" Inherits="HQCWeb.QualityMgt.Qua10.Qua10" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .searchCombo{
            text-align: center;
            z-index : 1;
            top : 6.5%;
            position : absolute; 
        }

        .number-column {
            text-align: right;
        }

        .string-column {
            text-align: left;
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
            } else if ($("#MainContent_txtStnCdHidden").val() == "") {
                alert("공정을 선택하세요.");
                return false;
            } else if ($("#MainContent_txtCarTypeHidden").val() == "") {
                alert("차종을 선택하세요.");
                return false;
            } else if ($("#MainContent_txtBarcode").val() != "" && $("#MainContent_txtSerialNo").val() != "") {
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

        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }

        var column, field, data;
        var container, dataProvider, gridView;
        var cStn, cCarType;

        // 그리드 생성
        function createGrid(_val) {
            container = document.getElementById('realgrid_2');
            dataProvider = new RealGrid.LocalDataProvider(false);
            gridView = new RealGrid.GridView(container);
            gridView.setDataSource(dataProvider);
            //column 넣기 전 포맷변경(엑셀 문제)
            column.forEach(item => {
                if (item.name == 'TORQUE_VALUE' || item.name == 'TORQUE_MAX' || item.name == 'TORQUE_MIN') {
                    item.numberFormat = "#,###.##";
                    item.excelFormat = "#,###.##";
                } else if (item.name == 'ANGLE_VALUE' || item.name == 'ANGLE_MIN' || item.name == 'ANGLE_MAX' || item.name == 'WORK_SEQ'
                    || item.name == 'R_ANGLE' || item.name == 'R_ANGLE_MIN' || item.name == 'R_ANGLE_MAX') {
                    item.numberFormat = "#,###";
                    item.excelFormat = "#,###";
                }
            });

            gridView.setColumns(column);
            //field 넣기 전 포맷변경(엑셀 문제)
            field.forEach(item => {
                if (item.fieldName == 'TORQUE_VALUE' || item.fieldName == 'TORQUE_MAX' || item.fieldName == 'TORQUE_MIN') {
                    item.dataType = 'number'; item.subType = 'unum'
                } else if (item.fieldName == 'ANGLE_VALUE' || item.fieldName == 'ANGLE_MIN' || item.fieldName == 'ANGLE_MAX' || item.fieldName == 'WORK_SEQ'
                    || item.fieldName == 'R_ANGLE' || item.fieldName == 'R_ANGLE_MIN' || item.fieldName == 'R_ANGLE_MAX'
                    ) {
                    item.dataType = 'number'; item.subType = 'int'
                }
            });

            //숫자 포맷 변경
            //gridView.setFormatOptions({ numberFormat: '#,##0' });

            dataProvider.setFields(field);
            dataProvider.setRows(data);
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

            //5차_화면수정요청 : NG인 경우 색 변경
            const f = function (grid, dataCell) {
                var ret = {}
                var rslt = grid.getValue(dataCell.index.itemIndex, "RESULT")

                if (rslt == 'NG') {
                    ret.style = { background: "#FFD966" }
                }

                return ret;
            }

            gridView.columnByName("RESULT").styleCallback = f;
            gridView.columnByName("REASON").styleCallback = f;

            gridView.columnByName("KEY_HID").visible = false;

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
            //gridView.setRowGroup({headerStatement: "${columnHeader}: ${groupValue} - ${rowCount} rows"}); //조회불가 버그 발생
            gridView.groupPanel.visible = true;//그룹핑 활성화
            gridView.filterPanel.visible = true;//필터패널 활성화
        }

        //콤보박스 초기화
        jQuery(document).ready(function ($) {
            fn_Set_Stn();
            fn_Set_CarType();

            //라벨 설정
            $("#spanTabLabel").text("(최대 조회 기간은 1개월 입니다.)")
        });

        //Shop코드 재설정
        function fn_Set_Stn() {
            $("#MainContent_txtStnCd").comboTree({
                source: cStn,
                comboReload: true,
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true,
                onLoadAllChk: true,
                valueChange: true,
                hidCon: "MainContent_txtStnCdHidden",
                functionCall: true,                    // 콤보값 선택시 자바스크립스 함수 호출 여부
                functionCallFunc: "fn_Stn_Checked"
            });
        }

        //Shop코드 onchange
        function fn_Stn_Checked() {
            $("#MainContent_btnFunctionCall").click();
        }

        //부품코드 재설정
        function fn_Set_CarType() {
            $("#MainContent_txtCarType").comboTree({
                source: cCarType,
                comboReload: true,
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true,
                onLoadAllChk: true,
                valueChange: true,
                hidCon: "MainContent_txtCarTypeHidden"
            });
        }
    </script>

    <asp:HiddenField ID="hidWidth" runat="server" Value="500" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="500" />

    <div class="contents" tabindex="0">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">Qua10</asp:Label> <span id="spanTabLabel"></span> </h3>
            <div class="al-r">
               <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                <input type="button" value="New" onclick="javascript:fn_Register();" ID="btnNew" runat="server" visible="false" />
                <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('Qua10'); return false;" />
                <asp:Button ID="btnFunctionCall" runat="server" Text="FunctionCall" OnClick="btnFunctionCall_Click"  style="display:none;"  />
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
                                <asp:DropDownList ID="ddlLineCd" runat="server" OnSelectedIndexChanged="ddlLineCd_SelectedIndexChanged" AutoPostBack="true" Width="200"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlEopFlag" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <th><asp:Label ID="lbStnCd" runat="server"></asp:Label></th>
                    <td style = "border-right: 0px;">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="searchCombo" style="width:250px; font-size:12px;">
                                    <input type="text" ID="txtStnCd" runat="server" style="background-color:white; color:black;" readonly/>
                                    <asp:TextBox id="txtStnCdHidden" runat="server" style="display:none;"></asp:TextBox>
                                </div>
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
                                <div class="searchCombo" style="width:250px; font-size:12px;">
                                    <input type="text" ID="txtCarType" runat="server" style="background-color:white; color:black;" readonly/>
                                    <asp:TextBox id="txtCarTypeHidden" runat="server" style="display:none;"></asp:TextBox>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlLineCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="btnFunctionCall" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <th><asp:Label ID="lbRslt1" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlRslt1" runat="server" Width="100"></asp:DropDownList>
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
                    <th><asp:Label ID="lbBarcode" runat="server"></asp:Label></th>
                    <td colspan="2"><asp:TextBox ID="txtBarcode" runat="server" Width="270" ></asp:TextBox></td>
                    <th><asp:Label ID="lbSerialNo" runat="server"></asp:Label></th>
                    <td colspan="5"><asp:TextBox ID="txtSerialNo" runat="server" Width="270"></asp:TextBox></td>
                    <%--<th><asp:Label ID="lbRslt2" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlRslt2" runat="server"></asp:DropDownList>
                    </td>--%>
                </tr>
            </table>
        </div>
        <br />
        <div>
            <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="realgrid_2" class="realgrid_2"></div>
                    <table>
                        <tr>
                            <td>
                                <div class="toolbar">
                                    <div id="gridPage" style="float:left" onselectstart="return false" ondragstart="return false"></div>
                                    &nbsp;
                                    <select id="current_page_value" onchange="setPaging(); return false;" style="font-size:12px; display:none; margin-top:10px; float:left" runat="server">
                                    </select>
                                    <input type="button" value="Set" id="btnSet" onclick="getCol('Qua10'); return false;" style="display:none; margin-top:10px; float:left" />
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