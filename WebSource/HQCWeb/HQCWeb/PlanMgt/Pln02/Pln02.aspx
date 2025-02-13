<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Pln02.aspx.cs" Inherits="HQCWeb.PlanMgt.Pln02.Pln02" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function fn_Register() {
            fn_OpenPop('/PlanMgt/Pln02/Pln02_p01.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        function fn_Modify(pkCode) {
            fn_PostOpenPop(pkCode, '/PlanMgt/Pln02/Pln02_p02.aspx', $("#MainContent_hidWidth").val(), "700");
        }

        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }

        function fn_Chk(pkCode) {

            var jsonData = JSON.stringify({ sParams: pkCode });

            $.ajax({
                type: "POST",
                url: "Pln02.aspx/GetPkCode",
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

        var column, field, data;
        var container, dataProvider, gridView;
        var cShop, cLine;

        // 그리드 생성
        function createGrid(_val) {
            container = document.getElementById('realgrid');
            dataProvider = new RealGrid.LocalDataProvider(false);
            gridView = new RealGrid.GridView(container);
            gridView.setDataSource(dataProvider);
            gridView.setColumns(column);

            //field 넣기 전 포맷변경(엑셀 문제)
            field.forEach(item => {
                if (item.fieldName == 'MES_PLAN_QTY' || item.fieldName == 'PLAN_D_QTY' || item.fieldName == 'PLAN_N_QTY' || item.fieldName == 'COMP_QTY') {
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
                var current = gridView.getCurrent();
                var value;

                if (clickData.cellType !== "data") {
                    return; // 데이터 셀이 아니면 이벤트 중단
                }
                if (clickData.column == 'PART_NO') {
                    value = dataProvider.getValue(current.dataRow, "ORDER_TYPE_CD");
                    if (value == 'D') {
                        alert("수정이 불가능한 생산계획입니다.");
                        return false;
                    }
                    else {
                        value = dataProvider.getValue(current.dataRow, "KEY_HID");
                        fn_Modify(value);
                    }
                }
                else if (clickData.column == 'COMP_QTY') {
                    if (dataProvider.getValue(current.dataRow, current.column) > 0) {
                        value = dataProvider.getValue(current.dataRow, "KEY_HID");
                        fn_Chk(value);
                    }
                    else {
                        alert("데이터가 없습니다.");
                    }
                }
            }

            if (_val != undefined) {
                gridView.setFixedOptions({
                    colCount: _val
                });
            }

            //1차_화면수정요청 : ORDER_TYPE_CD에 따라 색상변경
            const f = function (grid, dataCell) {
                var ret = {}
                var OrderTp = grid.getValue(dataCell.index.itemIndex, "ORDER_TYPE_CD")

                if (OrderTp == 'S') {
                    ret.style = { background: "#A9D18E" }
                }
                else if (OrderTp == 'M') {
                    ret.style = { background: "#F8CBAD" }
                }

                return ret;
            }

            //4차_화면수정요청 : 생산 컬럼은 색 변경
            const f2 = function (grid, dataCell) {
                var ret = {}
                var ProdTp = grid.getValue(dataCell.index.itemIndex, "PROD_TYPE")

                if (ProdTp == '생산') {
                    ret.style = { background: "#FFD966" }
                }

                return ret;
            }

            //4차_화면수정요청 : 완료수량 색 변경
            const f3 = function (grid, dataCell) {
                var ret = {}
                ret.style = { background: "#E2F0D9" }
                return ret;
            }
            
            gridView.columnByName("ORDER_TYPE").styleCallback = f;
            gridView.columnByName("PROD_TYPE").styleCallback = f2;
            gridView.columnByName("COMP_QTY").styleCallback = f3;

            gridView.columnByName("PART_NO").styleName = "grid-primary-column";
            gridView.columnByName("KEY_HID").visible = false;
            //gridView.columnByName("PLANT_CD").visible = false;
            gridView.columnByName("ORDER_TYPE_CD").visible = false;
            gridView.columnByName("ERP_PLAN_QTY").visible = false; // 1차_화면수정요청 : 컬럼제거
            gridView.columnByName("FINISH_FLG").visible = false; // 1차_화면수정요청 : 컬럼제거

            gridView.columnByName("CAR_TYPE").styleName = "string-column";

            gridView.columnByName("MES_PLAN_QTY").styleName = "number-column";
            gridView.columnByName("PLAN_D_QTY").styleName = "number-column";
            gridView.columnByName("PLAN_N_QTY").styleName = "number-column";
            gridView.columnByName("COMP_QTY").styleName = "grid-primary-column-number";

            dataProvider.softDeleting = true;
            //gridView.displayOptions.fitStyle = "even";//그리드 채우기
            gridView.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gridView.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gridView.pasteOptions.enabled = false;//붙여넣기 금지
            gridView.groupPanel.visible = true;//그룹핑 활성화
            gridView.filterPanel.visible = true;//필터패널 활성화
        }

        //콤보박스 초기화
        jQuery(document).ready(function ($) {
            fn_Set_Shop();
            fn_Set_Line();
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
                hidCon: "MainContent_txtLineCdHidden"
            });
        }

    </script>

    <style>
        .searchCombo{
            text-align: center;
            z-index : 1;
            top : 12%;
            position : absolute; 
        }

        .grid-primary-column {
            font-weight: bold;
            text-decoration: underline; cursor: pointer;
        }

        .grid-primary-column-number{
            font-weight: bold;
            text-decoration: underline; cursor: pointer;
            text-align: right;
        }

        .number-column {
            text-align: right;
        }

        .string-column {
            text-align: left;
        }
    </style>

    <asp:HiddenField ID="hidWidth" runat="server" Value="500" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="550" />

    <div class="contents" tabindex="0">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">Pln02</asp:Label></h3>
            <div class="al-r">
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="fn_WatingCall();" Visible="false" />
                <input type="button" value="New" onclick="javascript:fn_Register();" ID="btnNew" runat="server" visible="false" />
                <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('Pln02'); return false;" />
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
	                        <col style="width:135px;">
	                        <col style="width:60px;">
	                        <col style="width:80px;">
	                        <col style="width:220px;">
	                        <col style="width:60px;">
	                        <col style="width:120px;">
	                        <col style="width:60px;">
	                        <col style="width:120px;">
	                        <col style="width:60px;">
	                        <col style="width:120px;">
	                        <col>
                        </colgroup>
                        <tr>
                            <th><asp:Label ID="lbPlanDt" runat="server"></asp:Label></th>
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
                                <div class="searchCombo" style="width:115px; font-size:12px;">
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
                            <th><asp:Label ID="lbPlanCd" runat="server"></asp:Label></th>
                            <td>
                                <asp:DropDownList ID="ddlPlanCd" runat="server" OnSelectedIndexChanged="ddlPlanCd_SelectedIndexChanged" AutoPostBack="true" Width="100"></asp:DropDownList>
                            </td>
                            <th><asp:Label ID="lbPlanDetailCd" runat="server"></asp:Label></th>
                            <td>
                                <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlPlanDetailCd" runat="server" Width="100"></asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlPlanCd" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                            <th><asp:Label ID="lbOrderType" runat="server"></asp:Label></th>
                            <td>
                                <asp:DropDownList ID="ddlOrderType" runat="server" Width="100"></asp:DropDownList>
                            </td>


                            <td class="al-r">
                                <%--<asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="fn_WatingCall();" Visible="false" />
                                <input type="button" value="New" onclick="javascript:fn_Register();" ID="btnNew" runat="server" visible="false" />
                                <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('Pln02'); return false;" />
                                <asp:Button ID="btnFunctionCall" runat="server" Text="FunctionCall" OnClick="btnFunctionCall_Click"  style="display:none;"  />--%>
                            </td>
                        </tr>
                    </table>

        </div>
        <br />
        <div>
            <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="realgrid" class="realgrid"></div>
                    <table>
                        <tr>
                            <td>
                                <div class="toolbar">
                                    <div id="gridPage" style="float:left" onselectstart="return false" ondragstart="return false"></div>
                                    &nbsp;
                                    <select id="current_page_value" onchange="setPaging(); return false;" style="font-size:12px; display:none; margin-top:10px; float:left" runat="server">
                                    </select>
                                    <input type="button" value="Set" id="btnSet" onclick="getCol('Pln02'); return false;" style="display:none; margin-top:10px; float:left" />
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