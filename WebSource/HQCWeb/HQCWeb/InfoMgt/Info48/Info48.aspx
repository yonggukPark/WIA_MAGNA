<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Info48.aspx.cs" Inherits="HQCWeb.InfoMgt.Info48.Info48" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function fn_Register() {
            fn_OpenPop('InfoMgt/Info48/Info48_p01.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        function fn_Modify(pkCode) {
            fn_PostOpenPop(pkCode, 'InfoMgt/Info48/Info48_p02.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        function fn_Copy() {
            if (currentValue != undefined)
                fn_PostOpenPop(currentValue, 'InfoMgt/Info48/Info48_p03.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
            else
                alert("먼저 복사할 행을 선택해주세요.");
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
                currentValue = value;

                if (clickData.cellType !== "data") {
                    return; // 데이터 셀이 아니면 이벤트 중단
                }
                if (clickData.column == 'LINE_GROUP_CD') {
                    fn_Modify(value);
                }
            }

            if (_val != undefined) {
                gridView.setFixedOptions({
                    colCount: _val
                });
            }

            gridView.columnByName("LINE_GROUP_CD").styleName = "grid-primary-column"
            gridView.columnByName("LINE_GROUP_NM").styleName = "string-column"
            gridView.columnByName("LINE_NM").styleName = "string-column"

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
    <asp:HiddenField ID="hidHeight" runat="server" Value="380" />

    <div class="contents" tabindex="0">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">Info48</asp:Label></h3>
            <div class="al-r">
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="fn_WatingCall();" Visible="false" />
                <asp:Button ID="btnRestore" runat="server" Text="Restore" OnClick="btnRestore_Click" OnClientClick="fn_WatingCall();" Visible="false" />
                <input type="button" value="New" onclick="javascript:fn_Register();" ID="btnNew" runat="server" visible="false" />
                <input type="button" value="Copy" onclick="javascript:fn_Copy();" ID="btnCopy" runat="server" visible="false" />
                <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('Info48'); return false;" />
            </div>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:80px;" />
                    <col style="width:270px;" />
                    <col />
                </colgroup>
                <tr>
                     <th><asp:Label ID="lbLineGrpCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlLineGrpCd" runat="server" Width="250"></asp:DropDownList>
                    </td>
                    <td class="al-r">
                        <%--<asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="fn_WatingCall();" Visible="false" />
                        <asp:Button ID="btnRestore" runat="server" Text="Restore" OnClick="btnRestore_Click" OnClientClick="fn_WatingCall();" Visible="false" />
                        <input type="button" value="New" onclick="javascript:fn_Register();" ID="btnNew" runat="server" visible="false" />
                        <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('Info48'); return false;" />--%>
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
                                    <input type="button" value="Set" id="btnSet" onclick="getCol('Info48'); return false;" style="display:none; margin-top:10px; float:left" />
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