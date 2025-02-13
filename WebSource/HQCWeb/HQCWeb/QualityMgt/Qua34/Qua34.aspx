<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Qua34.aspx.cs" Inherits="HQCWeb.QualityMgt.Qua34.Qua34" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function fn_Validation() {
            fn_WatingCall();
            return true;
        }

        //수정
        function fn_Modify(pkCode) {
            fn_PostOpenPop(pkCode, 'QualityMgt/Qua34/Qua34_p01.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        //판정
        function fn_Confirm(pkCode) {
            fn_PostOpenPop(pkCode, 'QualityMgt/Qua34/Qua34_p02.aspx', $("#MainContent_hidWidth").val(), 300);
        }

        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }

        var column, field, data;
        var container, dataProvider, gridView;

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
                
                if (clickData.cellType !== "data") {
                    return; // 데이터 셀이 아니면 이벤트 중단
                }

                if (clickData.column == 'COMPLETE_NO') {
                    fn_Modify(value);
                }
                else if (clickData.column == 'SHIP_FLAG') {
                    var flag = dataProvider.getValue(current.dataRow, "SHIP_FLAG");
                    if (flag == "완료" || flag == "재판정") {
                        alert("이미 판정 완료된 데이터입니다.");
                        //if (confirm("재판정 하시겠습니까?")) {
                        //    fn_Confirm(value);
                        //}
                    }
                    else {
                        fn_Confirm(value);
                    }
                }
            }

            if (_val != undefined) {
                gridView.setFixedOptions({
                    colCount: _val
                });
            }

            gridView.columnByName("COMPLETE_NO").styleName = "grid-primary-column"
            gridView.columnByName("SHIP_FLAG").styleName = "grid-primary-column"
            gridView.columnByName("KEY_HID").visible = false;
            gridView.columnByName("REG_DATE").visible = false;
            gridView.columnByName("RETURN_KEY").visible = false;

            gridView.columnByName("D_RESP").styleName = "string-column";
            gridView.columnByName("D_RESP_DETAIL").styleName = "string-column";
            gridView.columnByName("START_STORAGE").styleName = "string-column";
            gridView.columnByName("DEST_STORAGE").styleName = "string-column";
            gridView.columnByName("CAR_TYPE").styleName = "string-column";
            gridView.columnByName("D_SYMPTOM").styleName = "string-column";
            gridView.columnByName("D_REASON").styleName = "string-column";
            gridView.columnByName("REWORK_LOG").styleName = "string-column";
            gridView.columnByName("DEST_STORAGE_CD_Q").styleName = "string-column";
            gridView.columnByName("START_STORAGE_CD_Q").styleName = "string-column";
            
            dataProvider.softDeleting = true;
            //gridView.displayOptions.fitStyle = "even";//그리드 채우기
            gridView.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gridView.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gridView.pasteOptions.enabled = false;//붙여넣기 금지
        }
    </script>

    <style>
        .grid-primary-column {
            font-weight: bold;
            text-decoration: underline; cursor: pointer;
        }

        .string-column {
            text-align: left;
        }
    </style>

    <asp:HiddenField ID="hidWidth" runat="server" Value="500" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="650" />

    <div class="contents" tabindex="0">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">Qua34</asp:Label></h3>
            <div class="al-r">
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                <input type="button" value="New" onclick="javascript:fn_Register();" ID="btnNew" runat="server" visible="false" />
                <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('Qua34'); return false;" />
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
	                <col style="width:120px;">
	                <col style="width:60px;">
	                <col style="width:120px;">
	                <col style="width:80px;">
	                <col style="width:120px;">
	                <col style="width:60px;">
	                <col style="width:120px;">
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbReturnDt" runat="server"></asp:Label></th>
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
                    <th><asp:Label ID="lbEvCheck" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlEvCheck" runat="server" Width="100"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbEolCheck" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlEolCheck" runat="server" Width="100"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbShipFlag" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlShipFlag" runat="server" Width="100"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbDRespCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlDRespCd" runat="server" Width="100"></asp:DropDownList>
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
                                    <input type="button" value="Set" id="btnSet" onclick="getCol('Qua34'); return false;" style="display:none; margin-top:10px; float:left" />
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