<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="AuthGroup000.aspx.cs" Inherits="HQCWeb.SystemMgt.AuthGroupManagement.AuthGroup000" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var column, field, data;
        var container, dataProvider, gridView;
                
        function fn_Register() {
            fn_OpenPop('/SystemMgt/AuthGroupManagement/AuthGroup001.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        function fn_Modify(pkCode) {
            fn_PostOpenPop(pkCode, '/SystemMgt/AuthGroupManagement/AuthGroup002.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        function fn_AuthGroupMenu_Register() {
            var items = gridView.getCheckedItems();

            if (items == "") {
                alert("권한 설정할 그룹을 선택하세요.");
                return;
            }           

            var value = dataProvider.getValue(items, "KEY_HID");
          
            fn_PostOpenPop(value, '/SystemMgt/AuthGroupManagement/AuthGroup003.aspx', "1200", "700");
        }

        function fn_AuthGroupUser_Register() {
            var items = gridView.getCheckedItems();

            if (items == "") {
                alert("권한 설정할 그룹을 선택하세요.");
                return;
            }

            var value = dataProvider.getValue(items, "KEY_HID");

            fn_PostOpenPop(value, '/SystemMgt/AuthGroupManagement/AuthGroup004.aspx', "600", "750");
        }

        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }

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

            gridView.setCheckBar({
                visible: true
                , exclusive: true
            });

            setContextMenu(gridView);
            setPaging();
            settingGrid(_val); if (data != undefined) document.getElementById('rowCnt').innerHTML = data.length;
        }

        function settingGrid(_val) {
            //PK 컬럼 클릭시 동작
            gridView.onCellClicked = function (grid, clickData) {
                if (clickData.column == 'AUTHGROUP_ID') {
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

            gridView.columnByName("AUTHGROUP_ID").styleName = "grid-primary-column"
            gridView.columnByName("AUTHGROUP_TXT_KR").styleName = "string-column"
            gridView.columnByName("AUTHGROUP_TXT_EN").styleName = "string-column"
            gridView.columnByName("AUTHGROUP_TXT_LO").styleName = "string-column"
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
    <asp:HiddenField ID="hidHeight" runat="server" Value="330" />

    <div class="contents" tabindex="0">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">WEB_00090</asp:Label></h3>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:140px;" />
                    <col style="width:190px;" />
                    <col style="width:140px;" />
                    <col style="width:190px;" />
                    <col />
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbAuthGroupID" runat="server"></asp:Label></th>
                    <td><asp:TextBox ID="txtAuthGroupID" runat="server" style="width:150px;"></asp:TextBox></td>
                    <th><asp:Label ID="lbAuthGroupNM" runat="server"></asp:Label></th>
                    <td><asp:TextBox ID="txtAuthGroupNM" runat="server" style="width:150px;"></asp:TextBox></td>
                    <td class="al-r">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="fn_WatingCall();" Visible="false" />
                        <input type="button" value="New" onclick="javascript:fn_Register();" ID="btnNew" runat="server" visible="false" />
                        <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('GridExcel'); return false;" />
                        <input type="button" value="권한 그룹 메뉴관리" onclick="javascript: fn_AuthGroupMenu_Register();" ID="btnAuthGroupMenu" runat="server" style="width:140px;" />
                        <input type="button" value="권한 그룹 사용자관리" onclick="javascript: fn_AuthGroupUser_Register();" ID="btnAuthGroupUser" runat="server" style="width:140px;" />  <%--display:none;--%>
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
                                    <input type="button" value="Set" id="btnSet" onclick="getCol('WEB_09000'); return false;" style="display:none; margin-top:10px; float:left" />
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