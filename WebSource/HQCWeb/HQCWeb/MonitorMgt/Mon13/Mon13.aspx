<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Mon13.aspx.cs" Inherits="HQCWeb.MonitorMgt.Mon13.Mon13" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        function fn_Validation() {
            if ($("#MainContent_ddlShopCd").val() == "") {
                alert("Shop Code를 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlLineCd").val() == "") {
                alert("Line Code를 선택하세요.");
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

                    // 1주일 이내인지 확인
                    if (dayDifference > 7) {
                        alert("최대 조회 기간은 1주일입니다.");
                        return false;
                    }
                }

                fn_WatingCall();
                return true;
            }
            return true;
        }

        // 제품추적 이동?
        function fn_Move(pkCode) {

            var aData = "";
            aData = "Qua98";

            var jsonData = JSON.stringify({ sMenu: aData, sData: pkCode });

            $.ajax({
                type: "POST",
                url: "Mon13.aspx/GetMenu",
                data: jsonData,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    fn_Search_Open(msg.d);
                }
            });
        }

        function fn_Search_Open(title) {
            if (title == "Error") {
                alert("시스템 에러가 발생했습니다. 관리자에게 문의 바랍니다.");

                return;
            }
            parent.fn_ModalCloseDiv();
            parent.fn_Add('/QualityMgt/Qua98/Qua98.aspx', title, 'Qua98', true);
        }

        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }

        var column, field, data, cType;
        var container, dataProvider, gridView;

        jQuery(document).ready(function ($) {
            //라벨 설정
            $("#spanTabLabel").text("(최대 조회 기간은 1주일 입니다.)")
        });

        // 그리드 생성
        function createGrid(_val) {
            container = document.getElementById('realgrid');
            dataProvider = new RealGrid.LocalDataProvider(false);
            gridView = new RealGrid.GridView(container);
            gridView.setDataSource(dataProvider);
            gridView.setColumns(column);

            //field 넣기 전 포맷변경(엑셀 문제)
            field.forEach(item => {
                if (item.fieldName == 'CT' || item.fieldName == 'CT2') {
                    item.dataType = 'number'; item.subType = 'int'
                }
            });

            //숫자 포맷 변경
            gridView.setFormatOptions({ numberFormat: '#,##0' });

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
                if (clickData.column == 'SERIAL_NO') {
                    var current = gridView.getCurrent();
                    var value = dataProvider.getValue(current.dataRow, "KEY_HID");

                    if (clickData.cellType !== "data") {
                        return; // 데이터 셀이 아니면 이벤트 중단
                    }
                    fn_Move(value);
                }
            }

            if (_val != undefined) {
                gridView.setFixedOptions({
                    colCount: _val
                });
            }

            gridView.columnByName("KEY_HID").visible = false;
            gridView.columnByName("SERIAL_NO").styleName = "grid-primary-column";
            gridView.columnByName("STN_NM").styleName = "string-column";
            gridView.columnByName("COMPLETE_NO").styleName = "string-column";
            gridView.columnByName("CT").styleName = "number-column";
            gridView.columnByName("CT2").styleName = "number-column";

            dataProvider.softDeleting = true;
            //gridView.displayOptions.fitStyle = "even";//그리드 채우기
            gridView.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gridView.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gridView.pasteOptions.enabled = false;//붙여넣기 금지
            gridView.groupPanel.visible = true;//그룹핑 활성화
            gridView.filterPanel.visible = true;//필터패널 활성화
        }

    </script>

    <style>
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

        .grid-primary-column {
            text-align: left;
            font-weight: bold;
            text-decoration: underline; cursor: pointer;
        }
    </style>

    <asp:HiddenField ID="hidWidth" runat="server" Value="500" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="500" />

    <div class="contents" tabindex="0">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">Mon13</asp:Label> <span id="spanTabLabel"></span> </h3>
            <div class="al-r">
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                <input type="button" value="New" onclick="javascript:fn_Register();" ID="btnNew" runat="server" visible="false" />
                <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('Mon13'); return false;" />
            </div>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:60px;">
                    <col style="width:290px;">
                    <col style="width:80px;">
                    <col style="width:290px;">
                    <col style="width:60px;">
	                <col style="width:80px;">
	                <col style="width:220px;">
                    <col style="width:60px;">
                    <col style="width:270px;">
                    <col>
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
                        <asp:DropDownList ID="ddlShopCd" runat="server" OnSelectedIndexChanged="ddlShopCd_SelectedIndexChanged" AutoPostBack="true" Width="270"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbLineCd" runat="server"></asp:Label></th>
                    <td style="border-right: none;">
                        <asp:DropDownList ID="ddlEopFlag" runat="server" Width="70" OnSelectedIndexChanged="ddlEopFlag_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
                    </td>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
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
                    <td >
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlStnCd" runat="server" AutoPostBack="true" CssClass="ellipsis-dropdown" Width="250"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlLineCd" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td class="al-r">
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbBarcode" runat="server"></asp:Label></th>
                    <td><asp:TextBox ID="txtBarcode" runat="server" Width="270" ></asp:TextBox></td>
                    <th><asp:Label ID="lbSerialNo" runat="server"></asp:Label></th>
                    <td><asp:TextBox ID="txtSerialNo" runat="server" Width="270"></asp:TextBox></td>
                    <th><asp:Label ID="lbCt" runat="server"></asp:Label></th>
                    <td colspan="4"><asp:TextBox ID="txtFrCt" runat="server" type="number" Width="50" MaxLength="4"></asp:TextBox> <span id="ctBetween">~</span> <asp:TextBox ID="txtToCt" runat="server" type="number" Width="50" MaxLength="4"></asp:TextBox></td>
                    <td class="al-r">
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div>
            <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="realgrid" class="realgrid_2"></div>
                    <table>
                        <tr>
                            <td>
                                <div class="toolbar">
                                    <div id="gridPage" style="float:left" onselectstart="return false" ondragstart="return false"></div>
                                    &nbsp;
                                    <select id="current_page_value" onchange="setPaging(); return false;" style="font-size:12px; display:none; margin-top:10px; float:left" runat="server">
                                    </select>
                                    <input type="button" value="Set" id="btnSet" onclick="getCol('Mon13'); return false;" style="display:none; margin-top:10px; float:left" />
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