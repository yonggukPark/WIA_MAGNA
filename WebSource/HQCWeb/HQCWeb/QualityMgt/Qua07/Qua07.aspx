<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Qua07.aspx.cs" Inherits="HQCWeb.QualityMgt.Qua07.Qua07" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function fn_Validation() {
            if ($("#MainContent_ddlShopCd").val() == "") {
                alert("Shop Code를 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlLineCd").val() == "") {
                alert("라인을 선택하세요.");
                return false;
            } else {
                fn_WatingCall();
                return true;
            }
            return true;
        }

        function fn_Register() {
            fn_OpenPop('팝업창 파일 풀 경로', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        function fn_Modify(pkCode) {
            fn_PostOpenPop(pkCode, '팝업창 파일 풀 경로', $("#MainContent_hidWidth").val(), $("#MainContent_hidHeight").val());
        }

        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }

        var column, field, data;
        var container, dataProvider, gridView;
        var column2, field2, data2;
        var container2, dataProvider2, gridView2;

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
            settingGrid(_val); if (data != undefined) document.getElementById('rowCnt1').innerHTML = data.length;
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
                if (clickData.column == 'ORDER_NO') {
                    fn_Modify(value);
                }
            }

            if (_val != undefined) {
                gridView.setFixedOptions({
                    colCount: _val
                });
            }

            gridView.columnByName("ORDER_NO").styleName = "grid-primary-column"
            gridView.columnByName("KEY_HID").visible = false;
            dataProvider.softDeleting = true;
            //gridView.displayOptions.fitStyle = "even";//그리드 채우기
            gridView.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gridView.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gridView.pasteOptions.enabled = false;//붙여넣기 금지
        }

        // 그리드 생성
        function createDetailGrid(_val) {
            container2 = document.getElementById('realDetailgrid');
            dataProvider2 = new RealGrid.LocalDataProvider(false);
            gridView2 = new RealGrid.GridView(container2);
            gridView2.setDataSource(dataProvider2);
            gridView2.setColumns(column2);
            dataProvider2.setFields(field2);
            dataProvider2.setRows(data2);
            gridView2.checkBar.visible = false;
            gridView2.stateBar.visible = false;
            setContextMenu(gridView2);
            setPaging2();
            settingDetailGrid(_val); if (data != undefined) document.getElementById('rowCnt2').innerHTML = data.length;
        }

        function settingDetailGrid(_val) {
            //PK 컬럼 클릭시 동작
            gridView2.onCellClicked = function (grid, clickData) {

                if (clickData.cellType !== "data") {
                    return; // 데이터 셀이 아니면 이벤트 중단
                }

                if (clickData.column == 'CERT_NO') {
                    var current = gridView2.getCurrent();
                    var value = dataProvider2.getValue(current.dataRow, "KEY_HID");
                    fn_Modify2(value);
                }
            }

            if (_val != undefined) {
                gridView2.setFixedOptions({
                    colCount: _val
                });
            }

            gridView2.columnByName("STATION").styleName = "grid-primary-column"
            gridView2.columnByName("KEY_HID").visible = false;
            dataProvider2.softDeleting = true;
            //gridView.displayOptions.fitStyle = "even";//그리드 채우기
            gridView2.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView2.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gridView2.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gridView2.pasteOptions.enabled = false;//붙여넣기 금지
        }

    </script>

    <style>
        .grid-primary-column {
            color: blue;
            font-weight: bold;
        }
    </style>

    <asp:HiddenField ID="hidWidth" runat="server" Value="500" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="500" />
    <asp:HiddenField ID="hidParams" runat="server" />

    <div class="contents">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">Qua07</asp:Label></h3>
            <div class="al-r">
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                <asp:Button ID="btnRestore" runat="server" Text="Restore" OnClick="btnRestore_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                <input type="button" value="New" onclick="javascript:fn_Register();" ID="btnNew" runat="server" visible="false" />
                <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('GridExcel'); return false;" />
            </div>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:50px;" />
                    <col style="width:150px;" />
                    <col style="width:420px;" />
                    <col style="width:50px;" />
                    <col style="width:370px;" />
                    <col style="width:50px;" />
                    <col style="width:190px;" />
                    <col />
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbLineCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlLineCd" runat="server" AutoPostBack="false"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                            <ContentTemplate>
                            <asp:TextBox ID="txtFromDt" runat="server" style="IME-MODE:disabled; background-color:white; color:black; padding-left:8px; padding-right:8px;"></asp:TextBox> 
                            <span id="spBetween">~</span>
                            <asp:TextBox ID="txtToDt" runat="server" style="background-color:white; color:black; padding-left:8px; padding-right:8px;"></asp:TextBox>
                            <asp:DropDownList ID="ddlWctCd" runat="server" OnSelectedIndexChanged="ddlWct_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlWctCd" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <th><asp:Label ID="lbLotNo" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtFromLot" runat="server" Text="0000"></asp:TextBox>~
                        <asp:TextBox ID="textToLot" runat="server" Text="9999"></asp:TextBox>
                    </td>
                    <th><asp:Label ID="lbCond_01" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="TextBox1" runat="server" placeholder="바코드를 3자리 이상 입력해주세요."></asp:TextBox>
                    </td>
                    <td class="al-r">
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div>
            <table>
                <tr>
                    <td>
                        <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div id="realgrid"  class="realgrid"></div>
                                            <div class="toolbar">
                                                <div id="gridPage" style="float:left" onselectstart="return false" ondragstart="return false"></div>
                                                &nbsp;
                                                <select id="current_page_value" onchange="setPaging(); return false;" style="font-size:12px; display:none; margin-top:10px; float:left" runat="server">
                                                </select>
                                                <input type="button" value="Set" id="btnSet" onclick="getCol('Qua07'); return false;" style="display:none; margin-top:10px; float:left" />
									            <div class="al-r total" ondragstart="return false">Total : <div id="rowCnt1" class="f02" style="float:right"></div></div>
                                            </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnRestore" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 30px;"></td>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div id="realDetailgrid"  class="realgrid"></div>
                                            <div class="toolbar">
                                                <div id="gridPage2" style="float:left" onselectstart="return false" ondragstart="return false"></div>
                                                &nbsp;
                                                <select id="current_page_value1" onchange="setPaging(); return false;" style="font-size:12px; display:none; margin-top:10px; float:left" runat="server">
                                                </select>
                                                <input type="button" value="Set" id="btnSet1" onclick="getCol('Qua07_1'); return false;" style="display:none; margin-top:10px; float:left" />
									            <div class="al-r total" ondragstart="return false">Total : <div id="rowCnt2" class="f02" style="float:right"></div></div>
                                            </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnRestore" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
    </div>

</asp:Content>