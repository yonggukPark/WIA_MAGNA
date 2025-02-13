<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Res28.aspx.cs" Inherits="HQCWeb.ResultMgt.Res28.Res28" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .searchCombo{
            text-align: center;
            z-index : 1;
            top : 6.5%;
            position : absolute; 
        }

        .grid-primary-column {
            font-weight: bold;
            text-decoration: underline; cursor: pointer;
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
            if ($("#MainContent_txtShopCdHidden").val() == "") {
                alert("Shop Code를 선택하세요.");
                return false;
            } else if ($("#MainContent_txtPartNoHidden").val() == "") {
                alert("품번을 선택하세요.");
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

        var column, field, data;
        var container, dataProvider, gridView;
        var cShop, cPart;

        // 그리드 생성
        function createGrid(_val) {
            container = document.getElementById('realgrid_2');
            dataProvider = new RealGrid.LocalDataProvider(false);
            gridView = new RealGrid.GridView(container);
            gridView.setDataSource(dataProvider);
            gridView.setColumns(column);

            //field 넣기 전 포맷변경(엑셀 문제)
            field.forEach(item => {
                if (item.fieldName == 'PLAN_QTY' || item.fieldName == 'DELIVERY_QTY') {
                    item.dataType = 'number'; item.subType = 'int'
                }
            });

            //숫자 포맷 변경
            gridView.setFormatOptions({ numberFormat: '#,##0' });

            dataProvider.setFields(field);
            dataProvider.setRows(data);
            gridView.checkBar.visible = false;
            gridView.stateBar.visible = false;
            //gridView.setCheckBar({useImages : true});
            setContextMenu(gridView);
            setPaging();
            settingGrid(_val); if (data != undefined) document.getElementById('rowCnt').innerHTML = data.length;
        }

        function settingGrid(_val) {

            if (gridView.columnByName("KEY_HID") != undefined)
                gridView.columnByName("KEY_HID").visible = false;

            if (_val != undefined) {
                gridView.setFixedOptions({
                    colCount: _val
                });
            }

            gridView.columnByName("출고창고").styleName = "string-column";
            gridView.columnByName("대상창고").styleName = "string-column";
            gridView.columnByName("고객사").styleName = "string-column";
            gridView.columnByName("운전자").styleName = "string-column";
            gridView.columnByName("운송차량").styleName = "string-column";

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
            fn_Set_Part();
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
    </script>

    <asp:HiddenField ID="hidWidth" runat="server" Value="530" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="250" />

    <div class="contents" tabindex="0">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">Res28</asp:Label></h3>
            <div class="al-r">
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:fn_Validation();" Visible="false" />
                <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('Res28'); return false;" />
                <asp:Button ID="btnFunctionCall" runat="server" Text="FunctionCall" OnClick="btnFunctionCall_Click"  style="display:none;"  />
            </div>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:70px;">
                    <col style="width:440px;">
                    <col style="width:70px;">
                    <col style="width:130px;">
                    <col style="width:60px;">
                    <col style="width:130px;">
                    <col style="width:60px;">
                    <col style="width:120px;">
                    <col style="width:70px;">
                    <col style="width:170px;">
                    <col style="width:70px;">
                    <col style="width:170px;">
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbPlanDt" runat="server"></asp:Label></th>
                    <td><asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
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
                    <td colspan = "3">
                        <div class="searchCombo" style="width:150px; font-size:12px;">
                            <input type="text" ID="txtShopCd" runat="server" style="background-color:white; color:black;" readonly/>
                            <asp:TextBox id="txtShopCdHidden" runat="server" style="display:none;"></asp:TextBox>
                        </div>
                    </td>
                     <th><asp:Label ID="lbPartNo" runat="server"></asp:Label></th>
                    <td colspan = "3">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="searchCombo" style="width:400px; font-size:12px;">
                                    <input type="text" ID="txtPartNo" runat="server" style="background-color:white; color:black;" readonly/>
                                    <asp:TextBox id="txtPartNoHidden" runat="server" style="display:none;"></asp:TextBox>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnFunctionCall" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <th><asp:Label ID="lbSubStorageCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlSubStorageCd" runat="server" Width="150"></asp:DropDownList>
                    </td>
                    <td class="al-r">
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbGubun" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDiv" runat="server" ></asp:DropDownList>
                        <asp:TextBox ID="txtIdNo" runat="server" Width="240" ></asp:TextBox>
                    </td>
                    <th><asp:Label ID="lbStatus" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlStatus" runat="server" Width="100"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbPlanType" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlPlanType" runat="server" Width="100"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbDriverCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDriverCd" runat="server" Width="150" OnSelectedIndexChanged="ddlDriverCd_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbCarCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                            <ContentTemplate>
                        <asp:DropDownList ID="ddlCarCd" runat="server" Width="150" ></asp:DropDownList>
                                </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlDriverCd" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <th><asp:Label ID="lbStorageCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlStorageCd" runat="server" Width="150"></asp:DropDownList>
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
                    <div id="realgrid_2" class="realgrid_2"></div>
                    <table>
                        <tr>
                            <td>
                                <div class="toolbar">
                                    <div id="gridPage" style="float:left" onselectstart="return false" ondragstart="return false"></div>
                                    &nbsp;
                                    <select id="current_page_value" onchange="setPaging(); return false;" style="font-size:12px; display:none; margin-top:10px; float:left" runat="server">
                                    </select>
                                    <input type="button" value="Set" id="btnSet" onclick="getCol('Res28'); return false;" style="display:none; margin-top:10px; float:left" />
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