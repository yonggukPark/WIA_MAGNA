<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Info08.aspx.cs" Inherits="HQCWeb.InfoMgt.Info08.Info08" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function fn_Validation() {
            if ($("#MainContent_ddlShopCd").val() == "") {
                alert("Shop Code를 선택하세요.");
                return false;
            } else if ($("#MainContent_txtLineCdHidden").val() == "") {
                alert("라인을 선택하세요.");
                return false;
            } else if ($("#MainContent_txtCarTypeHidden").val() == "") {
                alert("차종을 선택하세요.");
                return false;
            } else {
                fn_WatingCall();
                return true;
            }
            return true;
        }

        function fn_Register() {
            fn_OpenPop('InfoMgt/Info08/Info08_p01.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        function fn_Modify(pkCode) {
            fn_PostOpenPop(pkCode, 'InfoMgt/Info08/Info08_p02.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        function fn_Copy() {
            if (currentValue != undefined)
                fn_PostOpenPop(currentValue, 'InfoMgt/Info08/Info08_p03.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
            else
                alert("먼저 복사할 행을 선택해주세요.");
        }

        function fn_Image(pkCode) {
            fn_PostOpenPop(pkCode, 'InfoMgt/Info08/Info08_p04.aspx', '1500', '800');
        }

        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }

        var column, field, data, cLine, cCarType;
        var container, dataProvider, gridView;
        var currentValue;

        // 그리드 생성
        function createGrid(_val) {
            container = document.getElementById('realgrid');
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
                var current = gridView.getCurrent();
                var value = dataProvider.getValue(current.dataRow, "KEY_HID");
                currentValue = value;

                if (clickData.cellType !== "data") {
                    return; // 데이터 셀이 아니면 이벤트 중단
                }
                if (clickData.column == 'SEQ') {
                    fn_Modify(value);
                }
                else if (clickData.column == 'BARCODE_IMG') {
                    fn_Image(value);
                }
            }

            if (_val != undefined) {
                gridView.setFixedOptions({
                    colCount: _val
                });
            }

            gridView.columnByName("BARCODE_IMG").renderer = {
                type: "icon",
                iconCallback: function (grid, cell) {
                    return "/img/New/btn_colorpicker.gif";
                },
                iconHeight: 15,
                iconWidth: 15
            }

            gridView.columnByName("SEQ").styleName = "grid-primary-column";
            gridView.columnByName("BARCODE_IMG").styleName = "cursor-column";

            gridView.columnByName("LINE_NM").styleName = "string-column";
            gridView.columnByName("VALUE").styleName = "string-column";
            gridView.columnByName("ID_CODE_NM").styleName = "string-column";
            gridView.columnByName("ZPL").styleName = "string-column";

            gridView.columnByName("WIDTH").styleName = "number-column";
            gridView.columnByName("HEIGHT").styleName = "number-column";
            gridView.columnByName("FONT_WIDTH").styleName = "number-column";
            gridView.columnByName("FONT_HEIGHT").styleName = "number-column";
            
            gridView.columnByName("REMARK1").styleName = "string-column";
            gridView.columnByName("REMARK2").styleName = "string-column";

            gridView.columnByName("LINE_CD").visible = false;
            gridView.columnByName("KEY_HID").visible = false;
            dataProvider.softDeleting = true;
            //gridView.displayOptions.fitStyle = "even";//그리드 채우기
            gridView.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gridView.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gridView.pasteOptions.enabled = false;//붙여넣기 금지
        }

        //콤보박스 초기화
        jQuery(document).ready(function ($) {

            $("#MainContent_txtLineCd").comboTree({
                source: [],
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

            $("#MainContent_txtCarType").comboTree({
                source: [],
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true,
                onLoadAllChk: true,
                valueChange: true,
                hidCon: "MainContent_txtCarTypeHidden"
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

        //차종 재설정
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

    <style>
        .grid-primary-column {
            font-weight: bold;
            text-decoration: underline; cursor: pointer;
        }

        .cursor-column {
            cursor: pointer;
        }

        .searchCombo{
            text-align: center;
            z-index : 1;
            top : 14%;
            position : absolute; 
        }

        .string-column {
            text-align: left;
        }

        .number-column {
            text-align: right;
        }
    </style>

    <asp:HiddenField ID="hidWidth" runat="server" Value="800" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="500" />

    <div class="contents" tabindex="0">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">Info08</asp:Label></h3>
            <div class="al-r">
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                <asp:Button ID="btnRestore" runat="server" Text="Restore" OnClick="btnRestore_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                <input type="button" value="New" onclick="javascript:fn_Register();" ID="btnNew" runat="server" visible="false" />
                <input type="button" value="Copy" onclick="javascript:fn_Copy();" ID="btnCopy" runat="server" visible="false" />
                <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('Info08'); return false;" />
                <asp:Button ID="btnFunctionCall" runat="server" Text="FunctionCall" OnClick="btnFunctionCall_Click"  style="display:none;"  />
            </div>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:60px;">
                    <col style="width:105px;">
                    <col style="width:60px;">
                    <col style="width:320px;">
                    <col style="width:60px;">
                    <col style="width:320px;">
                    <col style="width:60px;">
                    <col style="width:320px;">
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbShopCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlShopCd" runat="server" OnSelectedIndexChanged="ddlShopCd_SelectedIndexChanged" AutoPostBack="true" Width="100"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbLineCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="searchCombo" style="width:250px; font-size:12px;">
                                <input type="text" ID="txtLineCd" runat="server" style="background-color:white; color:black;" readonly/>
                                <asp:TextBox id="txtLineCdHidden" runat="server" style="display:none;"></asp:TextBox>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                    </td>
                    <th><asp:Label ID="lbCarType" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="searchCombo" style="width:250px; font-size:12px;">
                                    <input type="text" ID="txtCarType" runat="server" style="background-color:white; color:black;" readonly/>
                                    <asp:TextBox id="txtCarTypeHidden" runat="server" style="display:none;"></asp:TextBox>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="btnFunctionCall" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <th><asp:Label ID="lbPrintId" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlPrintId" runat="server" Width="250"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="btnFunctionCall" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td class="al-r">
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
                                    <input type="button" value="Set" id="btnSet" onclick="getCol('Info08'); return false;" style="display:none; margin-top:10px; float:left" />
									<div class="al-r total" ondragstart="return false">Total : <div id="rowCnt" class="f02" style="float:right"></div></div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnRestore" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>