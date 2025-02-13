<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="ResComm.aspx.cs" Inherits="HQCWeb.ResultMgt.ResComm.ResComm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        // 제품추적 이동?
        function fn_Move(pkCode) {

            var aData = "";
            aData = "Qua98";

            var jsonData = JSON.stringify({ sMenu: aData, sData:pkCode });

            $.ajax({
                type: "POST",
                url: "ResComm.aspx/GetMenu",
                data: jsonData,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    fn_Search_Open(msg.d);
                }
            });
        }

        function fn_Search_Open(title) {
            if (title == "Error") {
                alert("시스템 에러가 발생했습니다. 관리자에게 문의 바랍니다.");

                return;
            }
            parent.fn_ModalCloseDiv();
            parent.fn_Add('/QualityMgt/Qua98/Qua98.aspx', title, 'Qua98', true);
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

                if (clickData.cellType !== "data") {
                    return; // 데이터 셀이 아니면 이벤트 중단
                }
                if (clickData.column == 'COMPLETE_NO') {
                    var current = gridView.getCurrent();
                    var value = dataProvider.getValue(current.dataRow, "KEY_HID");
                    fn_Move(value);
                }
            }

            if (_val != undefined) {
                gridView.setFixedOptions({
                    colCount: _val
                });
            }

            gridView.columnByName("COMPLETE_NO").styleName = "grid-primary-column"
            gridView.columnByName("CAR_TYPE").styleName = "string-column"
            gridView.columnByName("LINE_CD").styleName = "string-column"
            gridView.columnByName("PART_NO").styleName = "string-column"
            gridView.columnByName("PLAN_CD").styleName = "string-column"
            gridView.columnByName("PLAN_DETAIL_CD").styleName = "string-column"
            gridView.columnByName("KEY_HID").visible = false;
            dataProvider.softDeleting = true;
            //gridView.displayOptions.fitStyle = "even";//그리드 채우기
            gridView.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView.editOptions.updatable = false;//수정 불가능 설정(추후 해제결정)
            gridView.editOptions.editable = false;//수정 불가능 설정(추후 해제결정)
            gridView.pasteOptions.enabled = false;//붙여넣기 금지
            gridView.groupPanel.visible = true;//그룹핑 활성화
            gridView.filterPanel.visible = true;//필터패널 활성화

            if (data == undefined || data == null) {
                $("#MainContent_btnSearch").hide();
                setTimeout(function () {
                    $("#MainContent_btnSearch").click();
                }, 1000);
            }
        }

        //폭 조정
        $("div[name=divView]").width("1600px");
    </script>

    <style>
        .grid-primary-column {
            font-weight: bold;
            text-decoration: underline; cursor: pointer;
            text-align: left;
        }
        .string-column {
            text-align: left;
        }
    </style>

    <asp:HiddenField ID="hidWidth" runat="server" Value="500" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="500" />

    <div class="contents" tabindex="0">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">ResComm</asp:Label></h3>
            <div class="al-r">
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="fn_WatingCall();" Visible="false" />
                <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('ResComm'); return false;" />
                <input type="button" value="Close" id="btnExit" runat="server" visible="true" onclick="javascript: parent.fn_ModalCloseDiv();" />
            </div>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <!--<div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:100px;" />
                    <col style="width:190px;" />
                    <col />
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbCond_01" runat="server"></asp:Label></th>
                    <td><asp:TextBox ID="txtCondi1" runat="server" style="width:150px;"></asp:TextBox></td>
                    <td class="al-r">
                        <%--<asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                        <asp:Button ID="btnRestore" runat="server" Text="Restore" OnClick="btnRestore_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                        <input type="button" value="New" onclick="javascript:fn_Register();" ID="btnNew" runat="server" visible="false" />
                        <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('GridExcel'); return false;" />--%>
                    </td>
                </tr>
            </table>
        </div>-->
        <br />
        <div>
            <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="realgrid" class="realgrid_p"></div>
                    <table>
                        <tr>
                            <td>
                                <div class="toolbar">
                                    <div id="gridPage" style="float:left" onselectstart="return false" ondragstart="return false"></div>
                                    &nbsp;
                                    <select id="current_page_value" onchange="setPaging(); return false;" style="font-size:12px; display:none; margin-top:10px; float:left" runat="server">
                                    </select>
                                    <input type="button" value="Set" id="btnSet" onclick="getCol('ResComm'); return false;" style="display:none; margin-top:10px; float:left" />
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