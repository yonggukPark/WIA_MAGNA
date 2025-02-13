<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Pln05.aspx.cs" Inherits="HQCWeb.PlanMgt.Pln05.Pln05" %>

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
        }

        .number-column {
            text-align: right;
        }

        .string-column {
            text-align: left;
        }

        .upper{
            text-transform: uppercase;
            padding:15px;
        }

        .buttonStyle{
            padding: 23px;
            padding-bottom: 20px;
            padding-top: 18px;
            margin-left: 10px;
            font-family: 'Malgun Gothic';
            font-size:37px;
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

        //function fn_Modify(pkCode) {
        //    fn_PostOpenPop(pkCode, 'PlanMgt/Pln05/Pln05_p02.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        //}

        function fn_Ship() {
            var rowDatas = [];
            var rows = gridView.getCheckedRows();
            var status = 'N';
            var subStorage, storage, cust, event;

            if(rows.length > 0) {
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
        }

        function fn_AjaxCall(arr) {

            var jsonData = JSON.stringify({ sParams: arr });

            $.ajax({
                type: "POST",
                url: "Pln05.aspx/SetShipNo",
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
            setContextMenu(gridView);
            setPaging();
            settingGrid(_val); if (data != undefined) document.getElementById('rowCnt').innerHTML = data.length;
        }

        function settingGrid(_val) {
            //PK 컬럼 클릭시 동작
            gridView.onCellClicked = function (grid, clickData) {
                //if (clickData.column == 'SHIP_NO') {
                //    var current = gridView.getCurrent();
                //    var value = dataProvider.getValue(current.dataRow, "KEY_HID");
                //    fn_Modify(value);
                //}
            }

            if (_val != undefined) {
                gridView.setFixedOptions({
                    colCount: _val
                });
            }

            gridView.columnByName("KEY_HID").visible = false;
            //gridView.columnByName("ORDER_NO").visible = false;
            //gridView.columnByName("STATUS_FLG").visible = false;
            //gridView.columnByName("EVENT_FLG").visible = false;
            gridView.columnByName("SHIP_NO").styleName = "grid-primary-column"

            gridView.columnByName("CAR_NM_KOR").styleName = "string-column";
            gridView.columnByName("PART_DESC").styleName = "string-column";
            gridView.columnByName("S_STORAGE_NM").styleName = "string-column";
            gridView.columnByName("D_STORAGE_NM").styleName = "string-column";
            gridView.columnByName("CUSTOMER_NM").styleName = "string-column";
            gridView.columnByName("DRIVER_NM").styleName = "string-column";
            gridView.columnByName("TRANS_INFO").styleName = "string-column";

            gridView.columnByName("PLAN_QTY").styleName = "number-column";
            gridView.columnByName("DELIVERY_QTY").styleName = "number-column";

            dataProvider.softDeleting = true;
            //gridView.displayOptions.fitStyle = "even";//그리드 채우기
            gridView.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gridView.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gridView.pasteOptions.enabled = false;//붙여넣기 금지
            gridView.groupPanel.visible = true;//그룹핑 활성화
            gridView.filterPanel.visible = true;//필터패널 활성화

            if (data == undefined || data == null) {
                $("#MainContent_btnSearch").click();
            }
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
            <h3><asp:Label ID="lbTitle" runat="server">Pln05</asp:Label></h3>
            <div class="al-r">
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:fn_Validation();" Visible="false" />
                <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('Pln05'); return false;" />
            </div>
        </div>

        <br />
        <div>
            <table cellpadding="0" cellspacing="0" border="1" align="center" style="border-collapse:collapse; border:1px lightgray solid; height:270px;width:100%;">
                <tr>
                        <td align="center" style="width:30%;font-size:50px;height:70px;background-color:#f1f3f8;border:1px lightgray solid; font-weight:900">
                        출하증번호
                    </td>
                    <td align="center" style="width:60%;font-size:50px; border:1px lightgray solid;" runat="server" id="tdShipNo">
                        <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
							<ContentTemplate>
								<asp:TextBox runat="server" ID="txtShipNo" align="center" Width="100%" Text="" Font-Size="50px" BorderStyle="None" MaxLength="14" CssClass="upper"/>
							</ContentTemplate>
							<Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ibtnRegister" EventName="Click" />
							</Triggers>
						</asp:UpdatePanel>
                                                                
                    </td>
                    <td align="center" style="width:10%;border:1px lightgray solid;">
                        <asp:Button runat="server" text="등록" CssClass="buttonStyle" ID="ibtnRegister" OnClick="ibtnRegister_Click"/>
                    </td>
                </tr>
                <tr>
                    <td align="center" style="width:50%;font-size:40px;" colspan="3">
                        <table>
                            <tr>
                                <td align="center" style="width:50%;font-size:40px;">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
										<ContentTemplate>
											<asp:Label runat="server" ID="lbShipNo" Text="" Font-Bold="true" Font-Size="40px" CssClass="upper"/>
										</ContentTemplate>
										<Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ibtnRegister" EventName="Click" />
										</Triggers>
									</asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="width:50%;font-size:40px;"> 
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
										<ContentTemplate>
											<asp:Label runat="server" ID="lbShipResult" Text="" ForeColor="Red" Font-Bold="true"/>&nbsp;<asp:Label runat="server" ID="lbShipMsg" Font-Bold="true"/>
										</ContentTemplate>
										<Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ibtnRegister" EventName="Click" />
										</Triggers>
									</asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="realgrid" class="realgrid_c_1"></div>
                    <table>
                        <tr>
                            <td>
                                <div class="toolbar">
                                    <div id="gridPage" style="float:left" onselectstart="return false" ondragstart="return false"></div>
                                    &nbsp;
                                    <select id="current_page_value" onchange="setPaging(); return false;" style="font-size:12px; display:none; margin-top:10px; float:left" runat="server">
                                    </select>
                                    <input type="button" value="Set" id="btnSet" onclick="getCol('Pln05'); return false;" style="display:none; margin-top:10px; float:left" />
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