<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Info43.aspx.cs" Inherits="HQCWeb.InfoMgt.Info43.Info43" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function fn_Register() {
            fn_OpenPop('InfoMgt/Info43/Info43_p01.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        function fn_Modify(pkCode) {
            fn_PostOpenPop(pkCode, 'InfoMgt/Info43/Info43_p02.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        function fn_Copy() {
            if (currentValue != undefined)
                fn_PostOpenPop(currentValue, 'InfoMgt/Info43/Info43_p03.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
            else
                alert("먼저 복사할 행을 선택해주세요.");
        }

        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }

        var column, field, data;
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
                if (clickData.column == 'STORAGE_CD') {
                    fn_Modify(value);
                }
            }

            if (_val != undefined) {
                gridView.setFixedOptions({
                    colCount: _val
                });
            }

            gridView.columnByName("STORAGE_CD").styleName = "grid-primary-column"
            gridView.columnByName("STORAGE_NM").styleName = "string-column"

            gridView.columnByName("REMARK1").styleName = "string-column";
            gridView.columnByName("REMARK2").styleName = "string-column";

            gridView.columnByName("KEY_HID").visible = false;
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
    <asp:HiddenField ID="hidHeight" runat="server" Value="550" />

    <div class="contents" tabindex="0">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">Info43</asp:Label></h3>
            <div class="al-r">
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="fn_WatingCall();" Visible="false" />
                <asp:Button ID="btnRestore" runat="server" Text="Restore" OnClick="btnRestore_Click" OnClientClick="fn_WatingCall();" Visible="false" />
                <input type="button" value="New" onclick="javascript:fn_Register();" ID="btnNew" runat="server" visible="false" />
                <input type="button" value="Copy" onclick="javascript:fn_Copy();" ID="btnCopy" runat="server" visible="false" />
                <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('Info43'); return false;" />
            </div>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:60px;">
                    <col style="width:190px;">
                    <col style="width:60px;">
                    <col style="width:120px;">
                    <col style="width:60px;">
                    <col style="width:120px;">
                    <col>
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbStorageNm" runat="server"></asp:Label></th>
                    <td><asp:TextBox ID="txtStorageNm" runat="server" style="width:150px;"></asp:TextBox></td>
                    <th><asp:Label ID="lbStorageType" runat="server"></asp:Label></th>
                    <td><asp:DropDownList ID="ddlStorageType" runat="server" Width="100"></asp:DropDownList></td>
                    <th><asp:Label ID="lbUseYn" runat="server"></asp:Label></th>
                    <td><asp:DropDownList ID="ddlUseYn" runat="server" Width="100"></asp:DropDownList></td>
                    <td class="al-r">
                        <%--<asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                        <asp:Button ID="btnRestore" runat="server" Text="Restore" OnClick="btnRestore_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                        <input type="button" value="New" onclick="javascript:fn_Register();" ID="btnNew" runat="server" visible="false" />
                        <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('GridExcel'); return false;" />--%>
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
                                    <input type="button" value="Set" id="btnSet" onclick="getCol('Info43'); return false;" style="display:none; margin-top:10px; float:left" />
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