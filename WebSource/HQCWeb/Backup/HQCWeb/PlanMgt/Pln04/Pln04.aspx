<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Pln04.aspx.cs" Inherits="HQCWeb.PlanMgt.Pln04.Pln04" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .searchCombo{
            text-align: center;
            z-index : 1;
            top : 8%;
            position : absolute; 
        }

        .grid-primary-column {
            color: blue;
            font-weight: bold;
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

        function fn_Modify(pkCode) {
            fn_PostOpenPop(pkCode, 'PlanMgt/Pln04/Pln04_p02.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        function fn_Ship() {
            var rowDatas = [];
            var rows = gridView.getCheckedRows();
            var status = 'N';
            var subStorage, storage, cust, event;

            for (var i in rows) {
                var data = dataProvider.getJsonRow(rows[i]);

                if (data.SHIP_NO.length > 0) {
                    alert("이미 출하증이 발행되어 생성할 수 없습니다.");
                    return false;
                }
                else if (parseInt(data.DELIVERY_QTY) == 0) {
                    alert("실적수량이 있는 경우에만 출하증 발행이 가능합니다.");
                    return false;
                }
                else if(i==0) {
                    subStorage = data.S_STORAGE_NM;
                    storage = data.STORAGE_NM;
                    cust = data.CUSTOMER_NM;
                    event = data.EVENT_FLG;
                }
                else if (data.S_STORAGE_NM != subStorage || data.STORAGE_NM != storage || data.CUSTOMER_NM != cust || data.EVENT_FLG != event) {
                    alert("출고창고/대상창고/고객사/계획형태 정보가 상이합니다.");
                    return false;
                }

                if (data.STATUS_FLG == 'R')
                    status = 'Y'

                rowDatas.push(data.KEY_HID);//암호화된 데이터 배열 추가
            }

            if (status == 'Y') {
                status = "미완료 건이 포함되어 있습니다. 그래도 ";
            }
            else {
                status = "";
            }

            if (confirm(status+"출하증을 발행하시겠습니까?")) {
                fn_WatingCall();
                fn_AjaxCall(rowDatas);
            } else {
                return false;
            }
        }

        function fn_AjaxCall(arr) {

            var jsonData = JSON.stringify({ sParams: arr });

            $.ajax({
                type: "POST",
                url: "Pln04.aspx/SetShipNo",
                data: jsonData,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    fn_Return(msg.d);
                }
            });
        }

        function fn_Return(_vals) {
            if (_vals != "OK") {
                alert(_vals);
            }
            else {
                alert("출하번호 발행이 완료되었습니다.");
                $("#MainContent_btnSearch").click();
            }
            fn_loadingEnd();
        }

        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }

        var column, field, data;
        var container, dataProvider, gridView;
        var cShop, cPart;

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
            setContextMenu(gridView);
            setPaging();
            settingGrid(_val);
        }

        function settingGrid(_val) {
            //PK 컬럼 클릭시 동작
            gridView.onCellClicked = function (grid, clickData) {
                if (clickData.column == 'SHIP_NO') {
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

            gridView.columnByName("KEY_HID").visible = false;
            gridView.columnByName("ORDER_NO").visible = false;
            gridView.columnByName("STATUS_FLG").visible = false;
            gridView.columnByName("EVENT_FLG").visible = false;
            gridView.columnByName("SHIP_NO").styleName = "grid-primary-column"
            dataProvider.softDeleting = true;
            //gridView.displayOptions.fitStyle = "even";//그리드 채우기
            gridView.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gridView.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gridView.pasteOptions.enabled = false;//붙여넣기 금지
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
    
    <style>
    </style>

    <asp:HiddenField ID="hidWidth" runat="server" Value="530" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="250" />

    <div class="contents">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">Pln04</asp:Label></h3>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:80px;" />
                    <col style="width:200px;" />
                    <col style="width:80px;" />
                    <col style="width:240px;" />
                    <col style="width:80px;" />
                    <col style="width:100px;" />
                    <col style="width:80px;" />
                    <col style="width:210px;" />
                    <col style="width:80px;" />
                    <col style="width:220px;" />
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbPlanDt" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtFromDt" runat="server" Width="80" onkeydown="return fn_EventKeyChk(event);" onkeyup="removeChar(event);" style="IME-MODE:disabled; background-color:white; color:black;"></asp:TextBox> 
                        <span id="spBetween">~</span>
                        <asp:TextBox ID="txtToDt" runat="server" Width="80" onkeydown="return fn_EventKeyChk(event);" onkeyup="removeChar(event);" style="background-color:white; color:black;"></asp:TextBox>
                    </td>
                    <th><asp:Label ID="lbShopCd" runat="server"></asp:Label></th>
                    <td>
                        <div class="searchCombo" style="width:120px; font-size:12px;">
                            <input type="text" ID="txtShopCd" runat="server" style="background-color:white; color:black;" readonly/>
                            <asp:TextBox id="txtShopCdHidden" runat="server" style="display:none;"></asp:TextBox>
                        </div>
                    </td>
                     <th><asp:Label ID="lbPartNo" runat="server"></asp:Label></th>
                    <td colspan = "3">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="searchCombo" style="width:355px; font-size:12px;">
                                    <input type="text" ID="txtPartNo" runat="server" style="background-color:white; color:black;" readonly/>
                                    <asp:TextBox id="txtPartNoHidden" runat="server" style="display:none;"></asp:TextBox>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnFunctionCall" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <th><asp:Label ID="lbStatus" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlStatus" runat="server" ></asp:DropDownList>
                    </td>
                    <td class="al-r">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:fn_Validation();" Visible="false" />
                        <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('Pln04'); return false;" />
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbShipNo" runat="server"></asp:Label></th>
                    <td><asp:TextBox ID="txtShipNo" runat="server" Width="180" ></asp:TextBox></td>
                    <th><asp:Label ID="lbCustomerCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlCustomerCd" runat="server" ></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbPlanType" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlPlanType" runat="server" ></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbSubStorageCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlSubStorageCd" runat="server" ></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbStorageCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlStorageCd" runat="server" ></asp:DropDownList>
                    </td>
                    <td class="al-r">
                        <input type="button" value="Shipment" onclick="javascript:fn_Ship();" ID="btnNew" runat="server" visible="false" />
                        <asp:Button ID="btnFunctionCall" runat="server" Text="FunctionCall" OnClick="btnFunctionCall_Click"  style="display:none;"  />
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div>
            <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="realgrid" style="width: 100%; height: 440px;"></div>
                    <table>
                        <tr>
                            <td>
                                <div class="toolbar">
                                    <div id="gridPage" style="float:left" onselectstart="return false" ondragstart="return false"></div>
                                    &nbsp;
                                    <select id="current_page_value" onchange="setPaging(); return false;" style="font-size:12px; display:none; margin-top:2px;" runat="server">
                                    </select>
                                    <input type="button" value="Set" id="btnSet" onclick="getCol('Pln04'); return false;" style="display:none; margin-top:2px;" />
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