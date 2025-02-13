<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="PublicDefaultSample2.aspx.cs" Inherits="HQCWeb.SystemMgt.PublicDefaultSample2" %>

<%@ Register Src="~/UserContorls/GridControl.ascx" TagPrefix="uc1" TagName="GridControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function fn_Register() {
            fn_OpenPop('/SystemMgt/PublicPopupSample1.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        function fn_Modify(pkCode) {
            console.log("pkCode=", pkCode);
            fn_PostOpenPop(pkCode, '/SystemMgt/PublicPopupSample2.aspx', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }

        var column, field, data;
        var container, dataProvider, gridView;

        // 그리드 생성
        function createGrid() {
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
            settingGrid();
        }

        //세션 및 화면마다 다른 세팅
        function settingGrid() {
            //그리드 컬럼 변경시 감지
            //gridView.onColumnPropertyChanged = function (grid, column, property, newValue, oldValue) {

            //    console.log('column : ' + column + ' property : ' + property + ' newValue : ' + newValue + ' oldValue : ' + oldValue);
            //}
            //gridView.columnByName("DIC_ID").width = 150; 
            //gridView.layoutByColumn("DIC_ID").vindex = 3;

            //PK 컬럼 클릭시 동작
            gridView.onCellClicked = function (grid, clickData) {
                if (clickData.column == 'DIC_ID') {
                    var current = gridView.getCurrent();
                    var value = dataProvider.getValue(current.dataRow, "KEY_HID");
                    fn_Modify(value);
                }
            }

            gridView.columnByName("DIC_ID").styleName = "grid-primary-column"
            dataProvider.softDeleting = true;
            gridView.displayOptions.fitStyle = "even";//그리드 채우기
            gridView.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gridView.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gridView.pasteOptions.enabled = false;//붙여넣기 금지
            gridView.columnByName("KEY_HID").visible = false;

            //gridView.columnByName("REGIST_DATE").editable = false;
            //gridView.columnByName("REGIST_USER_ID").editable = false;
            //gridView.columnByName("MODIFY_DATE").editable = false;
            //gridView.columnByName("MODIFY_USER_ID").editable = false;
            //let col = gridView.columnByName('DIC_ID');
            //col.styleCallback = f1;
        }

        //function addEmptyRow() { // 끝으로 새행 추가
        //    var dataRow = dataProvider.addRow({});
        //    $('#gridPage').pagination($('#gridPage').pagination('getTotalPage'));//페이지 이동
        //    gridView.setCurrent({ dataRow: dataRow }); //추가된 행으로 포커스 이동
        //    setTimeout(function(){gridView.showEditor();}, 10); //바로 편집기를 표시하고 싶을때
        //}

        //function insertEmptyRow() { // 포커스된 곳으로 새행 추가
        //    var row = gridView.getCurrent().dataRow;
        //    dataProvider.insertRow(row, {});
        //    gridView.showEditor(); //바로 편집기를 표시하고 싶을때
        //}

        //javascript function f1 : 컬럼 신규입력 구분후 잠금처리
        //const f1 = function (grid, dataCell) {
        //    var ret = {}

        //    if(dataCell.item.rowState == 'created' || dataCell.item.itemState == 'appending' || dataCell.item.itemState == 'inserting'){
        //        ret.editable = true;
        //    } else {
        //        ret.editable = false;
        //    }

        //    return ret;
        //}

        <%--function fn_Save() {
            var rows = dataProvider.getAllStateRows();

            // 객체의 모든 값들을 배열로 추출
            const values = Object.values(rows);

            // 추출된 배열들을 하나의 배열로 평탄화
            const rowArr = values.flat();

            var updated = [];
            for (var i = 0; i < rowArr.length; i++) {
                updated.push(dataProvider.getJsonRow(rowArr[i]));
            }

            let JSONstring = JSON.stringify(updated);

            document.getElementById('<%= hidupdateJSON.ClientID %>').value = JSONstring;
        }--%>

    </script>
    <style>
    .grid-primary-column {
        color: blue;
        font-weight: bold;
    }

    </style>
    <asp:HiddenField ID="hidWidth" runat="server" Value="500" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="500" />

    <asp:TextBox ID="hidGridHeight" runat="server" Style="display: none;" />
    <asp:TextBox ID="hidScreenType" runat="server" Style="display: none;" />

    <asp:HiddenField ID="hidupdateJSON" runat="server" />
    <div class="contents">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3>
                <asp:Label ID="lbTitle" runat="server">WEB_11001</asp:Label></h3>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:100px;" />
                    <col style="width:190px;" />
                    <col style="width:100px;" />
                    <col style="width:190px;" />
                    <col />
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbDictionaryID" runat="server"></asp:Label></th>
                    <td><asp:TextBox ID="txtDictionaryID" runat="server" Style="width: 150px;"></asp:TextBox></td>
                    <th><asp:Label ID="lbDictionaryNM" runat="server">ddddd</asp:Label></th>
                    <td><asp:TextBox ID="txtDictionaryNM" runat="server" Style="width: 150px;"></asp:TextBox></td>
                    <td class="al-r">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="fn_WatingCall();" Visible="false" />
                        <%--<input type="button" value="New" id="btnNew" runat="server" visible="false" onclick="addEmptyRow(); return false;" />--%>
                        <input type="button" value="New" onclick="javascript:fn_Register();" ID="btnNew" runat="server" visible="false" />
                        <%--<asp:Button ID="btnSave" runat="server" OnClientClick="fn_Save();fn_WatingCall();" OnClick="btnSave_Click" Text="Save" />--%>
                        <input type="button" value="Excel" id="btnExcel2" runat="server" visible="true" onclick="fn_excelExport('GridExcel'); return false;" />
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
                                    <select id="current-page-value" onchange="setPaging(); return false;" style="font-size:14px; display:none;">
                                        <option>20</option>
                                        <option>50</option>
                                        <option>100</option>
                                        <option>200</option>
                                    </select>
                                    <input type="button" value="Set" id="btnSet" onclick="getCol('WEB_11001'); return false;"  style="display:none;"/>
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                    <%--<asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />--%>
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>
