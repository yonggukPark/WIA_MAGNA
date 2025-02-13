<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Lms11.aspx.cs" Inherits="HQCWeb.LmsMgt.Lms11.Lms11" %>

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
                if (clickData.column == 'PART_NO') {
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

            gridView.columnByName("PART_NO").styleName = "grid-primary-column"
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
            container = document.getElementById('realDetailgrid');
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
            settingDetailGrid(_val); if (data != undefined) document.getElementById('rowCnt2').innerHTML = data.length;
        }

        function settingDetailGrid(_val) {
            //PK 컬럼 클릭시 동작
            gridView.onCellClicked = function (grid, clickData) {
                if (clickData.column == 'STATION') {
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

            gridView.columnByName("STATION").styleName = "grid-primary-column"
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
            color: blue;
            font-weight: bold;
        }
    </style>

    <asp:HiddenField ID="hidWidth" runat="server" Value="500" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="500" />

    <div class="contents">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">Lms11</asp:Label></h3>
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
                    <col style="width:100px;" />
                    <col style="width:190px;" />
                    <col />
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbCond_01" runat="server"></asp:Label></th>
                    <td><asp:RadioButton ID="RadioButton1" runat="server" Text="계획일자"    AutoPostBack="false" GroupName="gubunGrp" Width="70px" />
                        <asp:RadioButton ID="RadioButton2" runat="server" Text="제품번호"    AutoPostBack="false" GroupName="gubunGrp" Width="80px" />
                    </td>
                    <th>
                    </th>
                    <th><asp:Label ID="lbLineCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlLineCd" runat="server" AutoPostBack="false" Width="250px"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbProdDt" runat="server"></asp:Label></th>
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
                    <th><asp:Label ID="lbLotNo" runat="server"></asp:Label></th>
                    <td>
                        <td>
                            <asp:TextBox ID="txtFromLot" runat="server" style="width:75px;" Text="0000"></asp:TextBox>~
                            <asp:TextBox ID="textToLot" runat="server" style="width:75px;" Text="9999"></asp:TextBox>
                        </td>
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
                                                <input type="button" value="Set" id="btnSet" onclick="getCol('Lms11'); return false;" style="display:none; margin-top:10px; float:left" />
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
                                                <input type="button" value="Set" id="btnSet1" onclick="getCol('Lms11_1'); return false;" style="display:none; margin-top:10px; float:left" />
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