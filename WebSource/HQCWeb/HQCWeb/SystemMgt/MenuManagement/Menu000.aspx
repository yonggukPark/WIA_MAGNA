<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Menu000.aspx.cs" Inherits="HQCWeb.SystemMgt.MenuManagement.Menu000" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        function fn_Register_BCate() {
            fn_OpenPop("/SystemMgt/MenuManagement/Menu001.aspx", "610", "370");
        }

        function fn_Modify(pkCode) {
            fn_PostOpenPop(pkCode, '/SystemMgt/MenuManagement/Menu003.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        function fn_Register_GCate() {
            fn_OpenPop("/SystemMgt/MenuManagement/Menu002.aspx", "610", $("#MainContent_hidHeight").val());
        }

        function fn_Function() {
            fn_OpenPop("/SystemMgt/MenuManagement/Menu004.aspx", "670", "670");
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

            dataProvider.setFields(field);
            gridView.setColumns(column);
            //gridView.setColumns(col2);
            
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
                if (clickData.column == 'MENU_ID') {
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

            gridView.columnByName("MENU_ID").styleName = "grid-primary-column"
            gridView.columnByName("MENU_NM").styleName = "string-column"
            gridView.columnByName("ASSEMBLY_ID").styleName = "string-column"
            gridView.columnByName("VIEW_ID").styleName = "string-column"
            gridView.columnByName("KEY_HID").visible = false;
            dataProvider.softDeleting = true;
            ////gridView.displayOptions.fitStyle = "even";//그리드 채우기
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

    <asp:HiddenField ID="hidWidth" runat="server" Value="610" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="550" />

    <div class="contents" tabindex="0">
    
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">WEB_00010</asp:Label></h3>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:90px;" />
                    <col style="width:190px;" />
                    <col style="width:90px;" />
                    <col style="width:190px;" />
                    <col />
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbMenuID" runat="server"></asp:Label></th>
                    <td><asp:TextBox ID="txtMenuID" runat="server" style="width:150px;"></asp:TextBox></td>
                    <th><asp:Label ID="lbMenuNM" runat="server"></asp:Label></th>
                    <td><asp:TextBox ID="txtMenuNM" runat="server" style="width:150px;"></asp:TextBox></td>
                    <td class="al-r">
                        <asp:Literal ID="ltButtonList" runat="server"></asp:Literal>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="fn_WatingCall();" Visible="false" />
                        <input type="button" value="대메뉴"         OnClick="javascript:fn_Register_BCate();" ID="btnBCate" runat="server" visible="false"  />
                        <input type="button" value="일반메뉴"       OnClick="javascript:fn_Register_GCate();" ID="btnGCate" runat="server" visible="false" />
                        <input type="button" value="메뉴기능관리"   OnClick="javascript:fn_Function();" ID="btnFunction" runat="server" visible="false"   />
                        <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('GridExcel'); return false;" />
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
                                    <input type="button" value="Set" id="btnSet" onclick="getCol('WEB_00010'); return false;" style="display:none; margin-top:10px; float:left" />
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